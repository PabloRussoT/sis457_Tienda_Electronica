using System;
using System.Collections.Generic;

namespace WebTiendaELectronica.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public long? Nit { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public string Nombres { get; set; } = null!;

    public string? Apellidos { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
