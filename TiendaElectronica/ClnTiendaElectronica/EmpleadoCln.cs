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
    public class EmpleadoCln
    {
        public static int insertar(Empleado empleado)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Empleado.Add(empleado);
                context.SaveChanges();
                return empleado.id;
            }
        }

        public static int actualizar(Empleado empleado, string v)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Empleado.Find(empleado.id);
                if (existente != null)
                {
                    existente.cedulaIdentidad = empleado.cedulaIdentidad;
                    existente.nombres = empleado.nombres;
                    existente.primerApellido = empleado.primerApellido;
                    existente.segundoApellido = empleado.segundoApellido;
                    existente.direccion = empleado.direccion;
                    existente.celular = empleado.celular;
                    existente.email = empleado.email;
                    existente.cargo = empleado.cargo;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var empleado = context.Empleado.Find(id);
                if (empleado != null)
                {
                    context.Empleado.Remove(empleado);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Empleado obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Empleado.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Empleado> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Empleado
                    .Where(e => string.IsNullOrEmpty(filtro) ||
                                e.nombres.Contains(filtro) ||
                                e.primerApellido.Contains(filtro) ||
                                e.segundoApellido.Contains(filtro))
                    .ToList();
            }
        }

        public static List<paEmpleadoListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.paEmpleadoListar(parametro).ToList();
            }
        }

        public static void insertar(Empleado empleado, Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public static void actualizar(Empleado empleado, string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public static void eliminar(int id, string usuario1)
        {
            throw new NotImplementedException();
        }
    }
}
