using TiendaElectronica; // Make sure this namespace points to your Entity Framework model
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // Required for .Include()
using System.Text;
using System.Threading.Tasks;

namespace ClnTiendaElectronica
{
    public class ProductoCln
    {
        public static int insertar(Producto producto)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // When inserting, the 'producto' object passed from the UI
                // should already have idCategoria and idMarca set.
                context.Producto.Add(producto);
                context.SaveChanges();
                return producto.id;
            }
        }

        public static int actualizar(Producto producto)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Producto.Find(producto.id);
                if (existente != null)
                {
                    existente.codigo = producto.codigo;
                    existente.descripcion = producto.descripcion;
                    existente.unidadMedida = producto.unidadMedida;
                    existente.saldo = producto.saldo;
                    existente.precioVenta = producto.precioVenta;
                    existente.usuarioRegistro = producto.usuarioRegistro;

                    // --- CAMPOS PARA CATEGORIA Y MARCA AÑADIDOS/ACTUALIZADOS AQUÍ ---
                    existente.idCategoria = producto.idCategoria;
                    existente.idMarca = producto.idMarca;
                    // -----------------------------------------------------------

                    // No se actualiza fechaRegistro aquí, ya que es DEFAULT GETDATE() en la BD.
                    // Si necesitas actualizarla, deberías pasarla como parámetro o manejarla explícitamente.
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var producto = context.Producto.Find(id);
                if (producto != null)
                {
                    producto.estado = -1; // Soft delete
                    producto.usuarioRegistro = usuario; // Record who made the change
                }
                return context.SaveChanges();
            }
        }

        public static Producto obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // *** IMPORTANTE: Usa .Include() para cargar las entidades relacionadas (Categoria y Marca) ***
                // Esto es necesario para que el `FrmProducto` pueda acceder a 'producto.Categoria.nombre' y 'producto.Marca.nombre'
                // cuando se edita un producto y se intenta mostrar el nombre de la categoría y marca.
                return context.Producto
                              .Include(p => p.Categoria) // Carga la entidad Categoria relacionada
                              .Include(p => p.Marca)     // Carga la entidad Marca relacionada
                              .FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Producto> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Incluye Categoria y Marca para poder acceder a sus nombres en la UI si es necesario
                return context.Producto
                              .Include(p => p.Categoria)
                              .Include(p => p.Marca)
                              .Where(x => x.estado != -1)
                              .ToList();
            }
        }

        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Este método asume que 'paProductoListar' es un Stored Procedure.
                // Si tu Stored Procedure ya devuelve la información de Categoría y Marca,
                // no necesitas un .Include() aquí, ya que el SP se encarga.
                // Sin embargo, si quieres filtrar por nombre de categoría o marca en el SP,
                // asegúrate de que el SP lo maneje.
                return context.paProductoListar(parametro).ToList();
            }
        }

        // Nuevo método para actualizar el saldo de un producto después de una venta
        public static void actualizarSaldo(int idProducto, decimal cantidadVendida)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var producto = context.Producto.Find(idProducto);
                if (producto != null)
                {
                    producto.saldo -= cantidadVendida;
                    context.SaveChanges();
                }
            }
        }
    }
}