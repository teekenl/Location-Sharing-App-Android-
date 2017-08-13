using Glados.Core.Interfaces;
using Glados.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestDemo.Core.Models;

namespace Glados.Core.ViewModels
{
    public class FavouriteViewModel
    : MvxViewModel
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
        private ObservableCollection<FavouriteContact> favouriteContacts = new ObservableCollection<FavouriteContact>();

        public ObservableCollection<FavouriteContact> FavouriteContacts
        {
            get { return favouriteContacts; }
            set { SetProperty(ref favouriteContacts, value); }
        }

        public void OnResume()
        {
            GetFavouriteContacts();
            GetContactsDatabase();
        }

        public async void GetFavouriteContacts()
        {
            Mvx.Resolve<IProgressBar>().ShowProgressDialog("Getting Favorites. ");
            var favouriteContacts = await Mvx.Resolve<IContactDatabase>().GetFavouriteContacts(SearchViewModel.user);
            FavouriteContacts.Clear();
            foreach (var favouriteContact in favouriteContacts)
            {
                FavouriteContacts.Add(favouriteContact);
            }
            Mvx.Resolve<IProgressBar>().HideProgressDialog();
        }

        public ICommand SelectFavouriteCommand { get; private set; }
        public FavouriteViewModel()
        {
            SelectFavouriteCommand = new MvxCommand<FavouriteContact>(async favourite =>
            {
                bool successfulRequest = await Mvx.Resolve<IDialog>().ShowDialog();
                if (successfulRequest)
                {
                    Mvx.Resolve<IDialog>().showConfirmationDialog();
                    NotificationItem notify = new NotificationItem() { SenderId = SearchViewModel.user, SenderMessage = "wants to know your status", RespondentId = favourite.fullName, MessageType = "request" };
                    await Mvx.Resolve<INotification>().InsertTodo(notify);
                    InsertRecentContact(GetContactId(favourite.fullName));
                    Mvx.Resolve<IDialog>().hideConfirmationDialog();
                }
            });
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
            contact = await Mvx.Resolve<IContactDatabase>().GetContact(respondentId);

            foreach (Contact aContact in contact)
            {
                contacts.Add(aContact);
            }

            if (respondentId != null)
            {
                await Mvx.Resolve<IContactDatabase>().InsertRecentContact(contacts[1], contacts[0]);
            }
        }


        public async void GetContactsDatabase()
        {
            var contactsAzure = await Mvx.Resolve<IContactDatabase>().GetContacts();
            contacts.Clear();

            foreach (var aContact in contactsAzure)
            {
                contacts.Add(aContact);
            }
        }

        private string GetContactId(string fullName)
        {
            string firstName = fullName.Split(',')[0];
            string lastName = fullName.Split(',')[1].Trim();

            foreach (Contact contact in contacts)
            {
                if (contact.FirstName.Equals(firstName) && contact.LastName.Equals(lastName))
                {
                    return contact.Id;
                }
            }
            return null;
        }
    }
}
