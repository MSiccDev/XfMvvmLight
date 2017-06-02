using System;
using Android.OS;
using XfMvvmLight.Abstractions;
using XfMvvmLight.Droid.PlatformImplementation;

[assembly: Xamarin.Forms.Dependency(typeof(OsVersionService))]
namespace XfMvvmLight.Droid.PlatformImplementation
{
    public class OsVersionService : IOsVersionService
    {
        public string GetOsVersion
        {
            get
            {
                var versionNb = Build.VERSION.Release;
                var codename = Build.VERSION.Codename;

                return $"Android {versionNb} ({codename})";
            }
        }
    }
}