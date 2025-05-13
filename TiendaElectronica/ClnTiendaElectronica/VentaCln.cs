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
    public class VentaCln
    {
        public static int insertar(Venta venta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Venta.Add(venta);
                context.SaveChanges();
                return venta.id;
            }
        }

        public static int actualizar(Venta venta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Venta.Find(venta.id);
                if (existente != null)
                {
                    existente.idCliente = venta.idCliente;
                    existente.idUsuario = venta.idUsuario;
                    existente.numeroFactura = venta.numeroFactura;
                    existente.fecha = venta.fecha;
                    existente.subtotal = venta.subtotal;
                    existente.descuento = venta.descuento;
                    existente.iva = venta.iva;
                    existente.total = venta.total;
                    existente.tipoPago = venta.tipoPago;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var venta = context.Venta.Find(id);
                if (venta != null)
                {
                    context.Venta.Remove(venta);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Venta obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Venta.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Venta> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Venta
                    .Where(v => string.IsNullOrEmpty(filtro) ||
                                v.numeroFactura.Contains(filtro) ||
                                v.Cliente.nombres.Contains(filtro))
                    .ToList();
            }
        }
    }
}
