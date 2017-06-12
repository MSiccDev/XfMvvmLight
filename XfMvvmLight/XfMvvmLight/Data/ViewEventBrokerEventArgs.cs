using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight
{
    public class ViewEventBrokerEventArgs : EventArgs
    {

        public ViewEventBrokerEventArgs(string pageKey, Type pageType, bool isModal)
        {
            PageKey = pageKey;
            PageType = pageType;
            IsModal = isModal;
        }



        public string PageKey { get; private set; }


        public Type PageType { get; private set; }


        public bool IsModal { get; private set; }
    }
}
