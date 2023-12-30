using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;

namespace LeBrowsPremiere.Managers
{
    public class CustomerManager
    {
        public static async Task AddAsync(AppDbContext context, string? email, string? firstName, string? lastName)
        {
            await context.Customers.AddAsync(new Customer()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            });
            await context.SaveChangesAsync();
        }
    }
}
