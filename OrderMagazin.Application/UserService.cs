using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application
{
    public class UserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }

}
