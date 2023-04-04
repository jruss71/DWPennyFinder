using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DWPennyFinder.Models;
using Xamarin.Forms;

namespace DWPennyFinder.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private string location;
        private string park;
        private double latitude;
        private double longitude;
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
            }
        }        

        
    }
}

