namespace Dtos.BeltTestsDTOS
{
    public class BeltTestViewDTO
    {
        public int TestID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int RankID { get; set; }
        public string RankName { get; set; } = string.Empty;
        public bool Result { get; set; }
        public DateTime TestDate { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public int InstructorID { get; set; }  
    }
}
