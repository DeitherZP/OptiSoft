using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Data.DTO
{
    public class ArticuloAgregadoEvent
    {
        public Articulo Articulo { get; set; } = default!;
        public int Cantidad { get; set; }
    }
}
