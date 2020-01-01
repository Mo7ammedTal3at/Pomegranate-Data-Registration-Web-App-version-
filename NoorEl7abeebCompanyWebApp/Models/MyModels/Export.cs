using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoorEl7abeebCompanyWebApp.Models.MyModels
{
    public class Export
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="من فضلك ادخل الوقت")]
        
        [DataType(DataType.DateTime)]
        [Display(Name = "الوقت")]
        public DateTime Date { get; set; }

        [Display(Name = "العدد")]
        [Required(ErrorMessage = "من فضلك ادخل العدد")]
        public int Quantity { get; set; }

        [Display(Name = "ملاحظات")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        [Required(ErrorMessage = "من فضلك اختر العميل")]
        [Display(Name ="اسم العميل")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]  
        public virtual Customer Customer { get; set; }
        [Required(ErrorMessage = "من فضلك اختر الصنف")]

        [Display(Name = "الصنف")]
        public int BoxTypeId { get; set; }
        [ForeignKey("BoxTypeId")]
        public virtual BoxType BoxType { get; set; }
    }
}