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
    public class CompraCln
    {
        public static int insertar(Compra compra)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Compra.Add(compra);
                context.SaveChanges();
                return compra.id;
            }
        }

        public static int actualizar(Compra compra)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Compra.Find(compra.id);
                if (existente != null)
                {
                    existente.idProveedor = compra.idProveedor;
                    existente.numeroFactura = compra.numeroFactura;
                    existente.fecha = compra.fecha;
                    existente.total = compra.total;
                    existente.observaciones = compra.observaciones;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var compra = context.Compra.Find(id);
                if (compra != null)
                {
                    context.Compra.Remove(compra);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Compra obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Compra.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Compra> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Compra
                    .Where(c => string.IsNullOrEmpty(filtro) ||
                                c.numeroFactura.Contains(filtro) ||
                                c.Proveedor.razonSocial.Contains(filtro))
                    .ToList();
            }
        }
    }
}
