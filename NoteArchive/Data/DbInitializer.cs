using NoteArchive.Models;
using Microsoft.AspNetCore.Identity;

namespace NoteArchive.Data
{
    public static class DbInitializer
    {
        public static void Initialize(NoteArchiveContext context)
        {
            context.Database.EnsureCreated();

            // --- USERS ---
            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User
                {
                    FirstMidName = "Ana",
                    LastName = "Kovač",
                    Email = "ana@example.com",
                    UserName = "ana@example.com",
                    NormalizedEmail = "ANA@EXAMPLE.COM",
                    NormalizedUserName = "ANA@EXAMPLE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                },

                new User
                {
                    FirstMidName = "Marko",
                    LastName = "Novak",
                    Email = "marko@example.com",
                    UserName = "marko@example.com",
                    NormalizedEmail = "MARKO@EXAMPLE.COM",
                    NormalizedUserName = "MARKO@EXAMPLE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            var passwordHasher = new PasswordHasher<User>();

            foreach (var user in users)
            {
                user.PasswordHash =
                    passwordHasher.HashPassword(user, "Test123!");

                context.Users.Add(user);
            }

            context.SaveChanges();

            // --- NOTES ---
            var notes = new Note[]
            {
                new Note
                {
                    Title = "Moonlight Sonata",
                    Author = "Ludwig van Beethoven",
                    Genre = "Classical",
                    Performer = "Piano",
                    PublicationDate = new DateTime(1801, 1, 1),
                    UserID = users[0].Id
                },

                new Note
                {
                    Title = "Nocturne Op.9 No.2",
                    Author = "Frederic Chopin",
                    Genre = "Romantic",
                    Performer = "Piano",
                    PublicationDate = new DateTime(1832, 1, 1),
                    UserID = users[1].Id
                }
            };

            context.Notes.AddRange(notes);
            context.SaveChanges();

            // --- NOTE IMAGES ---
            var images = new NoteImage[]
            {
                new NoteImage
                {
                    ImagePath = "/uploads/moonlight1.jpg",
                    NoteID = notes[0].ID
                },

                new NoteImage
                {
                    ImagePath = "/uploads/moonlight2.jpg",
                    NoteID = notes[0].ID
                },

                new NoteImage
                {
                    ImagePath = "/uploads/nocturne1.jpg",
                    NoteID = notes[1].ID
                },

                new NoteImage
                {
                    ImagePath = "/uploads/nocturne2.jpg",
                    NoteID = notes[1].ID
                },

                new NoteImage
                {
                    ImagePath = "/uploads/nocturne3.jpg",
                    NoteID = notes[1].ID
                }
            };

            context.NoteImages.AddRange(images);
            context.SaveChanges();

            // --- ROLES ---
            var roles = new IdentityRole[]
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },

                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role.Name))
                {
                    context.Roles.Add(role);
                }
            }

            context.SaveChanges();

            // --- ADMIN USER ---
            var admin = new User
            {
                FirstMidName = "Admin",
                LastName = "User",
                Email = "admin@notearchive.com",
                UserName = "admin@notearchive.com",
                NormalizedEmail = "ADMIN@NOTEARCHIVE.COM",
                NormalizedUserName = "ADMIN@NOTEARCHIVE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!context.Users.Any(u => u.UserName == admin.UserName))
            {
                admin.PasswordHash =
                    passwordHasher.HashPassword(admin, "Admin123!");

                context.Users.Add(admin);
                context.SaveChanges();
            }

            // --- ADMIN ROLE ASSIGN ---
            if (!context.UserRoles.Any(ur => ur.UserId == admin.Id))
            {
                context.UserRoles.Add(
                    new IdentityUserRole<string>
                    {
                        UserId = admin.Id,
                        RoleId = "1"
                    });

                context.SaveChanges();
            }
        }
    }
}