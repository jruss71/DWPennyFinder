using System;
using System.Collections.Generic;
using DWPennyFinder.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using DWPennyFinder.ViewModels;
using DWPennyFinder.Converters;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using Rg.Plugins.Popup.Services;

namespace DWPennyFinder.Views
{
    public partial class MapPage : ContentPage
    {
        private ItemDetail itemDetail;
        private ObservableCollection<ItemDetail> items;
        private ItemsViewModel _viewModel;
        //private CheckBoxContentView _checkboxView;

        private readonly double defaultZoomLevel = 16;

        public MapPage()
        {
            InitializeComponent();
            _viewModel = new ItemsViewModel();
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;
            Resources.Add("LocationParkConverter", new LocationParkConverter());
            slider.Value = defaultZoomLevel;
            customMap.MapClicked += CustomMap_MapClicked;
        }

        private void CustomMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            //checkboxList.IsVisible = false;
        }

        private async Task LoadItems()
        {
            await _viewModel.ExecuteLoadItemsCommand();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

            ItemDetail previousItem;

            if (BindingContext is ItemDetail vm)
            {
                itemDetail = vm;

                var pin = new CustomPin
                {
                    Position = itemDetail.machine.pinPosition, //pinPosition,
                    Name = itemDetail.item.Name,
                    Label = string.Empty,
                    Machine = itemDetail.machine.name,
                    Location = itemDetail.location.name,
                    MachineID = itemDetail.machine.Id
                };
                customMap.CustomPins = new List<CustomPin> { pin };
                customMap.Pins.Add(pin);
                var mapSpan = MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.5));
                customMap.MoveToRegion(mapSpan);

                // Set the slider value to match the default zoom level of the map
                var latlongDegrees = 360 / (Math.Pow(2, defaultZoomLevel));
                slider.Value = defaultZoomLevel;
                if (customMap.VisibleRegion != null)
                {
                    customMap.MoveToRegion(new MapSpan(customMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
                }
            }
            else if (BindingContext is ItemsViewModel vmList)
            {
                var mapSpan = MapSpan.FromCenterAndRadius(
                 new Position(28.385233, -81.56), Distance.FromMiles(3.0));
                customMap.MoveToRegion(mapSpan);

                await LoadItems();
                customMap.CustomPins = new List<CustomPin>();
                // This is the list view so lets add pins for the full list
                var itemDetails = new ObservableCollection<ItemDetail>(
                    vmList.Items
                    .OrderBy(item => item.location.name)
                    .ThenBy(item => item.item.Name));

                // we initialize the PrevLocation for our first item so it won't automatically be seen as a new "group"
                if (itemDetails.Count > 0)
                {
                    previousItem = itemDetails.First();
                    String pennyName = previousItem.item.Name;
                    foreach (ItemDetail itemDetail in itemDetails)
                    {
                        if (itemDetail.machine.name != previousItem.machine.name)
                        {
                            var pin = new CustomPin
                            {
                                Position = previousItem.machine.pinPosition,
                                Name = pennyName,
                                Machine = previousItem.machine.name,
                                Location = previousItem.location.name,
                                MachineID = previousItem.machine.Id,
                                Label = string.Empty,
                                Type = PinType.Place

                            };
                            pennyName = itemDetail.item.Name;

                            customMap.CustomPins.Add(pin);
                            customMap.Pins.Add(pin);
                        }
                        else
                        {
                            if (previousItem.item.Name != itemDetail.item.Name)
                            {
                                pennyName += "\n" + itemDetail.item.Name;
                                Console.WriteLine(pennyName);
                            }
                        }
                        previousItem = itemDetail;
                    }
                }

                var latlongDegrees = 360 / (Math.Pow(2, defaultZoomLevel));
                slider.Value = defaultZoomLevel;
              

            }
        }

        

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (customMap.VisibleRegion != null)
            {
                customMap.MoveToRegion(new MapSpan(customMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Button button = sender as Xamarin.Forms.Button;
            switch (button.Text)
            {
                case "Street":
                    customMap.MapType = MapType.Street;
                    break;
                case "Satellite":
                    customMap.MapType = MapType.Satellite;
                    break;
                case "Hybrid":
                    customMap.MapType = MapType.Hybrid;
                    break;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            customMap.Pins.Clear();
        }
    }

}
