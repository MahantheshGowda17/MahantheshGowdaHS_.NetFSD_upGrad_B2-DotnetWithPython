namespace EMS.API.DTOs
{
    public class EmployeeDtos
    {
        public class RegisterRequestDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; } = "Viewer";
        }

        public class LoginRequestDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class AuthResponseDto
        {
            public bool Success { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public string Token { get; set; }
            public string Message { get; set; }
        }
    }
}
