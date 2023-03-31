using System;
using System.Collections.Generic;
using DWPennyFinder.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace DWPennyFinder.Views
{	
	public partial class MapPage : ContentPage
	{	
		public MapPage ()
		{
			InitializeComponent ();
		}

        public MapPage(Item item)
        {
            InitializeComponent();

            var position = new Position(item.Latitude, item.Longitude);
            map.Pins.Clear();
            map.Pins.Add(new Pin { Position = position, Label = item.Name });
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1.0)));

            // Set MyLocationButtonEnabled property
            map.UiSettings.MyLocationButtonEnabled = true;
        }

    }

}

