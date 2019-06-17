using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ServiceImplementations
{
    public class XfActionSheetService : IActionSheetService
    {
        //I prefer a clear notation without looking at the signature definition
        //that's why I have three definitions here and insist of passing an array of strings as options

        public async Task<string> ShowActionSheetAsync(string title, string[] buttonTexts)
        {
            return await ShowActionSheetAsync(title, null, null, buttonTexts);
        }

        public async Task<string> ShowActionSheetAsync(string title, string cancelButtonText, string[] buttonTexts)
        {
            return await ShowActionSheetAsync(title, cancelButtonText, null, buttonTexts);
        }

        public async Task<string> ShowActionSheetAsync(string title, string cancelButtonText, string destructiveButtonText, string[] buttonTexts)
        {
            return await App.Current.MainPage.DisplayActionSheet(title, cancelButtonText, destructiveButtonText, buttonTexts);
        }
    }
}
