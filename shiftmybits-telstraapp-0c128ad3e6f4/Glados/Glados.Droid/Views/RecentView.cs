using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Glados.Core.ViewModels;

namespace Glados.Droid.Views
{
    [Activity(Label = "View for RecentViewModel")]
    public class RecentView : MvxActivity
    {
        public static RecentViewModel recentViewModel;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RecentView);

            recentViewModel = ViewModel as RecentViewModel;

        }

        protected override void OnResume()
        {
            var vm = (RecentViewModel)ViewModel;
            vm.OnResume();
            base.OnResume();
        }
    }
}