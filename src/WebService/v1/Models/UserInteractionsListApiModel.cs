using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class UserInteractionsListApiModel
    {
        private List<UserInteractionApiModel> items;
        [JsonProperty(PropertyName = "Items")]
        public List<UserInteractionApiModel> Items
        {
            get { return this.items; }
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public IDictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "UserInteractionsList;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/userinteractions" },
        };

        public UserInteractionsListApiModel(List<UserInteractionsServiceModel> interactions)
        {
            this.items = new List<UserInteractionApiModel>();
            if (interactions != null)
            {
                foreach (var interaction in interactions)
                {
                    this.items.Add(new UserInteractionApiModel(interaction));
                }
            }
        }
    }
}
