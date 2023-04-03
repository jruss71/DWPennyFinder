﻿using System;
using System.Collections.Generic;
using DWPennyFinder.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using DWPennyFinder.ViewModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using DWPennyFinder.Converters;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

//using DWPennyFinder.Services;

namespace DWPennyFinder.Views
{	
	public partial class MapPage : ContentPage
    {
        private Item item;
        private ObservableCollection<Item> items;
        private ItemsViewModel _viewModel;

        private double defaultZoomLevel = 16;

        public MapPage()
        {
            InitializeComponent();
            _viewModel = new ItemsViewModel();
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;
            Resources.Add("LocationParkConverter", new LocationParkConverter());
            slider.Value = defaultZoomLevel;
        }

        private async Task LoadItems()
        {
            await _viewModel.ExecuteLoadItemsCommand();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            List<CustomPin> custompinList = new List<CustomPin>();

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
                await LoadItems();
                Boolean movedToRegion = false;

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
                    customMap.CustomPins = new List<CustomPin>();
                    foreach (Item item in items)
                    {
                        if (item.Location != previousItem.Location)
                        {
                            var pin = new CustomPin
                            {
                                Position = previousItem.PinPosition,
                                Name = previousItem.Name,
                                Location = previousItem.Location,
                                Park = previousItem.Park,
                                Label = previousItem.Location,
                                Type = PinType.Place

                            };
                            customMap.CustomPins = new List<CustomPin> { pin };
                            customMap.Pins.Add(pin);
                            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));
                            //custompinList.Add(pin);
                            //customMap.CustomPins.Add(pin);
                            // customMap.Pins.Add(pin);
                            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(.5)));
                        }
                        else
                        {
                            if (previousItem.Name != item.Name)
                            {
                                previousItem.Name += "\n" + item.Name;
                            }
                        
                        }
                    }
                    customMap.CustomPins = custompinList;
                    if (!movedToRegion)
                    {
                        movedToRegion = true;
                        var mapSpan = MapSpan.FromCenterAndRadius(previousItem.PinPosition, Distance.FromMiles(1.0));
                        //customMap.MoveToRegion(mapSpan);

                    }
                }
               
                // Set the slider value to match the default zoom level of the map
                var latlongDegrees = 360 / (Math.Pow(2, defaultZoomLevel));
                slider.Value = defaultZoomLevel;
                //if (customMap.VisibleRegion != null)
                //{
                //    customMap.MoveToRegion(new MapSpan(customMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
                //}

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

