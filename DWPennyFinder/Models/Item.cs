using System;
using SQLite;

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
    }
}
