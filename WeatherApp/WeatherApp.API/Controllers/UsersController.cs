﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Data;
using WeatherApp.Shared.Dtos;
using WeatherApp.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")] // Solo administradores pueden listar todos los usuarios
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")] // Permitir acceso a Admin y User
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("Usuario no encontrado");

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")] // Solo administradores pueden crear usuarios
        public async Task<IActionResult> AddUser([FromBody] UserCreateDto userDto)
        {
            if (_context.Users.Any(u => u.Username == userDto.Username))
                return BadRequest("El nombre de usuario ya está en uso");

            if (_context.Users.Any(u => u.Email == userDto.Email))
                return BadRequest("El correo electrónico ya está registrado");

            if (userDto.Role != "Admin" && userDto.Role != "User")
                return BadRequest("Rol inválido. Los roles permitidos son 'Admin' y 'User'.");

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = HashPassword(userDto.Password),
                Role = userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Solo administradores pueden actualizar usuarios
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest("El ID del usuario no coincide");

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound("Usuario no encontrado");

            existingUser.Username = userDto.Username;
            existingUser.Email = userDto.Email;

            if (!string.IsNullOrWhiteSpace(userDto.Password))
                existingUser.PasswordHash = HashPassword(userDto.Password);

            if (userDto.Role != "Admin" && userDto.Role != "User")
                return BadRequest("Rol inválido. Los roles permitidos son 'Admin' y 'User'.");

            existingUser.Role = userDto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Solo administradores pueden eliminar usuarios
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(bytes);
        }
    }
}