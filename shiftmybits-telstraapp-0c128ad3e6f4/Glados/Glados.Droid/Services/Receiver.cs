using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glados.Droid.Views;
using Glados.Core.ViewModels;
using Glados.Droid.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using MvvmCross.Core.ViewModels;
using Glados.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Droid.Platform;
using Glados.Core.Interfaces;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Glados.Core.Models;

[assembly: Permission(Name = "glados.Droid.Glados.Droid.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "glados.Droid.Glados.Droid.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace Glados.Droid.Services
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE },
        Categories = new string[] { "glados.Droid.Glados.Droid" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
        Categories = new string[] { "glados.Droid.Glados.Droid" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
    Categories = new string[] { "glados.Droid.Glados.Droid" })]
    public class Receiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        // Set the Google sender ID, replace with the Google project number.
        public static string[] senderIDs = new string[] { "943926216936" };
       
    }

    // The ServiceAttribute must be applied to the class.
    [Service]
    public class PushHandlerService : GcmServiceBase
    {
        public MobileServiceClient client;
        public static string RegistrationID { get; private set; }

        public static string user = "Tee Ken, Lau";
        public static int randomNumber= 0;
        public int count = 0;

        public PushHandlerService() : base(Receiver.senderIDs) { }
        
        protected override async void OnRegistered(Context context, string registrationId)
        {
            System.Diagnostics.Debug.WriteLine("The device has been registered with GCM.", "Success!");
            String applicationUrl = "http://gladosnotification.azurewebsites.net";
            // Get the MobileServiceClient from the current activity instance.
            client = new MobileServiceClient(applicationUrl);
            var push = client.GetPush();

            // Define a message body for GCM.
            var templateBodyGCM = "{\"data\":{\"message\":\"$(messageParam)\" ,\"sender\":\"$(senderParam)\" ,\"receiver\":\"$(receiveParam)\",\"messagetype\":\"$(messageType)\" }}";

            // Define the template registration as JSON.
            JObject templates = new JObject();
            templates["genericMessage"] = new JObject
            {
              {"body", templateBodyGCM }
            };

            try
            {
                // Make sure we run the registration on the same thread as the activity, 
                // to avoid threading errors.
                var currentTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
                currentTopActivity.RunOnUiThread(

                // Register the template with Notification Hubs.
                async () => await push.RegisterAsync(registrationId, templates));
                
                //// Register for GCM notifications.
                //async () => await push.RegisterAsync(registrationId,templates));

                System.Diagnostics.Debug.WriteLine(
                    string.Format("Push Installation Id", push.InstallationId.ToString()));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    string.Format("Error with Azure push registration: {0}", ex.Message));
            }
            try
            {
                // Define two new tags as a JSON array.
                var body = new JArray();
                body.Add("broadcast");
                body.Add("test");

                // Call the custom API '/api/updatetags/<installationid>' 
                // with the JArray of tags.
                var response = await client
                    .InvokeApiAsync("updatetags/"
                    + client.InstallationId, body);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    string.Format("Error with Azure push registration: {0}", ex.Message));
            }

        }

        protected override void OnMessage(Context context, Intent intent)
        {
            
            string message = string.Empty;
            string messageType= intent.Extras.Get("messagetype").ToString();
            string sender = intent.Extras.Get("sender").ToString();
            string receiver = intent.Extras.Get("receiver").ToString();

            // Extract the push notification message from the intent.
            if (intent.Extras.ContainsKey("message") && SearchViewModel.user.Equals(receiver)) //&& !receiver.Equals(sender))
            {
                FirstView.notificationCount++;
                if (messageType.Equals("request"))
                {

                    message = intent.Extras.Get("message").ToString();
                    // Title of notification
                    var title = "Glados Request";
                    // Create a notification manager to send the notification.
                    var notificationManager =
                        GetSystemService(Context.NotificationService) as NotificationManager;

                    var request = new MvxViewModelRequest();
                    request.ParameterValues = new System.Collections.Generic.Dictionary<string, string>();
                    request.ViewModelType = typeof(RespondentViewModel);
                    var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

                    //Create an intent to show UI
                    var uiIntent = requestTranslator.GetIntentFor(request);

                    RespondentViewModel.respondentMessage = sender;
                    RespondentViewModel.user = sender;
                    RespondentViewModel.selectedName = receiver;

                    //randomNumber++;         
                    // Create a new intent to show the notification in the UI. 
                    PendingIntent contentIntent =
                        PendingIntent.GetActivity(context, FirstView.notificationCount, uiIntent, 0);

                    // Create the notification using the builder.
                    var builder = new Notification.Builder(context);
                    builder.SetAutoCancel(true);
                    builder.SetContentTitle(title);
                    builder.SetContentText(message);
                    builder.SetSmallIcon(Resource.Drawable.Icon);
                    builder.SetContentIntent(contentIntent);
                    var notification = builder.Build();

                    // Display the notification in the Notifications Area.
                    notificationManager.Notify(FirstView.notificationCount, notification);
                   
                }
                else
                {

                    message = intent.Extras.Get("message").ToString();
                    var title = "Glados Response";

                    // Create a notification manager to send the notification.
                    var notificationManager =
                        GetSystemService(Context.NotificationService) as NotificationManager;

                    var request = new MvxViewModelRequest();
                    request.ParameterValues = new System.Collections.Generic.Dictionary<string, string>();
                    request.ViewModelType = typeof(ResponseViewModel);
                    var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

                    //Create an intent to show UI
                    var uiIntent = requestTranslator.GetIntentFor(request);

                    ResponseViewModel.responseSender = sender;
                    ResponseViewModel.responseMessage = message;

                    // Create a new intent to show the notification in the UI. 
                    PendingIntent contentIntent =
                        PendingIntent.GetActivity(context, FirstView.notificationCount, uiIntent, 0);

                    // Create the notification using the builder.
                    var builder = new Notification.Builder(context);
                    builder.SetAutoCancel(true);
                    builder.SetContentTitle(title);
                    builder.SetContentText(message);
                    builder.SetSmallIcon(Resource.Drawable.Icon);
                    builder.SetContentIntent(contentIntent);
                    var notification = builder.Build();

                    // Display the notification in the Notifications Area.
                    notificationManager.Notify(FirstView.notificationCount, notification);

                }

            } 
        }
        protected override void OnUnRegistered(Context context, string registrationId)
        {
            throw new NotImplementedException();
        }

        protected override void OnError(Context context, string errorId)
        {
            System.Diagnostics.Debug.WriteLine(
                string.Format("Error occurred in the notification: {0}.", errorId));
        }

    }
}
