using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class AssetDataListApiModel
    {
        private List<AssetDataApiModel> items;
        [JsonProperty(PropertyName = "Items")]
        public List<AssetDataApiModel> Items
        {
            get { return this.items; }
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public IDictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "AssetDAtaList;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/AssetData" },
        };

        public AssetDataListApiModel(List<AssetDataServiceModel> interactions)
        {
            this.items = new List<AssetDataApiModel>();
            if (interactions != null)
            {
                foreach (var interaction in interactions)
                {
                    this.items.Add(new AssetDataApiModel(interaction));
                }
            }
        }
    }
}
