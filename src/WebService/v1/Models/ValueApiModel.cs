using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class ValueApiModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string objectid { get; set; }


        [JsonProperty("key")]
        public string key { get; set; }

        [JsonProperty("data")]
        public string data { get; set; }

        [JsonProperty("timestamp")]
        public DateTime timestamp { get; set; }

        [JsonProperty("tenantId")]
        public int tenantId { get; set; }


        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata;
        public ValueApiModel() { }
        public ValueApiModel(ValueServiceModel model)
        {
            this.objectid = model.objectid;
            this.key = model.key;
            this.data = model.data.ToJson();
            this.timestamp = model.timestamp;
            this.tenantId = model.tenantId;

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"Value;{Version.NUMBER}" },
                { "$modified", model.timestamp.ToString(CultureInfo.InvariantCulture) },
                { "$uri", $"/{Version.PATH}/collections/{model.key}" }
            };
        }

        public ValueServiceModel ToServiceModel()
        {


            return new ValueServiceModel()
            {
                objectid = this.objectid,
                key = this.key,
                data = BsonSerializer.Deserialize<BsonDocument>(this.data),
                timestamp = this.timestamp,
                tenantId = this.tenantId,

            };
        }
    }

}
