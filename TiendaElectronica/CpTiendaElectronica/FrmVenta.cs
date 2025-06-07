using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClnTiendaElectronica; // Lógica de negocio
using TiendaElectronica;    // Entidades del modelo

namespace CpTiendaElectronica
{
    public partial class FrmVenta : Form
    {
        // Lista para manejar los ítems del carrito de compras
        private List<VentaDetalleDisplay> detallesVenta = new List<VentaDetalleDisplay>();
        // Asumimos que el ID del empleado se obtiene de la sesión actual
        // Para este ejemplo, lo dejaremos como un valor fijo.
        private int idEmpleadoActual = 1;

        // NEW: Private list to hold all clients, for filtering purposes
        private List<TiendaElectronica.Cliente> todosLosClientes;

        public FrmVenta()
        {
            InitializeComponent();
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            CargarClientes(); // This will now load ALL clients initially
            CargarProductos();
            ConfigurarColumnasCarrito();
            dtpFechaVenta.Value = DateTime.Now; // Set the date picker to today's date
        }

        private void CargarClientes()
        {
            try
            {
                // Load all clients once and store them in todosLosClientes
                todosLosClientes = ClienteCln.listar();

                // Bind the ComboBox to the full list initially
                cbxClientes.DataSource = todosLosClientes;
                cbxClientes.DisplayMember = "nombreCompleto"; // Make sure Cliente entity has this property
                cbxClientes.ValueMember = "id";              // Make sure Cliente entity has this property
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NEW: Logic for client search
        private void txtBusquedaCliente_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBusquedaCliente.Text.Trim().ToLower();

            // If the search box is empty, show all clients
            if (string.IsNullOrWhiteSpace(searchText))
            {
                cbxClientes.DataSource = todosLosClientes;
            }
            else
            {
                // Filter the 'todosLosClientes' list based on the search text
                // Adjust the properties used for searching (e.g., nombreCompleto, nit, ci)
                var filteredClients = todosLosClientes.Where(c =>
                    c.nombreCompleto.ToLower().Contains(searchText) 
                  // Assuming 'nit' exists and is a string
                       // Assuming 'ci' exists and is a string
                ).ToList();

                // Update the ComboBox's DataSource with the filtered list
                cbxClientes.DataSource = filteredClients;

                // If no clients match, ensure the ComboBox is empty or shows no selection
                if (filteredClients.Count == 0)
                {
                    cbxClientes.SelectedIndex = -1; // No item selected
                }
            }
            // Re-bind to ensure the display updates correctly, even if the source is the same list object
            cbxClientes.DisplayMember = "nombreCompleto";
            cbxClientes.ValueMember = "id";
        }


        private void CargarProductos(string parametro = "")
        {
            try
            {
                // Carga productos usando el procedimiento almacenado para búsqueda
                dgvListaProductos.DataSource = ProductoCln.listarPa(parametro);

                foreach (DataGridViewColumn column in dgvListaProductos.Columns)
                {
                    column.Visible = false;
                }

                if (dgvListaProductos.Columns.Contains("descripcion"))
                {
                    dgvListaProductos.Columns["descripcion"].Visible = true;
                    dgvListaProductos.Columns["descripcion"].HeaderText = "Producto";
                }
                if (dgvListaProductos.Columns.Contains("precioVenta"))
                {
                    dgvListaProductos.Columns["precioVenta"].Visible = true;
                    dgvListaProductos.Columns["precioVenta"].HeaderText = "Precio Unit.";
                    dgvListaProductos.Columns["precioVenta"].DefaultCellStyle.Format = "N2";
                }
                if (dgvListaProductos.Columns.Contains("saldo"))
                {
                    dgvListaProductos.Columns["saldo"].Visible = true;
                    dgvListaProductos.Columns["saldo"].HeaderText = "Stock";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasCarrito()
        {
            dgvCarrito.Columns.Clear();
            dgvCarrito.AutoGenerateColumns = false;

            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "ProductoIdColumn", DataPropertyName = "IdProducto", HeaderText = "ID Producto", Visible = false });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "DescripcionProductoColumn", DataPropertyName = "DescripcionProducto", HeaderText = "Producto", Width = 200, ReadOnly = true });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "CantidadColumn", DataPropertyName = "Cantidad", HeaderText = "Cantidad", Width = 80, ReadOnly = true });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrecioUnitarioColumn", DataPropertyName = "PrecioUnitario", HeaderText = "Precio Unit.", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }, ReadOnly = true });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "SubtotalColumn", DataPropertyName = "Subtotal", HeaderText = "Subtotal", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }, ReadOnly = true });
        }

        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            CargarProductos(txtBusquedaProducto.Text.Trim());
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (dgvListaProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un producto de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filaSeleccionada = dgvListaProductos.SelectedRows[0];
            int idProducto = (int)filaSeleccionada.Cells["id"].Value;
            string descripcionProducto = filaSeleccionada.Cells["descripcion"].Value?.ToString() ?? "N/A";
            decimal precioVenta = (decimal)filaSeleccionada.Cells["precioVenta"].Value;
            decimal saldoDisponible = (decimal)filaSeleccionada.Cells["saldo"].Value;

            decimal cantidadAAgregar = nudCantidad.Value;

            if (cantidadAAgregar <= 0)
            {
                MessageBox.Show("La cantidad a agregar debe ser mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var itemExistente = detallesVenta.FirstOrDefault(p => p.IdProducto == idProducto);

            if (itemExistente != null)
            {
                if (itemExistente.Cantidad + cantidadAAgregar > saldoDisponible)
                {
                    MessageBox.Show("La cantidad solicitada excede el stock disponible para este producto.", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                itemExistente.Cantidad += cantidadAAgregar;
                itemExistente.Subtotal = itemExistente.Cantidad * itemExistente.PrecioUnitario;
            }
            else
            {
                if (cantidadAAgregar > saldoDisponible)
                {
                    MessageBox.Show("La cantidad solicitada excede el stock disponible para este producto.", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var nuevoItem = new VentaDetalleDisplay
                {
                    IdProducto = idProducto,
                    DescripcionProducto = descripcionProducto,
                    Cantidad = cantidadAAgregar,
                    PrecioUnitario = precioVenta,
                    Subtotal = cantidadAAgregar * precioVenta
                };
                detallesVenta.Add(nuevoItem);
            }

            ActualizarCarrito();
            nudCantidad.Value = 1;
        }

        private void ActualizarCarrito()
        {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = detallesVenta.ToList();
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            decimal total = detallesVenta.Sum(d => d.Subtotal);
            lblTotalValor.Text = $"S/. {total:N2}";
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un producto del carrito para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idProductoEliminar = (int)dgvCarrito.SelectedRows[0].Cells["ProductoIdColumn"].Value;
                var itemAEliminar = detallesVenta.FirstOrDefault(p => p.IdProducto == idProductoEliminar);

                if (itemAEliminar != null)
                {
                    detallesVenta.Remove(itemAEliminar);
                    ActualizarCarrito();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar eliminar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            if (cbxClientes.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (detallesVenta.Count == 0)
            {
                MessageBox.Show("El carrito de compras está vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idCliente = (int)cbxClientes.SelectedValue;
                // Get the selected client's name from the ComboBox
                string nombreClienteSeleccionado = cbxClientes.Text;
                decimal totalVentaCalculado = detallesVenta.Sum(d => d.Subtotal);

                Venta nuevaVenta = new Venta
                {
                    idCliente = idCliente,
                    idEmpleado = idEmpleadoActual,
                    fechaVenta = dtpFechaVenta.Value, // Use the date from the DateTimePicker
                    total = totalVentaCalculado,
                    usuarioRegistro = "admin",
                    fechaRegistro = DateTime.Now,
                    estado = 1
                };

                List<TiendaElectronica.VentaDetalle> ventaDetallesParaDB = new List<TiendaElectronica.VentaDetalle>();
                foreach (var displayItem in detallesVenta)
                {
                    ventaDetallesParaDB.Add(new TiendaElectronica.VentaDetalle
                    {
                        idProducto = displayItem.IdProducto,
                        cantidad = (int)displayItem.Cantidad,
                        precioUnitario = displayItem.PrecioUnitario,
                        total = displayItem.Subtotal,
                        usuarioRegistro = "admin",
                        fechaRegistro = DateTime.Now,
                        estado = 1
                    });
                }

                VentaCln.RegistrarVenta(nuevaVenta, ventaDetallesParaDB);

                MessageBox.Show("Venta registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // --- NEW CODE ADDED HERE TO OPEN FrmVentaDetalle ---
                // Create a copy of the list to pass, so modifications in FrmVentaDetalle don't affect FrmVenta's list
                List<VentaDetalleDisplay> detallesParaMostrar = detallesVenta.ToList();
                FrmVentaDetalle frmDetalle = new FrmVentaDetalle(detallesParaMostrar, nombreClienteSeleccionado, totalVentaCalculado);
                frmDetalle.ShowDialog(); // Show the detail form as a modal dialog
                // --- END NEW CODE ---

                LimpiarFormulario();
                CargarProductos(); // Recargar productos para reflejar el stock actualizado
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar la venta: {ex.Message}\n\nDetalle: {ex.InnerException?.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarFormulario()
        {
            detallesVenta.Clear();
            ActualizarCarrito();
            txtBusquedaProducto.Clear();
            txtBusquedaCliente.Clear(); // Clear the client search box
            if (cbxClientes.Items.Count > 0)
            {
                cbxClientes.SelectedIndex = 0; // Select the first client if available
            }
            nudCantidad.Value = 1;
            dtpFechaVenta.Value = DateTime.Now; // Reset date to today
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea cancelar la venta actual?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpiarFormulario();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    // This class is used for displaying data in the DataGridView.
    // It is defined here to be accessible by both FrmVenta and FrmVentaDetalle.
    public class VentaDetalleDisplay
    {
        public int IdProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}