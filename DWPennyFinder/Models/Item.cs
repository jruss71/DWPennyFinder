using System;
using System.ComponentModel;
using SQLite;
using Xamarin.Forms.Maps;

namespace DWPennyFinder.Models
{
    public class Item 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string itemId { get; set; }
        public string Name { get; set; }
        public string Park { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool HaveItem { get; internal set; }

        // Computed property PinPosition
        public Position PinPosition
        {
            get
            {
                return new Position(Latitude, Longitude);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
