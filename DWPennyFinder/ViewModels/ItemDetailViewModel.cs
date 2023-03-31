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
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Location = item.Location;
                Park = item.Park;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}

