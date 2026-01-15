using System;

namespace Dtos.SubscriptionPeriodDTOS
{
    public class SubscriptionPeriodDTO
    {
        public int PeriodID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal Fees { get; set; }
        public int memberId { get; set; }
        public int PaymentID { get; set; }
    }
}
