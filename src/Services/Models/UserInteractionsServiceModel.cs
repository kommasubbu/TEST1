using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models
{
   public  class UserInteractionsServiceModel
    {
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int UserId { get; set; }
        public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
        public string DateModified { get; set; } = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
        public int TenantId { get; set; }
        public double DetectedDistance { get; set; }
        public int SignalStrength { get; set; }

        public int DetectedUserId { get; set; }

        public DateTime DetectedTime { get; set; }
        [BsonRepresentation(BsonType.String)]
        public UserDetectionStateEnum UserDetectionState { get; set; }
    }
    public enum UserDetectionStateEnum
    {

        Close,
        Near,
        Far
    }

}
