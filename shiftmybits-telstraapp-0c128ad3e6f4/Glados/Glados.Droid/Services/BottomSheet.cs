using Android.App;
using Android.Widget;
using Android.Support.Design.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Glados.Core.Interfaces;
using Glados.Core.Models;
using Glados.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.ViewModels;
using System.Collections.ObjectModel;

namespace Glados.Droid.Services
{
    class BottomSheet : IBottomSheet
    {
        private string userID = "a650244a-cfff-4801-b1c3-3bd3ec7e0b38";
        public void Show(string respondentId, string fullname)
        {
            var currentTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            BottomSheetDialog dialog = new BottomSheetDialog(currentTopActivity.Activity);
            dialog.SetContentView(Resource.Layout.menu_bottom_sheet);

            //FrameLayout history = dialog.FindViewById<FrameLayout>(Resource.Id.historyFrame);
            FrameLayout favorite = dialog.FindViewById<FrameLayout>(Resource.Id.favoriteFrame);
            FrameLayout location = dialog.FindViewById<FrameLayout>(Resource.Id.locationFrame);
            favorite.Click += async delegate
            {
                dialog.Dismiss();
                bool successfullFavorite = await Mvx.Resolve<IDialog>().ShowFavoriteConfirmationDialog();
                if (successfullFavorite)
                {
                    InsertFavouriteContact(respondentId);
                }
            };
            location.Click += async delegate
            {
                dialog.Dismiss();
                bool successfullRequest = await Mvx.Resolve<IDialog>().ShowDialog();
                if (successfullRequest)
                {
                    Mvx.Resolve<IDialog>().showConfirmationDialog();
                    NotificationItem notify = new NotificationItem() { SenderId = SearchViewModel.user, SenderMessage = "wants to know your status", RespondentId = fullname, MessageType = "request" };
                    await Mvx.Resolve<INotification>().InsertTodo(notify);
                    InsertRecentContact(respondentId);
                    Mvx.Resolve<IDialog>().hideConfirmationDialog();

                }
            };
            //history.Click += delegate { Toast.MakeText(Application.Context, "history", ToastLength.Short).Show(); };

            dialog.Show();
        }

        private async void InsertFavouriteContact(string respondentId)
        {
            Contact newContact = new Contact();
            var contact = await Mvx.Resolve<IContactDatabase>().GetContact(respondentId);
            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
            foreach (Contact aContact in contact)
            {
                contacts.Add(aContact);
            }
            ObservableCollection<FavouriteContact> favContacts = new ObservableCollection<FavouriteContact>();
            var favouriteContacts = await Mvx.Resolve<IContactDatabase>().GetFavouriteContacts(SearchViewModel.user);

            foreach (var favouriteContact in favouriteContacts)
            {
                favContacts.Add(favouriteContact);
            }
            bool favContactExist = false;
            foreach (FavouriteContact favouriteContact in favContacts)
            {
                if (favouriteContact.Email.Equals(contacts[0].Email))
                {
                    favContactExist = true;
                }
            }
            if (favContactExist)
            {
                Toast.MakeText(Application.Context, "Contact exist in Favourites", ToastLength.Short).Show();
            }
            else
            {
                await Mvx.Resolve<IContactDatabase>().InsertFavouriteContact(SearchViewModel.user, contacts[0]);
                Toast.MakeText(Application.Context, "Contact added to Favourites", ToastLength.Short).Show();
            }
        }

        public async void InsertRecentContact(string respondentId)
        {
            // for some reason I can't call the this.GetContacts() which would incur error.
            Contact newContact = new Contact();
            var contact = await Mvx.Resolve<IContactDatabase>().GetContact(respondentId);
            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

            foreach (Contact aContact in contact)
            {
                contacts.Add(aContact);
            }
            contact = await Mvx.Resolve<IContactDatabase>().GetUser(SearchViewModel.user);

            foreach (Contact aContact in contact)
            {
                contacts.Add(aContact);
            }

            if (respondentId != null)
            {
                await Mvx.Resolve<IContactDatabase>().InsertRecentContact(contacts[1], contacts[0]);
            }
        }
    }
}