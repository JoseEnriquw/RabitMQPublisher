using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();

            builder.HasData(

                new User
                {
                    Id = 1,
                    Name = "User 1",
                    Email = "user1@gmail.com",
                    CreatedAt = new(2023, 7, 20)
                },
                new User
                {
                    Id = 2,
                    Name = "User 2",
                    Email = "user2@gmail.com",
                    CreatedAt = new(2023, 7, 21)
                },
                new User
                {
                    Id = 3,
                    Name = "User 3",
                    Email = "user3@gmail.com",
                    CreatedAt = new(2023, 7, 22)
                },
                new User
                {
                    Id = 4,
                    Name = "User 4",
                    Email = "user4@gmail.com",
                    CreatedAt = new(2023, 7, 23)
                }
                );
        }

    }
}
