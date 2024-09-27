using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapRoute.Common
{
    internal class LatLng
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    internal class Location
    {
        public LatLng? latLng { get; set; }
    }

    internal class Waypoint
    {
        public Location? location { get; set; }
        public string? address { get; set; }
    }

    internal class Local
    {
        public string? text { get; set; }
    }

    internal class LocalizedValues
    {
        public Local? distance { get; set; }
        public Local? staticDuration { get; set; }
        public Local? duration { get; set; }
    }

    internal class Polyline
    {
        public string? encodedPolyline { get; set; }
    }

    internal class NavigationInstruction
    {
        public string? maneuver { get; set; }
        public string? instructions { get; set; }
    }

    internal class Step
    {
        public string staticDuration { get; set; }
        public Polyline? polyline { get; set; }
        public Location? startLocation { get; set; }
        public Location? endLocation { get; set; }
        public NavigationInstruction? navigationInstruction { get; set; }
        public LocalizedValues? localizedValues { get; set; }
        public string? travelMode { get; set; }
    }

    internal class Leg
    {
        public string? duration { get; set; }
        public string? staticDuration { get; set; }
        public Polyline polyline { get; set; }
        public Location? startLocation { get; set; }
        public Location? endLocation { get; set; }
        public List<Step> steps { get; set; }
        public LocalizedValues localizedValues { get; set; }
    }

    internal class Viewport
    {
        public LatLng? low { get; set; }
        public LatLng? high { get; set; }
    }
}
