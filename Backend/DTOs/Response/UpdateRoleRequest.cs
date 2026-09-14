using Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.User
{
    public class UpdateRoleRequest
    {
        [Required]
        public Role Role { get; set; }
    }
}