﻿using System.Collections.Generic;
using DWPennyFinder.Views;
using Xamarin.Forms.Maps;

namespace DWPennyFinder
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }

        public CustomMap() { }

        public MapPage mapPage
        {
            get; set;
        }
    }
}
