using Android.Content;
using Android.Support.V7.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using XfMvvmLight.Droid.Renderer;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(PlatformXfNavPageRenderer))]
namespace XfMvvmLight.Droid.Renderer
{
    public class PlatformXfNavPageRenderer : NavigationPageRenderer, Android.Views.View.IOnClickListener
    {
        private readonly Context _context;
        private Toolbar _toolbar;

        public PlatformXfNavPageRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);

            _toolbar?.SetNavigationOnClickListener(this);
        }

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child is Toolbar toolbar)
                _toolbar = toolbar;

            _toolbar?.SetNavigationOnClickListener(this);
        }

        public new void OnClick(Android.Views.View androidView)
        {
            if (this.Element.Navigation.NavigationStack.Count > 1)
            {
                //return to Xamarin Forms as everything else is handled already there
                ((FormsAppCompatActivity)_context).OnBackPressed();
            }
            else if (this.Element?.Parent is MasterDetailPage masterDetailPage)
            {
                //keep MasterDetailPage menu working
                masterDetailPage.IsPresented = !masterDetailPage.IsPresented;
            }
        }

    }
}