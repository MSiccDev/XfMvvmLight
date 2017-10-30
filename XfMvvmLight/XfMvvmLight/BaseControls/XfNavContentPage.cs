using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using XfMvvmLight.Behaviors;
using XfMvvmLight.ViewModel;

namespace XfMvvmLight.BaseControls
{
    public class XfNavContentPage : ContentPage
    {
        private readonly IViewEventBrokerService _viewEventBroker;
        private readonly IXfNavigationService _navService;

        public event EventHandler BackButtonPressCanceled;
        public event EventHandler BackButtonPressed;

        public XfNavContentPage()
        {
            _viewEventBroker = SimpleIoc.Default.GetInstance<IViewEventBrokerService>();
            _navService = SimpleIoc.Default.GetInstance<IXfNavigationService>();

            this.BindingContextChanged += XfNavContentPage_BindingContextChanged;

            //hide the back button on Android and iOS:
            //NavigationPage.SetHasBackButton(this, false);

        }

        private void XfNavContentPage_BindingContextChanged(object sender, EventArgs e)
        {
            if (this.BindingContext is XfNavViewModelBase @base)
            {
                this.Behaviors.Add(new EventToCommandBehavior()
                {
                    EventName = "Appearing",
                    Command = @base.ViewAppearingCommand
                });

                this.Behaviors.Add(new EventToCommandBehavior()
                {
                    EventName = "Disappearing",
                    Command = @base.ViewDisappearingCommand
                });

                this.Behaviors.Add(new EventToCommandBehavior()
                {
                    EventName = "BackButtonPressed",
                    Command = @base.BackButtonPressedCommand
                });

                this.Behaviors.Add(new EventToCommandBehavior()
                {
                    EventName = "BackButtonPressCanceled",
                    Command = @base.BackButtonPressCanceledCommand
                });
            }
        }

        public static BindableProperty RegisteredPageKeyProperty = BindableProperty.Create("RegisteredPageKey", typeof(string), typeof(XfNavContentPage), default(string), BindingMode.Default, propertyChanged: OnRegisteredPageKeyChanged);

        private static void OnRegisteredPageKeyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //todo, but not needed atm.
        }

        public string RegisteredPageKey
        {
            get => (string)GetValue(RegisteredPageKeyProperty);
            set => SetValue(RegisteredPageKeyProperty, value);
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





        public static BindableProperty BlockBackNavigationProperty = BindableProperty.Create("BlockBackNavigation", typeof(bool), typeof(XfNavContentPage), default(bool), BindingMode.Default, propertyChanged: OnBlockBackNavigationChanged);

        private static void OnBlockBackNavigationChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            //not used in this sample
            //valid scneario would be some kind of validation or similar tasks
        }

        public bool BlockBackNavigation
        {
            get => (bool) GetValue(BlockBackNavigationProperty);
            set => SetValue(BlockBackNavigationProperty, value);
        }


        protected override bool OnBackButtonPressed()
        {
            if (this.BlockBackNavigation)
            {
                BackButtonPressCanceled?.Invoke(this, EventArgs.Empty);
                return true;
            }

            base.OnBackButtonPressed();
            BackButtonPressed?.Invoke(this, EventArgs.Empty);

            if (this.StackState.isModal)
                return true;
            else
            {
                return false;
            }
        }


    }
}