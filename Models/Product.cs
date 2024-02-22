using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TailwindApi.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; } =  Guid.NewGuid().ToString();
        public string? Name { get; set; }
        [JsonProperty("brand")]
        public Brand? Brand { get; set; }
        public double? Price { get; set; }
        public int StockUnits { get; set; }
    }

    public class Brand 
    {
        public string? Name { get; set; }    
    }
}
