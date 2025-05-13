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
    public class UsuarioCln
    {
        public static int insertar(Usuario usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Usuario.Add(usuario);
                context.SaveChanges();
                return usuario.id;
            }
        }

        public static int actualizar(Usuario usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Usuario.Find(usuario.id);
                if (existente != null)
                {
                    existente.usuario1 = usuario.usuario1;
                    existente.clave = usuario.clave;
                    existente.nivelAcceso = usuario.nivelAcceso;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var usuario = context.Usuario.Find(id);
                if (usuario != null)
                {
                    context.Usuario.Remove(usuario);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Usuario obtenerUnoPorEmpleado(int idEmpleado)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Usuario.FirstOrDefault(x => x.idEmpleado == idEmpleado);
            }
        }

        public static Usuario validar(string usuario, string clave)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Usuario
                    .FirstOrDefault(u => u.usuario1 == usuario && u.clave == clave);
            }
        }
    }
}
