using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ValidationFramework.Core;
using ValidationFramework.Notification;
using ValidationFramework.Result;
using ValidationFramework.Demo.WinUI.Models;
using ValidationFramework.Demo.WinUI.Notifiers;

namespace ValidationFramework.Demo.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private ValidationEngine _engine = null!;
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

            _engine.AddFluentValidator<UserModel>(b =>
            {
                // Username: must not contain special characters
                b.For(u => u.Username)
                    .Required()
                    .Must(s => !string.IsNullOrWhiteSpace(s) && !System.Text.RegularExpressions.Regex.IsMatch(s!, "[^a-zA-Z0-9]"), "Username must not contain special characters", "NO_SPECIAL_CHAR");

                // Password: must be strong
                b.For(u => u.Password)
                    .Required()
                    .Must(s => s is string ss && System.Text.RegularExpressions.Regex.IsMatch(ss, "[A-Z]"), "Password must contain at least one uppercase letter", "STRONG_PASSWORD_UPPER")
                    .Must(s => s is string ss && System.Text.RegularExpressions.Regex.IsMatch(ss, "[a-z]"), "Password must contain at least one lowercase letter", "STRONG_PASSWORD_LOWER")
                    .Must(s => s is string ss && System.Text.RegularExpressions.Regex.IsMatch(ss, "\\d"), "Password must contain at least one digit", "STRONG_PASSWORD_DIGIT");

                // ConfirmPassword: must match Password
                b.For(u => u.ConfirmPassword).Custom((value, propertyName) =>
                {
                    if (value is not string confirm)
                        return ValidationResult.Fail(propertyName, "Confirm Password must be a string.", value, "PASSWORD_MATCH_TYPE");

                    if (!string.Equals(txtPassword.Password, confirm, StringComparison.Ordinal))
                        return ValidationResult.Fail(propertyName, "Password and Confirm Password do not match.", value, "PASSWORD_MATCH");

                    return ValidationResult.Ok(propertyName);
                });

                // Email: extra rule for allowed domains
                b.For(u => u.Email).Custom((value, propertyName) =>
                {
                    if (value is string email && !string.IsNullOrWhiteSpace(email))
                    {
                        if (!email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) &&
                            !email.EndsWith("@outlook.com", StringComparison.OrdinalIgnoreCase) &&
                            !email.EndsWith("@yahoo.com", StringComparison.OrdinalIgnoreCase))
                        {
                            return ValidationResult.Fail(propertyName, "Email must be from @gmail.com, @outlook.com, or @yahoo.com", value, "EMAIL_DOMAIN");
                        }
                    }

                    return ValidationResult.Ok(propertyName);
                });
            });
        }

        private void SubscribeNotifiers()
        {
            // Create combined textbox dictionary for highlight notifier
            var allTextBoxes = new Dictionary<string, TextBox>(_textBoxes);

            if (chkContentDialog.IsChecked == true)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new ContentDialogNotifier(Content.XamlRoot));
            }

            if (chkHighlight.IsChecked == true)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new TextBoxHighlightNotifier(allTextBoxes, _errorTextBlocks));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new TextBoxHighlightNotifier(allTextBoxes, _errorTextBlocks));
            }

            if (chkInfoBar.IsChecked == true)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new InfoBarNotifier(infoBar));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new InfoBarNotifier(infoBar));
            }

            if (chkSummaryText.IsChecked == true)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new TextBlockNotifier(lblSummary));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new TextBlockNotifier(lblSummary));
            }
        }

        private void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            ConfigureEngine();
            SubscribeNotifiers();

            // Also handle PasswordBox highlighting manually
            ResetPasswordBoxStyles();

            var user = new UserModel
            {
                Username = txtUsername.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Password = txtPassword.Password,
                ConfirmPassword = txtConfirmPassword.Password
            };

            var results = _engine.Validate(user);

            // Highlight PasswordBoxes for errors
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_passwordBoxes.TryGetValue(result.PropertyName, out var passwordBox))
                {
                    passwordBox.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);
                }

                if (_errorTextBlocks.TryGetValue(result.PropertyName, out var errorTextBlock))
                {
                    errorTextBlock.Text = result.Message;
                    errorTextBlock.Visibility = Visibility.Visible;
                }
            }

            // Update summary if not using TextBlockNotifier
            if (chkSummaryText.IsChecked != true)
            {
                var errors = results.Where(r => !r.IsValid).ToList();
                if (errors.Count == 0)
                {
                    lblSummary.Text = "✓ All validations passed! User registration is valid.";
                    lblSummary.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                }
                else
                {
                    lblSummary.Text = $"Found {errors.Count} validation error(s). Please check the highlighted fields.";
                    lblSummary.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
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

            // Reset styles
            foreach (var textBox in _textBoxes.Values)
            {
                textBox.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray);
            }

            ResetPasswordBoxStyles();

            // Clear error labels
            foreach (var textBlock in _errorTextBlocks.Values)
            {
                textBlock.Text = string.Empty;
                textBlock.Visibility = Visibility.Collapsed;
            }

            // Reset summary
            lblSummary.Text = "Enter data and click Validate to check...";
            lblSummary.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray);

            // Close InfoBar
            infoBar.IsOpen = false;

            // Focus on first field
            txtUsername.Focus(FocusState.Programmatic);
        }

        private void ResetPasswordBoxStyles()
        {
            foreach (var passwordBox in _passwordBoxes.Values)
            {
                passwordBox.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray);
            }
        }
    }
}
