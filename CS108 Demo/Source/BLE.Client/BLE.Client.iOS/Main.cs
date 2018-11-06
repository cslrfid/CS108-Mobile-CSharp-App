using UIKit;

using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using CoreGraphics;

namespace BLE.Client.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
	        // if you want to use a different Application Delegate class from "AppDelegate"
	        // you can specify it here.
	        UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
