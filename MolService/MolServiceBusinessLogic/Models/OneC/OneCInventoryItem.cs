using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Models.OneC
{
    public class OneCInventoryItem
    {
        [JsonPropertyName("ОсновноеСредствоНаименование")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("ИнвентарныйНомер")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("ЦМОНаименование")]
        public string MolWithLocation { get; set; } = string.Empty;

        [JsonPropertyName("СчетУчета")]
        public string Account { get; set; } = string.Empty;
    }
}
