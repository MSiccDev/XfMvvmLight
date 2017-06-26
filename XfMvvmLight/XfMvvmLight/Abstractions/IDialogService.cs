using System;
using System.Threading.Tasks;

namespace XfMvvmLight.Abstractions
{
    public interface IDialogService
    {
        void CloseAllDialogs();

        Task ShowMessageAsync(string title, string message);

        Task ShowErrorAsync(string title, Exception error, string buttonText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false);

        Task ShowMessageAsync(string title, string message, string buttonText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false);

        Task ShowMessageAsync(string title, string message, string buttonConfirmText, string buttonCancelText, Action<bool> closeAction, bool cancelableOnTouchOutside = false, bool cancelable = false);
    }
}
