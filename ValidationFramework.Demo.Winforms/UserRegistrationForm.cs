using System.Text.RegularExpressions;
using ValidationFramework.Core;
using ValidationFramework.Notification;
using ValidationFramework.Result;
using ValidationFramework.Fluent;
using ValidationFramework.Demo.Winforms.Models;
using ValidationFramework.Demo.Winforms.Notifiers;

namespace ValidationFramework.Demo.Winforms
{
    public partial class UserRegistrationForm : Form
    {
        private ValidationEngine _engine = null!;
        private Dictionary<string, TextBox> _textBoxes = null!;
        private Dictionary<string, Control> _controls = null!;
        private Dictionary<string, Label> _errorLabels = null!;

        public UserRegistrationForm()
        {
            InitializeComponent();
            InitializeMaps();
            WireUpEvents();
        }

        // Initialize mappings only
        private void InitializeMaps()
        {
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
            // Create a fresh engine for each validation
            _engine = new ValidationEngine();

            // Register fluent validators for UserModel
            _engine.AddFluentValidator<UserModel>(b =>
            {
                // Username: required + no special characters
                b.For(u => u.Username)
                    .Required()
                    .Must(s => !string.IsNullOrEmpty(s) && !Regex.IsMatch(s!, "[^a-zA-Z0-9]"), "Username must not contain special characters", "NO_SPECIAL_CHAR");

                // Password: must be strong
                b.For(u => u.Password)
                    .Required()
                    .Must(s => s is string ss && Regex.IsMatch(ss, "[A-Z]"), "Password must contain at least one uppercase letter", "STRONG_PASSWORD_UPPER")
                    .Must(s => s is string ss && Regex.IsMatch(ss, "[a-z]"), "Password must contain at least one lowercase letter", "STRONG_PASSWORD_LOWER")
                    .Must(s => s is string ss && Regex.IsMatch(ss, "\\d"), "Password must contain at least one digit", "STRONG_PASSWORD_DIGIT");

                // ConfirmPassword: must match Password
                b.For(u => u.ConfirmPassword).Custom((value, propertyName) =>
                {
                    if (value is not string confirm)
                        return ValidationResult.Fail(propertyName, "Confirm Password must be a string.", value, "PASSWORD_MATCH_TYPE");

                    if (txtPassword.Text != confirm)
                        return ValidationResult.Fail(propertyName, "Password and Confirm Password do not match.", value, "PASSWORD_MATCH");

                    return ValidationResult.Ok(propertyName);
                });

                // Email: allowed domains
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

            // Validate - this will automatically notify subscribers
            var results = _engine.Validate(user);

            // Update summary if not using LabelNotifier
            if (!chkSummaryLabel.Checked)
            {
                var errors = results.Where(r => !r.IsValid).ToList();
                if (errors.Count == 0)
                {
                    lblSummary.Text = "? All validations passed! User registration is valid.";
                    lblSummary.ForeColor = Color.Green;
                    MessageBox.Show("Registration is valid!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblSummary.Text = $"Found {errors.Count} validation error(s). Please check the highlighted fields.";
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
