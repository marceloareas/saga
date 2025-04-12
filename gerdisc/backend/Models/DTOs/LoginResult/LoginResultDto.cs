namespace saga.Models.DTOs
{
    public class LoginResultDto
    {
        public string? Token { get; set; }
        public UserDto? User { get; set; }
    }
}
