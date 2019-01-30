using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using XfMvvmLight.iOS.Renderer;
using XfMvvmLight.BaseControls;

[assembly: ExportRenderer(typeof(XfNavContentPage), typeof(XfNavigationPageRenderer))]
namespace XfMvvmLight.iOS.Renderer
{
    public class XfNavigationPageRenderer : PageRenderer, IUIGestureRecognizerDelegate
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //making sure to use this only with non-modal pages
            if (Element is XfNavContentPage page && this.NavigationController != null)
            {
                var thisPageIndex = page.Navigation.NavigationStack.IndexOf(page);
                if (thisPageIndex >= 1)
                {
                    this.NavigationController.TopViewController.NavigationItem.SetLeftBarButtonItem(
                       new UIBarButtonItem(UIImage.FromBundle("arrow-back.png"), UIBarButtonItemStyle.Plain, OnCustomLeftNavigationBarButtonPressed), true);

                    //this is the important one because it makes our click handler work!
                    this.NavigationController.InteractivePopGestureRecognizer.Delegate = this;
                }
            }
        }

        [Export("gestureRecognizerShouldBegin:")]
        public bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            // admit the pop gesture when the page is not root page
            if (recognizer == this.NavigationController.InteractivePopGestureRecognizer)
                return this.NavigationController.ViewControllers.Length > 1;

            return true;
        }

        //handle the custom back button press
        private void OnCustomLeftNavigationBarButtonPressed(object sender, EventArgs e)
        {
            if (this.Element is XfNavContentPage page)
            {
                if (!page.BlockBackNavigation)
                    this.NavigationController.PopViewController(true);
                else
                    page.SendBackButtonPressed();
            }
        }
    }
}