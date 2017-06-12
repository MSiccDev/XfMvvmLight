using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using XfMvvmLight.ViewModel;

namespace XfMvvmLight.BaseControls
{
    public class XfNavContentPage : ContentPage
    {
        private IViewEventBrokerService _viewEventBroker;
        private IXfNavigationService _navService;

        public XfNavContentPage()
        {
            _viewEventBroker = SimpleIoc.Default.GetInstance<IViewEventBrokerService>();
            _navService = SimpleIoc.Default.GetInstance<IXfNavigationService>();

        }


        BindableProperty RegisteredPageKeyProperty = BindableProperty.Create("RegisteredPageKey", typeof(string), typeof(XfNavContentPage), default(string), BindingMode.Default, propertyChanged: OnRegisteredPageKeyChanged);

        private static void OnRegisteredPageKeyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //todo, but not needed atm.
        }

        public string RegisteredPageKey
        {
            get { return (string)GetValue(RegisteredPageKeyProperty); }
            set { SetValue(RegisteredPageKeyProperty, value); }
        }


        public (bool isRegistered, bool isModal) StackState { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(RegisteredPageKey))
            {
                StackState = _navService.StackContainsNavKey(RegisteredPageKey);

                if (StackState.isRegistered)
                {
                    _viewEventBroker?.RaiseViewAppearing(RegisteredPageKey, GetType(), StackState.isModal);
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (!string.IsNullOrEmpty(RegisteredPageKey))
            {
                if (StackState.isRegistered)
                {
                    _viewEventBroker?.RaiseViewDisAppearing(RegisteredPageKey, GetType(), StackState.isModal);
                }
            }
        }


    }
}