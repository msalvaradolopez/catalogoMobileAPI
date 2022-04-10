using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace catalogoMobileAPI.Models
{
    public class Ifiltros
    {

        public int idEmpresa { get; set; }
        public string nomArticulo { get; set; }
        public string SKU { get; set; }
        public string idArticulo { get; set; }
        public string idCategoria { get; set; }
        public string idMarca { get; set; }
        public string orden { get; set; }

        public string buscar { get; set; }
        public string idUsuario { get; set; }
    }
}