using TiendaElectronica; // Make sure this points to your Entity Framework model
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnTiendaElectronica
{
    public class CategoriaCln
    {
        // Method to insert a new category
        public static int insertar(Categoria categoria)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Categoria.Add(categoria);
                context.SaveChanges();
                return categoria.id;
            }
        }

        // Method to update an existing category
        public static int actualizar(Categoria categoria)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Categoria.Find(categoria.id);
                if (existente != null)
                {
                    existente.nombre = categoria.nombre;
                    existente.descripcion = categoria.descripcion;
                    // Note: estado is not updated here, assuming it's handled by 'eliminar' or specific UI actions.
                }
                return context.SaveChanges();
            }
        }

        // Method for soft-delete (setting estado to -1)
        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var categoria = context.Categoria.Find(id);
                if (categoria != null)
                {
                    categoria.estado = -1; // Set estado to -1 for logical deletion
                }
                return context.SaveChanges();
            }
        }

        // Method to get a single category by ID
        public static Categoria obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Categoria.Find(id);
            }
        }

        // Method to list all active categories
        public static List<Categoria> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Categoria.Where(x => x.estado == 1) // Only active categories
                                       .OrderBy(x => x.nombre)
                                       .ToList();
            }
        }

        // Optional: Method to list categories by a search parameter (e.g., name or description)
        public static List<Categoria> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var lista = context.Categoria.Where(x => x.estado == 1 &&
                                                (x.nombre.Contains(parametro) ||
                                                 (x.descripcion != null && x.descripcion.Contains(parametro))))
                                         .OrderBy(x => x.nombre)
                                         .ToList();
                return lista;
            }
        }
    }
}