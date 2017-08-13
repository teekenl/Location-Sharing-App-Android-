using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using MvvmCross.Droid.Views;
using Glados.Core.ViewModels;
using Glados.Core.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Collections.ObjectModel;
using Glados.Core.Models;

namespace Glados.Droid.Views
{
    [Activity(Label = "View for ResponseViewModel")]
    public class ResponseView : MvxActivity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            string respondentUser = RespondentViewModel.user;
            var contact = await Mvx.Resolve<IContactDatabase>().GetUser(respondentUser);
            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
            foreach(var acontact in contact)
            {
                contacts.Add(acontact);
            }
            string responsePhoneNumber = contacts[0].PhoneNumber.ToString();

            SetContentView(Resource.Layout.ResponseView);

            FrameLayout callFrame = FindViewById<FrameLayout>(Resource.Id.callFrame);
            FrameLayout sendMessageFrame = FindViewById<FrameLayout>(Resource.Id.sendMessageFrame);
            FrameLayout naviBackFrame = FindViewById<FrameLayout>(Resource.Id.naviBackFrame);

            naviBackFrame.Click += delegate
            {
                var request = new MvxViewModelRequest();
                request.ParameterValues = new System.Collections.Generic.Dictionary<string, string>();
                request.ViewModelType = typeof(FirstViewModel);
                var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

                //Create an intent to show UI
                var uiIntent = requestTranslator.GetIntentFor(request);
                StartActivity(uiIntent);
            };

            

            callFrame.Click += delegate
            {
                var uri = Android.Net.Uri.Parse("tel:0" + responsePhoneNumber);
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            };

         
            sendMessageFrame.Click += delegate
            {
                var smsUri = Android.Net.Uri.Parse("smsto:0"+ responsePhoneNumber);
                var smsIntent = new Intent(Intent.ActionSendto, smsUri);
                smsIntent.PutExtra("sms_body", "Please input your message");
                StartActivity(smsIntent);
            };

        }
    }
}