namespace Quiz.App.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}