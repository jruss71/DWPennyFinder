﻿using System;
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
        private Item item;
        private ObservableCollection<Item> items;
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
  
            Item previousItem;

            if (BindingContext is Item vm)
            {
                item = vm;

                var pin = new CustomPin
                {
                    Position = item.PinPosition, //pinPosition,
                    Name = item.Name,
                    Label = string.Empty,
                    Location = item.Location,
                    Park = item.Park,
                    Url = "www.disney.com"
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
                items = new ObservableCollection<Item>(
                    vmList.Items
                    .OrderBy(item => item.Location)
                    .ThenBy(item => item.Name));

                // we initialize the PrevLocation for our first item so it won't automatically be seen as a new "group"
                if (items.Count > 0)
                {
                    previousItem = items.First();
                    String pennyName = previousItem.Name;
                    customMap.CustomPins = new List<CustomPin>();
                    foreach (Item item in items)
                    {
                        if (item.Location != previousItem.Location)
                        {
                            var pin = new CustomPin
                            {
                                Position = previousItem.PinPosition,
                                Name = pennyName,
                                Location = previousItem.Location,
                                Park = previousItem.Park,
                                Label = string.Empty,
                                Type = PinType.Place

                            };
                            pennyName = item.Name;
                  
                            customMap.CustomPins.Add(pin);
                            customMap.Pins.Add(pin);
                        }
                        else
                        {
                            if (previousItem.Name != item.Name)
                            {
                                pennyName += "\n" + item.Name;
                                Console.WriteLine(pennyName);
                            }
                        }
                        previousItem = item;
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
