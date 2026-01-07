using ValidationFramework.Notification;
using ValidationFramework.Result;

namespace ValidationFramework.Demo.Winforms.Notifiers
{
    // Notifier that shows validation errors in a MessageBox
    public class WinFormsMessageBoxNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0) return;

            var message = string.Join(Environment.NewLine, errors.Select(r => $"• {r.PropertyName}: {r.Message}"));
            MessageBox.Show(message, "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // Notifier that displays errors in a Label control
    public class WinFormsLabelNotifier : IValidationNotifierSubscriber
    {
        private readonly Label _label;

        public WinFormsLabelNotifier(Label label)
        {
            _label = label;
        }

        public void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0)
            {
                _label.Text = "✓ All validations passed!";
                _label.ForeColor = Color.Green;
            }
            else
            {
                _label.Text = string.Join(Environment.NewLine, errors.Select(r => $"✗ {r.PropertyName}: {r.Message}"));
                _label.ForeColor = Color.Red;
            }
        }
    }

    // Notifier that highlights invalid TextBox controls with red border
    public class TextBoxHighlightNotifier : IValidationNotifierSubscriber
    {
        private readonly Dictionary<string, TextBox> _textBoxes;
        private readonly Dictionary<string, Label> _errorLabels;

        public TextBoxHighlightNotifier(Dictionary<string, TextBox> textBoxes, Dictionary<string, Label>? errorLabels = null)
        {
            _textBoxes = textBoxes;
            _errorLabels = errorLabels ?? new Dictionary<string, Label>();
        }

        public void Notify(List<ValidationResult> results)
        {
            // Reset all textboxes
            foreach (var kvp in _textBoxes)
            {
                kvp.Value.BackColor = SystemColors.Window;
                if (_errorLabels.TryGetValue(kvp.Key, out var label))
                {
                    label.Text = string.Empty;
                    label.Visible = false;
                }
            }

            // Highlight invalid ones
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_textBoxes.TryGetValue(result.PropertyName, out var textBox))
                {
                    textBox.BackColor = Color.MistyRose;
                }

                if (_errorLabels.TryGetValue(result.PropertyName, out var errorLabel))
                {
                    errorLabel.Text = result.Message;
                    errorLabel.ForeColor = Color.Red;
                    errorLabel.Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// Enhanced ToolTip notifier with better visibility and styling
    /// Shows beautiful balloon-style tooltips when hovering over controls
    /// </summary>
    public class WinFormsToolTipNotifier : IValidationNotifierSubscriber
    {
        private readonly ToolTip _toolTip;
        private readonly Dictionary<string, Control> _controls;

        public WinFormsToolTipNotifier(ToolTip toolTip, Dictionary<string, Control> controls)
        {
            _toolTip = toolTip;
            _controls = controls;

            // Configure ToolTip for maximum visibility and better UX
            _toolTip.AutoPopDelay = 8000;      // Show for 8 seconds
            _toolTip.InitialDelay = 50;     // Show almost immediately (50ms)
            _toolTip.ReshowDelay = 50;         // Quick reshow when moving between controls
            _toolTip.ShowAlways = true; // Show even if form is not active
            _toolTip.IsBalloon = true;         // Balloon style for better visibility
            _toolTip.ToolTipIcon = ToolTipIcon.Error;  // Red error icon
            _toolTip.ToolTipTitle = "❌ Validation Error";  // Title with emoji
            _toolTip.BackColor = Color.LightYellow;  // Light yellow background
            _toolTip.ForeColor = Color.DarkRed;      // Dark red text
        }

        public void Notify(List<ValidationResult> results)
        {
            // Clear all tooltips and reset text colors
            foreach (var control in _controls.Values)
            {
                _toolTip.SetToolTip(control, string.Empty);
                if (control is TextBox textBox)
                {
                    textBox.ForeColor = SystemColors.WindowText;
                }
            }

            // Set tooltips for invalid fields with enhanced formatting
            var errors = results.Where(r => !r.IsValid).ToList();

            foreach (var result in errors)
            {
                if (_controls.TryGetValue(result.PropertyName, out var control))
                {
                    // Format tooltip with field name and message
                    string tooltipText = $"⚠ {result.Message}";

                    _toolTip.SetToolTip(control, tooltipText);

                    // Visual indicator: red text color
                    if (control is TextBox textBox)
                    {
                        textBox.ForeColor = Color.DarkRed;
                    }
                }
            }

            // Force tooltip to show immediately on first invalid control
            if (errors.Count > 0 && _controls.TryGetValue(errors[0].PropertyName, out var firstControl))
            {
                // This helps make the tooltip more noticeable
                _toolTip.Show(_toolTip.GetToolTip(firstControl), firstControl, 0, firstControl.Height, 3000);
            }
        }
    }

    /// <summary>
    /// Enhanced ErrorProvider notifier with better visual feedback
    /// Shows red error icon (!) next to invalid controls
    /// </summary>
    public class ErrorProviderNotifier : IValidationNotifierSubscriber
    {
        private readonly ErrorProvider _errorProvider;
        private readonly Dictionary<string, Control> _controls;

        public ErrorProviderNotifier(ErrorProvider errorProvider, Dictionary<string, Control> controls)
        {
            _errorProvider = errorProvider;
            _controls = controls;

            // Configure ErrorProvider for better visibility
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;  // Don't blink
            _errorProvider.BlinkRate = 0;
            _errorProvider.Icon = SystemIcons.Error;  // Red error icon
        }

        public void Notify(List<ValidationResult> results)
        {
            // Clear all errors and reset control styles
            foreach (var control in _controls.Values)
            {
                _errorProvider.SetError(control, string.Empty);
                _errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                _errorProvider.SetIconPadding(control, 3);

                // Reset textbox border style
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.Fixed3D;
                }
            }

            // Set errors for invalid fields with enhanced formatting
            var errors = results.Where(r => !r.IsValid).ToList();

            foreach (var result in errors)
            {
                if (_controls.TryGetValue(result.PropertyName, out var control))
                {
                    // Format error message
                    string errorMessage = result.Message;

                    _errorProvider.SetError(control, errorMessage);

                    // Visual feedback: change border to indicate error
                    if (control is TextBox textBox)
                    {
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                    }
                }
            }
        }
    }
}
