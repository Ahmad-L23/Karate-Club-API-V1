using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.MembersDTOS
{
    public class FIndMemberDTO
    {
        public int MemberID { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string? Address {  get; set; }
        public string? ContacntInfo {  get; set; }
        public string? EmergencyContactInfo { get; set; }
        public int LastBeltRank { get; set; }
        public string LastBeltRankName { get; set; }
        public double BeltRankFees {  get; set; }
        public bool isActive { get; set; }
    }
}
