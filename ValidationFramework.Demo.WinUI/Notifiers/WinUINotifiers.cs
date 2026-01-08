using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ValidationFramework.Notification;
using ValidationFramework.Result;

namespace ValidationFramework.Demo.WinUI.Notifiers
{
    /// <summary>
    /// Shows validation errors in a beautiful ContentDialog popup
    /// </summary>
    public class ContentDialogNotifier : IValidationNotifierSubscriber
    {
        private readonly XamlRoot _xamlRoot;

        public ContentDialogNotifier(XamlRoot xamlRoot)
        {
            _xamlRoot = xamlRoot;
        }

        public async void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0) return;

            // Format error message with bullet points
            var message = string.Join(Environment.NewLine, errors.Select(r => $"• {r.PropertyName}: {r.Message}"));

            var dialog = new ContentDialog
            {
                Title = "❌ Validation Errors",
                Content = new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 10, 0, 0)
                },
                CloseButtonText = "OK",
                XamlRoot = _xamlRoot,
                DefaultButton = ContentDialogButton.Close
            };

            await dialog.ShowAsync();
        }
    }

    /// <summary>
    /// Displays validation summary in a TextBlock control
    /// Shows green for success, red for errors
    /// </summary>
    public class TextBlockNotifier : IValidationNotifierSubscriber
    {
        private readonly TextBlock _textBlock;

        public TextBlockNotifier(TextBlock textBlock)
        {
            _textBlock = textBlock;
        }

        public void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0)
            {
                _textBlock.Text = "✅ All validations passed! Registration is valid.";
                _textBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                _textBlock.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            }
            else
            {
                var errorMessages = errors.Select(r => $"• {r.PropertyName}: {r.Message}");
                _textBlock.Text = $"❌ Found {errors.Count} validation error(s):\n" + string.Join("\n", errorMessages);
                _textBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                _textBlock.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            }
        }
    }

    /// <summary>
    /// Highlights invalid TextBox controls with red border
    /// Shows error messages below each field
    /// </summary>
    public class TextBoxHighlightNotifier : IValidationNotifierSubscriber
    {
        private readonly Dictionary<string, TextBox> _textBoxes;
        private readonly Dictionary<string, TextBlock>? _errorTextBlocks;

        public TextBoxHighlightNotifier(Dictionary<string, TextBox> textBoxes, Dictionary<string, TextBlock>? errorTextBlocks = null)
        {
            _textBoxes = textBoxes;
            _errorTextBlocks = errorTextBlocks;
        }

        public void Notify(List<ValidationResult> results)
        {
            // Reset all textboxes to default style
            foreach (var kvp in _textBoxes)
            {
                kvp.Value.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray);
                kvp.Value.BorderThickness = new Thickness(1);

                if (_errorTextBlocks != null && _errorTextBlocks.TryGetValue(kvp.Key, out var textBlock))
                {
                    textBlock.Text = string.Empty;
                    textBlock.Visibility = Visibility.Collapsed;
                }
            }

            // Highlight invalid ones with thick red border
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_textBoxes.TryGetValue(result.PropertyName, out var textBox))
                {
                    textBox.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    textBox.BorderThickness = new Thickness(2);
                }

                if (_errorTextBlocks != null && _errorTextBlocks.TryGetValue(result.PropertyName, out var errorTextBlock))
                {
                    errorTextBlock.Text = $"⚠ {result.Message}";
                    errorTextBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    errorTextBlock.FontSize = 12;
                    errorTextBlock.Visibility = Visibility.Visible;
                }
            }
        }
    }

    /// <summary>
    /// Shows validation status in an InfoBar control
    /// InfoBar slides in from top with color-coded severity
    /// </summary>
    public class InfoBarNotifier : IValidationNotifierSubscriber
    {
        private readonly InfoBar _infoBar;

        public InfoBarNotifier(InfoBar infoBar)
        {
            _infoBar = infoBar;
        }

        public void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0)
            {
                _infoBar.Title = "✅ Validation Successful";
                _infoBar.Message = "All fields are valid! You can proceed with registration.";
                _infoBar.Severity = InfoBarSeverity.Success;
            }
            else
            {
                _infoBar.Title = $"❌ {errors.Count} Validation Error(s)";

                // Show first 3 errors in InfoBar
                var displayErrors = errors.Take(3).Select(r => $"• {r.PropertyName}: {r.Message}");
                var moreCount = errors.Count - 3;

                var message = string.Join("\n", displayErrors);
                if (moreCount > 0)
                {
                    message += $"\n... and {moreCount} more error(s)";
                }

                _infoBar.Message = message;
                _infoBar.Severity = InfoBarSeverity.Error;
            }

            _infoBar.IsOpen = true;

            // Auto-close success message after 5 seconds
            if (errors.Count == 0)
            {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
                timer.Tick += (s, e) =>
            {
                _infoBar.IsOpen = false;
                timer.Stop();
            };
                timer.Start();
            }
        }
    }

    /// <summary>
    /// Shows validation errors in a ToolTip when hovering over controls
    /// WinUI 3 version with enhanced styling
    /// </summary>
    public class ToolTipNotifier : IValidationNotifierSubscriber
    {
        private readonly Dictionary<string, UIElement> _controls;

        public ToolTipNotifier(Dictionary<string, UIElement> controls)
        {
            _controls = controls;
        }

        public void Notify(List<ValidationResult> results)
        {
            // Clear all tooltips
            foreach (var control in _controls.Values)
            {
                ToolTipService.SetToolTip(control, null);
            }

            // Set tooltips for invalid fields
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_controls.TryGetValue(result.PropertyName, out var control))
                {
                    var toolTipContent = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Spacing = 4
                    };

                    // Add icon
                    var icon = new TextBlock
                    {
                        Text = "❌",
                        FontSize = 16
                    };

                    // Add message
                    var message = new TextBlock
                    {
                        Text = result.Message,
                        TextWrapping = TextWrapping.Wrap,
                        MaxWidth = 300,
                        FontSize = 14
                    };

                    toolTipContent.Children.Add(icon);
                    toolTipContent.Children.Add(message);

                    ToolTipService.SetToolTip(control, toolTipContent);
                    ToolTipService.SetPlacement(control, Microsoft.UI.Xaml.Controls.Primitives.PlacementMode.Bottom);
                }
            }
        }
    }
}
