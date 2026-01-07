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

    // Notifier that shows tooltip on invalid fields
    public class WinFormsToolTipNotifier : IValidationNotifierSubscriber
    {
        private readonly ToolTip _toolTip;
        private readonly Dictionary<string, Control> _controls;

        public WinFormsToolTipNotifier(ToolTip toolTip, Dictionary<string, Control> controls)
        {
            _toolTip = toolTip;
            _controls = controls;
        }

        public void Notify(List<ValidationResult> results)
        {
            // Clear all tooltips first
            foreach (var control in _controls.Values)
            {
                _toolTip.SetToolTip(control, string.Empty);
            }

            // Set tooltips for invalid fields
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_controls.TryGetValue(result.PropertyName, out var control))
                {
                    _toolTip.SetToolTip(control, result.Message);
                }
            }
        }
    }

    // Notifier that updates ErrorProvider for each field
    public class ErrorProviderNotifier : IValidationNotifierSubscriber
    {
        private readonly ErrorProvider _errorProvider;
        private readonly Dictionary<string, Control> _controls;

        public ErrorProviderNotifier(ErrorProvider errorProvider, Dictionary<string, Control> controls)
        {
            _errorProvider = errorProvider;
            _controls = controls;
        }

        public void Notify(List<ValidationResult> results)
        {
            // Clear all errors first
            foreach (var control in _controls.Values)
            {
                _errorProvider.SetError(control, string.Empty);
            }

            // Set errors for invalid fields
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_controls.TryGetValue(result.PropertyName, out var control))
                {
                    _errorProvider.SetError(control, result.Message);
                }
            }
        }
    }
}
