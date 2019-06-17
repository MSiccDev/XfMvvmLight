using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XfMvvmLight.Abstractions
{
    public interface IActionSheetService
    {
        Task<string> ShowActionSheetAsync(string title, params string[] buttons);

        Task<string> ShowActionSheetAsync(string title, string cancel, params string[] buttons);

        Task<string> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons);
    }
}
