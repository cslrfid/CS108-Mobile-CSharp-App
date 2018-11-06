using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;

namespace BLE.Client.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate
    {
        UIWindow _window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            /*
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (
                object sender, 
                System.Security.Cryptography.X509Certificates.X509Certificate pCertificate, 
                System.Security.Cryptography.X509Certificates.X509Chain pChain, 
                System.Net.Security.SslPolicyErrors pSSLPolicyErrors) {
                    return true;
                };
            */

/*
            System.Net.ServicePointManager
    .ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;
*/

//            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
//            {
                //Toast.MakeText(this, "Certificate ", ToastLength.Short).Show();

//                System.Diagnostics.Debug.WriteLine(cert.GetSerialNumberString());
//                System.Diagnostics.Debug.WriteLine(cert.Issuer);
//                System.Diagnostics.Debug.WriteLine(cert.Subject);
//                return true;
//            };

            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, _window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            _window.MakeKeyAndVisible();

            return true;
        }
    }
}
