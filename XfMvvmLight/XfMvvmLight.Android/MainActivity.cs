using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Application = Android.App.Application;
using XfMvvmLight.ViewModel;

namespace XfMvvmLight.Droid
{
    [Activity(Label = "XfMvvmLight", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ViewModelLocator.Instance.Initialize();

            LoadApplication(new App());
        }



        //without this, OnOptionsItemSelected will never get triggered!
        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            var toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolBar);

            base.OnPostCreate(savedInstanceState);
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //if we are not hitting the internal "home" button, just return without any action
            if (item.ItemId != Android.Resource.Id.Home)
                return base.OnOptionsItemSelected(item);

            //this one triggers the hardware back button press handler - so we are back in XF without even mentioning it
            this.OnBackPressed();
            // return true to signal we have handled everything fine
            return true;
        }
    }
}

