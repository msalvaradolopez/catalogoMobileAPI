using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace catalogoMobileAPI.Models
{
    public class Ipedido
    {
        public int idPedido { get; set; }
        public int idEmpresa { get; set; }
        public string idUsuario { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string  notas { get; set; }
        public List<Icarrito> carrito { get; set; }
}

    public class Icarrito {
        public int idEmpresa { get; set; }
        public int idPedido { get; set; }
        public Iarticulo articulo { get; set; }
        public decimal precio { get; set; }
        public int cantidad { get; set; }
        public decimal importe { get; set; }
    }

    public class Iarticulo { 
        public int idEmpresa { get; set; }
        public string idArticulo { get; set; }
        public decimal precio { get; set; }


    }


}