using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class DocumentsApiModel
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = string.Empty;
       

        [JsonProperty(PropertyName = "TenantId", Order = 10)]
        public int TenantId { get; set; }

        [JsonProperty(PropertyName = "UserId", Order = 20)]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "CreatedDate",  NullValueHandling = NullValueHandling.Ignore,Order = 30)]
        public DateTime DateCreated { get; set; }

       
        [JsonProperty(PropertyName = "DateModified", NullValueHandling = NullValueHandling.Ignore, Order = 40)]
        public DateTime DateModified { get; set; } 

        [JsonProperty(PropertyName = "Fields", Order = 50)]
        public IList<FieldsApiModel> Fields { get; set; } = new List<FieldsApiModel>();

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "Documents;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/documents" }
        };
        public DocumentsApiModel() { }

        public DocumentsApiModel(DocumentsServiceModel Documents)
        {
            if (Documents != null)
            {
                this.DateCreated = Documents.DateCreated;
                this.Id = Documents.Id.ToString();
                this.DateModified = Documents.DateModified;
                this.TenantId = Documents.TenantId;
                this.UserId = Documents.UserId;
                if (Documents.Fields.Count > 0)
                {
                    this.Fields = new List<FieldsApiModel>();
                }

                foreach (var fields in Documents.Fields)
                {
                    this.Fields.Add(new FieldsApiModel(fields));
                }

            }

        }
        public DocumentsServiceModel ToServiceModel()
        {
            var fields = new List<Fields>();
          

            if (this.Fields != null)
            {
                foreach (var field in this.Fields)
                {
                    fields.Add(field.ToServiceModel());
                }
            }

            return new DocumentsServiceModel()
            {
                TenantId = this.TenantId,
                UserId = this.UserId,
                DateModified = this.DateModified,
                DateCreated = this.DateCreated,
                Fields = fields
            };
        }

    }
}
