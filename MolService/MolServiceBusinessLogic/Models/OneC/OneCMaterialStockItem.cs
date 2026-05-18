using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Models.OneC
{
    public class OneCMaterialStockItem
    {
        [JsonPropertyName("Номенклатура")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("НоменклатураКод")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("Количество")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("МОЛ")]
        public string Mol { get; set; } = string.Empty;
    }
}
