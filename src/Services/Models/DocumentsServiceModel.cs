using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models
{
    public class DocumentsServiceModel
    {
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public int TenantId { get; set; }
        public int UserId { get; set; }

        public DateTime DateCreated { get; set; }  
        public DateTime DateModified { get; set; } 
        public IList<Fields> Fields { get; set; } = new List<Fields>();
       
    }

    public class Fields
    {
        public string Field { get; set; }
        public string Property { get; set; }
        public string DataType { get; set; }
        public string SetValue { get; set; }
        public string Value { get; set; }

    }
}
