using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MolServiceContracts.BindingModels
{
    public class EquipmentMovementHistoryBindingModel : IEquipmentMovementHistoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Дата перемещения обязательна")]
        [Display(Name = "Дата перемещения")]
        public DateTime MoveDate { get; set; }

        [StringLength(1000, ErrorMessage = "Причина не должна превышать 1000 символов")]
        [Display(Name = "Причина")]
        public string Reason { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оборудование обязательно")]
        [Display(Name = "Оборудование")]
        public int MaterialTechnicalValueId { get; set; }
    }
}
