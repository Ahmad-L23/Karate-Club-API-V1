namespace Dtos.UsersDTOs
{
    public class UserWithPersonDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        public string Password { get; set; } = "";

        
        public int PersonId { get; set; }
    }
}
