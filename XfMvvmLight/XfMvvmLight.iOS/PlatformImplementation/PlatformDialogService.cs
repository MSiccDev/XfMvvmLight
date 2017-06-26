using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using XfMvvmLight.iOS.PlatformImplementation;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformDialogService))]
namespace XfMvvmLight.iOS.PlatformImplementation
{
    public class PlatformDialogService : IDialogService
    {

        List<UIAlertController> _openDialogs = new List<UIAlertController>();


        public void CloseAllDialogs()
        {
            foreach (var dialog in _openDialogs)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    dialog.DismissViewController(true, null);
                });
            }
            _openDialogs.Clear();

        }

        public async Task ShowErrorAsync(string title, Exception error, string buttonText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await Task.Run(() => { ShowAlert(title, error.ToString(), buttonText, null, closeAction, cancelableOnTouchOutside, cancelable); });
        }



        public async Task ShowMessageAsync(string title, string message)
        {
            await Task.Run(() => { ShowAlert(title, message, "OK", null, null, false, false); });
        }

        public async Task ShowMessageAsync(string title, string message, string buttonText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await Task.Run(() => { ShowAlert(title, message, buttonText, null, closeAction, cancelableOnTouchOutside, cancelable); });
        }

        public async Task ShowMessageAsync(string title, string message, string buttonConfirmText, string buttonCancelText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            await Task.Run(() => { ShowAlert(title, message, buttonConfirmText, buttonCancelText, closeAction, cancelableOnTouchOutside, cancelable); });
        }






        //one method to rule them all

        internal void ShowAlert(string title, string content, string confirmButtonText = null, string cancelButtonText = null, Action<bool> callback = null, bool cancelableOnTouchOutside = false, bool cancelable = false)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var dialogAlert = UIAlertController.Create(title, content, UIAlertControllerStyle.Alert);

            var okAction = UIAlertAction.Create(!string.IsNullOrEmpty(confirmButtonText) ? confirmButtonText : "OK", UIAlertActionStyle.Default, _ =>
            {
                callback?.Invoke(true);
                _openDialogs.Remove(dialogAlert);
            });
            dialogAlert.AddAction(okAction);


            if (!string.IsNullOrEmpty(cancelButtonText))
            {
                var cancelAction = UIAlertAction.Create(cancelButtonText, UIAlertActionStyle.Cancel, _ => {
                    callback?.Invoke(false);
                    _openDialogs.Remove(dialogAlert);
                    });
                dialogAlert.AddAction(cancelAction);
            }
                       


                _openDialogs.Add(dialogAlert);

                var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                rootController.PresentViewController(dialogAlert, true, null);
            });

        }


    }
}
