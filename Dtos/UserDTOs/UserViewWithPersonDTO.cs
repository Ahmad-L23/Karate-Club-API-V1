namespace Dtos.UsersDTOs
{
    public class UserViewWithPersonDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        // No Password Here ❌

        public int PersonId { get; set; }

        // Person Info
        public string PersonName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
