using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DWPennyFinder.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = DWPennyFinder.Models.Location;

namespace DWPennyFinder.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private Item _item;
        private Location _location;
        private Machine _machine;
        private double latitude;
        private double longitude;

        public INavigation Navigation { get; set; }

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            SaveCommand = new Command(async () => await SaveItemDetailAsync(), () => !IsBusy);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_item.Name)
                && !String.IsNullOrWhiteSpace(_location.name) && !String.IsNullOrWhiteSpace(_machine.name);
        }
        public Item Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public Machine Machine
        {
            get => _machine;
            set => SetProperty(ref _machine, value);
        }

        public Location Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
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
            //var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            //if (status == PermissionStatus.Granted)
            //{
            //    var locationGPS = await Geolocation.GetLocationAsync();
            //    var newItem = new ItemDetail()
            //    {
            //        //item = Guid.NewGuid().ToString(),
            //        item = new Item() {
            //            name = name,
            //            hasItem = false,
            //        machine = machine,
            //        location = location,
            //        Latitude = locationGPS.Latitude,
            //        Longitude = locationGPS.Longitude
            //    };

            //    await App.Database.SaveItemAsync(newItem);

            //    // This will pop the current page off the navigation stack
            //    await Navigation.PopModalAsync();
            //}
        }


        private async Task SaveItemDetailAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var itemDetail = new ItemDetail
                    {
                        Item = Item,
                        Machine = Machine,
                        Location = Location
                    };
                    await App.Database.SaveItemDetailAsync(itemDetail);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task DeleteItemDetailAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await App.Database.DeleteItemAsync(Item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

