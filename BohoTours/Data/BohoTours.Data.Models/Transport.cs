namespace BohoTours.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BohoTours.Data.Common.Models;
    using Microsoft.EntityFrameworkCore;

    public class Transport : BaseDeletableModel<int>
    {
        public string TransportType { get; set; }
    }
}
