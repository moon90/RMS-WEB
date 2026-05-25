using System;
using System.Collections.Generic;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class Payroll : BaseEntity, IMultiTenant
    {
        public int PayrollID { get; set; }
        public int StaffID { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal BasePay { get; set; }
        public decimal CommissionEarned { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal TotalPay => BasePay + CommissionEarned + BonusAmount;
        public int? BranchID { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
