using TiendaElectronica;
using ClnTiendaElectronica; // Make sure this namespace is correctly referenced
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpTiendaElectronica
{
    public partial class FrmProducto : Form
    {
        private bool esNuevo = false;
        public FrmProducto()
        {
            InitializeComponent();
        }

        private void CargarCategorias()
        {
            try
            {
                var categorias = CategoriaCln.listar();
                cbxCategoria.DataSource = categorias;
                cbxCategoria.DisplayMember = "nombre";
                cbxCategoria.ValueMember = "id";
                cbxCategoria.SelectedIndex = -1; // No category selected initially
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarMarcas()
        {
            try
            {
                var marcas = MarcaCln.listar();
                cbxMarca.DataSource = marcas;
                cbxMarca.DisplayMember = "nombre";
                cbxMarca.ValueMember = "id";
                cbxMarca.SelectedIndex = -1; // No brand selected initially
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listar()
        {
            var lista = ProductoCln.listarPa(txtParametro.Text.Trim());

            // If paProductoListar_Result already contains 'Categoria' and 'Marca' as direct columns,
            // you can assign it directly. Otherwise, you'd need to project it as before.
            // Assuming paProductoListar_Result has CategoriaNombre and MarcaNombre from your SP.
            dgvLista.DataSource = lista.Select(p => new
            {
                p.id,
                p.estado,
                p.codigo,
                p.descripcion,
                p.unidadMedida,
                p.saldo,
                p.precioVenta,
                p.usuarioRegistro,
                p.fechaRegistro,
                // Assuming paProductoListar_Result directly returns these columns
                Categoria = p.CategoriaNombre, // Corrected to use a direct column from SP result if available
                Marca = p.MarcaNombre          // Corrected to use a direct column from SP result if available
            }).ToList();

            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["codigo"].HeaderText = "Código";
            dgvLista.Columns["descripcion"].HeaderText = "Descripción";
            dgvLista.Columns["unidadMedida"].HeaderText = "Unidad de Medida";
            dgvLista.Columns["saldo"].HeaderText = "Saldo";
            dgvLista.Columns["precioVenta"].HeaderText = "Precio de Venta";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha Registro";
            dgvLista.Columns["Categoria"].HeaderText = "Categoría";
            dgvLista.Columns["Marca"].HeaderText = "Marca";

            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["codigo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            Size = new Size(835, 400);
            listar();
            CargarCategorias();
            CargarMarcas();
        }

        private void limpiar()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            cbxUnidadMedida.SelectedIndex = -1;
            nudPrecioVenta.Value = 0;
            nudSaldo.Value = 0;
            cbxCategoria.SelectedIndex = -1; // Clear selected category
            cbxMarca.SelectedIndex = -1;     // Clear selected brand

            // Clear any error messages
            erpCodigo.SetError(txtCodigo, "");
            erpDescripcion.SetError(txtDescripcion, "");
            // Corrected typo: erpUnidadMedida instead of erpUnidadMedMedida
            erpUnidadMedida.SetError(cbxUnidadMedida, "");
            erpPrecioVenta.SetError(nudPrecioVenta, "");
            erpSaldo.SetError(nudSaldo, "");
            erpCategoria.SetError(cbxCategoria, "");
            erpMarca.SetError(cbxMarca, "");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(835, 362);
            limpiar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(835, 580);
            limpiar();
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(835, 580);

            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var producto = ProductoCln.obtenerUno(id); // This now includes Categoria and Marca via .Include()

            txtCodigo.Text = producto.codigo;
            txtDescripcion.Text = producto.descripcion;

            // To correctly set the ComboBox value for UnidadMedida, use FindStringExact
            // if the product.unidadMedida isn't exactly in the Items list or if it's dynamic.
            int umIndex = cbxUnidadMedida.FindStringExact(producto.unidadMedida);
            if (umIndex != -1)
            {
                cbxUnidadMedida.SelectedIndex = umIndex;
            }
            else
            {
                cbxUnidadMedida.SelectedIndex = -1; // Clear if not found
            }

            nudPrecioVenta.Value = producto.precioVenta;
            nudSaldo.Value = producto.saldo;

            // Set selected category and brand based on product data
            // These will work because ProductoCln.obtenerUno now eagerly loads idCategoria and idMarca.
            if (producto.idCategoria > 0)
            {
                cbxCategoria.SelectedValue = producto.idCategoria;
            }
            else
            {
                cbxCategoria.SelectedIndex = -1;
            }

            if (producto.idMarca > 0)
            {
                cbxMarca.SelectedValue = producto.idMarca;
            }
            else
            {
                cbxMarca.SelectedIndex = -1;
            }

            txtCodigo.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpCodigo.SetError(txtCodigo, "");
            erpDescripcion.SetError(txtDescripcion, "");
            erpUnidadMedida.SetError(cbxUnidadMedida, "");
            erpPrecioVenta.SetError(nudPrecioVenta, "");
            erpSaldo.SetError(nudSaldo, "");
            erpCategoria.SetError(cbxCategoria, "");
            erpMarca.SetError(cbxMarca, "");

            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                erpCodigo.SetError(txtCodigo, "El campo Código es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                erpDescripcion.SetError(txtDescripcion, "El campo Descripción es obligatorio");
                esValido = false;
            }
            if (cbxUnidadMedida.SelectedIndex == -1) // Check if an item is selected
            {
                erpUnidadMedida.SetError(cbxUnidadMedida, "El campo Unidad de Medida es obligatorio");
                esValido = false;
            }
            if (nudPrecioVenta.Value <= 0)
            {
                erpPrecioVenta.SetError(nudPrecioVenta, "El Precio de Venta debe ser mayor a 0");
                esValido = false;
            }
            if (nudSaldo.Value < 0)
            {
                erpSaldo.SetError(nudSaldo, "El campo Saldo no puede ser menor a 0");
                esValido = false;
            }
            if (cbxCategoria.SelectedValue == null || (int)cbxCategoria.SelectedValue == 0) // Check for null or default 0 ID
            {
                erpCategoria.SetError(cbxCategoria, "Debe seleccionar una Categoría");
                esValido = false;
            }
            if (cbxMarca.SelectedValue == null || (int)cbxMarca.SelectedValue == 0) // Check for null or default 0 ID
            {
                erpMarca.SetError(cbxMarca, "Debe seleccionar una Marca");
                esValido = false;
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var producto = new Producto();
                producto.codigo = txtCodigo.Text.Trim();
                producto.descripcion = txtDescripcion.Text.Trim();
                producto.unidadMedida = cbxUnidadMedida.Text;
                producto.precioVenta = nudPrecioVenta.Value;
                producto.saldo = nudSaldo.Value;
                producto.usuarioRegistro = Util.usuario.usuario1;

                // Assign Category and Marca IDs
                producto.idCategoria = (int)cbxCategoria.SelectedValue;
                producto.idMarca = (int)cbxMarca.SelectedValue;

                if (esNuevo)
                {
                    producto.fechaRegistro = DateTime.Now;
                    producto.estado = 1;
                    ProductoCln.insertar(producto);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    producto.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    ProductoCln.actualizar(producto);
                }
                listar();
                btnCancelar.PerformClick(); // This will also call limpiar()
                MessageBox.Show("Producto guardado correctamente", "::: Minerva - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string codigo = dgvLista.Rows[index].Cells["codigo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar el producto {codigo}?",
                "::: Minerva - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                ProductoCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("Producto dado de baja correctamente", "::: Minerva - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}