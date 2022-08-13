using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class DocumentsListApiModel
    {
        private List<DocumentsApiModel> items;
        [JsonProperty(PropertyName = "Items")]
        public List<DocumentsApiModel> Items
        {
            get { return this.items; }
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public IDictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "DocumentsList;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/documents" },
        };

        public DocumentsListApiModel(List<DocumentsServiceModel> documents)
        {
            this.items = new List<DocumentsApiModel>();
            if (documents != null)
            {
                foreach (var document  in documents)
                {
                    this.items.Add(new DocumentsApiModel(document));
                }
            }
        }
    }
}
