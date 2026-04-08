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
    public class MaterialTechnicalValueBindingModel : IMaterialTechnicalValueModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Инвентарный номер обязателен")]
        [StringLength(100, ErrorMessage = "Инвентарный номер не должен превышать 100 символов")]
        [Display(Name = "Инвентарный номер")]
        public string InventoryNumber { get; set; } = string.Empty;

        [Display(Name = "Аудитория")]
        public int? ClassroomId { get; set; }

        [Required(ErrorMessage = "Наименование обязательно")]
        [StringLength(200, ErrorMessage = "Наименование не должно превышать 200 символов")]
        [Display(Name = "Наименование")]
        public string FullName { get; set; } = string.Empty;
        [Range(0, double.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        [Display(Name = "Количество")]
        public decimal Quantity { get; set; }

        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Местоположение не должно превышать 200 символов")]
        [Display(Name = "Местоположение")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Материально ответственное лицо обязательно")]
        [Display(Name = "Материально ответственное лицо")]
        public int MaterialResponsiblePersonId { get; set; }
    }
}
