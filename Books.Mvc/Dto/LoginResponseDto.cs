namespace Books.API.Models.Dto
{  
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }


    public class UserDto
    {
    }

}