using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MolServiceDataModels.Enums
{
    public enum ClassroomType
    {
        [Display(Name = "Лекционная")]
        Lecture = 0,

        [Display(Name = "Практическая")]
        Practice = 1,

        [Display(Name = "Компьютерная")]
        Computer = 2,

        [Display(Name = "Лаборатория")]
        Laboratory = 3,

        [Display(Name = "Другое")]
        Other = 4
    }
}
