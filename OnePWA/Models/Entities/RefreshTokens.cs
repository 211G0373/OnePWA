using System;
using System.Collections.Generic;

namespace OnePWA.Models.Entities;

public partial class RefreshTokens
{
    public int Id { get; set; }

    public int? IdUsuario { get; set; }

    public string? Token { get; set; }

    public DateTime? Expiracion { get; set; }

    public DateTime? Creado { get; set; }

    public sbyte? Usado { get; set; }

    public virtual Users? IdUsuarioNavigation { get; set; }
}
