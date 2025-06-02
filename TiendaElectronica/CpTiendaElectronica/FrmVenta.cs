using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClnTiendaElectronica; // Lógica de negocio (where VentaCln, ProductoCln, ClienteCln are)
using TiendaElectronica;    // IMPORTANT: This now brings in the shared VentaDetalle

namespace CpTiendaElectronica
{
    public partial class FrmVenta : Form
    {
        // MEJORA: Usar BindingList<T> para que el DataGridView se actualice automáticamente.
        // Ahora refiere a TiendaElectronica.VentaDetalle, la definición única y compartida.
        private BindingList<TiendaElectronica.VentaDetalle> detallesVenta = new BindingList<TiendaElectronica.VentaDetalle>();

        // Asumimos que el ID del empleado se obtiene de la sesión actual
        private int idEmpleadoActual = 1;

        public FrmVenta()
        {
            InitializeComponent();
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarProductos();
            ConfigurarColumnasCarrito();
            dgvCarrito.DataSource = detallesVenta;
            CalcularTotal();
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = ClienteCln.listar();
                if (clientes != null && clientes.Any())
                {
                    cbxClientes.DataSource = clientes;
                    cbxClientes.DisplayMember = "nombreCompleto";
                    cbxClientes.ValueMember = "id";
                    cbxClientes.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("No se encontraron clientes para cargar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxClientes.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos(string parametro = "")
        {
            try
            {
                var productos = ProductoCln.listarPa(parametro);

                var productosParaMostrar = productos.Select(p => new
                {
                    ID = p.id,
                    Descripcion = p.descripcion,
                    Stock = p.saldo,
                    PrecioVenta = p.precioVenta
                }).ToList();

                dgvListaProductos.DataSource = productosParaMostrar;

                if (dgvListaProductos.Columns.Contains("ID")) dgvListaProductos.Columns["ID"].HeaderText = "Código";
                if (dgvListaProductos.Columns.Contains("Descripcion")) dgvListaProductos.Columns["Descripcion"].HeaderText = "Descripción del Producto";
                if (dgvListaProductos.Columns.Contains("Stock")) dgvListaProductos.Columns["Stock"].HeaderText = "Cantidad Disponible";
                if (dgvListaProductos.Columns.Contains("PrecioVenta")) dgvListaProductos.Columns["PrecioVenta"].HeaderText = "Precio Unitario";

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

            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", DataPropertyName = "IdProducto", HeaderText = "ID Producto", Visible = false });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "DescripcionProducto", DataPropertyName = "DescripcionProducto", HeaderText = "Producto", Width = 300 });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cantidad", DataPropertyName = "Cantidad", HeaderText = "Cantidad" });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrecioUnitario", DataPropertyName = "PrecioUnitario", HeaderText = "Precio Unit." });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { Name = "Subtotal", DataPropertyName = "Subtotal", HeaderText = "Subtotal" });

            dgvCarrito.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
            dgvCarrito.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
        }

        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            CargarProductos(txtBusquedaProducto.Text.Trim());
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (dgvListaProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un producto de la lista para agregar al carrito.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filaSeleccionada = dgvListaProductos.SelectedRows[0];
            int idProducto = (int)filaSeleccionada.Cells["ID"].Value;
            string descripcionProducto = filaSeleccionada.Cells["Descripcion"].Value.ToString();
            decimal precioVenta = (decimal)filaSeleccionada.Cells["PrecioVenta"].Value;
            decimal saldoDisponible = (decimal)filaSeleccionada.Cells["Stock"].Value;

            decimal cantidadAAgregar = nudCantidad.Value;

            if (cantidadAAgregar <= 0)
            {
                MessageBox.Show("La cantidad a agregar debe ser mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var itemExistente = detallesVenta.FirstOrDefault(p => p.IdProducto == idProducto);

            if (itemExistente != null)
            {
                // Calculate potential new total quantity
                decimal nuevaCantidadTotal = itemExistente.Cantidad + cantidadAAgregar;

                if (nuevaCantidadTotal > saldoDisponible)
                {
                    MessageBox.Show($"La cantidad total ({nuevaCantidadTotal}) excede el stock disponible ({saldoDisponible}).", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                itemExistente.Cantidad = nuevaCantidadTotal; // Update the quantity
                // Corrected: Calculate subtotal with decimal precision. PrecioUnitario remains the same.
                itemExistente.Subtotal = (int)(itemExistente.Cantidad * itemExistente.PrecioUnitario);
            }
            else
            {
                if (cantidadAAgregar > saldoDisponible)
                {
                    MessageBox.Show($"La cantidad solicitada ({cantidadAAgregar}) excede el stock disponible ({saldoDisponible}).", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var nuevoItem = new TiendaElectronica.VentaDetalle
                {
                    IdProducto = idProducto,
                    DescripcionProducto = descripcionProducto,
                    Cantidad = cantidadAAgregar,
                    PrecioUnitario = precioVenta,
                    // Corrected: Calculate subtotal with decimal precision.
                    Subtotal = (int)(cantidadAAgregar * precioVenta)
                };
                detallesVenta.Add(nuevoItem);
            }

            ActualizarCarrito();
            nudCantidad.Value = 1;
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un producto del carrito para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvCarrito.SelectedRows[0].DataBoundItem is TiendaElectronica.VentaDetalle itemAEliminar)
            {
                detallesVenta.Remove(itemAEliminar);
                ActualizarCarrito();
            }
            else
            {
                MessageBox.Show("No se pudo obtener el producto seleccionado para eliminar. Intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarCarrito()
        {
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            decimal total = detallesVenta.Sum(d => d.Subtotal);
            lblTotalValor.Text = $"S/. {total:N2}";
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            if (cbxClientes.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un cliente para registrar la venta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (detallesVenta.Count == 0)
            {
                MessageBox.Show("El carrito de compras está vacío. Agregue productos para registrar una venta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idCliente = (int)cbxClientes.SelectedValue;
                DateTime fechaVenta = dtpFechaVenta.Value; // Get the date from the DateTimePicker

                // Call the correct insertarVenta overload with fechaVenta
                VentaCln.insertarVenta(idEmpleadoActual, idCliente, fechaVenta, detallesVenta.ToList());

                MessageBox.Show("Venta registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarProductos(); // Reload products to reflect stock changes
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar la venta: {ex.Message}\n\nDetalles técnicos:\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            detallesVenta.Clear();
            CalcularTotal();
            txtBusquedaProducto.Clear();
            cbxClientes.SelectedIndex = -1;
            nudCantidad.Value = 1;
            dtpFechaVenta.Value = DateTime.Now; // Reset date to current date
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea cancelar la venta actual? Se perderán todos los productos en el carrito.", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpiarFormulario();
            }
        }
    }
}