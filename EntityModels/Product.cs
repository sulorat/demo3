using System;
using System.Collections.Generic;

namespace demo3.EntityModels;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public float Cost { get; set; }

    public string? Description { get; set; }

    public string? Mainimagepath { get; set; }

    public int Isactive { get; set; }

    public int? Manufacturerid { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<Productphoto> Productphotos { get; set; } = new List<Productphoto>();

    public virtual ICollection<Productsale> Productsales { get; set; } = new List<Productsale>();
}
