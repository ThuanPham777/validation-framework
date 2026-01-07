using ValidationFramework.Core;
using ValidationFramework.Notification;
using ValidationFramework.Validator;
using ValidationFramework.Result;
using ValidationFramework.Demo.Winforms.Models;
using ValidationFramework.Demo.Winforms.Notifiers;
using ValidationFramework.Demo.Winforms.Validators;
using ValidationFramework.Extensions;
using System.Linq;

namespace ValidationFramework.Demo.Winforms
{
    public partial class UserRegistrationForm : Form
    {
        private ValidationEngine _engine = null!;
        private Dictionary<string, TextBox> _textBoxes = null!;
        private Dictionary<string, Control> _controls = null!;
        private Dictionary<string, Label> _errorLabels = null!;
        private UserModelValidator _userValidator = null!;

        public UserRegistrationForm()
        {
            InitializeComponent();
            InitializeValidation();
            WireUpEvents();
        }

        // Initialize the validation engine with validators and notifiers
        private void InitializeValidation()
        {
            _engine = new ValidationEngine();
            _userValidator = new UserModelValidator();

            // Map TextBoxes to property names
            _textBoxes = new Dictionary<string, TextBox>
            {
                { nameof(UserModel.Username), txtUsername },
                { nameof(UserModel.Email), txtEmail },
                { nameof(UserModel.Phone), txtPhone },
                { nameof(UserModel.Password), txtPassword },
                { nameof(UserModel.ConfirmPassword), txtConfirmPassword }
            };

            // Map Controls for ToolTip and ErrorProvider
            _controls = new Dictionary<string, Control>
            {
                { nameof(UserModel.Username), txtUsername },
                { nameof(UserModel.Email), txtEmail },
                { nameof(UserModel.Phone), txtPhone },
                { nameof(UserModel.Password), txtPassword },
                { nameof(UserModel.ConfirmPassword), txtConfirmPassword }
            };

            // Map error labels
            _errorLabels = new Dictionary<string, Label>
            {
                { nameof(UserModel.Username), lblUsernameError },
                { nameof(UserModel.Email), lblEmailError },
                { nameof(UserModel.Phone), lblPhoneError },
                { nameof(UserModel.Password), lblPasswordError },
                { nameof(UserModel.ConfirmPassword), lblConfirmPasswordError }
            };

            // Register fluent validator
            _engine.AddFluentValidator(_userValidator);

            // Add custom ConfirmPassword validator using DelegateValidator
            // This checks password matching which is cross-field validation
            _engine.AddValidator(nameof(UserModel.ConfirmPassword), new DelegateValidator((value, propertyName) =>
            {
                if (!(value is string confirmPassword))
                    return ValidationResult.Fail(propertyName, "Confirm Password must be a string.", value, "PASSWORD_MATCH_TYPE");

                if (txtPassword.Text != confirmPassword)
                    return ValidationResult.Fail(propertyName, "Password and Confirm Password do not match.", value, "PASSWORD_MATCH");

                return ValidationResult.Ok(propertyName);
            }));
        }

        // Wire up button click events
        private void WireUpEvents()
        {
            btnValidate.Click += BtnValidate_Click;
            btnClear.Click += BtnClear_Click;
        }

        // Handle Validate button click
        private void BtnValidate_Click(object? sender, EventArgs e)
        {
            // Create a fresh engine for each validation to re-subscribe notifiers
            _engine = new ValidationEngine();
            _userValidator = new UserModelValidator();

            // Re-register fluent validator
            _engine.AddFluentValidator(_userValidator);

            // Re-add ConfirmPassword validator
            _engine.AddValidator(nameof(UserModel.ConfirmPassword), new DelegateValidator((value, propertyName) =>
            {
                if (!(value is string confirmPassword))
                    return ValidationResult.Fail(propertyName, "Confirm Password must be a string.", value, "PASSWORD_MATCH_TYPE");

                if (txtPassword.Text != confirmPassword)
                    return ValidationResult.Fail(propertyName, "Password and Confirm Password do not match.", value, "PASSWORD_MATCH");

                return ValidationResult.Ok(propertyName);
            }));

            // Subscribe notifiers based on checkbox selections
            if (chkMessageBox.Checked)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new WinFormsMessageBoxNotifier());
            }

            if (chkHighlight.Checked)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new TextBoxHighlightNotifier(_textBoxes, _errorLabels));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new TextBoxHighlightNotifier(_textBoxes, _errorLabels));
            }

            if (chkErrorProvider.Checked)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new ErrorProviderNotifier(errorProvider, _controls));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new ErrorProviderNotifier(errorProvider, _controls));
            }

            if (chkToolTip.Checked)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new WinFormsToolTipNotifier(toolTip, _controls));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new WinFormsToolTipNotifier(toolTip, _controls));
            }

            if (chkSummaryLabel.Checked)
            {
                _engine.Publisher.Subscribe(ValidationEventType.Invalid, new WinFormsLabelNotifier(lblSummary));
                _engine.Publisher.Subscribe(ValidationEventType.Validated, new WinFormsLabelNotifier(lblSummary));
            }

            // Create model from form data
            var user = new UserModel
            {
                Username = txtUsername.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Password = txtPassword.Text,
                ConfirmPassword = txtConfirmPassword.Text
            };

            // Validate using both attribute and fluent validators
            var results = _engine.ValidateWithFluent(user);

            // Update summary if not using LabelNotifier
            if (!chkSummaryLabel.Checked)
            {
                var errors = results.Where(r => !r.IsValid).ToList();
                if (errors.Count == 0)
                {
                    lblSummary.Text = "✓ All validations passed! User registration is valid.";
                    lblSummary.ForeColor = Color.Green;
                    MessageBox.Show("Registration is valid!\n\nValidation approach:\n• Attributes: Basic rules (Required, Email, Phone, Length)\n• Fluent API: Complex rules (Password strength, Email domain)\n• Delegate: Cross-field validation (Password matching)",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblSummary.Text = $"✗ Found {errors.Count} validation error(s). Please check the highlighted fields.";
                    lblSummary.ForeColor = Color.Red;
                }
            }
        }

        // Handle Clear button click
        private void BtnClear_Click(object? sender, EventArgs e)
        {
            // Clear all textboxes
            txtUsername.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();

            // Reset textbox backgrounds
            foreach (var textBox in _textBoxes.Values)
            {
                textBox.BackColor = SystemColors.Window;
            }

            // Clear error labels
            foreach (var label in _errorLabels.Values)
            {
                label.Text = string.Empty;
                label.Visible = false;
            }

            // Clear error provider
            foreach (var control in _controls.Values)
            {
                errorProvider.SetError(control, string.Empty);
                toolTip.SetToolTip(control, string.Empty);
            }

            // Reset summary
            lblSummary.Text = "Enter data and click Validate to check...";
            lblSummary.ForeColor = SystemColors.ControlText;

            // Focus on first field
            txtUsername.Focus();
        }
    }
}
