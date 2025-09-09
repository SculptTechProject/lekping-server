namespace lekping.server.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Email { get; private set; } = null!;
        public string Name { get; private set; } = "";
        public string PasswordHash { get; private set; } = null!;
        public string Role { get; private set; } = "User"; // or "Admin"
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public User(string email) => Email = email.ToLowerInvariant();
        public void SetName(string name) => Name = name;
        public void SetPasswordHash(string hash) => PasswordHash = hash;
        public void SetRole(string role) => Role = role;
    }
}
