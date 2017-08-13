using Glados.Core.Interfaces;
using Glados.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Glados.Core.ViewModels
{
    public class SearchViewModel
    : MvxViewModel
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
        private ObservableCollection<Contact> contacts2 = new ObservableCollection<Contact>();
        public static string user = "Tee Ken, Lau";

        private string searchTerm="";
        public string SearchTerm
        {
            get { return searchTerm; }
            set { SetProperty(ref searchTerm, value); }
        }


        public ICommand SendRequestCommand
        {
            get;
            private set;
        }

        public SearchViewModel()
        {
            GetAllContact();
            SendRequestCommand = new MvxCommand(async() =>
            {
                if (validFormat(SearchTerm))
                {
                    if (CheckUser())
                    {
                        string fullName = searchTerm.Split('-')[0].Trim();
                        string firstName = fullName.Split(',')[0];
                        string lastName = fullName.Split(',')[1].Trim();

                        bool successfulRequest = await Mvx.Resolve<IDialog>().ShowDialog();
                        if (successfulRequest)
                        {
                            Mvx.Resolve<IDialog>().showConfirmationDialog();
                            NotificationItem notify = new NotificationItem() { SenderId = user, SenderMessage = "wants to know your status", RespondentId = fullName, MessageType = "request" };
                            await Mvx.Resolve<INotification>().InsertTodo(notify);
                            InsertRecentContact(SearchTerm);
                            Mvx.Resolve<IDialog>().hideConfirmationDialog();
                            
                            SearchTerm = "";
                        }
                    }
                    else
                    {
                        // No such user
                        Mvx.Resolve<IDialog>().showErrorConfirmationDialog("User is not found");
                    }
                } else
                {
                    // Invalid format
                    Mvx.Resolve<IDialog>().showErrorConfirmationDialog("Invalid Format");
                }


            });
        }

        private bool validFormat(string searchTerm)
        {
            bool valid = true;
            if(searchTerm.Equals(""))
            {
                valid = false;
            }
            if(!searchTerm.Contains(",") && !searchTerm.Contains("-"))
            {
                valid = false;
            } 

            return valid;
        }

        private bool CheckUser()
        {
            bool hasUser = false;

            string fullName = searchTerm.Split('-')[0].Trim();
            string firstName = fullName.Split(',')[0];
            string lastName = fullName.Split(',')[1].Trim();

            foreach (Contact contact in contacts2)
            {
                if (contact.FirstName.Equals(firstName) && contact.LastName.Equals(lastName))
                {
                    hasUser = true;
                    return hasUser;

                }
            }

            return hasUser;
        }



        public async void GetAllContact()
        {

            var allContacts = await Mvx.Resolve<IContactDatabase>().GetContacts();
            foreach (var contact in allContacts)
            {
                contacts2.Add(contact);
            }
        }

        public Contact getContact(string fullNameDepartment)
        {
            string fullName = fullNameDepartment.Split('-')[0].Trim();
            string firstName = fullName.Split(',')[0];
            string lastName = fullName.Split(',')[1].Trim();

            foreach (Contact contact in contacts)
            {
                if (contact.FirstName.Equals(firstName) && contact.LastName.Equals(lastName))
                {
                    return contact;
                }
            }
            return null;
        }

        public async void InsertRecentContact(string searchTerm)
        {
            // for some reason I can't call the this.GetContacts() which would incur error.
            var contactsAzure = await Mvx.Resolve<IContactDatabase>().GetContacts();
            contacts.Clear();

            foreach (var aContact in contactsAzure)
            {
                contacts.Add(aContact);
            }

            if (searchTerm != null)
            {
                await Mvx.Resolve<IContactDatabase>().InsertRecentContact(getContact(user), getContact(searchTerm));
            }
        }
    }
}
