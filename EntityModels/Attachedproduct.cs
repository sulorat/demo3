using System;
using System.Collections.Generic;

namespace demo3.EntityModels;

public partial class Attachedproduct
{
    public int Mainproductid { get; set; }

    public int Attachedproductid { get; set; }

    public virtual Product AttachedproductNavigation { get; set; } = null!;

    public virtual Product Mainproduct { get; set; } = null!;
}
