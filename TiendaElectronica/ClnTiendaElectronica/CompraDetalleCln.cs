using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaElectronica;

namespace ClnTiendaElectronica
{
    public class CompraDetalleCln
    {
        public static int insertar(CompraDetalle compraDetalle)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.CompraDetalle.Add(compraDetalle);
                context.SaveChanges();
                return compraDetalle.id;
            }
        }

        public static int actualizar(CompraDetalle compraDetalle)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.CompraDetalle.Find(compraDetalle.id);
                existente.idCompra = compraDetalle.idCompra;
                existente.idProducto = compraDetalle.idProducto;
                existente.cantidad = compraDetalle.cantidad;
                existente.precioUnitario = compraDetalle.precioUnitario;
                existente.total = compraDetalle.total;
                existente.usuarioRegistro = compraDetalle.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var compraDetalle = context.CompraDetalle.Find(id);
                compraDetalle.estado = -1;
                compraDetalle.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static CompraDetalle obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.CompraDetalle.Find(id);
            }
        }

        public static List<CompraDetalle> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.CompraDetalle.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paCompraDetalleListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.paCompraDetalleListar(parametro).ToList();
            }
        }
    }
}