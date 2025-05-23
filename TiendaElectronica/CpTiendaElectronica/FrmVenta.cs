using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClnTiendaElectronica; // Lógica de negocio
using TiendaElectronica;     // Entidades del modelo

namespace CpTiendaElectronica
{
    public partial class FrmVenta : Form
    {
        // Lista para manejar los ítems del carrito de compras
        private List<VentaDetalleItem> detallesVenta = new List<VentaDetalleItem>();
        // Asumimos que el ID del empleado se obtiene de la sesión actual
        // Para este ejemplo, lo dejaremos como un valor fijo.
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
        }

        private void CargarClientes()
        {
            // Carga los clientes en el ComboBox
            cbxClientes.DataSource = ClienteCln.listar();
            cbxClientes.DisplayMember = "nombreCompleto";
            cbxClientes.ValueMember = "id";
        }

        private void CargarProductos(string parametro = "")
        {
            // Carga productos usando el procedimiento almacenado para búsqueda
            dgvListaProductos.DataSource = ProductoCln.listarPa(parametro);
            dgvListaProductos.Columns["id"].Visible = false; // Ocultar ID
            dgvListaProductos.Columns["usuarioRegistro"].Visible = false;
            dgvListaProductos.Columns["fechaRegistro"].Visible = false;
            dgvListaProductos.Columns["estado"].Visible = false;
        }

        private void ConfigurarColumnasCarrito()
        {
            // Configura las columnas del DataGridView del carrito
            dgvCarrito.Columns.Clear();
            dgvCarrito.AutoGenerateColumns = false;

            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdProducto", HeaderText = "ID Producto", Visible = false });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DescripcionProducto", HeaderText = "Producto", Width = 300 });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cantidad", HeaderText = "Cantidad" });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrecioUnitario", HeaderText = "Precio Unit." });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subtotal", HeaderText = "Subtotal" });
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
            string descripcionProducto = filaSeleccionada.Cells["descripcion"].Value.ToString();
            decimal precioVenta = (decimal)filaSeleccionada.Cells["precioVenta"].Value;
            decimal saldoDisponible = (decimal)filaSeleccionada.Cells["saldo"].Value;
            decimal cantidadAAgregar = nudCantidad.Value;

            // Verificar si el producto ya está en el carrito
            var itemExistente = detallesVenta.FirstOrDefault(p => p.IdProducto == idProducto);

            if (itemExistente != null)
            {
                // Si el producto ya existe en el carrito, verificamos el saldo contra la cantidad total que habrá
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
                // Si es un producto nuevo en el carrito, solo verificamos contra el saldo
                if (cantidadAAgregar > saldoDisponible)
                {
                    MessageBox.Show("La cantidad solicitada excede el stock disponible para este producto.", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Añadir nuevo item al carrito
                var nuevoItem = new VentaDetalleItem
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
            nudCantidad.Value = 1; // Resetear cantidad
        }

        private void ActualizarCarrito()
        {
            // Refresca el DataGridView del carrito y calcula el total
            dgvCarrito.DataSource = null; // Desvincular para actualizar correctamente
            dgvCarrito.DataSource = detallesVenta.ToList(); // Volver a vincular la lista

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

            int idProductoEliminar = (int)dgvCarrito.SelectedRows[0].Cells["IdProducto"].Value;
            var itemAEliminar = detallesVenta.FirstOrDefault(p => p.IdProducto == idProductoEliminar);

            if (itemAEliminar != null)
            {
                detallesVenta.Remove(itemAEliminar);
                ActualizarCarrito();
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
                // Llamar al método de la capa de negocio para insertar la venta y sus detalles
                VentaCln.insertarVenta(idEmpleadoActual, idCliente, detallesVenta);

                MessageBox.Show("Venta registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarProductos(); // Recargar productos para reflejar el nuevo stock

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            detallesVenta.Clear();
            ActualizarCarrito();
            txtBusquedaProducto.Clear();
            cbxClientes.SelectedIndex = 0;
            nudCantidad.Value = 1;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea cancelar la venta actual?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpiarFormulario();
            }
        }
    }
}