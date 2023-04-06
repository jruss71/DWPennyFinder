using System;
using System.ComponentModel;
using SQLite;
using Xamarin.Forms.Maps;

namespace DWPennyFinder.Models
{

    public class ItemDetail
    {
        public Item item { get; set; }
        public Machine machine { get; set; }
        public Location location { get; set; }
    }

    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string name { get; set; }
    }

    public class Machine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int locationId { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Position pinPosition
        {
            get
            {
                return new Position(latitude, longitude);
            }
        }
    }

    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int machineId { get; set; }
        public string Name { get; set; }
        public bool Collected { get; set; }
        // Computed property PinPosition
       
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
