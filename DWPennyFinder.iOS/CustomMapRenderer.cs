using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using CoreGraphics;
using CustomRenderer;
using CustomRenderer.iOS;
using DWPennyFinder;
using DWPennyFinder.iOS;
using Foundation;
using MapKit;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CustomRenderer.iOS
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        List<CustomPin> customPins;
        CustomMap formsMap;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = null;
                nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
            }

            if (e.NewElement != null)
            {
                formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                customPins = formsMap.CustomPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
            }
            if (Control != null)
            {
                var map = Control as MKMapView;
                map.DidDeselectAnnotationView += (object sender, MKAnnotationViewEventArgs eventArgs) =>
                {
                    foreach (var anno in ((MKMapView)sender).Annotations)
                    {
                        // ((MKMapView)sender).SelectAnnotation(anno, true);
                    }
                };
                
            }
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;
            customPins = formsMap.CustomPins;
            var customPin = GetCustomPin(annotation as MKPointAnnotation);
          

            annotationView = mapView.DequeueReusableAnnotation(customPin.Name);
            if (annotationView == null)
            {

                annotationView = new CustomMKAnnotationView(annotation, customPin.Name);
                annotationView.Image = UIImage.FromFile("pin.png");
                annotationView.CalloutOffset = new CGPoint(0, 0);
                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);

                ((CustomMKAnnotationView)annotationView).Name = customPin.Name;
                ((CustomMKAnnotationView)annotationView).Location = customPin.Location;
                ((CustomMKAnnotationView)annotationView).Park = customPin.Park;
            }
            annotationView.CanShowCallout = true;
            configureDetailView(annotationView);
            return annotationView;
        }
        void configureDetailView(MKAnnotationView annotationView)
        {
            int width = 100;
            int height = 100;

            var snapshotView = new UIView();
            var options = new MKMapSnapshotOptions();
            options.Size = new CGSize(width, height);
            options.MapType = MKMapType.SatelliteFlyover;
            var snapshotter = new MKMapSnapshotter(options);
            snapshotter.Start((snapshot, error) =>
            {
                if (snapshot != null)
                {
                    UILabel label = new UILabel();
                    UILabel label2 = new UILabel();
                    UILabel label3 = new UILabel();

                    label.Text = ((CustomMKAnnotationView)annotationView).Park;
                    label2.Text = ((CustomMKAnnotationView)annotationView).Location;
                    label3.Text = ((CustomMKAnnotationView)annotationView).Name;
                    label3.LineBreakMode = UILineBreakMode.WordWrap;
                    label3.Lines = 0;

                    int numLines = label3.Text.Split('\n').Length;
                    var snapshotHeight =0;
                    if(numLines != 4)
                    {
                     
                        snapshotHeight = 75;
                    }
                    else
                    {
                        snapshotHeight = 100;
                    }

                    snapshotView.TranslatesAutoresizingMaskIntoConstraints = false;
                    NSDictionary views = NSDictionary.FromObjectAndKey(snapshotView, new NSString("snapshotView"));
                    snapshotView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[snapshotView(250)]", new NSLayoutFormatOptions(), null, views));
                    snapshotView.AddConstraints(NSLayoutConstraint.FromVisualFormat($"V:[snapshotView({snapshotHeight})]", new NSLayoutFormatOptions(), null, views));

          
                    label.Frame = new CGRect(0, 0, 250, 20);
                    label2.Frame = new CGRect(0, 15, 250, 20);
                    label3.Frame = new CGRect(0, 25, 250, 70);

                    label.TextAlignment = UITextAlignment.Left;
                    label2.TextAlignment = UITextAlignment.Left;
                    label3.TextAlignment = UITextAlignment.Left;


                    label.Font = UIFont.BoldSystemFontOfSize(16);
                    label2.Font = UIFont.SystemFontOfSize(14);
                    label3.Font = UIFont.SystemFontOfSize(12);

                    snapshotView.AddSubviews(label, label2, label3);
                }
            });

            annotationView.DetailCalloutAccessoryView = snapshotView;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            if (!string.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            }
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();

        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);

            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}