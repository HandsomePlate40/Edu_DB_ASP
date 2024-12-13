using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string ProfilePictureUrl { get; set; } = string.Empty;
}
