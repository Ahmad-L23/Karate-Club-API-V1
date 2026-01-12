using System.ComponentModel.DataAnnotations;

namespace Dtos.BeltRankDTOS
{
    public class CreateBeltRankDTO
    {
        public int BeltRankID { get; set; }

        [MaxLength(100, ErrorMessage = "RankName cannot exceed 100 characters")]
        public string? RankName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "TestFees must be a non-negative value")]
        public double TestFees { get; set; }
    }
}
