using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.MembersDTOS
{
    public class MemberDTO
    {
        public int MemberID { get; set; }
        public int PersonId { get; set; }
        public string? EmergencyContactInfo { get; set; }
        public int LastBeltRank {  get; set; }
        public bool isActive {  get; set; }
    }
}
