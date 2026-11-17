namespace ApprovalService.Services;

public class UserManagementClient : IUserManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserManagementClient> _logger;

    public UserManagementClient(
        IHttpClientFactory httpClientFactory,
        ILogger<UserManagementClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("UserManagementApi");
        _logger = logger;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"api/users/{userId}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "User with ID {UserId} was not found in UserManagementApi",
                userId);

            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }
}