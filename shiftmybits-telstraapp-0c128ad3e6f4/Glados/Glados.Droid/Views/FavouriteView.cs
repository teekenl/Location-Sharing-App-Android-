using Android.App;
using Android.OS;
using Glados.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace Glados.Droid.Views
{
    [Activity(Label = "View for FavouriteViewModel")]
    public class FavouriteView : MvxActivity
    {
        public static FavouriteViewModel favouriteViewModel;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FavouriteView);
            favouriteViewModel = ViewModel as FavouriteViewModel;
        }

        protected override void OnResume()
        {
            var vm = (FavouriteViewModel)ViewModel;
            vm.OnResume();
            base.OnResume();
        }
    }
}