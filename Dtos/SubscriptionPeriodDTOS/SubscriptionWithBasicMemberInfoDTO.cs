using System;

namespace Dtos.SubscriptionPeriodDTOS
{
    public class SubscriptionWithBasicMemberInfoDTO
    {
        public int PeriodID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal Fees { get; set; }
        public int PaymentID { get; set; }

        public string? Name { get; set; }
        public string? ContactInfo { get; set; }

        public string? LastBeltRank { get; set; }
        public bool MemberIsActive { get; set; }
    }
}
