using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MolServiceContracts.BindingModels
{
    public class OneCImportBindingModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [Display(Name = "Логин")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
