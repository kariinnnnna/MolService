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
    public class MaterialTechnicalValueRecordBindingModel : IMaterialTechnicalValueRecordModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Группа обязательна")]
        [Display(Name = "Группа")]
        public int MaterialTechnicalValueGroupId { get; set; }

        [Required(ErrorMessage = "Оборудование обязательно")]
        [Display(Name = "Оборудование")]
        public int MaterialTechnicalValueId { get; set; }

        [Required(ErrorMessage = "Название поля обязательно")]
        [StringLength(200, ErrorMessage = "Название поля не должно превышать 200 символов")]
        [Display(Name = "Название поля")]
        public string FieldName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Значение поля не должно превышать 1000 символов")]
        [Display(Name = "Значение поля")]
        public string FieldValue { get; set; } = string.Empty;

        [Display(Name = "Порядок")]
        public int Order { get; set; }
    }
}
