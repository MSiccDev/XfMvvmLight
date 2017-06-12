using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ServiceImplementations
{
    public class ViewEventBrokerService : IViewEventBrokerService
    {
        public event EventHandler<ViewEventBrokerEventArgs> ViewAppearing;
        public event EventHandler<ViewEventBrokerEventArgs> ViewDisAppearing;


        public void RaiseViewAppearing(string pageKey, Type pageType, bool isModal)
        {
            ViewAppearing?.Invoke(this, new ViewEventBrokerEventArgs(pageKey, pageType, isModal));
        }

        public void RaiseViewDisAppearing(string pageKey, Type pageType, bool isModal)
        {
            ViewDisAppearing?.Invoke(this, new ViewEventBrokerEventArgs(pageKey, pageType, isModal));
        }
    }

}
