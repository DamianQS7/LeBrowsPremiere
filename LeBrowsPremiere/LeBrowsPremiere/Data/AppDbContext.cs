using LeBrowsPremiere.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeBrowsPremiere.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string password = "Admin123#";
            string roleName = "Admin";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            // if username doesn't exist, create it and add it to role
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // call base class version to setup Identity relations:
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Province>().HasData(
                new Province { ProvinceId = 1, ProvinceName = "Alberta" },
                new Province { ProvinceId = 2, ProvinceName = "British Columbia" },
                new Province { ProvinceId = 3, ProvinceName = "Manitoba" },
                new Province { ProvinceId = 4, ProvinceName = "New Brunswick" },
                new Province { ProvinceId = 5, ProvinceName = "Newfoundland and Labrador" },
                new Province { ProvinceId = 6, ProvinceName = "Northwest Territories" },
                new Province { ProvinceId = 7, ProvinceName = "Nova Scotia" },
                new Province { ProvinceId = 8, ProvinceName = "Nunavut" },
                new Province { ProvinceId = 9, ProvinceName = "Ontario" },
                new Province { ProvinceId = 10, ProvinceName = "Prince Edward Island" },
                new Province { ProvinceId = 11, ProvinceName = "Quebec" },
                new Province { ProvinceId = 12, ProvinceName = "Saskatchewan" },
                new Province { ProvinceId = 13, ProvinceName = "Yukon" }
                );

            //Seed initial services data
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Name = "Ombre Powder Brows (2 sessions)",
                    Description = "Ombré shading is the newest trend in eyebrow semi-permanent makeup. " +
                    "It gives a soft shaded brow pencil look. Ombre powder brows are great for any skin type, " +
                    "less abrasive than microblading leave your eyebrows look fuller and more defined. " +
                    "Last longer usually up to 2 years. Rate is for 2 sessions with after care kit.)",
                    Duration = 120,
                    Price = 299
                },

                new Service
                {
                    Id = 2,
                    Name = "Classic Lashes (Full Set)",
                    Description = "Classic eyelash extensions are a simple, beautiful, natural-looking lash style. " +
                     "They are applied on a 1:1 ratio, which means one extension is attached to one natural lash. " +
                     "This gives you a natural looking lash.",
                    Duration = 90,
                    Price = 60
                },

                new Service
                {
                    Id = 3,
                    Name = "Hybrid Lashes (Full Set)",
                    Description = "Hybrid sets combine the classic and volume technique into one. " +
                     "In this method, some natural lashes get a single extension while others get multiple. " +
                     "Possessing the definition of the classic and the fluffiness of the volume, " +
                     "the hybrid set is the best of both worlds!",
                    Duration = 120,
                    Price = 75
                },

                new Service
                {
                    Id = 4,
                    Name = "Volume Lashes (Full Set)",
                    Description = "Uses 5D up to 20D but still super lightweight lashes that are crafted into a fan " +
                     "before being applied to individual natural lashes. This technique increases your lash count giving " +
                     "you a dramatic look with unrivalled fullness.",
                    Duration = 150,
                    Price = 85
                },

                new Service
                {
                    Id = 5,
                    Name = "Ombre Powder Brows (1 session)",
                    Description = "Ombré shading is the newest trend in eyebrow semi-permanent makeup. " +
                    "It gives a soft shaded brow pencil look. Ombre powder brows are great for any skin type, " +
                    "less abrasive than microblading leave your eyebrows look fuller and more defined. " +
                    "Last longer usually up to 2 years. (Rate is per session with after care kit.) ",
                    Duration = 120,
                    Price = 180
                },

                new Service
                {
                    Id = 6,
                    Name = "Classic Lashes (Refill)",
                    Description = "Classic eyelash extensions are a simple, beautiful, natural-looking lash style. " +
                     "They are applied on a 1:1 ratio, which means one extension is attached to one natural lash. " +
                     "This gives you a natural looking lash. (Refill is 2-3 weeks with at least 50% lashes.)",
                    Duration = 90,
                    Price = 40
                },

                new Service
                {
                    Id = 7,
                    Name = "Hybrid Lashes (Refill)",
                    Description = "Hybrid sets combine the classic and volume technique into one. " +
                     "In this method, some natural lashes get a single extension while others get multiple. " +
                     "Possessing the definition of the classic and the fluffiness of the volume, " +
                     "the hybrid set is the best of both worlds! (Refill is 2-3 weeks with at least 50% lashes.)",
                    Duration = 90,
                    Price = 55
                },

                new Service
                {
                    Id = 8,
                    Name = "Volume Lashes (Refill)",
                    Description = "Uses 5D up to 20D but still super lightweight lashes that are crafted into a fan " +
                     "before being applied to individual natural lashes. This technique increases your lash count giving " +
                     "you a dramatic look with unrivalled fullness. (Refill is 2-3 weeks with at least 50% lashes.)",
                    Duration = 120,
                    Price = 65
                }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Eyebrows" },
                new Category { CategoryId = 2, Name = "Eyelashes" }
                );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    SupplierId = 1,
                    CompanyName = "FakeCompany",
                    ContactFirstName = "Steve",
                    ContactLastName = "Jobs",
                    ContactEmail = "stevejobs@fakecompany.com",
                    ContactPhone = "519-547-8965"
                }
                );

            modelBuilder.Entity<Setting>().HasData(
                    new Setting { Id = 1, Code = "AppointmentStartTime", Value = DateTime.MinValue.AddHours(10).ToString(), CreatedDate = DateTime.Now, CreatedBy = "admin" },
                    new Setting { Id = 2, Code = "AppointmentEndTime", Value = DateTime.MinValue.AddHours(16).ToString(), CreatedDate = DateTime.Now, CreatedBy = "admin" },
                    new Setting { Id = 3, Code = "AppointmentIntervalInMinutes", Value = "30", CreatedDate = DateTime.Now, CreatedBy = "admin" }
                );

            modelBuilder.Entity<Blog>().HasData(
                    new Blog { Id = 1, Title = "The Ultimate Guide to Achieving Beautiful Eyelashes", Description = "In this blog post, we'll explore the various ways you can enhance the beauty of your eyelashes, including tips for mascara application, eyelash curling, and natural remedies for promoting lash growth.", CreatedDate = DateTime.Now.AddDays(-5) },
                    new Blog { Id = 2, Title = "Lash Extensions: Are They Right for You?", Description = "If you're looking for a low-maintenance way to achieve long, luscious lashes, lash extensions might be just the thing for you. In this post, we'll explore the pros and cons of lash extensions and provide tips for taking care of them.", CreatedDate = DateTime.Now.AddDays(-15) },
                    new Blog { Id = 3, Title = "How to Care for Your Eyelashes: A Beginner's Guide", Description = "Just like any other part of your body, your eyelashes require proper care and maintenance to stay healthy and beautiful. In this post, we'll cover the basics of eyelash care, including how to remove makeup without damaging your lashes and how to choose the right mascara for your needs.", CreatedDate = DateTime.Now.AddDays(-25) },
                    new Blog { Id = 4, Title = "Natural Remedies for Promoting Eyelash Growth", Description = "If you're looking for a more natural approach to achieving longer, fuller lashes, this post is for you. We'll explore some of the best natural remedies for promoting eyelash growth, including castor oil, coconut oil, and vitamin E.", CreatedDate = DateTime.Now.AddDays(-35) },
                    new Blog { Id = 5, Title = "The Benefits of Lash Serums for Your Eye Health", Description = "Did you know that using a lash serum can not only enhance the appearance of your lashes, but also promote better eye health? In this post, we'll explore the benefits of using a lash serum, as well as provide tips for choosing the right one for your needs.", CreatedDate = DateTime.Now.AddDays(-45) }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
					Id = 1,
					Name = "Eyelash Shampoo",
                    Brand = "Premieré",
					Description = "Removes germ and dust between natural eyelashes with consistent foaming formula eyelashes. 50 mL",
					Price = 12,
					ImageUrl = @"\img\products\lash_shampoo.JPG",
                    MinimumStock = 1,
                    CurrentStock = 0,
					CategoryId = 2,
					SupplierId = 1
				},
				new Product
				{
					Id = 2,
					Name = "Lash Remover",
					Brand = "Premieré",
					Description = "Latex and Formaldehyde Free. 10 g",
					Price = 9,
					ImageUrl = @"\img\products\lash_remover.JPG",
					MinimumStock = 1,
					CurrentStock = 0,
					CategoryId = 2,
					SupplierId = 1
				},
				new Product
				{
					Id = 3,
					Name = "Lash Primer Cleanser",
					Brand = "Premieré",
					Description = "Latex and Formaldehyde Free. 15 mL",
					Price = 10,
					ImageUrl = @"\img\products\lash_primer.JPG",
					MinimumStock = 1,
					CurrentStock = 0,
					CategoryId = 2,
					SupplierId = 1
				},
				new Product
				{
					Id = 4,
					Name = "Eyelash Glue",
					Brand = "Premieré",
					Description = "Latex and Formaldehyde Free. 5 mL",
					Price = 25,
					ImageUrl = @"\img\products\lash_glue.JPG",
					MinimumStock = 1,
					CurrentStock = 0,
					CategoryId = 2,
					SupplierId = 1
				}
				);
        }
    }
}
