
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
                    existente.nombre = producto.nombre;
                    existente.descripcion = producto.descripcion;
                    existente.idCategoria = producto.idCategoria;
                    existente.idMarca = producto.idMarca;
                    existente.modelo = producto.modelo;
                    existente.stock = producto.stock;
                    existente.precioCompra = producto.precioCompra;
                    existente.precioVenta = producto.precioVenta;
                    existente.garantiaMeses = producto.garantiaMeses;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var producto = context.Producto.Find(id);
                if (producto != null)
                {
                    context.Producto.Remove(producto);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Producto obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Producto.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Producto> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Producto
                    .Where(p => string.IsNullOrEmpty(filtro) ||
                                p.nombre.Contains(filtro) ||
                                p.descripcion.Contains(filtro) ||
                                p.codigo.Contains(filtro))
                    .ToList();
            }
        }
    }
}
