using System;
using System.Collections.Generic;
using System.Linq;
using TiendaElectronica;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace ClnTiendaElectronica

{
    public class CategoriaCln
    {
        public static int insertar(Categoria categoria)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Categoria.Add(categoria);
                context.SaveChanges();
                return categoria.id;
            }
        }

        public static int actualizar(Categoria categoria)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Categoria.Find(categoria.id);
                if (existente != null)
                {
                    existente.nombre = categoria.nombre;
                    existente.descripcion = categoria.descripcion;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var categoria = context.Categoria.Find(id);
                if (categoria != null)
                {
                    context.Categoria.Remove(categoria);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Categoria obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Categoria.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Categoria> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Categoria
                    .Where(c => string.IsNullOrEmpty(filtro) ||
                                c.nombre.Contains(filtro) ||
                                c.descripcion.Contains(filtro))
                    .ToList();
            }
        }
    }
}
