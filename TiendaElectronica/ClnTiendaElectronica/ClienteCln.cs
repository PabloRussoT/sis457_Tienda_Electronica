using TiendaElectronica; // Asegúrate de que este namespace apunte a tu modelo de Entity Framework (TiendaElectronica.edmx)
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para LINQ (Where, ToList, etc.)
using System.Text;
using System.Threading.Tasks;

namespace ClnTiendaElectronica
{
    public class ClienteCln
    {
        // Método para insertar un nuevo cliente en la base de datos
        public static int insertar(Cliente cliente)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Asegúrate de que el campo 'fechaRegistro' y 'estado' sean establecidos por el modelo EF
                // o pasarlos explícitamente si no tienen DEFAULT en la BD o si necesitas controlarlos aquí.
                // context.Cliente.Add(cliente) por defecto insertará los valores que tenga el objeto cliente.
                // Los campos con DEFAULT en la BD se poblarán automáticamente si no se les asigna un valor.
                context.Cliente.Add(cliente);
                context.SaveChanges();
                return cliente.id; // Devuelve el ID generado por la base de datos
            }
        }

        // Método para actualizar un cliente existente en la base de datos
        public static int actualizar(Cliente cliente)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Busca el cliente existente por su ID
                var existente = context.Cliente.Find(cliente.id);
                if (existente != null)
                {
                    // Actualiza solo los campos modificables
                    existente.nit = cliente.nit;
                    existente.nombres = cliente.nombres;
                    existente.apellidos = cliente.apellidos;
                    existente.nombreCompleto = cliente.nombreCompleto;
                    existente.direccion = cliente.direccion;
                    existente.telefono = cliente.telefono;
                    existente.email = cliente.email;
                    // usuarioRegistro y fechaRegistro generalmente no se actualizan en un UPDATE,
                    // a menos que tengas campos específicos para "usuarioModificacion" y "fechaModificacion".
                    // existente.usuarioRegistro = cliente.usuarioRegistro; // Esto podría ser un campo de auditoría de modificación
                    // existente.fechaModificacion = DateTime.Now; // Si tu tabla tiene este campo
                }
                return context.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método para realizar una eliminación lógica de un cliente (cambiando su estado a -1)
        public static int eliminar(int id, string usuario)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Busca el cliente por su ID
                var cliente = context.Cliente.Find(id);
                if (cliente != null)
                {
                    // Cambia el estado a -1 (eliminado lógicamente)
                    cliente.estado = -1;
                    // Actualiza el usuario que realizó la "eliminación"
                    cliente.usuarioRegistro = usuario; // Podrías tener un campo específico para 'usuarioEliminacion'
                }
                return context.SaveChanges(); // Guarda los cambios
            }
        }

        // Método para obtener un cliente por su ID
        public static Cliente obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Find() es el método más rápido para buscar por clave primaria
                return context.Cliente.Find(id);
            }
        }

        // Método para listar todos los clientes activos (estado distinto de -1)
        public static List<Cliente> listar()
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Filtra por estado y devuelve la lista de clientes
                return context.Cliente.Where(x => x.estado != -1).ToList();
            }
        }

        // Método para listar clientes usando el procedimiento almacenado paClienteListar
        // paClienteListar_Result debe ser la clase generada por Entity Framework
        // para el resultado de tu procedimiento almacenado.
        public static List<paClienteListar_Result> listarPa(string parametro)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                // Llama al procedimiento almacenado mapeado en tu modelo de Entity Framework
                // Si el parámetro es nulo o vacío, el procedimiento almacenado lo interpretará como NULL
                // y listará todos los activos.
                return context.paClienteListar(parametro).ToList();
            }
        }
    }
}