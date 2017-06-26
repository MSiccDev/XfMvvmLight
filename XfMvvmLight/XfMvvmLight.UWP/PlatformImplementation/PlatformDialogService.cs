using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using XfMvvmLight.UWP.PlatformImplementation;


namespace XfMvvmLight.UWP.PlatformImplementation
{
    public class PlatformDialogService : IDialogService
    {
        List<ContentDialog> _openDialogs = new List<ContentDialog>();


        public void CloseAllDialogs()
        {
            foreach (var dialog in _openDialogs)
            {
                dialog.Hide();
            }
            _openDialogs.Clear();
        }


        public async Task ShowErrorAsync(string title, Exception error, string buttonText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await ShowContentDialog(title, error.ToString(), buttonText, null, closeAction, cancelableOnTouchOutside, cancelable);
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            await ShowContentDialog(title, message, "OK", null, null, false, false);
        }

        public async Task ShowMessageAsync(string title, string message, string buttonText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await ShowContentDialog(title, message, buttonText, null, closeAction, cancelableOnTouchOutside, cancelable);
        }

        public async Task ShowMessageAsync(string title, string message, string buttonConfirmText, string buttonCancelText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await ShowContentDialog(title, message, buttonConfirmText, buttonCancelText, closeAction, cancelableOnTouchOutside, cancelable);
        }



        internal async Task ShowContentDialog(string title, string content, string confirmButtonText = null, string cancelButtonText = null, Action<bool> callback = null, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
                var messageDialog = new ContentDialog()
                {
                    Title = title,
                    Content = content,

                    DefaultButton = ContentDialogButton.Primary
                };


                if (!string.IsNullOrEmpty(confirmButtonText))
                {
                    messageDialog.PrimaryButtonText = confirmButtonText;

                    messageDialog.PrimaryButtonClick += (sender, e) =>
                    {
                        callback?.Invoke(true);
                        _openDialogs.Remove((ContentDialog)sender);
                    };
                }

                if (!string.IsNullOrEmpty(cancelButtonText))
                {
                    messageDialog.SecondaryButtonText = cancelButtonText;

                    messageDialog.SecondaryButtonClick += (sender, e) =>
                    {
                        callback?.Invoke(false);
                        _openDialogs.Remove((ContentDialog)sender);
                    };
                }


                messageDialog.CloseButtonClick += (sender, e) =>
                {
                    callback?.Invoke(false);
                    _openDialogs.Remove((ContentDialog)sender);
                };

            Device.BeginInvokeOnMainThread(async () =>
            {
                _openDialogs.Add(messageDialog);

                await messageDialog.ShowAsync();
            });
        }
    }
}
