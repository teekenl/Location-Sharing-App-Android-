using Glados.Core.Interfaces;
using Glados.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Glados.Core.ViewModels
{
    public class RecentViewModel
    : MvxViewModel
    {
        private IContactDatabase RecentContactDatabase;
        private ObservableCollection<RecentContact> recentContacts = new ObservableCollection<RecentContact>();
        private ObservableCollection<RecentContact> recentContactsCopy = new ObservableCollection<RecentContact>();

        public ObservableCollection<RecentContact> RecentContacts
        {
            get { return recentContacts; }
            set { SetProperty(ref recentContacts, value); }
        }

        public void OnResume()
        { 
            GetRecentContacts();
        }

        public async void GetRecentContacts()
        {
            Mvx.Resolve<IProgressBar>().ShowProgressDialog("Getting History. ");
            var recentContacts = await RecentContactDatabase.GetRecentContacts();
            RecentContacts.Clear();
            recentContactsCopy.Clear();
            foreach (var recentContact in recentContacts)
            {
                recentContactsCopy.Add(recentContact);
            }
            foreach (RecentContact recentContact in recentContactsCopy)
            {
                if (recentContact.RequesterFullName.Equals(SearchViewModel.user) || recentContact.RespondentFullName.Equals(SearchViewModel.user)) {
                    RecentContacts.Add(recentContact);
                }
            }
            foreach (RecentContact recentContact in RecentContacts)
            {
                if (recentContact.RequesterFullName.Equals(SearchViewModel.user))
                {
                    string copy = recentContact.RequestedAtString;
                    recentContact.RequestedAtString = "Sent " + copy;
                }
                else if (recentContact.RespondentFullName.Equals(SearchViewModel.user))
                {
                    string RespondentFullNameCopy = recentContact.RespondentFullName;
                    string RequestedAtStringcopy = recentContact.RequestedAtString;
                    recentContact.RequestedAtString = "Received " + RequestedAtStringcopy;
                    recentContact.RespondentFullName = recentContact.RequesterFullName;
                    recentContact.RequesterFullName = RespondentFullNameCopy;
                }
            }
            Mvx.Resolve<IProgressBar>().HideProgressDialog();

        }

        public ICommand SelectRecentCommand { get; private set; }
        public RecentViewModel(IContactDatabase RecentContactDatabase)
        {
            this.RecentContactDatabase = RecentContactDatabase;

            SelectRecentCommand = new MvxCommand<RecentContact>(recent => Mvx.Resolve<IBottomSheet>().Show(recent.RespondentId, recent.RespondentFullName));
        }
    }
}
