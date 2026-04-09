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
    public class MaterialResponsiblePersonBindingModel : IMaterialResponsiblePersonModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(200, ErrorMessage = "ФИО не должно превышать 200 символов")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Должность не должна превышать 150 символов")]
        [Display(Name = "Должность")]
        public string Position { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Некорректный формат телефона")]
        [StringLength(30, ErrorMessage = "Телефон не должен превышать 30 символов")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [StringLength(150, ErrorMessage = "Email не должен превышать 150 символов")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
