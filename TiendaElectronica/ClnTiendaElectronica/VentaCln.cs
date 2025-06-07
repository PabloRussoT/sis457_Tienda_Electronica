// ClnTiendaElectronica\VentaCln.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaElectronica; // IMPORTANT: This references the shared VentaDetalle and EF models

namespace ClnTiendaElectronica
{
    public class VentaCln
    {
        // Este método inserta solo la cabecera de la venta.
        // Puede ser útil si necesitas insertar ventas sin detalles inicialmente,
        // pero para una venta completa con detalles, el método 'RegistrarVenta' es preferible.
        public static int insertar(Venta venta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Venta.Add(venta);
                context.SaveChanges();
                return venta.id; // Retorna el ID de la venta recién creada
            }
        }

        public static int actualizar(Venta venta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Venta.Find(venta.id);
                if (existente != null)
                {
                    existente.idEmpleado = venta.idEmpleado;
                    existente.total = venta.total;
                    existente.usuarioRegistro = venta.usuarioRegistro;
                    // Asegúrate de actualizar otras propiedades relevantes si es necesario
                    // existente.fechaVenta = venta.fechaVenta; // Si la fecha se puede actualizar
                    // existente.idCliente = venta.idCliente; // Si el cliente se puede cambiar
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var venta = context.Venta.Find(id);
                if (venta != null)
                {
                    venta.estado = -1; // Marcado como eliminado lógicamente
                    venta.usuarioRegistro = usuario;
                }
                return context.SaveChanges();
            }
        }

        public static Venta obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Venta.Find(id);
            }
        }

        public static List<Venta> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Venta.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paVentaListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.paVentaListar(parametro).ToList();
            }
        }

        /// <summary>
        /// Registra una venta completa (cabecera y detalles) y actualiza el stock de los productos vendidos.
        /// Esta operación se realiza dentro de una transacción para asegurar la consistencia de los datos.
        /// </summary>
        /// <param name="nuevaVenta">El objeto Venta con los datos de la cabecera.</param>
        /// <param name="detallesVenta">Una lista de objetos VentaDetalle que representan los productos y cantidades vendidas.</param>
        public static void RegistrarVenta(Venta nuevaVenta, List<TiendaElectronica.VentaDetalle> detallesVenta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar la cabecera de la venta
                        // La propiedad 'total' de 'nuevaVenta' ya debería venir calculada desde el formulario,
                        // pero la recalculamos aquí para asegurarnos.
                        nuevaVenta.total = detallesVenta.Sum(d => d.total); // Suma los totales de los detalles
                        context.Venta.Add(nuevaVenta);
                        context.SaveChanges(); // Guarda la venta para obtener su ID auto-generado

                        int idVentaGenerado = nuevaVenta.id; // Obtiene el ID de la venta recién creada

                        // 2. Insertar los detalles de la venta y actualizar el stock de productos
                        foreach (var detalle in detallesVenta)
                        {
                            // Asignar el ID de la venta a cada detalle
                            detalle.idVenta = idVentaGenerado;
                            context.VentaDetalle.Add(detalle);

                            // Encontrar el producto para actualizar su stock
                            var producto = context.Producto.Find(detalle.idProducto); // Asegúrate de que idProducto sea el nombre correcto en VentaDetalle
                            if (producto != null)
                            {
                                // Validar y restar la cantidad vendida del stock
                                // Asegúrate de que producto.saldo y detalle.cantidad sean del mismo tipo (decimal o int)
                                // Si saldo es int y cantidad es decimal, deberás decidir el redondeo.
                                // Si ambos son decimal, la resta es directa.
                                if (producto.saldo >= detalle.cantidad) // Asumo que saldo y cantidad son del mismo tipo (ej. decimal)
                                {
                                    producto.saldo -= detalle.cantidad; // Resta la cantidad vendida del stock
                                }
                                else
                                {
                                    // Esto debería ser capturado por la validación de la UI, pero es una salvaguarda.
                                    throw new Exception($"Stock insuficiente para el producto {producto.descripcion} (ID: {detalle.idProducto}). Saldo actual: {producto.saldo}, Cantidad a vender: {detalle.cantidad}.");
                                }
                            }
                            else
                            {
                                throw new Exception($"Producto con ID {detalle.idProducto} no encontrado en la base de datos.");
                            }
                        }

                        // 3. Guardar todos los cambios (detalles y actualizaciones de stock)
                        context.SaveChanges();
                        transaction.Commit(); // Confirmar la transacción
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Revertir todos los cambios si ocurre un error
                        Console.WriteLine("Error al registrar venta en VentaCln: " + ex.Message);
                        throw; // Re-lanzar la excepción para que sea manejada en la UI
                    }
                }
            }
        }
    }
}