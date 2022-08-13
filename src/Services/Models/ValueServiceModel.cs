using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models
{
    public class ValueServiceModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string objectid { get; set; }
       
        public string key { get; set; }
        public int tenantId { get; set; }

        public BsonDocument data { get; set; }

        public DateTime timestamp { get; set; }

        public ValueServiceModel()
        {
        }

      

    }

}
