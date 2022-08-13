using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class AssetDataApiModel
    {
        [JsonProperty(PropertyName = "AssetId")]
        public int assetId { get; set; }

        [JsonProperty(PropertyName = "TenantId")]
        public int tenantId { get; set; }
        [JsonProperty(PropertyName = "BatchId")]
        public Guid batchId { get; set; }
        [JsonProperty(PropertyName = "Payload")]
        public string payload { get; set; }
        [JsonProperty(PropertyName = "Tag")]
        public string tag { get; set; }
        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "AssetData;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/AssetData" }
        };
        public AssetDataApiModel(AssetDataServiceModel AssetInfo)
        {
            if (AssetInfo != null)
            {
                this.batchId = AssetInfo.batchId;
                this.assetId = AssetInfo.assetId;
                this.tenantId = AssetInfo.tenantId;
                this.tag = AssetInfo.tag;
                this.payload = AssetInfo.payload.ToJson();
            }

        }

        public AssetDataServiceModel ToServiceModel()
        {
            return new AssetDataServiceModel()
            {
                tenantId = this.tenantId,
                batchId = this.batchId,
                assetId = this.assetId,
                tag = this.tag,
                payload = BsonSerializer.Deserialize<BsonDocument>(this.payload)

            };
        }
    }
}
