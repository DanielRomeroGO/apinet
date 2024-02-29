using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiNetDani.Models;

public partial class Equipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Fundacion { get; set; }

    [JsonIgnore]

    public virtual ICollection<Jugador> Jugador { get; set; } = new List<Jugador>();
}
