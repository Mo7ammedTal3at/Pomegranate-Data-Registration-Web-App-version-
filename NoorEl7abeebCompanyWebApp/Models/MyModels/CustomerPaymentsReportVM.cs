using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoorEl7abeebCompanyWebApp.Models.MyModels
{
    public class CustomerPaymentsReportVM
    {
        [Display(Name ="اسم العميل")]
        public string CustomerName { get; set; }
        [Display(Name = "المدفوع")]
        [DataType(DataType.Currency)]
        public double PaidForHim { get; set; } = 0;
        [Display(Name = "المتبقى")]
        [DataType(DataType.Currency)]
        public double RestPaymentsForHim { get; set; } = 0;
        [Display(Name = "الاجمالى")]
        [DataType(DataType.Currency)]
        public double TotalPayments { get; set; } = 0;
    }
}