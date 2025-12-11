using System;
using System.Collections.Generic;

namespace OnePWA.Models.Entities;

public partial class Users
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int WonGames { get; set; }

    public string Password { get; set; } = null!;

    public string ProfilePictures { get; set; } = null!;
}
