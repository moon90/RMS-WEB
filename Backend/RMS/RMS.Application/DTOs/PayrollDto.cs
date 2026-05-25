using System;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs
{
    public class PayrollDto
    {
        public int PayrollID { get; set; }
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string Role { get; set; }
        public DateTime PayDate { get; set; }
        public decimal BasePay { get; set; }
        public decimal CommissionEarned { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal TotalPay { get; set; }
    }

    public class CreatePayrollDto
    {
        public int StaffID { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal BonusAmount { get; set; } = 0;
    }
}
