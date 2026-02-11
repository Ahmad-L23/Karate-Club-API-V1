public class UserWithPersonDTO
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public int PersonId { get; set; }

    // Person info
    public string PersonName { get; set; } = "";
    public string Address { get; set; } = "";
    public string ContactInfo { get; set; } = "";
}
