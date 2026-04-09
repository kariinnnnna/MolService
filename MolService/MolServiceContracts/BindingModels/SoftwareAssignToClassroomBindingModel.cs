using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MolServiceContracts.BindingModels
{
    public class SoftwareAssignToClassroomBindingModel
    {
        [Required(ErrorMessage = "Аудитория обязательна")]
        [Display(Name = "Аудитория")]
        public int ClassroomId { get; set; }

        [Required(ErrorMessage = "Программное обеспечение обязательно")]
        [Display(Name = "Программное обеспечение")]
        public int SoftwareId { get; set; }

        [StringLength(1000, ErrorMessage = "Описание установки не должно превышать 1000 символов")]
        [Display(Name = "Описание установки")]
        public string SetupDescription { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Номер заявки не должен превышать 200 символов")]
        [Display(Name = "Номер заявки")]
        public string ClaimNumber { get; set; } = string.Empty;
    }
}
