using System;
using System.Collections.Generic;

namespace demo3.EntityModels;

public partial class Documentbyservice
{
    public int Id { get; set; }

    public int Clientserviceid { get; set; }

    public string Documentpath { get; set; } = null!;

    public virtual Clientservice Clientservice { get; set; } = null!;
}
