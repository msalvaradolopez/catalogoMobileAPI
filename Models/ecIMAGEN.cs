//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace catalogoMobileAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ecIMAGEN
    {
        public int idImagen { get; set; }
        public int idEmpresa { get; set; }
        public string idArticulo { get; set; }
        public string idCategoria { get; set; }
        public string idMarca { get; set; }
        public string nomImagen { get; set; }
        public string urlImagen { get; set; }
        public string principal { get; set; }
    
        public virtual ecARTICULO ecARTICULO { get; set; }
        public virtual ecCATEGORIA ecCATEGORIA { get; set; }
        public virtual ecMARCA ecMARCA { get; set; }
    }
}
