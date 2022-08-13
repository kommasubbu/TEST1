using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class UserLocationListApiModel
    {
        private List<UserLocationApiModel> items;
        [JsonProperty(PropertyName = "Items")]
        public List<UserLocationApiModel> Items
        {
            get { return this.items; }
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public IDictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "UserLocationsList;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/userlocation" },
        };

        public UserLocationListApiModel(List<UserLocationServiceModel> interactions)
        {
            this.items = new List<UserLocationApiModel>();
            if (interactions != null)
            {
                foreach (var interaction in interactions)
                {
                    this.items.Add(new UserLocationApiModel(interaction));
                }
            }
        }
    }
}
