using GalaSoft.MvvmLight;
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
    public class XfNavViewModelBase : ViewModelBase
    {
        protected readonly IXfNavigationService _navService;
        protected readonly IViewEventBrokerService _viewEventBroker;

        private string _correspondingViewKey;
        private RelayCommand _viewAppearingCommand;
        private RelayCommand _viewDisappearingCommand;


        public XfNavViewModelBase()
        {
            _navService = SimpleIoc.Default.GetInstance<IXfNavigationService>();
            _viewEventBroker = SimpleIoc.Default.GetInstance<IViewEventBrokerService>();
            _viewEventBroker.ViewAppearing += OnCorrespondingViewAppearing;
            _viewEventBroker.ViewDisAppearing += OnCorrespondingViewDisappearing;
        }


        protected virtual void OnCorrespondingViewAppearing(object sender, ViewEventBrokerEventArgs e)
        {
        }

        protected virtual void OnCorrespondingViewDisappearing(object sender, ViewEventBrokerEventArgs e)
        {
        }






        public RelayCommand ViewAppearingCommand => _viewAppearingCommand ?? (_viewAppearingCommand = new RelayCommand(ExecuteViewAppearingCommand, CanExecuteViewAppearingCommand));

        public virtual void ExecuteViewAppearingCommand()
        {

        }

        public virtual bool CanExecuteViewAppearingCommand()
        {
            return true;
        }





        public RelayCommand ViewDisappearingCommand => _viewDisappearingCommand ?? (_viewDisappearingCommand = new RelayCommand(ExecuteViewDisappearingCommand, CanExecuteViewDisappearingCommand));

        public virtual void ExecuteViewDisappearingCommand()
        {

        }

        public virtual bool CanExecuteViewDisappearingCommand()
        {
            return true;
        }



        public string CorrespondingViewKey
        {
            get => _correspondingViewKey; set => Set(ref _correspondingViewKey, value);
        }

        public bool IsBoundToModalView()
        {
            if (!string.IsNullOrEmpty(CorrespondingViewKey))
            {
                var checkIfIsModal = _navService.StackContainsNavKey(CorrespondingViewKey);

                if (checkIfIsModal.isRegistered)
                {
                    return checkIfIsModal.isModal;
                }
            }
            return false;
        }
        
    }
}
