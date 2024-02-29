using System;
using System.Collections.Generic;

namespace ApiNetDani.Models;

public partial class Jugador
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public string? Bandera { get; set; }

    public byte[]? Banderablob { get; set; }

    public int? IdEquipo { get; set; }

    public virtual Equipo? oEquipo { get; set; }
}
