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
    public class SoftwareBindingModel : ISoftwareModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Наименование ПО обязательно")]
        [StringLength(200, ErrorMessage = "Наименование ПО не должно превышать 200 символов")]
        [Display(Name = "Наименование ПО")]
        public string SoftwareName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        [Display(Name = "Описание")]
        public string SoftwareDescription { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Ключ не должен превышать 500 символов")]
        [Display(Name = "Ключ")]
        public string SoftwareKey { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Дополнительный ключ не должен превышать 500 символов")]
        [Display(Name = "Дополнительный ключ")]
        public string SoftwareK { get; set; } = string.Empty;
    }
}
