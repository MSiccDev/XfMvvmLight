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
    public class XfNavigationPageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //making sure to use this only with non-modal pages
            if (Element is XfNavContentPage page && this.NavigationController != null)
            {
                //disabling back swipe complettely:
                this.NavigationController.InteractivePopGestureRecognizer.Enabled = false;

                var backarrowImg = UIImage.FromBundle("arrow-back.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                var backButton = new UIButton(UIButtonType.Custom)
                {
                    HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
                    TitleEdgeInsets = new UIEdgeInsets(11.5f, 0f, 10f, 0f),
                    //we need to move the image a bit more left to get closer to the OS-look
                    ImageEdgeInsets = new UIEdgeInsets(1f, -8f, 0f, 0f)
                };

                //this makes sure we use the same behavior as the OS
                //if there is no parent, it must throw an exception because something is wrong
                //with the navigation structure
                var parentStackIndex = page.Navigation.NavigationStack.IndexOf(page) - 1;
                var parent = page.Navigation.NavigationStack[parentStackIndex];
                backButton.SetTitle(string.IsNullOrEmpty(parent.Title) ? "Back" : parent.Title, UIControlState.Normal);

                backButton.SetTitleColor(this.View.TintColor, UIControlState.Normal);
                backButton.SetImage(backarrowImg, UIControlState.Normal);
                backButton.SizeToFit();

                backButton.TouchDown += (sender, e) =>
                {
                    if (!page.BlockBackNavigation)
                    {
                        this.NavigationController.PopViewController(animated);
                    }
                    page.SendBackButtonPressed();
                };

                backButton.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width / 4, NavigationController.NavigationBar.Frame.Height);

                var view = new UIView(new CGRect(0, 0, backButton.Frame.Width, backButton.Frame.Height));
                view.AddSubview(backButton);


                var backButtonItem = new UIBarButtonItem(string.Empty, UIBarButtonItemStyle.Plain, null)
                {
                    CustomView = backButton
                };

                NavigationController.TopViewController.NavigationItem.SetLeftBarButtonItem(backButtonItem, animated);
            }
        }
    }
}