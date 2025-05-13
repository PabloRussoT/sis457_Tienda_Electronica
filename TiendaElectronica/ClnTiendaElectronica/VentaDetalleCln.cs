using System;
using System.Collections.Generic;
using System.Linq;
using TiendaElectronica;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
namespace ClnTiendaElectronica
{
    public class VentaDetalleCln
    {
        public static int insertar(VentaDetalle ventaDetalle)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.VentaDetalle.Add(ventaDetalle);
                context.SaveChanges();
                return ventaDetalle.id;
            }
        }

        public static int actualizar(VentaDetalle ventaDetalle)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.VentaDetalle.Find(ventaDetalle.id);
                if (existente != null)
                {
                    existente.idVenta = ventaDetalle.idVenta;
                    existente.idProducto = ventaDetalle.idProducto;
                    existente.cantidad = ventaDetalle.cantidad;
                    existente.precioUnitario = ventaDetalle.precioUnitario;
                    existente.descuento = ventaDetalle.descuento;
                    existente.subtotal = ventaDetalle.subtotal;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var ventaDetalle = context.VentaDetalle.Find(id);
                if (ventaDetalle != null)
                {
                    context.VentaDetalle.Remove(ventaDetalle);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static VentaDetalle obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.VentaDetalle.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<VentaDetalle> listarPorVenta(int idVenta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.VentaDetalle.Where(vd => vd.idVenta == idVenta).ToList();
            }
        }
    }
}
