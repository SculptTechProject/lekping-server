namespace lekping.server.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Email { get; private set; } = null!;
        public string Name { get; private set; } = "";
        public string PasswordHash { get; private set; } = null!;
        public List<string> Roles { get; private set; } = new();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string Timezone { get; private set; } = "Europe/Warsaw";
        public ICollection<Med> Meds { get; private set; } = new List<Med>(); // relation 1:N

        public User(string email) => Email = email.ToLowerInvariant();
        public void SetName(string name) => Name = name;
        public void SetPasswordHash(string hash) => PasswordHash = hash;
        public void AddRole(string role)
        {
            if (!string.IsNullOrWhiteSpace(role) && !Roles.Contains(role))
                Roles.Add(role);
        }
        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    }
}
