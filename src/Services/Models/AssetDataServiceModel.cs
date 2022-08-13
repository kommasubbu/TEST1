using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models
{
    public class AssetDataServiceModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public int assetId { get; set; }
        public int tenantId { get; set; }
        public Guid batchId { get; set; }
        public string tag { get; set; }
        public BsonDocument payload { get; set; }

        public AssetDataServiceModel()
        {
            this.assetId = 0;
            this.tenantId = 0;
            this.batchId = new Guid();
            this.payload = null;
            this.tag = string.Empty;
        }

    }
}
