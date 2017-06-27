using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using XfMvvmLight.Droid.PlatformImplementation;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformDialogService))]
namespace XfMvvmLight.Droid.PlatformImplementation
{
    public class PlatformDialogService : IDialogService
    {
        List<AlertDialog> _openDialogs = new List<AlertDialog>();

        public void CloseAllDialogs()
        {
            foreach (var dialog in _openDialogs)
            {
                dialog.Dismiss();
            }
            _openDialogs.Clear();
        }


        public async Task ShowMessageAsync(string title, string message)
        {
            await Task.Run(() => ShowAlert(title, message, "OK", null, null, false, false));
        }


        public async Task ShowErrorAsync(string title, Exception error, string buttonText, Action<bool> callback, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await Task.Run(() => ShowAlert(title, error.ToString(), buttonText, null, callback, cancelableOnTouchOutside, cancelable));
        }

        public async Task ShowMessageAsync(string title, string message, string buttonText, Action<bool> callback, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await Task.Run(() => ShowAlert(title, message, buttonText, null, callback, cancelableOnTouchOutside, cancelable));
        }


        public async Task ShowMessageAsync(string title, string message, string buttonConfirmText, string buttonCancelText, Action<bool> callback, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await Task.Run(() => ShowAlert(title, message, buttonConfirmText, buttonCancelText, callback, cancelableOnTouchOutside, cancelable));
        }



        //one method to rule them all
        internal void ShowAlert(string title, string content, string confirmButtonText = null, string cancelButtonText = null, Action<bool> callback = null, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            var alert = new AlertDialog.Builder(Forms.Context);
            alert.SetTitle(title);
            alert.SetMessage(content);

            if (!string.IsNullOrEmpty(confirmButtonText))
            {
                alert.SetPositiveButton(confirmButtonText, (sender, e) =>
                {
                    callback?.Invoke(true);
                    _openDialogs.Remove((AlertDialog)sender);
                });
            }

            if (!string.IsNullOrEmpty(cancelButtonText))
            {
                alert.SetNegativeButton(cancelButtonText, (sender, e) =>
                {
                    callback?.Invoke(false);
                    _openDialogs.Remove((AlertDialog)sender);
                });
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                var dialog = alert.Show();
                _openDialogs.Add(dialog);
                dialog.SetCanceledOnTouchOutside(cancelableOnTouchOutside);
                dialog.SetCancelable(cancelable);

                if (cancelableOnTouchOutside || cancelable)
                {
                    dialog.CancelEvent += (sender, e) =>
                    {
                        callback?.Invoke(false);
                        _openDialogs.Remove((AlertDialog)sender);
                    };
                }


            });
        }

    }
}