using HotDesk.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace HotDesk
{
    public class HotDeskSeeder
    {
        private readonly HotDeskDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public HotDeskSeeder(HotDeskDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    _dbContext.Roles.AddRange(SeedDataRoles());
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Locations.Any())
                {
                    _dbContext.Locations.AddRange(SeedDataLocations());
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any())
                {
                    var users = SeedDataUsers();

                    var admin = users.FirstOrDefault(user => user.RoleId == 1);
                    var employee = users.FirstOrDefault(user => user.RoleId == 2);

                    if(admin is not null && employee is not null)
                    {
                        var hashedAdminPassword = _passwordHasher.HashPassword(admin, admin.Password);
                        var hashedEmployeePassword = _passwordHasher.HashPassword(employee, employee.Password);

                        admin.Password = hashedAdminPassword;
                        employee.Password = hashedEmployeePassword;

                        _dbContext.Users.Add(admin);
                        _dbContext.Users.Add(employee);
                        _dbContext.SaveChanges();
                    }
                }
            }
        }

        private IEnumerable<Role> SeedDataRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    RoleName = "Administrator"
                },
                new Role()
                {
                    RoleName = "Employee"
                }
            };

            return roles;
        }

        private IEnumerable<Location> SeedDataLocations()
        {
            var locations = new List<Location>()
            {
                new Location()
                {
                    Country = "Poland",
                    City = "Gdansk",
                    Address = "Losowa 23",
                    PostalCode = "80-888",
                    Desks = new List<Desk>
                    { 
                        new Desk()
                        {
                            Description = "Big desk located next to the window"
                        },
                        new Desk()
                        {
                            Description = "Computer workstation for one employee"
                        },
                        new Desk()
                        {
                            Description = "Computer workstation for one employee"
                        },
                        new Desk()
                        {
                            Description = "Computer workstation for one employee"
                        },
                        new Desk()
                        {
                            Description = "Big desk located next to the window"
                        },
                        new Desk()
                        {
                        },
                    }
                    
                },
                new Location()
                {
                    Country = "Poland",
                    City = "Cracow",
                    Address = "Marcepanowa 123",
                    PostalCode = "12-234",
                    Desks = new List<Desk>
                    {
                        new Desk()
                        {
                            Description = "Big desk located next to the window"
                        },
                        new Desk()
                        {
                            Description = "Computer workstation for one employee"
                        },
                        new Desk()
                        {
                        },
                        new Desk()
                        {
                        },
                        new Desk()
                        {
                        },
                        new Desk()
                        {
                        },
                    }

                },
                new Location()
                {
                    Country = "United States of America",
                    City = "Chicago",
                    Address = "Random 89",
                    PostalCode = "11101",
                    Desks = new List<Desk>
                    {
                        new Desk()
                        {
                            Description = "Big desk located next to the window"
                        },
                        new Desk()
                        {
                            Description = "Computer workstation for one employee"
                        },
                        new Desk()
                        {
                            Description = "Computer workstation for group of employees"
                        },
                        new Desk()
                        {
                        },
                        new Desk()
                        {
                        },
                        new Desk()
                        {
                        },
                    }

                }
                
            };
            return locations;
        }

        private IEnumerable<User> SeedDataUsers()
        {
            //var hashedPassword = _passwordHasher.HashPassword()
            var users = new List<User>()
            {
                new User()
                {
                    Name = "James",
                    Surname = "Smith",
                    Email = "admin@gmail.com",
                    Password = "admin",
                    RoleId = 1
                },
                new User()
                {
                    Name = "Joe",
                    Surname = "Johnson",
                    Email = "employee@gmail.com",
                    Password = "employee",
                    RoleId = 2
                }
            };
            return users;
        }
    }
}
