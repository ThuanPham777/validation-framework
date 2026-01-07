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
    // Notifier that shows validation errors in a ContentDialog
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

            var message = string.Join(Environment.NewLine, errors.Select(r => $"• {r.PropertyName}: {r.Message}"));

            var dialog = new ContentDialog
            {
                Title = "Validation Errors",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = _xamlRoot
            };

            await dialog.ShowAsync();
        }
    }

    // Notifier that displays errors in a TextBlock control
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
                _textBlock.Text = "✓ All validations passed!";
                _textBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
            }
            else
            {
                _textBlock.Text = string.Join(Environment.NewLine, errors.Select(r => $"✗ {r.PropertyName}: {r.Message}"));
                _textBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
            }
        }
    }

    // Notifier that highlights invalid TextBox controls
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
            // Reset all textboxes
            foreach (var kvp in _textBoxes)
            {
                kvp.Value.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray);
                if (_errorTextBlocks is not null && _errorTextBlocks.TryGetValue(kvp.Key, out var textBlock))
                {
                    textBlock.Text = string.Empty;
                    textBlock.Visibility = Visibility.Collapsed;
                }
            }

            // Highlight invalid ones
            foreach (var result in results.Where(r => !r.IsValid))
            {
                if (_textBoxes.TryGetValue(result.PropertyName, out var textBox))
                {
                    textBox.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);
                }

                if (_errorTextBlocks is not null && _errorTextBlocks.TryGetValue(result.PropertyName, out var errorTextBlock))
                {
                    errorTextBlock.Text = result.Message;
                    errorTextBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    errorTextBlock.Visibility = Visibility.Visible;
                }
            }
        }
    }

    // Notifier that shows InfoBar notifications
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
                _infoBar.Title = "Success";
                _infoBar.Message = "All validations passed!";
                _infoBar.Severity = InfoBarSeverity.Success;
            }
            else
            {
                _infoBar.Title = "Validation Errors";
                _infoBar.Message = string.Join("; ", errors.Select(r => $"{r.PropertyName}: {r.Message}"));
                _infoBar.Severity = InfoBarSeverity.Error;
            }
            _infoBar.IsOpen = true;
        }
    }
}
