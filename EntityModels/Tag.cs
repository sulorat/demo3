using System;
using System.Collections.Generic;

namespace demo3.EntityModels;

public partial class Tag
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Color { get; set; } = null!;
}
