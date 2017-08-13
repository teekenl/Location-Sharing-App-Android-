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
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;
using System.Threading.Tasks;

namespace Glados.Droid.Services
{
    public class BackPressedDialog : IDialog
    {
        AlertDialog dialog;
        AlertDialog requestDialog2;
        public async Task<bool>ShowDialog()
        {
            bool buttonPressed = false;
            bool chosenOption  = false;
            Application.SynchronizationContext.Post(_ =>
            {
                var mvxTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                Android.App.AlertDialog.Builder alertDialog = new AlertDialog.Builder(mvxTopActivity.Activity);
                alertDialog.SetTitle("Request");
                alertDialog.SetMessage("Confirm send request?");
                alertDialog.SetNegativeButton("No", (s, args) =>
                {
                    chosenOption = false;
                });
                alertDialog.SetPositiveButton("Yes", (s, args) =>
                {
                    chosenOption = true;
                });

                dialog = alertDialog.Create();
                dialog.DismissEvent += (object sender, EventArgs e) =>
                {
                    buttonPressed = true;
                    dialog.Dismiss();
                };
                dialog.Show();
            }, null);
            while (!buttonPressed)
            {
                await Task.Delay(100);
            }
            return chosenOption;
        }
        public async Task<bool> ShowFavoriteConfirmationDialog()
        {
            bool buttonPressed = false;
            bool chosenOption = false;
            Application.SynchronizationContext.Post(_ =>
            {
                var mvxTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                Android.App.AlertDialog.Builder alertDialog = new AlertDialog.Builder(mvxTopActivity.Activity);
                alertDialog.SetTitle("Add Favorite");
                alertDialog.SetMessage("Confirm add to favorite?");
                alertDialog.SetNegativeButton("No", (s, args) =>
                {
                    chosenOption = false;
                });
                alertDialog.SetPositiveButton("Yes", (s, args) =>
                {
                    chosenOption = true;
                });

                dialog = alertDialog.Create();
                dialog.DismissEvent += (object sender, EventArgs e) =>
                {
                    buttonPressed = true;
                    dialog.Dismiss();
                };
                dialog.Show();
            }, null);
            while (!buttonPressed)
            {
                await Task.Delay(100);
            }
            return chosenOption;
        }

        public async Task<bool> ShowCancelResponseConfirmationDialog()
        {
            bool buttonPressed = false;
            bool chosenOption = false;
            Application.SynchronizationContext.Post(_ =>
            {
                var mvxTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                Android.App.AlertDialog.Builder alertDialog = new AlertDialog.Builder(mvxTopActivity.Activity);
                alertDialog.SetTitle("Cancel Request");
                alertDialog.SetMessage("Confirm to not reply?");
                alertDialog.SetNegativeButton("No", (s, args) =>
                {
                    chosenOption = false;
                });
                alertDialog.SetPositiveButton("Yes", (s, args) =>
                {
                    chosenOption = true;
                });

                dialog = alertDialog.Create();
                dialog.DismissEvent += (object sender, EventArgs e) =>
                {
                    buttonPressed = true;
                    dialog.Dismiss();
                };
                dialog.Show();
            }, null);
            while (!buttonPressed)
            {
                await Task.Delay(100);
            }
            return chosenOption;
        }


        public void showConfirmationDialog()
        {
            var inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            View requestView = inflater.Inflate(Resource.Layout.requestDialogLayout, null);
            var mvxTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            AlertDialog.Builder requestDialog = new AlertDialog.Builder(mvxTopActivity.Activity);
         
            requestDialog.SetView(requestView);
            requestDialog2 = requestDialog.Create();
            requestDialog2.Show();
        }

        public void hideConfirmationDialog()
        {
            requestDialog2.Hide();
        }

        public void showErrorConfirmationDialog(string error)
        {

            var mvxTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            Android.App.AlertDialog.Builder alertDialog = new AlertDialog.Builder(mvxTopActivity.Activity);
            alertDialog.SetTitle("Error");
            alertDialog.SetMessage(error);
            alertDialog.SetNegativeButton("OK", (s, args) =>
            {
               
            }).Show();
        
        }

    }
}