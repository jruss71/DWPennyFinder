using System;
using System.Collections.Generic;
using DWPennyFinder.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using DWPennyFinder.ViewModels;
using Map = Xamarin.Forms.Maps.Map;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using DWPennyFinder.Services;

namespace DWPennyFinder.Views
{	
	public partial class MapPage : ContentPage
    {
        private Item item;
        public MapPage()
        {
            InitializeComponent();
            Item item = null;
            
            // Get the itemId from the query string parameters
            if (BindingContext is MapViewModel vm)
            {
                item = vm.item;   
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is Item vm)
            {
                item = vm;
                var pinPosition = new Position(item.Latitude, item.Longitude);
                var pin = new Pin
                {
                    Position = pinPosition,
                    Label = item.Name,
                    Address = item.Location
                };
                map.Pins.Add(pin);
                var mapSpan = MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.5));
                map.MoveToRegion(mapSpan);

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            map.Pins.Clear();
        }

        public MapPage (string itemId)
        {
            InitializeComponent();
        }

        public MapPage (Item item)
        {
            InitializeComponent();
            BindingContext = new MapViewModel();
            //Item item = null;

            // Get the itemId from the query string parameters
            if (BindingContext is MapViewModel vm)
            {
                item = vm.item;
            }



            // Create a Pin for the Item's location
            var position = new Position(item.Latitude, item.Longitude);
            var pin = new Pin
            {
                Label = item.Name,
                Position = position
            };
        }
    }

}

