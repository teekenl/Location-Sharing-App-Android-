using Glados.Core.Interfaces;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;
using Glados.Core.Models;
using System.Collections.ObjectModel;
using MvvmCross.Platform;

namespace Glados.Core.ViewModels
{
    public class RespondentViewModel
        : MvxViewModel
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

        public static string respondentMessage;
        public string RespondentMessage
        {
            get { return respondentMessage; }
            set
            {
                SetProperty(ref respondentMessage, value);
            }
        }

        public static string selectedName;
        public string SelectedName
        {
            get { return selectedName; }
            set
            {
                SetProperty(ref selectedName, value);
            }
        }

        public static string user;
        public string User
        {
            get { return user; }
            set
            {
                SetProperty(ref user, value);

            }
        }

        public static string locationMessage;
        public string LocationMessage
        {
            get { return locationMessage; }
            set
            {
                SetProperty(ref locationMessage, value);
            }
        }


        public ICommand SendResponseCommand
        {
            get;
            set;
        }


        public RespondentViewModel()
        {

            SendResponseCommand = new MvxCommand(() =>
            {
                
                NotificationItem notify = new NotificationItem() { SenderId = selectedName, SenderMessage = "your message goes here", RespondentId = user, MessageType = "response" };
                Mvx.Resolve<INotification>().InsertTodo(notify);

                Mvx.Resolve<ILocation>().GetCurrentAddress();

            });
        }
    }
}
