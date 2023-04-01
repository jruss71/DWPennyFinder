using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using DWPennyFinder.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DWPennyFinder.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string name;
        private string park;
        private string location;
        private double latitude;
        private double longitude;

        public INavigation Navigation { get; set; }

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(park) && !String.IsNullOrWhiteSpace(location);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Park
        {
            get => park;
            set => SetProperty(ref park, value);
        }

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }
        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Navigation.PopModalAsync();
        }

        private async void OnSave()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                var location = await Geolocation.GetLocationAsync();
                var latitude = location.Latitude;
                var longitude = location.Longitude;

                Item newItem = new Item()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Name,
                    Park = Park,
                    Location = Location,
                    Latitude = latitude,
                    Longitude = longitude
                };

                await DataStore.AddItemAsync(newItem);

                // This will pop the current page off the navigation stack
                await Navigation.PopModalAsync();
            }
        }
    }
}
