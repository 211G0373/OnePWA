using System;
using System.Collections.Generic;

namespace OnePWA.Models.Entities;

public partial class Cards
{
    public int Id { get; set; }

    public bool Special { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;
}
