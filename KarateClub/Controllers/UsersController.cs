using Dtos.UsersDTOs;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // ===============================
        // GET ALL USERS
        // ===============================
        [HttpGet]
        public ActionResult<List<UserViewWithPersonDTO>> GetAll()
        {
            var users = clsUser.GetAll();
            return Ok(users);
        }

        // ===============================
        // GET USER BY ID
        // ===============================
        [HttpGet("{id}")]
        public ActionResult<UserViewWithPersonDTO> GetById(int id)
        {
            var user = clsUser.Find(id);
            if (user == null)
                return NotFound("User not found.");

            // Convert BLL to view DTO without password
            return Ok(new UserViewWithPersonDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PersonId = user.PersonId
            });
        }

        // ===============================
        // ADD NEW USER
        // ===============================
        [HttpPost]
        public ActionResult Add([FromBody] UserWithPersonDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Username and password are required.");

            if (clsUser.IsUserNameExist(dto.UserName))
                return BadRequest("Username already exists.");

            var user = new clsUser(dto, clsUser.enMode.add);
            user.Password = PasswordHelper.HashPassword(dto.Password);
            bool saved = user.Save();

            if (!saved)
                return StatusCode(500, "Error saving user.");

            return Ok(new { user.UserId });
        }

        // ===============================
        // UPDATE USER
        // ===============================
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UserWithPersonDTO dto)
        {
            var existingUser = clsUser.Find(id);
            if (existingUser == null)
                return NotFound("User not found.");

            existingUser.UserName = dto.UserName;

            // Only update password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
                existingUser.Password = PasswordHelper.HashPassword(dto.Password);

            existingUser.PersonId = dto.PersonId;
            existingUser.Mode = clsUser.enMode.update;

            bool updated = existingUser.Save();
            if (!updated)
                return StatusCode(500, "Error updating user.");

            return Ok("User updated successfully.");
        }

        // ===============================
        // DELETE USER
        // ===============================
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingUser = clsUser.Find(id);
            if (existingUser == null)
                return NotFound("User not found.");

            bool deleted = clsUser.Delete(id);
            if (!deleted)
                return StatusCode(500, "Error deleting user.");

            return Ok("User deleted successfully.");
        }

        // ===============================
        // LOGIN
        // ===============================
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Username and password are required.");

            // 1️⃣ Find user by username (includes hashed password)
            var user = clsUser.FindByUserName(dto.UserName);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            // 2️⃣ Verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid username or password.");

            // 3️⃣ Return user info without password
            var userView = new UserViewWithPersonDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PersonId = user.PersonId,
                PersonName = user.PersonName,
                Email = user.Address,
                Phone = user.ContactInfo
            };

            return Ok(userView);
        }
    }
}
