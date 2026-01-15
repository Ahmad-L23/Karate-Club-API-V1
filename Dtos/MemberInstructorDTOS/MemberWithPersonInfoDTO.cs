using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.MemberInstructorDTOS
{
    public class MemberWithPersonInfoDTO
    {
        public int MemberID { get; set; }
        public string? Name { get; set; }
        public string? EmergencyContactInfo { get; set; }
        public int LastBeltRank { get; set; }
        public bool IsActive { get; set; }
    }
}
