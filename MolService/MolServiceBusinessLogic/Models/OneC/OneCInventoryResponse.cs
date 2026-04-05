using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Models.OneC
{
    public class OneCInventoryResponse
    {
        [JsonPropertyName("ОС")]
        public List<OneCInventoryItem> Items { get; set; } = new();
    }
}
