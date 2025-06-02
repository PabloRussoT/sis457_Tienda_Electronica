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
        public static int insertar(Venta venta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Venta.Add(venta);
                context.SaveChanges();
                return venta.id;
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
                    venta.estado = -1;
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
        /// <param name="idEmpleado">El ID del empleado que realiza la venta.</param>
        /// <param name="idClienteActual">El ID del cliente al que se le realiza la venta.</param>
        /// <param name="fechaVenta">La fecha de la venta.</param>
        /// <param name="detallesVenta">Una lista de objetos VentaDetalle que representan los productos y cantidades vendidas.</param>
        public static void insertarVenta(int idEmpleado, int idClienteActual, DateTime fechaVenta, List<TiendaElectronica.VentaDetalle> detallesVenta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar la cabecera de la venta
                        Venta nuevaVenta = new Venta
                        {
                            idEmpleado = idEmpleado,
                            idCliente = idClienteActual,
                            fecha = fechaVenta, // Use the provided fechaVenta
                            total = detallesVenta.Sum(d => d.Subtotal),
                            usuarioRegistro = Environment.UserName,
                            fechaRegistro = DateTime.Now,
                            estado = 1
                        };
                        context.Venta.Add(nuevaVenta);
                        context.SaveChanges();

                        int idVenta = nuevaVenta.id;

                        // 2. Insertar los detalles de la venta y actualizar el stock de productos
                        foreach (var detalle in detallesVenta)
                        {
                            VentaDetalle nuevoDetalle = new VentaDetalle
                            {
                                idVenta = idVenta,
                                idProducto = detalle.IdProducto,
                                cantidad = detalle.Cantidad,
                                precioUnitario = detalle.PrecioUnitario,
                                total = detalle.Subtotal,
                                usuarioRegistro = Environment.UserName,
                                fechaRegistro = DateTime.Now,
                                estado = 1
                            };
                            context.VentaDetalle.Add(nuevoDetalle);

                            // Assuming ProductoCln.actualizarSaldo subtracts the quantity
                            ProductoCln.actualizarSaldo(detalle.IdProducto, detalle.Cantidad);
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error al insertar venta en VentaCln: " + ex.Message);
                        throw; // Re-throw the exception to be caught in FrmVenta
                    }
                }
            }
        }
        // REMOVED THE UNIMPLEMENTED OVERLOAD:
        // public static void insertarVenta(int idEmpleadoActual, int idCliente, List<VentaDetalle> ventaDetalles)
        // {
        //     throw new NotImplementedException();
        // }
    }
}