using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapRoute.Common
{
    internal class RouteResponse
    {
        public List<Route> routes { get; set; }
        public object? geocodingResults {  get; set; }
    }

    internal class Route
    {
        public List<Leg> legs { get; set; }
        public int distanceMeters { get; set; }
        public string duration { get; set; }
        public string staticDuratin { get; set; }
        public Polyline polyline { get; set; }
        public string description { get; set; }
        public List<int> optimizedIntermediateWaypointIndex { get; set; }
        public LocalizedValues localizedValues { get; set; }
        public List<string> routeLabels { get; set; }
        public object travelAdvisory { get; set; }
        public Viewport viewport { get; set; }
    }
}
