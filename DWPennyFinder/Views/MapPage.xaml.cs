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

        private double defaultZoomLevel = 16;

        public MapPage()
        {
            InitializeComponent();
            Item item = null;
            
            // Get the itemId from the query string parameters
            if (BindingContext is MapViewModel vm)
            {
                item = vm.item;   
            }
            slider.Value = defaultZoomLevel;
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

                // Set the slider value to match the default zoom level of the map
                var latlongDegrees = 360 / (Math.Pow(2, defaultZoomLevel));
                slider.Value = defaultZoomLevel;
                if (map.VisibleRegion != null)
                {
                    map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
                }
            }
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (map.VisibleRegion != null)
            {
                map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Button button = sender as Xamarin.Forms.Button;
            switch (button.Text)
            {
                case "Street":
                    map.MapType = MapType.Street;
                    break;
                case "Satellite":
                    map.MapType = MapType.Satellite;
                    break;
                case "Hybrid":
                    map.MapType = MapType.Hybrid;
                    break;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            map.Pins.Clear();
        }

      

        
    }

}

