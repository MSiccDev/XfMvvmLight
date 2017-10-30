using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ViewModel
{
    public class NavigatedPageViewModel :XfNavViewModelBase
    {
        private readonly IDialogService _dialogService;

        public NavigatedPageViewModel()
        {
            _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
            CorrespondingViewKey = ViewModelLocator.NavigatedPageKey;
        }


        protected override void OnCorrespondingViewAppearing(object sender, ViewEventBrokerEventArgs e)
        {
            base.OnCorrespondingViewAppearing(sender, e);
        }

        protected override void OnCorrespondingViewDisappearing(object sender, ViewEventBrokerEventArgs e)
        {
            base.OnCorrespondingViewDisappearing(sender, e);
        }



        private RelayCommand _goBackCommand;

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(async () =>
        {
            if (!BlockBackNavigation)
            {
                await NavService.GoBackAsync();
            }
        }));


        public override async void ExecuteBackButtonPressedCommand()
        {
            await _dialogService.ShowMessageAsync($"Hit {nameof(ExecuteBackButtonPressedCommand)}",
                $"Back button was pressed, {nameof(BlockBackNavigation)} is set to {BlockBackNavigation}");
            base.ExecuteBackButtonPressedCommand();
        }

        public override async void ExecuteBackButtonPressCanceledCommand()
        {
            await _dialogService.ShowMessageAsync($"Hit {nameof(ExecuteBackButtonPressCanceledCommand)}",
                $"Back button was pressed, {nameof(BlockBackNavigation)} is set to {BlockBackNavigation}");

            base.ExecuteBackButtonPressCanceledCommand();
        }
    }
}
