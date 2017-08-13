using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using Glados.Core.Interfaces;
using Glados.Droid.Services;
using MvvmCross.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Shared.Presenter;
using Glados.Droid.Database;
using Glados.Core.Database;
using MvvmCross.Platform.Platform;

namespace Glados.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Glados.Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeFirstChance()
        {
            Mvx.LazyConstructAndRegisterSingleton<IBottomSheet, BottomSheet>();
            Mvx.LazyConstructAndRegisterSingleton<IAzureDatabase, AzureDatabase>();
            Mvx.LazyConstructAndRegisterSingleton<IContactDatabase, ContactDatabaseAzure>();
            Mvx.LazyConstructAndRegisterSingleton<INotification, NotificationDatabaseAzure>();
            Mvx.LazyConstructAndRegisterSingleton<IDialog, BackPressedDialog>();
            Mvx.LazyConstructAndRegisterSingleton<ILocation, LocationAddress>();
            Mvx.LazyConstructAndRegisterSingleton<IProgressBar, ProgressBar>();
            base.InitializeFirstChance();
        }
    }
}
