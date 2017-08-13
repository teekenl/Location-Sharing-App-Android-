// Author Yew Lren Chong n9552286

using Android.App;
using Android.OS;
using Android.Widget;
using Glados.Core.ViewModels;
using MvvmCross.Droid.Views;
using Glados.Core.Interfaces;
using Glados.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Platform;
using Glados.Droid.Services;
using Gcm.Client;

namespace Glados.Droid.Views
{
    [Activity(Label = "Glados")]
    public class FirstView : MvxTabActivity
    {
        public static int notificationCount = 0;

        protected FirstViewModel FirstViewModel
        {
            get { return base.ViewModel as FirstViewModel; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);

            // Make sure the device support for google play services
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register the app for push notifications.
            GcmClient.Register(this, Receiver.senderIDs);

            TabHost.TabSpec spec;
           
            spec = TabHost.NewTabSpec("search");
            spec.SetIndicator("", Resources.GetDrawable(Resource.Drawable.indicator_search));
            spec.SetContent(this.CreateIntentFor(FirstViewModel.Search));
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("recent");
            spec.SetIndicator("", Resources.GetDrawable(Resource.Drawable.indicator_history));
            spec.SetContent(this.CreateIntentFor(FirstViewModel.Recent));
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("favourite");

            spec.SetIndicator("", Resources.GetDrawable(Resource.Drawable.indicator_favorite));
            spec.SetContent(this.CreateIntentFor(FirstViewModel.Favourite));
            TabHost.AddTab(spec);
        }
    }
}
