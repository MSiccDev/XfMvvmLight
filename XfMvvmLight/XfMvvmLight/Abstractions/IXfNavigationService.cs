using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XfMvvmLight.Abstractions
{
    public interface IXfNavigationService
    {
        void Initialize(NavigationPage navigation);

        void Configure(string pageKey, Type pageType);




        Task GoHomeAsync();

        Task GoBackAsync();

        Task GoBackModalAsync();




        int ModalStackCount { get; }
        string CurrentModalPageKey { get; }

        Task ShowModalPageAsync(string pageKey, bool animated = true);

        Task ShowModalPageAsync(string pageKey, object parameter, bool animated = true);





        string CurrentPageKey { get; }


        Task NavigateToAsync(string pageKey, bool animated = true);
        Task NavigateToAsync(string pageKey, object parameter, bool animated = true);


    }
}
