using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Cart { get; set; }

    public virtual Role Role { get; set; } = null!;
}
