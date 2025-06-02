namespace CpTiendaElectronica
{
    partial class FrmProducto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the supplied method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtParametro = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.dgvLista = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.nudSaldo = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudPrecioVenta = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxUnidadMedida = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.erpCodigo = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpDescripcion = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpUnidadMedida = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpPrecioVenta = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpSaldo = new System.Windows.Forms.ErrorProvider(this.components);
            // --- NEW CONTROLS FOR CATEGORY AND BRAND ---
            this.label9 = new System.Windows.Forms.Label();
            this.cbxCategoria = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbxMarca = new System.Windows.Forms.ComboBox();
            this.erpCategoria = new System.Windows.Forms.ErrorProvider(this.components); // New ErrorProvider
            this.erpMarca = new System.Windows.Forms.ErrorProvider(this.components);    // New ErrorProvider
            // --- END NEW CONTROLS ---

            ((System.ComponentModel.ISupportInitialize)(this.dgvLista)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSaldo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecioVenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpCodigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpDescripcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpPrecioVenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpSaldo)).BeginInit();
            // --- NEW ERROR PROVIDERS INITIALIZATION ---
            ((System.ComponentModel.ISupportInitialize)(this.erpCategoria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpMarca)).BeginInit();
            // --- END NEW ERROR PROVIDERS INITIALIZATION ---
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(308, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Gestión de Producto";
            //
            // txtParametro
            //
            this.txtParametro.Location = new System.Drawing.Point(12, 49);
            this.txtParametro.Name = "txtParametro";
            this.txtParametro.Size = new System.Drawing.Size(606, 20);
            this.txtParametro.TabIndex = 1;
            this.txtParametro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParametro_KeyPress);
            //
            // btnBuscar
            //
            this.btnBuscar.Location = new System.Drawing.Point(624, 47);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            // btnNuevo
            //
            this.btnNuevo.Location = new System.Drawing.Point(12, 75);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(75, 23);
            this.btnNuevo.TabIndex = 3;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnEditar
            //
            this.btnEditar.Location = new System.Drawing.Point(93, 75);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 23);
            this.btnEditar.TabIndex = 4;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Location = new System.Drawing.Point(174, 75);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 5;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Location = new System.Drawing.Point(704, 47);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 7;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // dgvLista
            //
            this.dgvLista.AllowUserToAddRows = false;
            this.dgvLista.AllowUserToDeleteRows = false;
            this.dgvLista.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLista.Location = new System.Drawing.Point(12, 104);
            this.dgvLista.MultiSelect = false;
            this.dgvLista.Name = "dgvLista";
            this.dgvLista.ReadOnly = true;
            this.dgvLista.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLista.Size = new System.Drawing.Size(767, 240);
            this.dgvLista.TabIndex = 6;
            //
            // groupBox1
            //
            // Ensure new controls are added to groupBox1.Controls
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbxMarca);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbxCategoria);
            this.groupBox1.Controls.Add(this.btnCancelar);
            this.groupBox1.Controls.Add(this.btnGuardar);
            this.groupBox1.Controls.Add(this.nudSaldo);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.nudPrecioVenta);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbxUnidadMedida);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtDescripcion);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCodigo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 368);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 190); // Increased height to accommodate new fields
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos del Producto";
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(408, 140); // Adjust location
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 10; // Adjust TabIndex
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(327, 140); // Adjust location
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 9; // Adjust TabIndex
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // nudSaldo
            //
            this.nudSaldo.Location = new System.Drawing.Point(120, 115);
            this.nudSaldo.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudSaldo.Name = "nudSaldo";
            this.nudSaldo.Size = new System.Drawing.Size(120, 20);
            this.nudSaldo.TabIndex = 8; // Adjust TabIndex
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Saldo:";
            //
            // nudPrecioVenta
            //
            this.nudPrecioVenta.DecimalPlaces = 2;
            this.nudPrecioVenta.Location = new System.Drawing.Point(120, 89);
            this.nudPrecioVenta.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudPrecioVenta.Name = "nudPrecioVenta";
            this.nudPrecioVenta.Size = new System.Drawing.Size(120, 20);
            this.nudPrecioVenta.TabIndex = 6; // Adjust TabIndex
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Precio de Venta:";
            //
            // cbxUnidadMedida
            //
            this.cbxUnidadMedida.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUnidadMedida.FormattingEnabled = true;
            this.cbxUnidadMedida.Items.AddRange(new object[] {
            "Unidad",
            "Caja",
            "Paquete",
            "Litro",
            "Kilo"});
            this.cbxUnidadMedida.Location = new System.Drawing.Point(460, 89); // Adjust location
            this.cbxUnidadMedida.Name = "cbxUnidadMedida";
            this.cbxUnidadMedida.Size = new System.Drawing.Size(150, 21);
            this.cbxUnidadMedida.TabIndex = 4; // Adjust TabIndex
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(360, 92); // Adjust location
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Unidad de Medida:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(120, 57);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(200, 20);
            this.txtDescripcion.TabIndex = 2; // Adjust TabIndex
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Descripción:";
            //
            // txtCodigo
            //
            this.txtCodigo.Location = new System.Drawing.Point(120, 27);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(200, 20);
            this.txtCodigo.TabIndex = 0; // Adjust TabIndex
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Código:";
            //
            // erpCodigo
            //
            this.erpCodigo.ContainerControl = this;
            //
            // erpDescripcion
            //
            this.erpDescripcion.ContainerControl = this;
            //
            // erpUnidadMedida
            //
            this.erpUnidadMedida.ContainerControl = this;
            //
            // erpPrecioVenta
            //
            this.erpPrecioVenta.ContainerControl = this;
            //
            // erpSaldo
            //
            this.erpSaldo.ContainerControl = this;
            //
            // label9 (New: Category Label)
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(360, 30); // Adjust location relative to groupBox1
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 11; // Next available TabIndex
            this.label9.Text = "Categoría:";
            //
            // cbxCategoria (New: Category ComboBox)
            //
            this.cbxCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategoria.FormattingEnabled = true;
            this.cbxCategoria.Location = new System.Drawing.Point(460, 27); // Adjust location
            this.cbxCategoria.Name = "cbxCategoria";
            this.cbxCategoria.Size = new System.Drawing.Size(200, 21);
            this.cbxCategoria.TabIndex = 12; // Next available TabIndex
            //
            // label10 (New: Marca Label)
            //
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(360, 60); // Adjust location
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 13; // Next available TabIndex
            this.label10.Text = "Marca:";
            //
            // cbxMarca (New: Marca ComboBox)
            //
            this.cbxMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMarca.FormattingEnabled = true;
            this.cbxMarca.Location = new System.Drawing.Point(460, 57); // Adjust location
            this.cbxMarca.Name = "cbxMarca";
            this.cbxMarca.Size = new System.Drawing.Size(200, 21);
            this.cbxMarca.TabIndex = 14; // Next available TabIndex
            //
            // erpCategoria
            //
            this.erpCategoria.ContainerControl = this;
            //
            // erpMarca
            //
            this.erpMarca.ContainerControl = this;
            //
            // FrmProducto
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 580); // Increased height to 580 for new fields
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvLista);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtParametro);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FrmProducto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Producto";
            this.Load += new System.EventHandler(this.FrmProducto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSaldo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecioVenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpCodigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpDescripcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpPrecioVenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpSaldo)).EndInit();
            // --- NEW ERROR PROVIDERS INITIALIZATION ---
            ((System.ComponentModel.ISupportInitialize)(this.erpCategoria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpMarca)).EndInit();
            // --- END NEW ERROR PROVIDERS INITIALIZATION ---
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtParametro;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.DataGridView dgvLista;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudSaldo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudPrecioVenta;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxUnidadMedida;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.ErrorProvider erpCodigo;
        private System.Windows.Forms.ErrorProvider erpDescripcion;
        private System.Windows.Forms.ErrorProvider erpUnidadMedida;
        private System.Windows.Forms.ErrorProvider erpPrecioVenta;
        private System.Windows.Forms.ErrorProvider erpSaldo;
        // --- NEW CONTROL DECLARATIONS ---
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxCategoria;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbxMarca;
        private System.Windows.Forms.ErrorProvider erpCategoria; // New ErrorProvider declaration
        private System.Windows.Forms.ErrorProvider erpMarca;    // New ErrorProvider declaration
    }
}