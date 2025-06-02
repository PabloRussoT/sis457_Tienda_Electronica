using TiendaElectronica; // Make sure this points to your Entity Framework model
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnTiendaElectronica
{
    public class MarcaCln
    {
        // Method to insert a new brand
        public static int insertar(Marca marca)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Marca.Add(marca);
                context.SaveChanges();
                return marca.id;
            }
        }

        // Method to update an existing brand
        public static int actualizar(Marca marca)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Marca.Find(marca.id);
                if (existente != null)
                {
                    existente.nombre = marca.nombre;
                    existente.descripcion = marca.descripcion;
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
                var marca = context.Marca.Find(id);
                if (marca != null)
                {
                    marca.estado = -1; // Set estado to -1 for logical deletion
                }
                return context.SaveChanges();
            }
        }

        // Method to get a single brand by ID
        public static Marca obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Marca.Find(id);
            }
        }

        // Method to list all active brands
        public static List<Marca> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Marca.Where(x => x.estado == 1) // Only active brands
                                    .OrderBy(x => x.nombre)
                                    .ToList();
            }
        }

        // Optional: Method to list brands by a search parameter (e.g., name or description)
        public static List<Marca> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var lista = context.Marca.Where(x => x.estado == 1 &&
                                                (x.nombre.Contains(parametro) ||
                                                 (x.descripcion != null && x.descripcion.Contains(parametro))))
                                     .OrderBy(x => x.nombre)
                                     .ToList();
                return lista;
            }
        }
    }
}