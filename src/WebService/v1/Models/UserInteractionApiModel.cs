using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class UserInteractionApiModel
    {

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = string.Empty;
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";

        [JsonProperty(PropertyName = "TenantId", Order = 10)]
        public int TenantId { get; set; }

        [JsonProperty(PropertyName = "UserId", Order = 20)]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "CreatedDate", Order = 30)]
        public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);


        [JsonProperty(PropertyName = "DateModified", Order = 40)]
        public string DateModified { get; set; } = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);

        [JsonProperty(PropertyName = "DetectedDistance", Order = 50)]
        public double  DetectedDistance { get; set; }

        [JsonProperty(PropertyName = "SignalStrength", Order = 60)]
        public int SignalStrength { get; set; }


        [JsonProperty(PropertyName = "DetectedUserId", Order = 70)]
        public int DetectedUserId { get; set; }

        [JsonProperty(PropertyName = "DetectedTime", Order = 80)]
        public DateTime DetectedTime { get; set; }

        [JsonProperty(PropertyName = "UserDetectionState", Order = 80)]
        public UserDetectionStateEnum UserDetectionState { get; set; }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "UserInteractions;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/userinteraction" }
        };
        public UserInteractionApiModel() { }

        public UserInteractionApiModel(UserInteractionsServiceModel interactions)
        {
            if (interactions != null)
            {
                this.DateCreated = interactions.DateCreated;
                this.Id = interactions.Id.ToString();
                this.DateModified = interactions.DateModified;
                this.TenantId = interactions.TenantId;
                this.DetectedTime = interactions.DetectedTime;
                this.DetectedDistance = interactions.DetectedDistance;
                this.DetectedUserId = interactions.DetectedUserId;
                this.UserDetectionState = interactions.UserDetectionState;
                this.SignalStrength = interactions.SignalStrength;
                this.UserId = interactions.UserId;


            }

        }
        public UserInteractionsServiceModel ToServiceModel()
        {
          

            return new UserInteractionsServiceModel()
            {
                TenantId = this.TenantId,
                UserId = this.UserId,
                DateModified = this.DateModified,
                DateCreated = this.DateCreated,
                DetectedUserId=this.DetectedUserId,
                DetectedDistance=this.DetectedDistance,
                DetectedTime=this.DetectedTime,
                UserDetectionState=this.UserDetectionState,
                SignalStrength=this.SignalStrength

               
            };
        }
    }
}
