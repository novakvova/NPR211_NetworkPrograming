using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models.City
{
    public class SettlementRequestDTO
    {
        [JsonProperty(PropertyName = "apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty(PropertyName = "modelName")]
        public string ModelName { get; set; }
        [JsonProperty(PropertyName = "calledMethod")]
        public string CalledMethod { get; set; }
        [JsonProperty(PropertyName = "methodProperties")]
        public SettlementRequestPropertyDTO MethodProperties { get; set; }
    }
}
