using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models
{
    public class UserLocationServiceModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; } 
        public int TenantId { get; set; }
        public string  Location { get; set; }       
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CalculatedDistance { get; set; }

        public bool IsGenofenceViolated { get; set; }
    }
}
