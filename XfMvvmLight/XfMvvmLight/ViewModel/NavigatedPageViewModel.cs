using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XfMvvmLight.ViewModel
{
    public class NavigatedPageViewModel :XfNavViewModelBase
    {



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
            await _navService.GoBackAsync();

        }));



    }
}
