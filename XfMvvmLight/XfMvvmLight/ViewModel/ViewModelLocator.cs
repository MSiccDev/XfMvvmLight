using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using XfMvvmLight.Abstractions;
using Xamarin.Forms;
using XfMvvmLight.View;
using XfMvvmLight.ServiceImplementations;

namespace XfMvvmLight.ViewModel
{
    public class ViewModelLocator
    {
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator());

        public void Initialize()
        {
            RegisterServices();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ModalPageViewModel>();
            SimpleIoc.Default.Register<NavigatedPageViewModel>();
            SimpleIoc.Default.Register<CommandChainingDemoViewModel>();

            var actionSheetService = SimpleIoc.Default.GetInstance<IActionSheetService>();
            SimpleIoc.Default.Register(() => new ActionSheetDemoViewModel(actionSheetService));
        }

        private static void RegisterServices()
        {
            if (!SimpleIoc.Default.IsRegistered<IOsVersionService>())
            {
                //this one gets the correct service implementation from platform implementation
                var osService = DependencyService.Get<IOsVersionService>();

                // which can be used to register the service class with MVVMLight's Ioc
                SimpleIoc.Default.Register<IOsVersionService>(() => osService);
            }

            if (!SimpleIoc.Default.IsRegistered<IDialogService>())
            {
                //aaaand... we did it again ;-)
                var dialogService = DependencyService.Get<IDialogService>();
                SimpleIoc.Default.Register<IDialogService>(() => dialogService);
            }

            if (!SimpleIoc.Default.IsRegistered<IXfNavigationService>())
            {
                SimpleIoc.Default.Register<IXfNavigationService>(GetPageInstances);
            }

            if (!SimpleIoc.Default.IsRegistered<IViewEventBrokerService>())
            {
                SimpleIoc.Default.Register<IViewEventBrokerService, ViewEventBrokerService>();
            }

            if (!SimpleIoc.Default.IsRegistered<IActionSheetService>())
            {
                SimpleIoc.Default.Register<IActionSheetService, XfActionSheetService>();
            }
        }


        public static XfNavigationService GetPageInstances()
        {
            var nav = new XfNavigationService();

            nav.Configure(ModalPageKey, typeof(ModalPage));
            nav.Configure(NavigatedPageKey, typeof(NavigatedPage));
            nav.Configure(CommandChainingDemoPageKey, typeof(CommandChainingDemoPage));
            nav.Configure(ActionSheetDemoPageKey, typeof(ActionSheetDemoPage));

            return nav;
        }




        #region ViewModels
        public MainViewModel MainVm => SimpleIoc.Default.GetInstance<MainViewModel>();
        public ModalPageViewModel ModalPageVm => SimpleIoc.Default.GetInstance<ModalPageViewModel>();

        public NavigatedPageViewModel NavigatedPageVm => SimpleIoc.Default.GetInstance<NavigatedPageViewModel>();

        public CommandChainingDemoViewModel CommandChainingDemoVm => SimpleIoc.Default.GetInstance<CommandChainingDemoViewModel>();

        public ActionSheetDemoViewModel ActionSheetDemoVm => SimpleIoc.Default.GetInstance<ActionSheetDemoViewModel>();

        #endregion




        #region PageKeys

        public static string ModalPageKey => nameof(ModalPage);

        public static string NavigatedPageKey => nameof(NavigatedPage);

        public static string CommandChainingDemoPageKey => nameof(CommandChainingDemoPage);

        public static string ActionSheetDemoPageKey = nameof(ActionSheetDemoPage);
        #endregion
    }
}
