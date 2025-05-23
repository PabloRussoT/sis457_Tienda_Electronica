using TiendaElectronica; // Asegúrate de que este namespace apunte a tu modelo de Entity Framework
using System;
using System.Collections.Generic;
using System.Linq;
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
                    producto.estado = -1;
                    producto.usuarioRegistro = usuario;
                }
                return context.SaveChanges();
            }
        }

        public static Producto obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Producto.Find(id);
            }
        }

        public static List<Producto> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Producto.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
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
