using MvvmCross.Core.ViewModels;
using Glados.Core.Interfaces;

namespace Glados.Core.ViewModels
{

    public class FirstViewModel 
        : MvxViewModel
    {
        private IContactDatabase contactDatabase;
        
        public FirstViewModel(IContactDatabase contactDatabase)
        {
            this.contactDatabase = contactDatabase;
            Search = new SearchViewModel();
            Recent = new RecentViewModel(contactDatabase);
            Favourite = new FavouriteViewModel();
        }
        private SearchViewModel search;
        public SearchViewModel Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(() => Search); }
        }

        private RecentViewModel recent;
        public RecentViewModel Recent
        {
            get { return recent; }
            set { recent = value; RaisePropertyChanged(() => Recent); }
        }

        private FavouriteViewModel favourite;
        public FavouriteViewModel Favourite
        {
            get { return favourite; }
            set { favourite = value; RaisePropertyChanged(() => Favourite); }
        }
    }
}
