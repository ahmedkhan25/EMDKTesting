using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Symbol.XamarinEMDK;
using Symbol.XamarinEMDK.Barcode;

namespace TestingZebraEMDK.Droid
{
    [Activity(Label = "TestingZebraEMDK", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, EMDKManager.IEMDKListener
    {
       

        #region Ring Barcode Scanner Variables
        // Declare a variable to store EMDKManager object
        private EMDKManager emdkManager = null;

        // Declare a variable to store BarcodeManager object
        private BarcodeManager barcodeManager = null;

        // Declare a variable to store Scanner object
        private Scanner scanner = null;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

           

            base.OnCreate(savedInstanceState);

            //Ring Scanner related
            EMDKResults results = EMDKManager.GetEMDKManager(Android.App.Application.Context, this);

  
         

            RequestedOrientation = ScreenOrientation.Portrait;
        }

  

 

        protected override void OnResume()
        {
            base.OnResume();

      

            #region Ring Scanner On Resume
            if (emdkManager != null)
            {
                try
                {
                    barcodeManager = (BarcodeManager)emdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);


                }
                catch (Exception e)
                {
                    //TODO:   log here
                    Console.WriteLine("Exception: " + e.StackTrace);
                }
                #endregion
            }
        }
        protected override void OnPause()
        {
          
            base.OnPause();

            #region Ring Scanner On Pause
            // De-initialize scanner
            DeInitScanner();

            if (barcodeManager != null)
            {
                barcodeManager = null;
            }

            // Release the barcode manager resources
            if (emdkManager != null)
            {
                emdkManager.Release(EMDKManager.FEATURE_TYPE.Barcode);
            }

            #endregion
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            #region Ring Scanner On Destroy
            // De-initialize scanner
            DeInitScanner();
            // Clean up the objects created by EMDK manager
            if (barcodeManager != null)
            {
                // Remove connection listener

                barcodeManager = null;
            }

            if (emdkManager != null)
            {
                emdkManager.Release();
                emdkManager = null;
            }
            #endregion
        }

       


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
              base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

 

   

        #region RingScanner Specific Code
        public void OnOpened(EMDKManager emdkManagerInstance)
        {

            this.emdkManager = emdkManagerInstance;

            try
            {
                // Acquire the barcode manager resources
                barcodeManager = (BarcodeManager)emdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);

                InitScanner();


            }
            catch (Exception e)
            {
                //TODO Log here 
                Console.WriteLine("Exception:" + e.StackTrace);
            }
        }

        public void OnClosed()
        {
            // This callback will be issued when the EMDK closes unexpectedly.

            #region Ring Scanner On Closed
            if (emdkManager != null)
            {
                if (barcodeManager != null)
                {

                    barcodeManager = null;
                }

                // Release all the resources
                emdkManager.Release();
                emdkManager = null;
            }
            #endregion
        }
        void InitScanner()
        {
            if (emdkManager != null)
            {

                if (barcodeManager == null)
                {
                    try
                    {

                        //Get the feature object such as BarcodeManager object for accessing the feature.
                        barcodeManager = (BarcodeManager)emdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);

                        scanner = barcodeManager.GetDevice(BarcodeManager.DeviceIdentifier.Default);

                        if (scanner != null)
                        {





                            //EMDK: Configure the scanner settings
                            /*ScannerConfig config = scanner.GetConfig();
                            config.SkipOnUnsupported = ScannerConfig.SkipOnUnSupported.None;
                            config.ScanParams.DecodeLEDFeedback = true;
                            config.ReaderParams.ReaderSpecific.ImagerSpecific.PickList = ScannerConfig.PickList.Enabled;
                            config.DecoderParams.Code39.Enabled = true;
                            config.DecoderParams.Code128.Enabled = false;
                            scanner.SetConfig(config); */
                            InterfaceConfig t = scanner.GetInterfaceConfig();
                            t.DisplayBluetoothAddressBarcode = true;
                        }
                        else
                        {
                            //TODO log status here
                        }
                    }
                    catch (ScannerException e)
                    {
                        //TODO log error here
                    }
                    catch (Exception ex)
                    {
                        //TODO log error here
                    }
                }
            }




        }

        private void DeInitScanner()
        {
            if (scanner != null)
            {



                try
                {
                    // Release the scanner
                    scanner.Release();
                }
                catch (ScannerException e)
                {
                    //TODO log here
                    Console.WriteLine(e.StackTrace);
                }

                scanner = null;
            }
        }

        #endregion
    }
 
}