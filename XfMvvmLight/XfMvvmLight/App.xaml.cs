using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            var rootNavigation = new NavigationPage(new View.MainPage());

            MainPage = rootNavigation;

            SimpleIoc.Default.GetInstance<IXfNavigationService>().Initialize(rootNavigation);
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
