namespace MapRoute.Common
{
    internal class RouteRequest
    {
        public Waypoint origin { get; set; }
        public Waypoint destination { get; set; }
        public bool optimizeWaypointOrder { get; set; }
        public List<Waypoint> intermediates { get; set; }
        public string travelMode { get; set; }
    }
}
