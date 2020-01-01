using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoorEl7abeebCompanyWebApp.Models.MyModels
{
    public class BoxType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك اسم النوع")]
        [Display(Name ="الصنف")]
        public string Name { get; set; }
        public virtual List<Import> Imports { get; set; }
        public virtual List<Export> Exports { get; set; }
        public virtual List<RestBoxes> RestBoxeses { get; set; }

    }
}