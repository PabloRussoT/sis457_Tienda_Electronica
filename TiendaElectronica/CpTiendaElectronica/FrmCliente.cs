using TiendaElectronica; // Asegúrate de que este namespace apunte a tu modelo de Entity Framework
using ClnTiendaElectronica; // Para la clase ClienteCln (capa de negocio)
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
    public partial class FrmCliente : Form
    {
        private bool esNuevo = false;

        public FrmCliente()
        {
            InitializeComponent();
        }

        // Método para listar los clientes en el DataGridView
        private void listar()
        {
            try
            {
                // Llama al método listarPa de ClienteCln, pasando el texto del buscador
                var lista = ClienteCln.listarPa(txtParametro.Text.Trim());

                // Asigna la lista como origen de datos del DataGridView
                dgvLista.DataSource = lista;

                // Ocultar columnas que no son relevantes para el usuario o son internas
                dgvLista.Columns["id"].Visible = false;
                dgvLista.Columns["estado"].Visible = false;
                dgvLista.Columns["usuarioRegistro"].Visible = false;
                dgvLista.Columns["fechaRegistro"].Visible = false;

                // Renombrar encabezados de columnas para una mejor presentación
                dgvLista.Columns["nit"].HeaderText = "NIT";
                dgvLista.Columns["nombres"].HeaderText = "Nombres";
                dgvLista.Columns["apellidos"].HeaderText = "Apellidos";
               dgvLista.Columns["nombreCompleto"].Visible = false;
                dgvLista.Columns["direccion"].HeaderText = "Dirección";
                dgvLista.Columns["telefono"].HeaderText = "Teléfono";
                dgvLista.Columns["email"].HeaderText = "Email";

                // Si hay datos en la lista, selecciona la primera fila por defecto
                if (lista.Count > 0)
                {
                    dgvLista.CurrentCell = dgvLista.Rows[0].Cells["nit"];
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                }
                else
                {
                    // Si no hay datos, deshabilitar los botones de editar y eliminar
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar clientes: {ex.Message}", "::: Minerva - Error :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para limpiar los campos de texto del formulario
        private void limpiar()
        {
            txtNit.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtEmail.Clear();

            // Limpiar mensajes de error de los ErrorProvider
            erpNit.SetError(txtNit, "");
            erpNombres.SetError(txtNombres, "");
            erpApellidos.SetError(txtApellidos, "");
            erpTelefono.SetError(txtTelefono, "");
            erpDireccion.SetError(txtDireccion, "");
            erpEmail.SetError(txtEmail, "");
        }

        // Método para validar la entrada de datos antes de guardar
        private bool validar()
        {
            bool esValido = true;

            // Limpiar todos los ErrorProvider al inicio de la validación
            erpNit.SetError(txtNit, "");
            erpNombres.SetError(txtNombres, "");
            erpApellidos.SetError(txtApellidos, "");
            erpTelefono.SetError(txtTelefono, "");
            erpDireccion.SetError(txtDireccion, "");
            erpEmail.SetError(txtEmail, "");

            // Reglas de validación
            if (string.IsNullOrEmpty(txtNit.Text))
            {
                erpNit.SetError(txtNit, "El campo NIT es obligatorio.");
                esValido = false;
            }
            else if (!long.TryParse(txtNit.Text, out _))
            {
                erpNit.SetError(txtNit, "El campo NIT solo acepta números.");
                esValido = false;
            }

            if (string.IsNullOrEmpty(txtNombres.Text))
            {
                erpNombres.SetError(txtNombres, "El campo Nombres es obligatorio.");
                esValido = false;
            }
            else if (txtNombres.Text.Length > 50)
            {
                erpNombres.SetError(txtNombres, "El campo Nombres debe tener como máximo 50 caracteres.");
                esValido = false;
            }

            if (string.IsNullOrEmpty(txtApellidos.Text))
            {
                erpApellidos.SetError(txtApellidos, "El campo Apellidos es obligatorio.");
                esValido = false;
            }
            else if (txtApellidos.Text.Length > 50)
            {
                erpApellidos.SetError(txtApellidos, "El campo Apellidos debe tener como máximo 50 caracteres.");
                esValido = false;
            }

            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                erpTelefono.SetError(txtTelefono, "El campo Teléfono es obligatorio.");
                esValido = false;
            }
            else if (!long.TryParse(txtTelefono.Text, out _))
            {
                erpTelefono.SetError(txtTelefono, "El campo Teléfono solo acepta números.");
                esValido = false;
            }
            else if (txtTelefono.Text.Length > 15)
            {
                erpTelefono.SetError(txtTelefono, "El campo Teléfono debe tener como máximo 15 caracteres.");
                esValido = false;
            }

            if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                erpDireccion.SetError(txtDireccion, "El campo Dirección es obligatorio.");
                esValido = false;
            }
            else if (txtDireccion.Text.Length > 250)
            {
                erpDireccion.SetError(txtDireccion, "El campo Dirección debe tener como máximo 250 caracteres.");
                esValido = false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                erpEmail.SetError(txtEmail, "El campo Email es obligatorio.");
                esValido = false;
            }
            else if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                erpEmail.SetError(txtEmail, "El campo Email no tiene un formato válido.");
                esValido = false;
            }
            else if (txtEmail.Text.Length > 100)
            {
                erpEmail.SetError(txtEmail, "El campo Email debe tener como máximo 100 caracteres.");
                esValido = false;
            }

            return esValido;
        }

        //--- Eventos del Formulario ---

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            // Ajusta el tamaño inicial del formulario para mostrar solo la tabla de clientes
            Size = new Size(1131, 400); // Matches FrmProducto initial size
            listar(); // Carga la lista de clientes al iniciar el formulario
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar(); // Realiza la búsqueda y actualiza el DataGridView
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si se presiona Enter en el campo de búsqueda, se ejecuta la búsqueda
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true; // Indica que se va a crear un nuevo registro
            // Expande el formulario para mostrar la sección de entrada de datos
            Size = new Size(1131, 761); // Matches FrmProducto expanded size
            limpiar();       // Limpia todos los campos
            txtNit.Focus(); // Pone el foco en el primer campo
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false; // Indica que se va a editar un registro existente
            // Expande el formulario para mostrar la sección de entrada de datos
            Size = new Size(1131, 761); // Matches FrmProducto expanded size

            // Obtiene el índice de la fila seleccionada y el ID del cliente
            int index = dgvLista.CurrentCell.RowIndex;
            int idCliente = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);

            try
            {
                // Obtiene los datos del cliente desde la capa de negocio
                var cliente = ClienteCln.obtenerUno(idCliente);

                // Carga los datos del cliente en los controles del formulario
                txtNit.Text = cliente.nit?.ToString(); // Usa el operador ?. para manejar valores nulos
                txtNombres.Text = cliente.nombres;
                txtApellidos.Text = cliente.apellidos;
                txtDireccion.Text = cliente.direccion;
                txtTelefono.Text = cliente.telefono;
                txtEmail.Text = cliente.email;

                txtNit.Focus(); // Pone el foco en el campo NIT
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del cliente para editar: {ex.Message}", "::: Minerva - Error :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar()) // Valida los datos antes de intentar guardar
            {
                var cliente = new Cliente();
                // Asigna los valores de los controles al objeto Cliente
                // Para el NIT, si el campo es numérico y no nulo en la BD, se puede parsear directamente.
                // Si el NIT es opcional (BIGINT NULL), debes manejar el caso de campo vacío en el formulario.
                cliente.nit = string.IsNullOrEmpty(txtNit.Text.Trim()) ? (long?)null : long.Parse(txtNit.Text.Trim());
                cliente.nombres = txtNombres.Text.Trim();
                cliente.apellidos = txtApellidos.Text.Trim();
                cliente.nombreCompleto = $"{cliente.nombres} {cliente.apellidos}"; // Concatena nombres y apellidos
                cliente.direccion = txtDireccion.Text.Trim();
                cliente.telefono = txtTelefono.Text.Trim();
                cliente.email = txtEmail.Text.Trim();

                // Asume que Util.usuario.usuario1 contiene el nombre de usuario actual
                // Si no tienes esta clase o variable, reemplázala con una cadena fija como "admin"
                cliente.usuarioRegistro = "admin"; // Ejemplo: Util.usuario.usuario1;

                if (esNuevo)
                {
                    // fechaRegistro y estado suelen tener DEFAULT en la tabla y se manejan en el modelo/BD
                    // cliente.fechaRegistro = DateTime.Now;  
                    // cliente.estado = 1;  
                    try
                    {
                        // Llama al método de inserción de la capa de negocio
                        ClienteCln.insertar(cliente);
                        MessageBox.Show("Cliente insertado correctamente.", "::: Minerva - Mensaje :::",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al insertar cliente: {ex.Message}", "::: Minerva - Error :::",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Si es una edición
                {
                    // Obtiene el ID del cliente seleccionado
                    int index = dgvLista.CurrentCell.RowIndex;
                    cliente.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    // Mantén el estado actual o cámbialo si tu UI lo permite.
                    cliente.estado = Convert.ToInt16(dgvLista.Rows[index].Cells["estado"].Value);

                    try
                    {
                        // Llama al método de actualización de la capa de negocio
                        ClienteCln.actualizar(cliente);
                        MessageBox.Show("Cliente actualizado correctamente.", "::: Minerva - Mensaje :::",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar cliente: {ex.Message}", "::: Minerva - Error :::",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                listar();              // Refresca el DataGridView
                btnCancelar.PerformClick(); // Vuelve al estado inicial del formulario
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Restablece el tamaño del formulario
            Size = new Size(835, 400); // Matches FrmProducto cancelled size
            limpiar(); // Limpia los campos de entrada
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Verifica que haya una fila seleccionada
            if (dgvLista.CurrentCell == null)
            {
                MessageBox.Show("Seleccione un cliente para eliminar.", "::: Minerva - Advertencia :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = dgvLista.CurrentCell.RowIndex;
            int idCliente = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string nitCliente = dgvLista.Rows[index].Cells["nit"].Value?.ToString() ?? "N/A"; // Manejar NIT nulo
            string nombreCompletoCliente = dgvLista.Rows[index].Cells["nombreCompleto"].Value.ToString();

            // Pide confirmación al usuario
            DialogResult dialog = MessageBox.Show(
                $"¿Está seguro de dar de baja al cliente con NIT: {nitCliente} ({nombreCompletoCliente})?",
                "::: Minerva - Confirmación de Eliminación :::",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialog == DialogResult.Yes)
            {
                try
                {
                    // Asume que Util.usuario.usuario1 contiene el nombre de usuario actual
                    // Si no tienes esta clase o variable, reemplázala con una cadena fija como "admin"
                    string usuarioEliminacion = "admin"; // Ejemplo: Util.usuario.usuario1;
                    ClienteCln.eliminar(idCliente, usuarioEliminacion); // Llama al método de eliminación lógica
                    MessageBox.Show("Cliente dado de baja correctamente.", "::: Minerva - Mensaje :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listar(); // Refresca el DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al dar de baja al cliente: {ex.Message}", "::: Minerva - Error :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close(); // Cierra el formulario
        }
    }
}