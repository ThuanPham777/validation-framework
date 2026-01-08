using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ValidationFramework.Core;
using ValidationFramework.Extensions;
using ValidationFramework.Notification;
using ValidationFramework.Result;
using ValidationFramework.Validator;
using ValidationFramework.Demo.WinUI.Models;
using ValidationFramework.Demo.WinUI.Notifiers;
using ValidationFramework.Demo.WinUI.Validators;

namespace ValidationFramework.Demo.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private ValidationEngine _engine = null!;
        private UserModelValidator _userValidator = null!;
        private Dictionary<string, TextBox> _textBoxes = null!;
        private Dictionary<string, PasswordBox> _passwordBoxes = null!;
        private Dictionary<string, TextBlock> _errorTextBlocks = null!;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMaps();
        }

        private void InitializeMaps()
        {
            _textBoxes = new Dictionary<string, TextBox>
            {
                { nameof(UserModel.Username), txtUsername },
                { nameof(UserModel.Email), txtEmail },
                { nameof(UserModel.Phone), txtPhone }
            };

            _passwordBoxes = new Dictionary<string, PasswordBox>
            {
                { nameof(UserModel.Password), txtPassword },
                { nameof(UserModel.ConfirmPassword), txtConfirmPassword }
            };

            _errorTextBlocks = new Dictionary<string, TextBlock>
            {
                { nameof(UserModel.Username), lblUsernameError },
                { nameof(UserModel.Email), lblEmailError },
                { nameof(UserModel.Phone), lblPhoneError },
                { nameof(UserModel.Password), lblPasswordError },
                { nameof(UserModel.ConfirmPassword), lblConfirmPasswordError }
            };
        }

        private void ConfigureEngine()
        {
            _engine = new ValidationEngine();
            _userValidator = new UserModelValidator();

            // Register fluent validator
            _engine.AddFluentValidator(_userValidator);

            // ConfirmPassword: cross-field validation for password matching
            _engine.AddValidator(
                nameof(UserModel.ConfirmPassword),
                new DelegateValidator((value, propertyName) =>
                {
                    if (value is not string confirmPassword)
                    {
                        return ValidationResult.Fail(
                            propertyName,
                            "Confirm Password must be a string.",
                            value,
                            "PASSWORD_MATCH_TYPE"
                        );
                    }

                    if (!string.Equals(
                            txtPassword.Password,
                            confirmPassword,
                            StringComparison.Ordinal))
                    {
                        return ValidationResult.Fail(
                            propertyName,
                            "Password and Confirm Password do not match.",
                            value,
                            "PASSWORD_MATCH"
                        );
                    }

                    return ValidationResult.Ok(propertyName);
                })
            );
        }

        private void SubscribeNotifiers()
        {
            // Create combined textbox dictionary for highlight notifier
            var allTextBoxes = new Dictionary<string, TextBox>(_textBoxes);

            if (chkContentDialog.IsChecked == true)
            {
                _engine.Publisher.Subscribe(
                    ValidationEventType.Invalid,
                    new ContentDialogNotifier(Content.XamlRoot)
                );
            }

            if (chkHighlight.IsChecked == true)
            {
                _engine.Publisher.Subscribe(
                    ValidationEventType.Invalid,
                    new TextBoxHighlightNotifier(allTextBoxes, _errorTextBlocks)
                );

                _engine.Publisher.Subscribe(
                    ValidationEventType.Validated,
                    new TextBoxHighlightNotifier(allTextBoxes, _errorTextBlocks)
                );
            }

            if (chkInfoBar.IsChecked == true)
            {
                _engine.Publisher.Subscribe(
                    ValidationEventType.Invalid,
                    new InfoBarNotifier(infoBar)
                );

                _engine.Publisher.Subscribe(
                    ValidationEventType.Validated,
                    new InfoBarNotifier(infoBar)
                );
            }

            if (chkSummaryText.IsChecked == true)
            {
                _engine.Publisher.Subscribe(
                    ValidationEventType.Invalid,
                    new TextBlockNotifier(lblSummary)
                );

                _engine.Publisher.Subscribe(
                    ValidationEventType.Validated,
                    new TextBlockNotifier(lblSummary)
                );
            }
        }

        private void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            ConfigureEngine();
            SubscribeNotifiers();

            // Reset all control styles
            ResetPasswordBoxStyles();

            foreach (var textBox in _textBoxes.Values)
            {
                textBox.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray);
                textBox.BorderThickness = new Thickness(1);
            }

            // Clear error labels
            foreach (var textBlock in _errorTextBlocks.Values)
            {
                textBlock.Text = string.Empty;
                textBlock.Visibility = Visibility.Collapsed;
            }

            var user = new UserModel
            {
                Username = txtUsername.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Password = txtPassword.Password,
                ConfirmPassword = txtConfirmPassword.Password
            };

            // Validate using both attribute and fluent validators
            var results = _engine.ValidateWithFluent(user);

            // Only manually handle PasswordBoxes highlighting if highlight is enabled
            // (TextBoxes are handled by TextBoxHighlightNotifier)
            if (chkHighlight.IsChecked == true)
            {
                foreach (var result in results.Where(r => !r.IsValid))
                {
                    if (_passwordBoxes.TryGetValue(
                            result.PropertyName,
                            out var passwordBox))
                    {
                        passwordBox.BorderBrush =
                            new SolidColorBrush(Microsoft.UI.Colors.Red);
                        passwordBox.BorderThickness = new Thickness(2);
                    }

                    // Error labels are handled by TextBoxHighlightNotifier
                    // Only handle password error labels manually
                    if ((result.PropertyName == nameof(UserModel.Password) ||
                         result.PropertyName == nameof(UserModel.ConfirmPassword)) &&
                        _errorTextBlocks.TryGetValue(
                            result.PropertyName,
                            out var errorTextBlock))
                    {
                        errorTextBlock.Text = $"⚠ {result.Message}";
                        errorTextBlock.Foreground =
                            new SolidColorBrush(Microsoft.UI.Colors.Red);
                        errorTextBlock.FontSize = 12;
                        errorTextBlock.Visibility = Visibility.Visible;
                    }
                }
            }

            // Update summary if not using TextBlockNotifier
            if (chkSummaryText.IsChecked != true)
            {
                var errors = results.Where(r => !r.IsValid).ToList();

                if (errors.Count == 0)
                {
                    lblSummary.Text =
                        "✅ All validations passed! User registration is valid.";
                    lblSummary.Foreground =
                        new SolidColorBrush(Microsoft.UI.Colors.Green);
                    lblSummary.FontWeight =
                        Microsoft.UI.Text.FontWeights.SemiBold;
                }
                else
                {
                    lblSummary.Text =
                        $"❌ Found {errors.Count} validation error(s). Please check the highlighted fields.";
                    lblSummary.Foreground =
                        new SolidColorBrush(Microsoft.UI.Colors.Red);
                    lblSummary.FontWeight =
                        Microsoft.UI.Text.FontWeights.Normal;
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            // Clear TextBoxes
            txtUsername.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;

            // Clear PasswordBoxes
            txtPassword.Password = string.Empty;
            txtConfirmPassword.Password = string.Empty;

            // Reset TextBox styles
            foreach (var textBox in _textBoxes.Values)
            {
                textBox.BorderBrush =
                    new SolidColorBrush(Microsoft.UI.Colors.Gray);
                textBox.BorderThickness = new Thickness(1);
            }

            ResetPasswordBoxStyles();

            // Clear error labels
            foreach (var textBlock in _errorTextBlocks.Values)
            {
                textBlock.Text = string.Empty;
                textBlock.Visibility = Visibility.Collapsed;
            }

            // Reset summary
            lblSummary.Text =
                "Enter data and click Validate to check...";
            lblSummary.Foreground =
                new SolidColorBrush(Microsoft.UI.Colors.Gray);
            lblSummary.FontWeight =
                Microsoft.UI.Text.FontWeights.Normal;

            // Close InfoBar
            infoBar.IsOpen = false;

            // Focus on first field
            txtUsername.Focus(FocusState.Programmatic);
        }

        private void ResetPasswordBoxStyles()
        {
            foreach (var passwordBox in _passwordBoxes.Values)
            {
                passwordBox.BorderBrush =
                    new SolidColorBrush(Microsoft.UI.Colors.Gray);
                passwordBox.BorderThickness = new Thickness(1);
            }
        }
    }
}
