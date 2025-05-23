using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaElectronica; // Asegúrate de que este namespace apunte a tu modelo de Entity Framework

namespace ClnTiendaElectronica
{
    // Clase para representar un detalle de venta en la lógica de negocio
    // Esta clase es pública para ser accesible desde FrmVenta
    public class VentaDetalleItem
    {
        public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaCln
    {
        // Cambiado de 'internal' a 'public' para que sea accesible desde FrmVenta
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

        // Método para registrar una venta completa (cabecera y detalles)
        // y actualizar el stock de productos, usando Entity Framework.
        public static void insertarVenta(int idEmpleado, List<VentaDetalleItem> detallesVenta)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Iniciar una transacción de Entity Framework
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar la cabecera de la venta
                        Venta nuevaVenta = new Venta
                        {
                            idEmpleado = idEmpleado,
                            fecha = DateTime.Now,
                            total = detallesVenta.Sum(d => d.Subtotal),
                            usuarioRegistro = Environment.UserName, // O el usuario loggeado
                            fechaRegistro = DateTime.Now,
                            estado = 1
                        };
                        context.Venta.Add(nuevaVenta);
                        context.SaveChanges(); // Guarda la venta para obtener su ID

                        // Obtener el ID de la venta recién insertada
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
                                usuarioRegistro = Environment.UserName, // O el usuario loggeado
                                fechaRegistro = DateTime.Now,
                                estado = 1
                            };
                            context.VentaDetalle.Add(nuevoDetalle);

                            // Actualizar el saldo del producto usando el método en ProductoCln
                            ProductoCln.actualizarSaldo(detalle.IdProducto, detalle.Cantidad);
                        }

                        context.SaveChanges(); // Guarda los detalles de venta y los cambios de saldo
                        transaction.Commit(); // Confirmar la transacción si todo fue exitoso
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Revertir la transacción en caso de error
                        Console.WriteLine("Error al insertar venta: " + ex.Message);
                        throw; // Relanzar la excepción
                    }
                }
            }
        }
    }
}
