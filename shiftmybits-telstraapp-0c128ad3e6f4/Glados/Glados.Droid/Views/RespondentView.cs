using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Glados.Core.ViewModels;
using MvvmCross.Droid.Views;

using Android.Gms.Location;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Util;
using Android.Locations;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;
using System.Threading.Tasks;
using System.Collections.Generic;
using Glados.Core.Models;
using System.Text;
using System;
using System.Linq;
using Glados.Core.Interfaces;
using MvvmCross.Core.ViewModels;

namespace Glados.Droid.Views
{
    [Activity(Label = "View for RespondentViewModel")]
    public class RespondentView : MvxActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        //Initialize the variable for getting current address later
        GoogleApiClient apiClient;
        LocationRequest locRequest;
        Location _currentLocation;
        TextView addressCurrent;
        Button responseCommand,cancelCommand;
        NotificationItem notify;
        CheckBox location;
        CheckBox available;

        // Initialize the permission is disabled.
        bool availabilityPermission = false, locationPermission =  false, locationChecked = false;

        protected override void OnCreate(Bundle bundle)
        {
         
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RespondentView);

            //addressCurrent = FindViewById<TextView>(Resource.Id.address_text);
            responseCommand = FindViewById<Button>(Resource.Id.responseButton);
            cancelCommand = FindViewById<Button>(Resource.Id.cancelButton);
            location = FindViewById<CheckBox>(Resource.Id.locationAddress);
            available = FindViewById<CheckBox>(Resource.Id.availability);

            available.Click += (o, e) =>
            {
                if (available.Checked)
                {
                    availabilityPermission = true;
                } else
                {
                    availabilityPermission = false;
                }
            };

            location.Click += async (o, e) =>
            {
                if (location.Checked)
                {
                    locationPermission = true;
                    
                    if (!locationChecked)
                    {
                        // Setting location priority to PRIORITY_HIGH_ACCURACY (100)
                        locRequest.SetPriority(100);

                        // Setting interval between updates, in milliseconds
                        // NOTE: the default FastestInterval is 1 minute. If you want to receive location updates more than 
                        // once a minute, you _must_ also change the FastestInterval to be less than or equal to your Interval
                        locRequest.SetFastestInterval(500);
                        locRequest.SetInterval(1000);
                        await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);
                        locRequest.SetFastestInterval(500000);
                        locRequest.SetInterval(1000000);
                        locationChecked = true;
                    }
                    
                }
                else
                {
                    locationPermission = false;
                }
            };
     

            if (IsGooglePlayServicesInstalled())
            {
                

                apiClient = new GoogleApiClient.Builder(this, this, this).AddApi(LocationServices.API).Build();
                
                locRequest = new LocationRequest();

                apiClient.Connect();
            
            }
            else
            {
                Log.Error("OnCreate", "Google Play Services is not installed");
                Toast.MakeText(Application.Context, "Google Play Services is not Installed", ToastLength.Short).Show();
            }

        }

        protected override void OnStart()
        {
            base.OnStart();
            apiClient.Connect();

            cancelCommand.Click += async delegate
            {
                bool current = await Mvx.Resolve<IDialog>().ShowCancelResponseConfirmationDialog();
                if (current)
                {
                    
                    notify = new NotificationItem() { SenderId = RespondentViewModel.selectedName, SenderMessage = "He is Busy or Not Available", RespondentId = RespondentViewModel.user, MessageType = "response" };
                    await Mvx.Resolve<INotification>().InsertTodo(notify);
                    var request = new MvxViewModelRequest();
                    request.ParameterValues = new System.Collections.Generic.Dictionary<string, string>();
                    request.ViewModelType = typeof(FirstViewModel);
                    var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

                    //Create an intent to show UI
                    var uiIntent = requestTranslator.GetIntentFor(request);
                    StartActivity(uiIntent);
                }
            };

            responseCommand.Click += async delegate
            {
                Mvx.Resolve<IDialog>().showConfirmationDialog();
                bool current = responseNotification();
                if (current)
                {
                   
                    if (!String.IsNullOrEmpty(RespondentViewModel.locationMessage))
                    {
                        if(!availabilityPermission && !locationPermission)
                        {
                            RespondentViewModel.locationMessage = "He is Busy or Not Available";
                        } 
                        if(!availabilityPermission && locationPermission)
                        {
                            RespondentViewModel.locationMessage += ", He is not available";
                        } 
                        if(locationPermission && availabilityPermission)
                        {
                            RespondentViewModel.locationMessage += ", He is available";
                        }
                        if(! locationPermission && availabilityPermission)
                        {
                            RespondentViewModel.locationMessage = "He is available now.";
                        }
                   
                    }

                    notify = new NotificationItem() { SenderId = RespondentViewModel.selectedName, SenderMessage = RespondentViewModel.locationMessage, RespondentId = RespondentViewModel.user, MessageType = "response" };
                    await Mvx.Resolve<INotification>().InsertTodo(notify);
                    Mvx.Resolve<IDialog>().hideConfirmationDialog();
                    var request = new MvxViewModelRequest();
                    request.ParameterValues = new System.Collections.Generic.Dictionary<string, string>();
                    request.ViewModelType = typeof(FirstViewModel);
                    var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

                    //Create an intent to show UI
                    var uiIntent = requestTranslator.GetIntentFor(request);
                    StartActivity(uiIntent);
                } else
                {
                    Toast.MakeText(this, "Please try again short while, Your response is not available", ToastLength.Short).Show();
                }
            };

        }

       

        public bool responseNotification()
        {

            bool current = false;

            if (!availabilityPermission || !locationPermission)
            {
                RespondentViewModel.locationMessage = "He is Busy or Not Available";
            }

            //await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);
            if (!String.IsNullOrEmpty(RespondentViewModel.locationMessage))
            {
                current = true;
            }

            return current;
        }

        public bool IsGooglePlayServicesInstalled()
        {
            var currentTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(currentTopActivity);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info("MainActivity", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("ManActivity", "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);

                // Show error dialog to let user debug google play services
            }
            return false;
        }

        public void OnConnected(Bundle bundle)
        {

            Log.Info("LocationClient", "Now connected to client");
        }

        public void OnDisconnected()
        {

            Log.Info("LocationClient", "Now disconnected from client");
        }

        public void OnConnectionFailed(ConnectionResult bundle)
        {

            Log.Info("LocationClient", "Connection failed, attempting to reach google play services");
        }

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                Toast.MakeText(Application.Context, "Please try again short while", ToastLength.Short).Show();
            }
            else
            {
                Mvx.Resolve<IProgressBar>().ShowProgressDialog("Getting Location.");
                Address address = await ReverseGeocodeCurrentLocation();
                DisplayAddress(address);
                Mvx.Resolve<IProgressBar>().HideProgressDialog();
                locRequest.SetFastestInterval(0);
                locRequest.SetInterval(0);
            }
        }


        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            var currentTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            Geocoder geocoder = new Geocoder(currentTopActivity);
            IList<Address> addressList =
                    await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }


        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                //addressCurrent.Text = deviceAddress.ToString();
                RespondentViewModel.locationMessage = deviceAddress.ToString();
                apiClient.Disconnect();
            }
            else
            {
                RespondentViewModel.locationMessage = "";
                Toast.MakeText(Application.Context, "Unable to get current address", ToastLength.Short).Show();
            }
        }

        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
        }
    }
}