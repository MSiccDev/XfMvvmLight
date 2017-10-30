using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ViewModel
{
    public class ModalPageViewModel : XfNavViewModelBase
    {

        private IDialogService _dialogService;

        public ModalPageViewModel()
        {
            CorrespondingViewKey = ViewModelLocator.ModalPageKey;
            _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        }

        //public override async void ExecuteViewAppearingCommand()
        //{
        //    base.ExecuteViewAppearingCommand();
        //    await _dialogService.ShowMessageAsync(this.CorrespondingViewKey, $"from overriden {nameof(ExecuteViewAppearingCommand)}");
        //}

        //public override async void ExecuteViewDisappearingCommand()
        //{
        //    base.ExecuteViewDisappearingCommand();
        //    await _dialogService.ShowMessageAsync(this.CorrespondingViewKey, $"from overriden {nameof(ExecuteViewDisappearingCommand)}");
        //}



        private RelayCommand _goBackCommand;

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(async () =>
        {
            await NavService.GoBackModalAsync();
        }));


        

    }

}
