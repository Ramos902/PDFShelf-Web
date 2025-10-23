public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public double StorageUsedMB { get; set; }
    public int PlanId { get; set; }
    public string Token { get; set; } = string.Empty; // JWT
}
