using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using DWPennyFinder.Models;
using DWPennyFinder.Views;
using System;
using System.Diagnostics;
using System.ComponentModel;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Collections.Generic;

namespace DWPennyFinder.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private ItemDetail  _selectedItem;
        private Collection<ItemDetail> SourceItems { get; }
        public ObservableCollection<ItemDetail> Items { get; }
      
        public ICommand AddItemCommand { get; }

        public ICommand ItemTapped { get; }
        public ICommand ItemCollected { get; }
        public ICommand ItemRemoved { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AltFilterCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }


        public string Id { get; set; }
        public string Name { get; set; }
        public string Park { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public INavigation Navigation { get; set; }

        public ObservableCollection<Item> CheckBoxItems { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse 100th Pennies";
            Items = new ObservableCollection<ItemDetail>();
            SourceItems = new Collection<ItemDetail>();
            CheckBoxItems = new ObservableCollection<Item>();
            ItemTapped = new Command<ItemDetail>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            AltFilterCommand = new Command(OnFilterCommand);
            ItemCollected = new Command<ItemDetail>(OnItemCollected);
            ItemRemoved = new Command<ItemDetail>(OnItemRemoved);
            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await ExecuteLoadItemsCommand();
                IsRefreshing = false;
            });
           
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
                    var machine = await App.Database.GetMachineByIdAsync(item.MachineId);
                    var location = await App.Database.GetLocationAsync(machine.locationId);

                    ItemDetail itemDetail = new ItemDetail
                    {
                        item = item,
                        machine = machine,
                        location = location
                    };
                    SourceItems.Add(itemDetail);
                    Items.Add(itemDetail); 
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
            SelectedItem = null;
        }

       


        public ItemDetail SelectedItem
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

        private async void OnFilterCommand(object obj)
        {
            //await Navigation.PushAsync(new NewItemPage());
            Console.Write("Alt Filter Command here");
            FilterPage filterPage = new FilterPage(this);
            await PopupNavigation.Instance.PushAsync(filterPage);

        }


        private async void OnItemCollected(ItemDetail itemDetail)
        {
            if (itemDetail == null)
                return;
            // update the Collected value
            itemDetail.item.Collected = true;

            // save the item to the database
            await App.Database.SaveItemAsync(itemDetail.item);
            OnPropertyChanged(nameof(Item.Collected));

        }
        private async void OnItemRemoved(ItemDetail itemDetail)
        {
            if (itemDetail == null)
                return;

            // update the Collected value
            itemDetail.item.Collected = false;

            // save the item to the database
            await App.Database.SaveItemAsync(itemDetail.item);
            OnPropertyChanged(nameof(Item.Collected));



        }
        async void OnItemSelected(ItemDetail item)
        {
            if (item == null)
                return;


            var mapPage = new MapPage();
            mapPage.BindingContext = item;
            await Navigation.PushAsync(mapPage);

        }
        public async Task CheckBoxItemsForMachine(int machine)
        {
            CheckBoxItems.Clear();

            var items = await App.Database.GetItemsByMachineAsync(machine);
            foreach (var item in items)
            {
                CheckBoxItems.Add(item);
            }
        }
        public void ClearFilter()
        {
            // Clear any applied filters
            Items.Clear();

            // Reload all items
            ExecuteLoadItemsCommand();
        }

        public void FilterItemsByLocation(string location)
        {
            Console.WriteLine("filter items by location");

            IEnumerable<ItemDetail>  itemsByLocation = SourceItems.Where(itemDetail => itemDetail.location.name == location);
            Console.WriteLine("Filtered items count: " + itemsByLocation.Count());
            Console.WriteLine("Unfiltered items count: " + Items.Count());
            //Items.Clear();
            foreach (var item in SourceItems)
            {
                if (!itemsByLocation.Contains(item))
                {
                    Items.Remove(item);   
                }
                else if (!Items.Contains(item))
                {
                    Items.Add(item);
                }
            }
        }

        public void FilterItemsByResorts()
        {
            var itemsByResorts = Items.Where(itemDetail =>
                !itemDetail.location.name.Equals("Epcot") &&
                !itemDetail.location.name.Equals("Animal Kingdom") &&
                !itemDetail.location.name.Equals("Magic Kingdom") &&
                !itemDetail.location.name.Equals("Hollywood Studios") &&
                !itemDetail.location.name.Equals("Disney Springs")
            );

            if (itemsByResorts != null)
            {
                Items.Clear();
                foreach (var item in itemsByResorts)
                {
                    Items.Add(item);
                }
            }
        }






    }
}
