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
            //BindingContext = new MapViewModel();
            Item item = null;
            
            // Get the itemId from the query string parameters
            if (BindingContext is MapViewModel vm)
            {
                item = vm.item;   
            }
        }


        //Position position = new Position(36.9628066, -122.0194722);
        //MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);

        //Map map = new Map(mapSpan);

        //Pin pin = new Pin
        //{
        //    Label = "Santa Cruz",
        //    Address = "The city with a boardwalk",
        //    Type = PinType.Place,
        //    Position = position
        //};
        //map.Pins.Add(pin);

        //Content = new StackLayout
        //{
        //    Children = { map }
        //};
        //}s

        //public MapPage(string itemId, string name, string park, string location, double latitude, double longitude)
        //{
        //    InitializeComponent();

        //    // Create a Pin for the location
        //    var position = new Position(latitude, longitude);
        //    var pin = new Pin
        //    {
        //        Label = name + park,
        //        Type = PinType.Place,
        //        Position = position
        //    };

        //    // Create a Map with the Pin centered on the location
        //    var mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1));
        //    var map = new Map(mapSpan)
        //    {
        //        IsShowingUser = true,
        //        VerticalOptions = LayoutOptions.FillAndExpand,
        //        HorizontalOptions = LayoutOptions.FillAndExpand
        //    };
        //    map.Pins.Add(pin);

        //    Content = new StackLayout
        //    {
        //        Children = { map }
        //    };
        //}

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

