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
    public class ClienteCln
    {
        public static int insertar(Cliente cliente)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Cliente.Add(cliente);
                context.SaveChanges();
                return cliente.id;
            }
        }

        public static int actualizar(Cliente cliente)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Cliente.Find(cliente.id);
                if (existente != null)
                {
                    existente.tipoDocumento = cliente.tipoDocumento;
                    existente.numeroDocumento = cliente.numeroDocumento;
                    existente.nombres = cliente.nombres;
                    existente.apellidos = cliente.apellidos;
                    existente.direccion = cliente.direccion;
                    existente.telefono = cliente.telefono;
                    existente.email = cliente.email;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var cliente = context.Cliente.Find(id);
                if (cliente != null)
                {
                    context.Cliente.Remove(cliente);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Cliente obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Cliente.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Cliente> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Cliente
                    .Where(c => string.IsNullOrEmpty(filtro) ||
                                c.numeroDocumento.Contains(filtro) ||
                                c.nombres.Contains(filtro) ||
                                c.apellidos.Contains(filtro))
                    .ToList();
            }
        }
    }
}
