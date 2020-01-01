using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoorEl7abeebCompanyWebApp.Models.MyModels
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم العميل")]
        [Display(Name = "أسم العميل")]
        public string Name { get; set; }
        // navigation proparties
        public virtual List<Import> Imports { get; set; }
        public virtual List<Export> Exports { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual List<RestBoxes> RestBoxeses { get; set; }
    }
}