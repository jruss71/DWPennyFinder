using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DWPennyFinder.Models;
using Xamarin.Forms;

namespace DWPennyFinder.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class MapViewModel : BaseViewModel
	{

        public MapViewModel()
        {
            Title = "Map";
        }

        


        private string itemId;
        private string name;
        private string location;
        private string park;
        private double latitude;
        private double longitude;
        public Item item { get; }

        public string Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }
        public string Park
        {
            get => park;
            set => SetProperty(ref park, value);
        }
        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                ItemId = item.itemId;
                Name = item.Name;
                Location = item.Location;
                Park = item.Park;
                Latitude = item.Latitude;
                Longitude = item.Longitude;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }


    }
}

