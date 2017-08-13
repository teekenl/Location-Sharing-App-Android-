using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Glados.Core.Interfaces;
using Glados.Core.Models;
using Glados.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Glados.Droid.Views
{
    [Activity(Label = "View for SearchViewModel")]
    public class SearchView : MvxActivity
    {
        private static List<string> names = new List<string>();
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SearchView);

            ObservableCollection<Contact> contactsCollection = new ObservableCollection<Contact>();
            string userFullName = SearchViewModel.user.Split('-')[0];
            string userFirstName = userFullName.Split(',')[0];
            string userLastName = userFullName.Split(',')[1].Trim();

            if (names.Count <= 0)
            {
                Mvx.Resolve<IProgressBar>().ShowProgressDialog("Loading Contacts.");
                var contacts = await Mvx.Resolve<IContactDatabase>().GetContacts();
                Mvx.Resolve<IProgressBar>().HideProgressDialog();
                foreach (var contact in contacts)
                {
                    contactsCollection.Add(contact);
                }
                
                foreach (Contact contact in contactsCollection)
                {
                    if (!contact.FirstName.Equals(userFirstName) && !contact.LastName.Equals(userLastName))
                    {
                        names.Add(contact.FirstName + ", " + contact.LastName + " - " + contact.Department);
                    }
                }
            }
            
            ArrayAdapter autoCompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, names);
            var autocompleteTextView = FindViewById<AutoCompleteTextView>(Resource.Id.AutoCompleteInput);
            autocompleteTextView.Adapter = autoCompleteAdapter;
        }
    }
}