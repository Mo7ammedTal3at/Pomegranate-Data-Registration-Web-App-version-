using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoorEl7abeebCompanyWebApp.Models.MyModels
{
    public class RestBoxes
    {
        public int Id { get; set; }
        [Display(Name ="العدد")]
        public int Count { get; set; } = 0;
        public int CustomerId { get; set; }
        public int BoxTypeId { get; set; }
        [ForeignKey("BoxTypeId")]
        public virtual BoxType BoxType { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}