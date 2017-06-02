using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using XfMvvmLight.iOS.PlatformImplementation;
using XfMvvmLight.Abstractions;
using ObjCRuntime;

[assembly: Xamarin.Forms.Dependency(typeof(OsVersionService))]
namespace XfMvvmLight.iOS.PlatformImplementation
{
    public class OsVersionService : IOsVersionService
    {
        public string GetOsVersion
        {
            get
            {
                try
                {
                    return $"iOS {UIDevice.CurrentDevice.SystemVersion} ({UIDevice.CurrentDevice.UserInterfaceIdiom})";

                }
                catch
                {
                    return "This demo supports iOS only for the moment";
                }
            }
        }
    }
}