using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XfMvvmLight.Abstractions
{
    public interface IViewEventBrokerService
    {
        event EventHandler<ViewEventBrokerEventArgs> ViewAppearing;

        event EventHandler<ViewEventBrokerEventArgs> ViewDisAppearing;

        void RaiseViewAppearing(string pageKey, Type pageType, bool isModal);

        void RaiseViewDisAppearing(string pageKey, Type pageType, bool isModal);
    }


}
