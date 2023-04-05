using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using DWPennyFinder.Models;
using DWPennyFinder.Views;
using System;
using System.Diagnostics;
using System.Linq;

namespace DWPennyFinder.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand ItemTapped { get; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Park { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsSelected { get; set; }

        public INavigation Navigation { get; set; }
        public bool IsCheckboxListVisible { get; set; }

        public ObservableCollection<Item> CheckBoxItems { get; set; }
        

        public ItemsViewModel()
        {
            Title = "Browse 100th Pennies";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            CheckBoxItems = new ObservableCollection<Item>();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await App.Database.GetItemsAsync();
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
            await Navigation.PushAsync(new NewItemPage());
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            var mapPage = new MapPage();
            mapPage.BindingContext = item;
            await Navigation.PushAsync(mapPage);

        }

        public async Task CheckBoxItemsByLocation(string location)
        {
            CheckBoxItems.Clear();

            var items = await App.Database.GetItemsByLocation(location);
            foreach (var item in items)
            {
                CheckBoxItems.Add(item);
            }
        }

    }
}
