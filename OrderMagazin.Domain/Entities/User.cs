namespace OrderManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Constructor fără parametri pentru EF Core
        public User() { }

        // Constructor opțional pentru inițializarea manuală
        public User(string name, string email, string phone)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}
