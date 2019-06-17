using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using GalaSoft.MvvmLight.Ioc;

namespace XfMvvmLight.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private IXfNavigationService _navigationService;

        public MainViewModel()
        {
           _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
            _navigationService = SimpleIoc.Default.GetInstance<IXfNavigationService>();
        }



        public string HelloWorldString { get; private set; } = "Hello from Xamarin Forms with MVVM Light";


        private string _osVersionViaDs;
        public string OsVersionViaDs
        {
            get { return _osVersionViaDs; }
            set { Set(ref _osVersionViaDs, value); }
        }


        private RelayCommand _getOSVersionViaDsCommand;

        public RelayCommand GetOsVersionViaDsCommand => _getOSVersionViaDsCommand ?? (_getOSVersionViaDsCommand = new RelayCommand(() =>
        {
            OsVersionViaDs = DependencyService.Get<IOsVersionService>().GetOsVersion;

        }));




        private string _osVersionViaSimpleIoc;
        public string OsVersionViaSimpleIoc
        {
            get { return _osVersionViaSimpleIoc; }
            set { Set(ref _osVersionViaSimpleIoc, value); }
        }


        private RelayCommand _getOSVersionViaSimpleIocCommand;

        public RelayCommand GetOsVersionViaSimpleIocCommand => _getOSVersionViaSimpleIocCommand ?? (_getOSVersionViaSimpleIocCommand = new RelayCommand(() =>
        {
            OsVersionViaSimpleIoc = SimpleIoc.Default.GetInstance<IOsVersionService>().GetOsVersion;

        }));



        private RelayCommand _showModalPageCommand;

        public RelayCommand ShowModalPageCommand => _showModalPageCommand ?? (_showModalPageCommand = new RelayCommand(async () =>
        {
            await _navigationService.ShowModalPageAsync(ViewModelLocator.ModalPageKey, true);
        }));



        private RelayCommand _navigateToPageCommand;

        public RelayCommand NavigateToPageCommand => _navigateToPageCommand ?? (_navigateToPageCommand = new RelayCommand(async () =>
        {
            await _navigationService.NavigateToAsync(ViewModelLocator.NavigatedPageKey, true);
        }));







        private RelayCommand _showMessageCommand;

        public RelayCommand ShowMessageCommand => _showMessageCommand ?? (_showMessageCommand = new RelayCommand(async () =>
        {
            await _dialogService.ShowMessageAsync("Cool... 😎", "You really clicked this button!");
        }));





        private RelayCommand _showErrorWithExceptionCommand;

        public RelayCommand ShowErrorWithExceptionCommand => _showErrorWithExceptionCommand ?? (_showErrorWithExceptionCommand = new RelayCommand(async () =>
        {
            try
            {
                throw new NotSupportedException("You tried to fool me, which is not supported!");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Error", ex, "Sorry",
                    returnValue =>
                    {
                        Debug.WriteLine($"{nameof(ShowErrorWithExceptionCommand)}'s dialog returns: {returnValue}");
                    }, false, false);
            }
        })); 



        private RelayCommand _showSelectionCommand;

        public RelayCommand ShowSelectionCommand => _showSelectionCommand ?? (_showSelectionCommand = new RelayCommand(async () =>
        {

            await _dialogService.ShowMessageAsync("Question:",
                "Do you enjoy this blog series about MVVMLight and Xamarin Forms?", "yeah!", "nope", async returnvalue =>
                {
                    if (returnvalue)
                    {
                        await _dialogService
                            .ShowMessageAsync("Awesome!", "I am glad you like it");
                    }
                    else
                    {
                        await _dialogService
                            .ShowMessageAsync("Oh no...", "Maybe you could send me some feedback on how to improve it?");
                    }
                },
                false, false);

        }));


        private RelayCommand _showCommandChainingDemoPage;

        public RelayCommand ShowCommandChainingDemoPage => _showCommandChainingDemoPage ?? (_showCommandChainingDemoPage = new RelayCommand(async () =>
        {
            await _navigationService.NavigateToAsync(ViewModelLocator.CommandChainingDemoPageKey, true);
        }));


        

        private RelayCommand _showActionSheetDemoPage;

        public RelayCommand ShowActionSheetDemoPage => _showActionSheetDemoPage ?? (_showActionSheetDemoPage = new RelayCommand(async () =>
        {
            await _navigationService.NavigateToAsync(ViewModelLocator.ActionSheetDemoPageKey, true);
        }));
    }
}
