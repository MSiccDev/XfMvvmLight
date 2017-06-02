using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using XfMvvmLight.Abstractions;
using Xamarin.Forms;

namespace XfMvvmLight.ViewModel
{
    public class ViewModelLocator
    {
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator());

        public void Initialize()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


            RegisterServices();


            SimpleIoc.Default.Register<MainViewModel>();

        }

        private static void RegisterServices()
        {
            //this one gets the correct service implementation from platform implementation
            var osService = DependencyService.Get<IOsVersionService>();

            // which can be used to register the service class with MVVMLight's Ioc
            SimpleIoc.Default.Register<IOsVersionService>(() => osService);

        }




        #region ViewModels
        public MainViewModel MainVm => ServiceLocator.Current.GetInstance<MainViewModel>();

        #endregion




        #region PageKeys


        #endregion
    }
}
