namespace UserManagementApi.Web.Models;

public class EditUserRequest
{
    public string Name { get; set; } = "";
    public string Login { get; set; } = "";
    public string Email { get; set; } = "";
}