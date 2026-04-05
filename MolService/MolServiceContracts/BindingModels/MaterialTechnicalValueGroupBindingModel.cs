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
    public class MaterialTechnicalValueGroupBindingModel : IMaterialTechnicalValueGroupModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название группы обязательно")]
        [StringLength(200, ErrorMessage = "Название группы не должно превышать 200 символов")]
        [Display(Name = "Название группы")]
        public string GroupName { get; set; } = string.Empty;

        [Display(Name = "Порядок")]
        public int Order { get; set; }
    }
}
