using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

using Android.Content;

using Android.App;
using Android.Media;

[assembly: Xamarin.Forms.Dependency(typeof(BLE.Client.Droid.Version_Android))]
namespace BLE.Client.Droid
{
    public class Version_Android : Activity, BLE.Client.IAppVersion
    {
        public string GetVersion()
        {
            var context = global::Android.App.Application.Context;

            Android.Content.PM.PackageManager manager = context.PackageManager;
            Android.Content.PM.PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public int GetBuild()
        {
            var context = global::Android.App.Application.Context;

            Android.Content.PM.PackageManager manager = context.PackageManager;
            Android.Content.PM.PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionCode;
        }
    }
}