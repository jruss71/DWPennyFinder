using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using DWPennyFinder.Models;
using DWPennyFinder.Views;
using Xamarin.Forms.Maps;

namespace DWPennyFinder.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;
        private Position _pinposition;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get;  }
        public Command<Item> ItemTapped { get; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Park { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public INavigation Navigation { get; set; }
        public Position PinPosition => _pinposition;

        public ItemsViewModel()
        {
            Title = "Browse 100th Pennies";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
            // This will push the MapViewPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapViewModel.Park)}={item.Park}");
            //await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapViewModel.Latitude)}={item.Latitude}&{nameof(MapViewModel.Longitude)}={item.Longitude}&{nameof(MapViewModel.Name)}={item.Name}");
            // await Shell.Current.GoToAsync(nameof(MapPage));

            var mapPage = new MapPage();
            mapPage.BindingContext = item;
            await Navigation.PushAsync(mapPage);
            // await Shell.Current.GoToAsync($"{nameof(MapPage)}?ItemId={item.Id}");
            // await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapViewModel.ItemId)}={item.Id}");
            // Navigate to the MapPage with a pin showing the location of the item
            //await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapViewModel.ItemId)}={item.Id}&{nameof(MapViewModel.Park)}={item.Park}&{nameof(MapViewModel.Name)}={item.Name}&{nameof(MapViewModel.Location)}={item.Location}&{nameof(MapViewModel.Latitude)}={item.Latitude}&{nameof(MapViewModel.Longitude)}={item.Longitude}");
            // Navigate to the MapPage with a pin showing the location of the item
            //await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapViewModel.ItemId)}={item.Id}&{nameof(MapViewModel.Name)}={item.Name}&{nameof(MapViewModel.Park)}={item.Park}&{nameof(MapViewModel.Location)}={item.Location}&{nameof(MapViewModel.Latitude)}={item.Latitude}&{nameof(MapViewModel.Longitude)}={item.Longitude}");

        }
    }
}
