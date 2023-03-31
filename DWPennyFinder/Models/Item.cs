using System;

namespace DWPennyFinder.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Park { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
