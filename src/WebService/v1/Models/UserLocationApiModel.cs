using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class UserLocationApiModel
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "TenantId", Order = 10)]
        public int TenantId { get; set; }

        [JsonProperty(PropertyName = "UserId", Order = 20)]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "DateCreated", Order = 30)]
        public DateTimeOffset DateCreated { get; set; }


        [JsonProperty(PropertyName = "DateModified", Order = 40)]
        public DateTimeOffset DateModified { get; set; }

        [JsonProperty(PropertyName = "Location", Order = 50)]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "Latitude", Order = 60)]
        public double Latitude { get; set; }
        [JsonProperty(PropertyName = "Longitude", Order = 70)]
        public double Longitude { get; set; }
        [JsonProperty(PropertyName = "CalculatedDistance", Order = 80)]
        public int CalculatedDistance { get; set; }
        [JsonProperty(PropertyName = "IsGenofenceViolated", Order = 90)]
        public bool IsGenofenceViolated { get; set; }



        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "UserLocations;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/userlocation" }
        };
        public UserLocationApiModel() { }

        public UserLocationApiModel(UserLocationServiceModel locationinfo)
        {
            if (locationinfo != null)
            {
                this.DateCreated = locationinfo.DateCreated;
                this.Id = locationinfo.Id.ToString();
                this.DateModified = locationinfo.DateModified;
                this.TenantId = locationinfo.TenantId;
                this.Location = locationinfo.Location;
                this.Longitude = locationinfo.Longitude;
                this.Latitude = locationinfo.Latitude;             
                this.UserId = locationinfo.UserId;
                this.CalculatedDistance = locationinfo.CalculatedDistance;
                this.IsGenofenceViolated = locationinfo.IsGenofenceViolated;


            }

        }
        public UserLocationServiceModel ToServiceModel()
        {


            return new UserLocationServiceModel()
            {
                TenantId = this.TenantId,
                UserId = this.UserId,
                DateModified = this.DateModified,
                DateCreated = this.DateCreated,
                Latitude =this.Latitude,
                Longitude = this.Longitude,
                Location = this.Location,
                CalculatedDistance =this.CalculatedDistance,
                IsGenofenceViolated = this.IsGenofenceViolated,

            };
        }
    }
}
