using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using catalogoMobileAPI.Models;
using correoService;

namespace catalogoMobileAPI.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getArticulos")]
        public IEnumerable<object> getArticulos([FromBody] Ifiltros pFiltros)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    var _catalogoListado = db.spArticulosListInicio(pFiltros.idEmpresa, pFiltros.orden, pFiltros.idCategoria, pFiltros.idMarca, pFiltros.buscar)
                    .ToList();

                    return _catalogoListado;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getArticuloByID")]
        public object getArticuloByID([FromBody] Ifiltros Filtros)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    var _articulo = db.ecARTICULO
                        .Where(x => x.idEmpresa == Filtros.idEmpresa && x.idArticulo == Filtros.idArticulo)
                        .Select(x => new
                        {
                            x.idEmpresa,
                            x.idArticulo,
                            x.nomArticulo,
                            x.descripcion,
                            x.precio,
                            x.tipo,
                            x.idUnidad,
                            x.idMarca,
                            x.idCategoria,
                            imagenes = x.ecIMAGEN.Select(img => new { img.idImagen, img.idEmpresa, img.nomImagen, img.urlImagen, img.principal }).ToList()
                        }).FirstOrDefault();


                    return _articulo;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getCatalogos")]
        public IEnumerable<object> getCatalogos([FromBody] Ifiltros Filtros)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    var _categorias = db.ecCATEGORIA
                        .Where(x => x.idEmpresa == Filtros.idEmpresa)
                        .Select(x => new
                        {
                            x.idCategoria,
                            x.idEmpresa,
                            imagen = x.ecIMAGEN
                                        .Where(img => img.principal == "S")
                                        .Select(img => new { img.idImagen, img.idEmpresa, img.nomImagen, img.principal, urlImagen = img.urlImagen ?? "" })
                                        .FirstOrDefault(),
                            ctdArticulos = x.ecARTICULO
                                           .Count(art => art.idCategoria == x.idCategoria && art.estatus == "A")
                        })
                        .ToList();

                    return _categorias;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getMarcas")]
        public IEnumerable<object> getMarcas([FromBody] Ifiltros Filtros)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    var _marcas = db.ecMARCA
                        .Where(x => x.idEmpresa == Filtros.idEmpresa)
                        .Select(x => new
                        {
                            x.idMarca,
                            x.idEmpresa,
                            imagen = x.ecIMAGEN
                                        .Where(img => img.principal == "S")
                                        .Select(img => new { img.idImagen, img.idEmpresa, img.nomImagen, img.principal, urlImagen = img.urlImagen ?? "" })
                                        .FirstOrDefault(),
                            ctdArticulos = x.ecARTICULO
                                           .Count(art => art.idMarca == x.idMarca && art.estatus == "A")
                        })
                        .ToList();

                    return _marcas;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getEmpresas")]
        public IEnumerable<object> getEmpresas([FromBody] Ifiltros pFiltros)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    var _empresas = db.spEmpresaList(pFiltros.idEmpresa, pFiltros.orden, pFiltros.buscar)
                        .ToList();


                    return _empresas;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getUsuarioById")]
        public object getUsuarioById([FromBody] Ifiltros pFiltros)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    var _usuario = db.ecUSUARIO.Where(x => x.idEmpresa == pFiltros.idEmpresa && x.idUsuario == pFiltros.idUsuario)
                        .FirstOrDefault();

                    if (_usuario == null)
                        throw new Exception("El usuario no existe.");

                    if (_usuario.estatus == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    return _usuario;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insUsuarioGoogle")]
        public void insUsuarioGoogle([FromBody] ecUSUARIO pUsuario)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    ecUSUARIO _usuario = db.ecUSUARIO.Where(x => x.idEmpresa == pUsuario.idEmpresa  &&  x.idUsuario == pUsuario.idUsuario)
                        .SingleOrDefault();


                    if (_usuario == null) {
                        _usuario = new ecUSUARIO();
                        _usuario.idEmpresa = pUsuario.idEmpresa;
                        _usuario.idUsuario = pUsuario.idUsuario;
                        _usuario.nombre = pUsuario.nombre;
                        _usuario.genero = "H";
                        _usuario.estatus = "A";
                        _usuario.fecha = DateTime.Now;
                        db.ecUSUARIO.Add(_usuario);

                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        /*  AGREGA PEDIDO */
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insPedido")]
        public string insPedido([FromBody] Ipedido pPedido)
        {
            using (dbCatalogoDigital db = new dbCatalogoDigital())
            {
                try
                {

                    using (DbContextTransaction trans = db.Database.BeginTransaction()) {
                        try
                        {
                            ecPEDIDO _pedido = db.ecPEDIDO.Where(x => x.idEmpresa == pPedido.idEmpresa && x.idPedido == pPedido.idPedido)
                            .SingleOrDefault();

                            if (_pedido != null)
                                throw new Exception("Pedido ya existe.");

                            _pedido = new ecPEDIDO();
                            _pedido.idEmpresa = pPedido.idEmpresa;
                            _pedido.idUsuario = pPedido.idUsuario;
                            _pedido.direccion = pPedido.direccion;
                            _pedido.telefono = pPedido.telefono;
                            _pedido.notas = pPedido.notas;
                            _pedido.fecha = DateTime.Now;
                            db.ecPEDIDO.Add(_pedido);
                            db.SaveChanges();

                            if (pPedido.carrito.Count() <= 0)
                                throw new Exception("No existen registros del carrito.");


                            pPedido.carrito.ForEach(carritoItem => {

                                ecCARRITO _carritoDb = new ecCARRITO();

                                _carritoDb.idPedido = _pedido.idPedido;
                                _carritoDb.idEmpresa = carritoItem.idEmpresa;
                                _carritoDb.idArticulo = carritoItem.articulo.idArticulo;
                                _carritoDb.precio = carritoItem.precio;
                                _carritoDb.cantidad = carritoItem.cantidad;
                                _carritoDb.importe = carritoItem.importe;
                                db.ecCARRITO.Add(_carritoDb);
                            });

                            db.SaveChanges();
                            trans.Commit();
                        }
                        catch (Exception ex) {
                            trans.Rollback();
                            throw ex;
                        }
                            
                    }
                    return "Pedido ingresado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("servicioCorreo")]
        public void servicioCorreo([FromBody] Ifiltros pFiltros)
        {
                
            try
            {                    

                EnvioCorreo enviarCorreo = new EnvioCorreo();
                enviarCorreo.sendEmail();
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
