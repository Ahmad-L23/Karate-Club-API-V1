namespace Dtos.PaymentsDTOS
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }
    }
}
