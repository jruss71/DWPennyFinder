using System;
using System.Collections.Generic;
using CoreGraphics;
using CustomRenderer.iOS;
using DWPennyFinder;
using DWPennyFinder.iOS;
using DWPennyFinder.ViewModels;
using DWPennyFinder.Views;
using Foundation;
using MapKit;
using Rg.Plugins.Popup.Services;
using UIKit;
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
           
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;
            customPins = formsMap.CustomPins;
            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Name);
            if (annotationView == null)
            {

                annotationView = new CustomMKAnnotationView(annotation, customPin.Name);
                annotationView.Image = UIImage.FromFile("pin.png");
                annotationView.CalloutOffset = new CGPoint(0, 0);
                string hexColor = "#886EB6"; 
                UIColor uiColor = UIColor.FromRGB(
                    Convert.ToInt32(hexColor.Substring(1, 2), 16),
                    Convert.ToInt32(hexColor.Substring(3, 2), 16),
                    Convert.ToInt32(hexColor.Substring(5, 2), 16)
                );

                //UIButton infoButton = UIButton.FromType(UIButtonType.DetailDisclosure);
                UIButton infoButton = UIButton.FromType(UIButtonType.System);
                infoButton.SetTitle("Collect", UIControlState.Normal);

                infoButton.TintColor = uiColor;

                annotationView.RightCalloutAccessoryView = infoButton;

                ((CustomMKAnnotationView)annotationView).Name = customPin.Name;
                ((CustomMKAnnotationView)annotationView).Location = customPin.Location;
                ((CustomMKAnnotationView)annotationView).Machine = customPin.Machine;
                ((CustomMKAnnotationView)annotationView).MachineID = customPin.MachineID;
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
            snapshotView.TranslatesAutoresizingMaskIntoConstraints = false;
            NSDictionary views = NSDictionary.FromObjectAndKey(snapshotView, new NSString("snapshotView"));
            snapshotView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[snapshotView(250)]", new NSLayoutFormatOptions(), null, views));
            snapshotView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[snapshotView(100)]", new NSLayoutFormatOptions(), null, views));

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


                    label.Text = ((CustomMKAnnotationView)annotationView).Location;
                    label2.Text = ((CustomMKAnnotationView)annotationView).Machine;
                    label3.Text = ((CustomMKAnnotationView)annotationView).Name;
                    label3.LineBreakMode = UILineBreakMode.WordWrap;
                    label3.Lines = 0;
                    
                    label.Frame = new CGRect(0, 0, 250, 20);
                    label2.Frame = new CGRect(0, 15, 250, 20);
                    label3.Frame = new CGRect(0, 25, 250, 70);
                    // Add your custom controls here

                    label.TextAlignment = UITextAlignment.Left;
                    label2.TextAlignment = UITextAlignment.Left;
                    label3.TextAlignment = UITextAlignment.Left;


                    //label.Font = UIFont.SystemFontOfSize(16);
                    label.Font = UIFont.BoldSystemFontOfSize(16);
                    label2.Font = UIFont.SystemFontOfSize(14);
                    label3.Font = UIFont.SystemFontOfSize(12);

                    //snapshotView.AddSubviews(label, label2, label3, uIButton);
                    snapshotView.AddSubviews(label, label2, label3);
                }
            });

            annotationView.DetailCalloutAccessoryView = snapshotView;
        }

        async void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            ItemsViewModel viewModel = new ItemsViewModel();
            if (!(customView.Machine == null))
            {
                await viewModel.CheckBoxItemsForMachine(customView.MachineID);
                var page = new CheckBoxContentPage(viewModel.CheckBoxItems);
                await PopupNavigation.Instance.PushAsync(page);

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