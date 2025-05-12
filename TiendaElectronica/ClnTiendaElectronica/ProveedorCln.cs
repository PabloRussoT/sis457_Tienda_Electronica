using System;
using System.Collections.Generic;
using System.Linq;
using TiendaElectronica;

namespace ClnTiendaElectronica
{
    public class ProveedorCln
    {
        public static int insertar(Proveedor proveedor)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Proveedor.Add(proveedor);
                context.SaveChanges();
                return proveedor.id;
            }
        }

        public static int actualizar(Proveedor proveedor)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Proveedor.Find(proveedor.id);
                if (existente != null)
                {
                    existente.nit = proveedor.nit;
                    existente.razonSocial = proveedor.razonSocial;
                    existente.direccion = proveedor.direccion;
                    existente.telefono = proveedor.telefono;
                    existente.email = proveedor.email;
                    existente.sitioWeb = proveedor.sitioWeb;
                    existente.representante = proveedor.representante;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var proveedor = context.Proveedor.Find(id);
                if (proveedor != null)
                {
                    context.Proveedor.Remove(proveedor);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Proveedor obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Proveedor.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Proveedor> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Proveedor
                    .Where(p => string.IsNullOrEmpty(filtro) ||
                                p.razonSocial.Contains(filtro) ||
                                p.representante.Contains(filtro))
                    .ToList();
            }
        }
    }
}
