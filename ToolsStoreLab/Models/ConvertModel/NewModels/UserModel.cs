using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models.ConvertModel.NewModels;

public partial class UserModel
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; }    
}
