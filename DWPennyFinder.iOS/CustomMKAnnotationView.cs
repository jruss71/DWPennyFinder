using CoreGraphics;
using Foundation;
using MapKit;
using System;
using UIKit;

namespace DWPennyFinder.iOS
{
    public class CustomMKAnnotationView : MKAnnotationView
    {

        private readonly CGRect annotationFrame = new CGRect(x: 0, y: 0, width: 40, height: 40);
        private readonly UILabel label;

        public string Name { get; set; }

        public string Url { get; set; }

        public string Location { get; set; }

        public string Machine { get; set; }

        public int MachineID { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id){
        }
        
    }
}
