using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.UWP.PlatformImplementation
{
    public class OsVersionService : IOsVersionService
    {
        public string GetOsVersion
        {
            get
            {
                var currentOS = AnalyticsInfo.VersionInfo.DeviceFamily;

                var v = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
                var v1 = (v & 0xFFFF000000000000L) >> 48;
                var v2 = (v & 0x0000FFFF00000000L) >> 32;
                var v3 = (v & 0x00000000FFFF0000L) >> 16;
                var v4 = v & 0x000000000000FFFFL;
                var versionNb = $"{v1}.{v2}.{v3}.{v4}";


                return $"{currentOS} {versionNb} ({AnalyticsInfo.DeviceForm})";
            }
        }
    }
}
