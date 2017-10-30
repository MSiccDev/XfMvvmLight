using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ViewModel
{
    public abstract class XfNavViewModelBase : ViewModelBase
    {
        protected readonly IXfNavigationService NavService;
        protected readonly IViewEventBrokerService ViewEventBroker;


        private string _correspondingViewKey;
        private RelayCommand _viewAppearingCommand;
        private RelayCommand _viewDisappearingCommand;
        private bool _blockBackNavigation;
        private RelayCommand _backButtonPressCanceledCommand;
        private RelayCommand _backButtonPressedCommand;


        public XfNavViewModelBase()
        {
            NavService = SimpleIoc.Default.GetInstance<IXfNavigationService>();
            ViewEventBroker = SimpleIoc.Default.GetInstance<IViewEventBrokerService>();
            ViewEventBroker.ViewAppearing += OnCorrespondingViewAppearing;
            ViewEventBroker.ViewDisAppearing += OnCorrespondingViewDisappearing;

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
                var checkIfIsModal = NavService.StackContainsNavKey(CorrespondingViewKey);

                if (checkIfIsModal.isRegistered)
                {
                    return checkIfIsModal.isModal;
                }
            }
            return false;
        }



        //this one allows us to block back navigation in view as well as in ViewModel
        public virtual bool BlockBackNavigation
        {
            get => _blockBackNavigation;

            set => Set(ref _blockBackNavigation, value);
        }



        public RelayCommand BackButtonPressCanceledCommand =>
            _backButtonPressCanceledCommand ?? (_backButtonPressCanceledCommand = new RelayCommand(ExecuteBackButtonPressCanceledCommand, CanExecuteBackButtonPressCanceledCommand));

        public virtual void ExecuteBackButtonPressCanceledCommand()
        {
            
        }

        public virtual bool CanExecuteBackButtonPressCanceledCommand()
        {
            return true;
        }




        public RelayCommand BackButtonPressedCommand =>
            _backButtonPressedCommand ?? (_backButtonPressedCommand = new RelayCommand(ExecuteBackButtonPressedCommand, CanExecuteBackButtonPressedCommand));

        public virtual void ExecuteBackButtonPressedCommand()
        {

        }

        public virtual bool CanExecuteBackButtonPressedCommand()
        {
            return true;
        }


        
    }
}
