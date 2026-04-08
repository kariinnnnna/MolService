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

        [Required(ErrorMessage = "Дата списания обязательна")]
        [Display(Name = "Дата списания")]
        public DateTime MoveDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Причина списания обязательна")]
        [StringLength(1000, ErrorMessage = "Причина не должна превышать 1000 символов")]
        [Display(Name = "Причина списания")]
        public string Reason { get; set; } = string.Empty;

        [Required(ErrorMessage = "Количество обязательно")]
        [Range(typeof(decimal), "0,01", "999999999", ErrorMessage = "Количество должно быть больше 0")]
        [Display(Name = "Количество к списанию")]
        public decimal Quantity { get; set; }

        [StringLength(1000, ErrorMessage = "Комментарий не должен превышать 1000 символов")]
        [Display(Name = "Комментарий / основание")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оборудование обязательно")]
        [Display(Name = "Оборудование")]
        public int MaterialTechnicalValueId { get; set; }
    }
}
