// ClnTiendaElectronica\VentaDetalleCln.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaElectronica; // Asegúrate de que este namespace apunte a tu modelo de Entity Framework (TiendaElectronica.edmx)

namespace ClnTiendaElectronica
{
    public class VentaDetalleCln
    {
        /// <summary>
        /// Inserta un nuevo detalle de venta en la base de datos.
        /// </summary>
        /// <param name="ventaDetalle">Objeto VentaDetalle con los datos a insertar.</param>
        /// <returns>El ID del detalle de venta recién insertado.</returns>
        public static int insertar(VentaDetalle ventaDetalle)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.VentaDetalle.Add(ventaDetalle);
                context.SaveChanges();
                return ventaDetalle.id;
            }
        }

        /// <summary>
        /// Actualiza un detalle de venta existente en la base de datos.
        /// </summary>
        /// <param name="ventaDetalle">Objeto VentaDetalle con los datos actualizados.</param>
        /// <returns>Número de registros afectados (normalmente 1 si tiene éxito).</returns>
        public static int actualizar(VentaDetalle ventaDetalle)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.VentaDetalle.Find(ventaDetalle.id);
                if (existente != null)
                {
                    existente.idVenta = ventaDetalle.idVenta;
                    existente.idProducto = ventaDetalle.idProducto;
                    existente.cantidad = ventaDetalle.cantidad;
                    existente.precioUnitario = ventaDetalle.precioUnitario;
                    existente.total = ventaDetalle.total;
                    existente.usuarioRegistro = ventaDetalle.usuarioRegistro;
                    // No se actualiza fechaRegistro aquí, ya que es DEFAULT GETDATE() en la BD.
                    // Si necesitas actualizarla, deberías pasarla como parámetro o manejarla explícitamente.
                }
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica de un detalle de venta (cambia su estado a -1).
        /// </summary>
        /// <param name="id">El ID del detalle de venta a eliminar.</param>
        /// <param name="usuario">El nombre de usuario que realiza la eliminación.</param>
        /// <returns>Número de registros afectados (normalmente 1 si tiene éxito).</returns>
        public static int eliminar(int id, string usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var ventaDetalle = context.VentaDetalle.Find(id);
                if (ventaDetalle != null)
                {
                    ventaDetalle.estado = -1;
                    ventaDetalle.usuarioRegistro = usuario;
                }
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene un único detalle de venta por su ID.
        /// </summary>
        /// <param name="id">El ID del detalle de venta a obtener.</param>
        /// <returns>El objeto VentaDetalle correspondiente al ID o null si no se encuentra.</returns>
        public static VentaDetalle obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.VentaDetalle.Find(id);
            }
        }

        /// <summary>
        /// Lista todos los detalles de venta activos (estado diferente de -1).
        /// </summary>
        /// <returns>Una lista de objetos VentaDetalle.</returns>
        public static List<VentaDetalle> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.VentaDetalle.Where(x => x.estado != -1).ToList();
            }
        }

        /// <summary>
        /// Lista detalles de venta utilizando el procedimiento almacenado paVentaDetalleListar.
        /// </summary>
        /// <param name="parametro">Parámetro de búsqueda para filtrar los resultados.</param>
        /// <returns>Una lista de objetos paVentaDetalleListar_Result.</returns>
        public static List<paVentaDetalleListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.paVentaDetalleListar(parametro).ToList();
            }
        }
    }
}