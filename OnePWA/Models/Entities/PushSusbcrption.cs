using System;
using System.Collections.Generic;

namespace OnePWA.Models.Entities;

public partial class PushSusbcrption
{
    public int Id { get; set; }

    public string Endpoint { get; set; } = null!;

    public string P256dh { get; set; } = null!;

    public string Auth { get; set; } = null!;

    public string? UserAgent { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimaNotificacion { get; set; }

    public bool? Activo { get; set; }
}
