using ClnTiendaElectronica; // Asegúrate de que esta referencia sea correcta
using TiendaElectronica; // Necesario para acceder a las clases de tu modelo de Entity Framework (Producto, Venta, etc.)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CpTiendaElectronica
{
    // Clase para representar un ítem en el carrito de compras
    // Sus propiedades son mutables (lectura y escritura)
    public class CarritoItem
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
        public decimal saldo { get; set; } // Stock original del producto para validación
    }

    public partial class FrmVenta : Form
    {
        // Esta lista contendrá los productos actualmente en el carrito de ventas.
        // Ahora usamos la clase CarritoItem personalizada.
        private List<CarritoItem> productosEnCarrito = new List<CarritoItem>();

        // ID del empleado que realiza la venta (puedes obtenerlo de un login o una variable global)
        // Por ahora, lo inicializamos con un valor de ejemplo.
        private int idEmpleadoActual = 1; // ¡¡¡CAMBIA ESTO POR EL ID DEL EMPLEADO LOGGEADO!!!

        public FrmVenta()
        {
            InitializeComponent();
            // Llama a listarProductos() cuando el formulario se inicializa para cargar todos los productos.
            listarProductos();
            // Configurar las columnas del DataGridView del carrito al inicio
            ConfigurarCarritoDataGridView();
        }

        // --- Métodos para la Lista de Productos (dgvLista) ---

        // Método para listar productos en el dgvLista DataGridView
        private void listarProductos()
        {
            try
            {
                // Llama al método listarPa de ProductoCln para obtener la lista de productos
                // Ahora retorna List<paProductoListar_Result>
                var lista = ProductoCln.listarPa(txtParametro.Text.Trim());
                dgvLista.DataSource = lista;

                // Configuración de columnas para la lista de productos
                dgvLista.Columns["id"].Visible = false;       // Ocultar columna ID
                dgvLista.Columns["estado"].Visible = false;   // Ocultar columna estado
                dgvLista.Columns["codigo"].HeaderText = "Código";
                dgvLista.Columns["descripcion"].HeaderText = "Descripción";
                dgvLista.Columns["unidadMedida"].HeaderText = "Unidad";
                dgvLista.Columns["saldo"].HeaderText = "Stock";
                dgvLista.Columns["precioVenta"].HeaderText = "Precio Unitario"; // Mostrar precio de venta
                // Ocultar otras columnas que puedan venir del SP pero no son necesarias en la UI
                if (dgvLista.Columns.Contains("idCategoria")) dgvLista.Columns["idCategoria"].Visible = false;
                if (dgvLista.Columns.Contains("idMarca")) dgvLista.Columns["idMarca"].Visible = false;
                if (dgvLista.Columns.Contains("usuarioRegistro")) dgvLista.Columns["usuarioRegistro"].Visible = false;
                if (dgvLista.Columns.Contains("fechaRegistro")) dgvLista.Columns["fechaRegistro"].Visible = false;


                // Ajustar el tamaño de las columnas automáticamente para mejor lectura
                dgvLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Re-añadir la columna de botón "Añadir" directamente en la grilla
                if (!dgvLista.Columns.Contains("btnAnadirProductoEnGrid"))
                {
                    DataGridViewButtonColumn btnColumna = new DataGridViewButtonColumn
                    {
                        HeaderText = "Acción",
                        Text = "Añadir",
                        UseColumnTextForButtonValue = true,
                        Name = "btnAnadirProductoEnGrid" // Nombre único para la columna de botones en la grilla
                    };
                    dgvLista.Columns.Add(btnColumna);
                }

                if (lista.Count > 0)
                {
                    dgvLista.CurrentCell = dgvLista.Rows[0].Cells["codigo"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Métodos para el Carrito de Ventas (dgvCarrito) ---

        // Configura las columnas para el DataGridView del carrito de ventas
        private void ConfigurarCarritoDataGridView()
        {
            // Limpiar columnas existentes en caso de re-configuración
            dgvCarrito.Columns.Clear();

            // Añadir columnas manualmente para asegurar un orden y comportamiento específico
            dgvCarrito.Columns.Add("idProducto", "ID"); // Oculto pero útil para seguimiento interno
            dgvCarrito.Columns.Add("descripcion", "Descripción");
            dgvCarrito.Columns.Add("cantidad", "Cantidad");
            dgvCarrito.Columns.Add("precioUnitario", "P. Unitario");
            dgvCarrito.Columns.Add("subtotal", "Subtotal");
            dgvCarrito.Columns.Add("stockDisponible", "Stock Disp."); // Para mostrar el stock restante

            // Establecer visibilidad
            dgvCarrito.Columns["idProducto"].Visible = false;
            dgvCarrito.Columns["stockDisponible"].Visible = false; // Ocultar stock en el carrito, pero mantener para validación

            // Añadir columnas de botones "Editar" y "Eliminar" para los ítems del carrito
            DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn
            {
                HeaderText = "Editar",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                Name = "btnEditarCarrito"
            };
            dgvCarrito.Columns.Add(btnEditar);

            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn
            {
                HeaderText = "Eliminar",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                Name = "btnEliminarCarrito"
            };
            dgvCarrito.Columns.Add(btnEliminar);

            dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Actualiza el dgvCarrito DataGridView y calcula el total
        private void actualizarCarrito()
        {
            dgvCarrito.Rows.Clear(); // Limpiar filas existentes antes de volver a poblar
            decimal totalGeneral = 0;

            foreach (var producto in productosEnCarrito)
            {
                // Añadir detalles del producto al dgvCarrito
                int rowIndex = dgvCarrito.Rows.Add();
                dgvCarrito.Rows[rowIndex].Cells["idProducto"].Value = producto.id;
                dgvCarrito.Rows[rowIndex].Cells["descripcion"].Value = producto.descripcion;
                dgvCarrito.Rows[rowIndex].Cells["cantidad"].Value = producto.cantidad;
                dgvCarrito.Rows[rowIndex].Cells["precioUnitario"].Value = producto.precioUnitario;
                dgvCarrito.Rows[rowIndex].Cells["subtotal"].Value = producto.subtotal;
                dgvCarrito.Rows[rowIndex].Cells["stockDisponible"].Value = producto.saldo; // Stock original

                totalGeneral += producto.subtotal;
            }

            // Actualizar la etiqueta del total
            lblTotal.Text = $"Total: {totalGeneral:C}"; // 'C' para formato de moneda
        }

        // --- Manejadores de Eventos ---

        // Manejador de evento para el botón "Buscar"
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listarProductos();
        }

        // Método auxiliar para añadir o aumentar la cantidad de un producto en el carrito
        private void ManejarAnadirProductoAlCarrito(paProductoListar_Result productoSeleccionado)
        {
            if (productoSeleccionado == null) return; // Asegurarse de que el objeto no sea nulo

            int idProducto = productoSeleccionado.id;
            string descripcion = productoSeleccionado.descripcion;
            decimal saldo = productoSeleccionado.saldo;
            decimal precioUnitario = productoSeleccionado.precioVenta;

            if (saldo <= 0)
            {
                MessageBox.Show($"El producto '{descripcion}' no tiene stock disponible.", "Sin Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salir si no hay stock
            }

            // Verificar si el producto ya está en el carrito
            CarritoItem productoEnCarrito = productosEnCarrito.FirstOrDefault(p => p.id == idProducto);

            if (productoEnCarrito != null)
            {
                // El producto ya está en el carrito, intentar aumentar la cantidad
                if (productoEnCarrito.cantidad < saldo) // Verificar contra el stock original
                {
                    productoEnCarrito.cantidad++; // Modificar directamente la propiedad
                    productoEnCarrito.subtotal = productoEnCarrito.cantidad * productoEnCarrito.precioUnitario; // Modificar directamente la propiedad
                    MessageBox.Show($"Cantidad de '{descripcion}' aumentada a {productoEnCarrito.cantidad}.", "Cantidad Actualizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"No se puede añadir más de '{descripcion}'. Se alcanzó el stock máximo disponible ({saldo}).", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // El producto no está en el carrito, añadirlo como un nuevo CarritoItem
                productosEnCarrito.Add(new CarritoItem
                {
                    id = idProducto,
                    descripcion = descripcion,
                    cantidad = 1,
                    precioUnitario = precioUnitario,
                    subtotal = precioUnitario, // Subtotal inicial para 1 cantidad
                    saldo = saldo // Mantener el stock original para validación
                });
                MessageBox.Show($"'{descripcion}' añadido al carrito.", "Producto Añadido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            actualizarCarrito(); // Refrescar la visualización del carrito
        }

        // Manejador de evento para el nuevo botón "Añadir/Aumentar" (externo)
        private void btnAumentarCantidad_Click(object sender, EventArgs e)
        {
            // Verificar si hay una fila seleccionada en el dgvLista
            if (dgvLista.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la lista para añadirlo al carrito.", "Producto No Seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Obtener el producto seleccionado de la fila.
            var productoSeleccionado = dgvLista.SelectedRows[0].DataBoundItem as paProductoListar_Result;
            ManejarAnadirProductoAlCarrito(productoSeleccionado);
        }

        // Manejador de evento para clics dentro del DataGridView de la lista de productos (dgvLista)
        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Si se hizo clic en la columna del botón "Añadir" en la grilla
            if (e.RowIndex >= 0 && dgvLista.Columns[e.ColumnIndex].Name == "btnAnadirProductoEnGrid")
            {
                var productoSeleccionado = dgvLista.Rows[e.RowIndex].DataBoundItem as paProductoListar_Result;
                ManejarAnadirProductoAlCarrito(productoSeleccionado);
            }
            // Si en el futuro se añaden otras columnas de botón a dgvLista, se pueden manejar aquí.
        }

        // Manejador de evento para clics dentro del DataGridView del carrito de ventas (dgvCarrito)
        private void dgvCarrito_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener los datos del producto de la fila clicada en el carrito.
                // Asegúrate de que las celdas existan antes de acceder a ellas.
                // Se usa TryParse para evitar errores si el valor es nulo o no numérico.
                int idProducto = 0;
                if (dgvCarrito.Rows[e.RowIndex].Cells["idProducto"].Value != null &&
                    int.TryParse(dgvCarrito.Rows[e.RowIndex].Cells["idProducto"].Value.ToString(), out idProducto))
                {
                    string descripcion = dgvCarrito.Rows[e.RowIndex].Cells["descripcion"].Value?.ToString() ?? "Producto Desconocido";
                    int cantidadActual = 0;
                    if (dgvCarrito.Rows[e.RowIndex].Cells["cantidad"].Value != null)
                        int.TryParse(dgvCarrito.Rows[e.RowIndex].Cells["cantidad"].Value.ToString(), out cantidadActual);

                    decimal stockDisponible = 0;
                    if (dgvCarrito.Rows[e.RowIndex].Cells["stockDisponible"].Value != null)
                        decimal.TryParse(dgvCarrito.Rows[e.RowIndex].Cells["stockDisponible"].Value.ToString(), out stockDisponible);


                    // --- Manejar clic en el botón "Editar" ---
                    if (dgvCarrito.Columns[e.ColumnIndex].Name == "btnEditarCarrito")
                    {
                        // Solicitar nueva cantidad
                        int nuevaCantidad = PromptCantidad(descripcion, cantidadActual, stockDisponible);

                        if (nuevaCantidad > 0 && nuevaCantidad <= stockDisponible)
                        {
                            // Encontrar el producto en la lista del carrito y actualizar sus propiedades
                            CarritoItem productoAActualizar = productosEnCarrito.FirstOrDefault(p => p.id == idProducto);
                            if (productoAActualizar != null)
                            {
                                productoAActualizar.cantidad = nuevaCantidad; // Modificar directamente
                                productoAActualizar.subtotal = nuevaCantidad * productoAActualizar.precioUnitario; // Modificar directamente

                                actualizarCarrito();
                                MessageBox.Show($"Cantidad de '{descripcion}' actualizada a {nuevaCantidad}.", "Cantidad Modificada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (nuevaCantidad <= 0)
                        {
                            MessageBox.Show("La cantidad debe ser mayor que cero.", "Cantidad Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show($"La cantidad no puede exceder el stock disponible ({stockDisponible}).", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    // --- Manejar clic en el botón "Eliminar" ---
                    else if (dgvCarrito.Columns[e.ColumnIndex].Name == "btnEliminarCarrito")
                    {
                        DialogResult confirmacion = MessageBox.Show($"¿Desea eliminar '{descripcion}' del carrito?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmacion == DialogResult.Yes)
                        {
                            // Eliminar el producto de la lista del carrito
                            CarritoItem productoAEliminar = productosEnCarrito.FirstOrDefault(p => p.id == idProducto);
                            if (productoAEliminar != null)
                            {
                                productosEnCarrito.Remove(productoAEliminar);
                                actualizarCarrito(); // Refrescar la visualización del carrito
                                MessageBox.Show($"'{descripcion}' eliminado del carrito.", "Producto Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
        }

        // Manejador de evento para el botón "Realizar Venta"
        private void btnRealizarVenta_Click(object sender, EventArgs e)
        {
            if (productosEnCarrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío. Añada productos para realizar una venta.", "Carrito Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Confirmar la realización de la venta?", "Confirmar Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // Mapear los productos del carrito a la estructura esperada por VentaCln
                    List<VentaDetalleItem> detallesParaVenta = productosEnCarrito.Select(p => new VentaDetalleItem
                    {
                        IdProducto = p.id,
                        Cantidad = p.cantidad,
                        PrecioUnitario = p.precioUnitario,
                        Subtotal = p.subtotal
                    }).ToList();

                    // Llamar al método de la capa de negocio para insertar la venta
                    // idEmpleadoActual debe ser el ID del empleado loggeado
                    VentaCln.insertarVenta(idEmpleadoActual, detallesParaVenta);

                    MessageBox.Show("Venta realizada con éxito!", "Venta Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    productosEnCarrito.Clear(); // Limpiar el carrito después de la venta
                    actualizarCarrito(); // Refrescar la visualización del carrito
                    listarProductos(); // Refrescar la lista de productos para mostrar el stock actualizado
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al realizar la venta: " + ex.Message, "Error de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Manejador de evento para el botón "Limpiar Carrito"
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (productosEnCarrito.Count == 0)
            {
                MessageBox.Show("El carrito ya está vacío.", "Carrito Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea limpiar todo el carrito?", "Limpiar Carrito", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.Yes)
            {
                productosEnCarrito.Clear();
                actualizarCarrito();
                MessageBox.Show("El carrito ha sido limpiado.", "Carrito Limpio", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- Método Auxiliar ---

        // Método para solicitar cantidad con validación de stock
        private int PromptCantidad(string descripcionProducto, int cantidadActual, decimal stockDisponible)
        {
            Form inputDialog = new Form
            {
                Text = "Editar Cantidad",
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lblPrompt = new Label { Left = 20, Top = 20, Text = $"Cantidad para '{descripcionProducto}':", Width = 280 };
            Label lblStockInfo = new Label { Left = 20, Top = 45, Text = $"Stock disponible: {stockDisponible}", Width = 280, ForeColor = System.Drawing.Color.Blue }; // Mostrar info de stock
            TextBox txtInput = new TextBox { Left = 20, Top = 70, Width = 280, Text = cantidadActual.ToString() };
            Button btnOk = new Button { Text = "Aceptar", Left = 100, Width = 80, Top = 110, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancelar", Left = 200, Width = 80, Top = 110, DialogResult = DialogResult.Cancel };

            txtInput.KeyPress += (sender, e) =>
            {
                // Permitir solo dígitos y teclas de control (como Backspace)
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            inputDialog.Controls.Add(lblPrompt);
            inputDialog.Controls.Add(lblStockInfo);
            inputDialog.Controls.Add(txtInput);
            inputDialog.Controls.Add(btnOk);
            inputDialog.Controls.Add(btnCancel);

            inputDialog.AcceptButton = btnOk;
            inputDialog.CancelButton = btnCancel;

            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                if (int.TryParse(txtInput.Text, out int nuevaCantidad))
                {
                    return nuevaCantidad;
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un número válido.", "Entrada Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return cantidadActual; // Retornar cantidad original si el parseo falla
                }
            }
            return cantidadActual; // Retornar cantidad original si el diálogo es cancelado
        }
    }
}
