using System;

namespace Dtos.BeltTestsDTOS
{
    public class BeltTestDTO
    {
        public int TestID { get; set; }
        public int MemberID { get; set; }
        public int RankID { get; set; }
        public bool Result { get; set; }
        public DateTime TestDate { get; set; }
        public int TestByInstructor { get; set; }
        public int? PaymentID { get; set; }
    }
}
