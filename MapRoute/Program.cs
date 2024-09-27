using System.Text;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using MapRoute.Models;
using MapRoute.Common;

namespace MapRoute
{
    class Program
    {
        const string connectionString = @"Server=localhost;Database=mapdb;User Id=sa;Password=Dev1121!;TrustServerCertificate=True;";

        private static void RunInsertUpdateDistictAddressWalkListOrder(long id, int workOrder)
        {
            string procedureName = "usp_InsertUpdateDistictAddressWalkListOrder";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.BigInt)).Value = id;
                        cmd.Parameters.Add(new SqlParameter("@WalkOrder", System.Data.SqlDbType.BigInt)).Value = workOrder;

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private static List<Address> ReadAddresses()
        {
            string query = "SELECT * FROM FloridaVotersDistinctAddresses";
            var addresses = new List<Address>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    addresses.Add(new Address(){
                        ID = (Int64)reader["ID"],
                        CleanAddress = (string)reader["CleanAddress"],
                        Lat = Convert.ToDouble(reader["Lat"]),
                        Lon = Convert.ToDouble(reader["Lon"]),
                        WalkOrder = 0,
                    });
                }
            }

            return addresses;
        }

        private static async Task ComputeRouteMap(int subIndex, List<Address> addresses)
        {
            string apiKey = "AIzaSyCHeJyeuixXnqdWyn5018h7DEclejyk9u8";

            var originPoint = addresses.First();
            var destinationPoint = addresses.Last();

            var origin = new Waypoint
            {
                location = new Location { latLng = new LatLng { latitude = originPoint.Lat, longitude = originPoint.Lon } }
            };

            var destination = new Waypoint
            {
                location = new Location { latLng = new LatLng { latitude = destinationPoint.Lat, longitude = destinationPoint.Lon } }
            };

            var intermediates = new List<Waypoint>();

            foreach (var address in addresses)
            {
                intermediates.Add(new Waypoint
                {
                    location = new Location { latLng = new LatLng { latitude = address.Lat, longitude = address.Lon } }
                });
            }

            RouteRequest request = new RouteRequest
            {
                origin = origin,
                destination = destination,
                optimizeWaypointOrder = true,
                travelMode = "WALK",
                intermediates = intermediates
            };

            var requestBody = JsonConvert.SerializeObject(request);

            using (HttpClient client = new HttpClient())
            {
                string url = $"https://routes.googleapis.com/directions/v2:computeRoutes?key={apiKey}";

                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "*");

                HttpResponseMessage response = await client.PostAsync(url, requestContent);

                string responseBody = await response.Content.ReadAsStringAsync();

                await File.WriteAllTextAsync($"result_{subIndex}.json", responseBody);

                var result = JsonConvert.DeserializeObject<RouteResponse>(responseBody);

                if (result.routes.Count > 0)
                {
                    var orders = result.routes[0].optimizedIntermediateWaypointIndex;

                    for (int i = 0; i < orders.Count; i++)
                    {
                        int walkorder = subIndex * 25 + orders[i];
                        RunInsertUpdateDistictAddressWalkListOrder(addresses[i].ID, walkorder);
                    }
                }
            }
        }

        static async Task Main(string[] args)
        {
            var addresses = ReadAddresses();

            var count = addresses.Count;

            var subCount = count / 25 + (count % 25 == 0 ? 0 : 1);

            for (int i = 0; i < subCount; i++)
            {
                List<Address> subAddresses = i == subCount - 1 ?
                    addresses.GetRange(i * 25, count - i * 25) :
                    addresses.GetRange(i * 25, 25);
                await ComputeRouteMap(i, subAddresses);
            }
        }
    }
}