using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models
{
    public class FieldsApiModel
    {
        [JsonProperty(PropertyName = "Field")]
        public string Field { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "Property")]
        public string Property { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "DataType")]
        public string DataType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "SetValue")]
        public string SetValue { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; } = string.Empty;
        public FieldsApiModel()
        {

        }

        public FieldsApiModel(Fields Field)
        {
            if (Field != null)
            {
                this.Field = Field.Field;
                this.DataType = Field.DataType.ToString();
                this.Value = Field.Value;
                this.Property = Field.Property;
                this.SetValue = Field.SetValue;
            }
        }

        public Fields ToServiceModel()
        {
          
            return new Fields()
            {
                Field = this.Field,
                Property = Property,
                Value = this.Value,
                DataType = this.DataType,
                SetValue = this.SetValue
            };
        }
    }
}
