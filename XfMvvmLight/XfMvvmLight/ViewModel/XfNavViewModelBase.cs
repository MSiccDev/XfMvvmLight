using GalaSoft.MvvmLight;
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



        private string _correspondingViewKey;

        public string CorrespondingViewKey
        {
            get { return _correspondingViewKey; }
            set { Set(ref _correspondingViewKey, value);}
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
