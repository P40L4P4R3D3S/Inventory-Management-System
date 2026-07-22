namespace Inventory_Management_System.Api.Models.Requests
{
    public class LoginRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
