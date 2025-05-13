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
                if (existente != null)
                {
                    existente.idCompra = compraDetalle.idCompra;
                    existente.idProducto = compraDetalle.idProducto;
                    existente.cantidad = compraDetalle.cantidad;
                    existente.precioUnitario = compraDetalle.precioUnitario;
                    existente.subtotal = compraDetalle.subtotal;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var compraDetalle = context.CompraDetalle.Find(id);
                if (compraDetalle != null)
                {
                    context.CompraDetalle.Remove(compraDetalle);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static CompraDetalle obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.CompraDetalle.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<CompraDetalle> listarPorCompra(int idCompra)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.CompraDetalle.Where(cd => cd.idCompra == idCompra).ToList();
            }
        }
    }
}
