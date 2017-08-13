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

using Glados.Core.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace Glados.Droid.Services
{
    public class ProgressBar : IProgressBar
    {
        ProgressDialog progress;
        ProgressDialog progress2;
        public void ShowProgressDialog(string message)
        {
            var currentTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            progress = new ProgressDialog(currentTopActivity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Connecting to server. Please Wait...");
            progress.SetCancelable(true);
            progress.Show();

            progress2 = ProgressDialog.Show(currentTopActivity, "Please Wait...", message,true);

        }

        public void HideProgressDialog()
        {

            progress.Hide();
            progress2.Hide();
        }

    }
}