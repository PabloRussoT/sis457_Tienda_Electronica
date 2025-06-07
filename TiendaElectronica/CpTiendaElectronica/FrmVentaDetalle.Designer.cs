namespace CpTiendaElectronica
{
    partial class FrmVentaDetalle
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
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTituloDetalle = new System.Windows.Forms.Label();
            this.lblClienteStatic = new System.Windows.Forms.Label();
            this.lblClienteValor = new System.Windows.Forms.Label();
            this.lblFechaVentaStatic = new System.Windows.Forms.Label();
            this.lblFechaVentaValor = new System.Windows.Forms.Label();
            this.dgvDetalleVenta = new System.Windows.Forms.DataGridView();
            this.lblTotalVentaStatic = new System.Windows.Forms.Label();
            this.lblTotalVentaValor = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.grpPago = new System.Windows.Forms.GroupBox();
            this.lblCambioDetalleValor = new System.Windows.Forms.Label();
            this.lblMontoPagadoDetalleValor = new System.Windows.Forms.Label();
            this.lblMetodoPagoDetalleValor = new System.Windows.Forms.Label();
            this.lblCambioDetalle = new System.Windows.Forms.Label();
            this.lblMontoPagadoDetalle = new System.Windows.Forms.Label();
            this.lblMetodoPagoDetalle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleVenta)).BeginInit();
            this.grpPago.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTituloDetalle
            // 
            this.lblTituloDetalle.AutoSize = true;
            this.lblTituloDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloDetalle.ForeColor = System.Drawing.Color.Navy;
            this.lblTituloDetalle.Location = new System.Drawing.Point(20, 20);
            this.lblTituloDetalle.Name = "lblTituloDetalle";
            this.lblTituloDetalle.Size = new System.Drawing.Size(234, 29);
            this.lblTituloDetalle.TabIndex = 0;
            this.lblTituloDetalle.Text = "Detalle de la Venta";
            // 
            // lblClienteStatic
            // 
            this.lblClienteStatic.AutoSize = true;
            this.lblClienteStatic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClienteStatic.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblClienteStatic.Location = new System.Drawing.Point(20, 70);
            this.lblClienteStatic.Name = "lblClienteStatic";
            this.lblClienteStatic.Size = new System.Drawing.Size(65, 18);
            this.lblClienteStatic.TabIndex = 1;
            this.lblClienteStatic.Text = "Cliente:";
            // 
            // lblClienteValor
            // 
            this.lblClienteValor.AutoSize = true;
            this.lblClienteValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClienteValor.ForeColor = System.Drawing.Color.Black;
            this.lblClienteValor.Location = new System.Drawing.Point(90, 70);
            this.lblClienteValor.Name = "lblClienteValor";
            this.lblClienteValor.Size = new System.Drawing.Size(119, 18);
            this.lblClienteValor.TabIndex = 2;
            this.lblClienteValor.Text = "[Nombre Cliente]";
            // 
            // lblFechaVentaStatic
            // 
            this.lblFechaVentaStatic.AutoSize = true;
            this.lblFechaVentaStatic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVentaStatic.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblFechaVentaStatic.Location = new System.Drawing.Point(20, 100);
            this.lblFechaVentaStatic.Name = "lblFechaVentaStatic";
            this.lblFechaVentaStatic.Size = new System.Drawing.Size(161, 18);
            this.lblFechaVentaStatic.TabIndex = 3;
            this.lblFechaVentaStatic.Text = "Fecha y Hora Venta:";
            // 
            // lblFechaVentaValor
            // 
            this.lblFechaVentaValor.AutoSize = true;
            this.lblFechaVentaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVentaValor.ForeColor = System.Drawing.Color.Black;
            this.lblFechaVentaValor.Location = new System.Drawing.Point(190, 100);
            this.lblFechaVentaValor.Name = "lblFechaVentaValor";
            this.lblFechaVentaValor.Size = new System.Drawing.Size(149, 18);
            this.lblFechaVentaValor.TabIndex = 4;
            this.lblFechaVentaValor.Text = "[Fecha y Hora Actual]";
            // 
            // dgvDetalleVenta
            // 
            this.dgvDetalleVenta.AllowUserToAddRows = false;
            this.dgvDetalleVenta.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvDetalleVenta.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetalleVenta.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetalleVenta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetalleVenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleVenta.EnableHeadersVisualStyles = false;
            this.dgvDetalleVenta.Location = new System.Drawing.Point(20, 140);
            this.dgvDetalleVenta.Name = "dgvDetalleVenta";
            this.dgvDetalleVenta.ReadOnly = true;
            this.dgvDetalleVenta.RowHeadersWidth = 51;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.dgvDetalleVenta.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDetalleVenta.RowTemplate.Height = 24;
            this.dgvDetalleVenta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalleVenta.Size = new System.Drawing.Size(760, 170);
            this.dgvDetalleVenta.TabIndex = 5;
            // 
            // lblTotalVentaStatic
            // 
            this.lblTotalVentaStatic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalVentaStatic.AutoSize = true;
            this.lblTotalVentaStatic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalVentaStatic.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTotalVentaStatic.Location = new System.Drawing.Point(520, 340);
            this.lblTotalVentaStatic.Name = "lblTotalVentaStatic";
            this.lblTotalVentaStatic.Size = new System.Drawing.Size(161, 25);
            this.lblTotalVentaStatic.TabIndex = 6;
            this.lblTotalVentaStatic.Text = "Total de Venta:";
            // 
            // lblTotalVentaValor
            // 
            this.lblTotalVentaValor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalVentaValor.AutoSize = true;
            this.lblTotalVentaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalVentaValor.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTotalVentaValor.Location = new System.Drawing.Point(680, 340);
            this.lblTotalVentaValor.Name = "lblTotalVentaValor";
            this.lblTotalVentaValor.Size = new System.Drawing.Size(98, 25);
            this.lblTotalVentaValor.TabIndex = 7;
            this.lblTotalVentaValor.Text = "Bs/. 0.00";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAceptar.FlatAppearance.BorderSize = 0;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(660, 400);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(120, 38);
            this.btnAceptar.TabIndex = 8;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // grpPago
            // 
            this.grpPago.Controls.Add(this.lblCambioDetalleValor);
            this.grpPago.Controls.Add(this.lblMontoPagadoDetalleValor);
            this.grpPago.Controls.Add(this.lblMetodoPagoDetalleValor);
            this.grpPago.Controls.Add(this.lblCambioDetalle);
            this.grpPago.Controls.Add(this.lblMontoPagadoDetalle);
            this.grpPago.Controls.Add(this.lblMetodoPagoDetalle);
            this.grpPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPago.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.grpPago.Location = new System.Drawing.Point(20, 325);
            this.grpPago.Name = "grpPago";
            this.grpPago.Size = new System.Drawing.Size(400, 120);
            this.grpPago.TabIndex = 9;
            this.grpPago.TabStop = false;
            this.grpPago.Text = "Detalles de Pago";
            // 
            // lblCambioDetalleValor
            // 
            this.lblCambioDetalleValor.AutoSize = true;
            this.lblCambioDetalleValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCambioDetalleValor.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblCambioDetalleValor.Location = new System.Drawing.Point(160, 90);
            this.lblCambioDetalleValor.Name = "lblCambioDetalleValor";
            this.lblCambioDetalleValor.Size = new System.Drawing.Size(75, 18);
            this.lblCambioDetalleValor.TabIndex = 5;
            this.lblCambioDetalleValor.Text = "Bs/. 0.00";
            // 
            // lblMontoPagadoDetalleValor
            // 
            this.lblMontoPagadoDetalleValor.AutoSize = true;
            this.lblMontoPagadoDetalleValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontoPagadoDetalleValor.ForeColor = System.Drawing.Color.Black;
            this.lblMontoPagadoDetalleValor.Location = new System.Drawing.Point(160, 60);
            this.lblMontoPagadoDetalleValor.Name = "lblMontoPagadoDetalleValor";
            this.lblMontoPagadoDetalleValor.Size = new System.Drawing.Size(66, 18);
            this.lblMontoPagadoDetalleValor.TabIndex = 3;
            this.lblMontoPagadoDetalleValor.Text = "Bs/. 0.00";
            // 
            // lblMetodoPagoDetalleValor
            // 
            this.lblMetodoPagoDetalleValor.AutoSize = true;
            this.lblMetodoPagoDetalleValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetodoPagoDetalleValor.ForeColor = System.Drawing.Color.Black;
            this.lblMetodoPagoDetalleValor.Location = new System.Drawing.Point(160, 30);
            this.lblMetodoPagoDetalleValor.Name = "lblMetodoPagoDetalleValor";
            this.lblMetodoPagoDetalleValor.Size = new System.Drawing.Size(106, 18);
            this.lblMetodoPagoDetalleValor.TabIndex = 1;
            this.lblMetodoPagoDetalleValor.Text = "[Método Pago]";
            // 
            // lblCambioDetalle
            // 
            this.lblCambioDetalle.AutoSize = true;
            this.lblCambioDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCambioDetalle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblCambioDetalle.Location = new System.Drawing.Point(15, 90);
            this.lblCambioDetalle.Name = "lblCambioDetalle";
            this.lblCambioDetalle.Size = new System.Drawing.Size(71, 18);
            this.lblCambioDetalle.TabIndex = 4;
            this.lblCambioDetalle.Text = "Cambio:";
            // 
            // lblMontoPagadoDetalle
            // 
            this.lblMontoPagadoDetalle.AutoSize = true;
            this.lblMontoPagadoDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontoPagadoDetalle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblMontoPagadoDetalle.Location = new System.Drawing.Point(15, 60);
            this.lblMontoPagadoDetalle.Name = "lblMontoPagadoDetalle";
            this.lblMontoPagadoDetalle.Size = new System.Drawing.Size(123, 18);
            this.lblMontoPagadoDetalle.TabIndex = 2;
            this.lblMontoPagadoDetalle.Text = "Monto Pagado:";
            // 
            // lblMetodoPagoDetalle
            // 
            this.lblMetodoPagoDetalle.AutoSize = true;
            this.lblMetodoPagoDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetodoPagoDetalle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblMetodoPagoDetalle.Location = new System.Drawing.Point(15, 30);
            this.lblMetodoPagoDetalle.Name = "lblMetodoPagoDetalle";
            this.lblMetodoPagoDetalle.Size = new System.Drawing.Size(137, 18);
            this.lblMetodoPagoDetalle.TabIndex = 0;
            this.lblMetodoPagoDetalle.Text = "Método de Pago:";
            // 
            // FrmVentaDetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(800, 460);
            this.Controls.Add(this.grpPago);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.lblTotalVentaValor);
            this.Controls.Add(this.lblTotalVentaStatic);
            this.Controls.Add(this.dgvDetalleVenta);
            this.Controls.Add(this.lblFechaVentaValor);
            this.Controls.Add(this.lblFechaVentaStatic);
            this.Controls.Add(this.lblClienteValor);
            this.Controls.Add(this.lblClienteStatic);
            this.Controls.Add(this.lblTituloDetalle);
            this.Name = "FrmVentaDetalle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalle de Venta";
            this.Load += new System.EventHandler(this.FrmVentaDetalle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleVenta)).EndInit();
            this.grpPago.ResumeLayout(false);
            this.grpPago.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Existing Controls
        private System.Windows.Forms.Label lblTituloDetalle;
        private System.Windows.Forms.Label lblClienteStatic;
        private System.Windows.Forms.Label lblClienteValor;
        private System.Windows.Forms.Label lblFechaVentaStatic;
        private System.Windows.Forms.Label lblFechaVentaValor;
        private System.Windows.Forms.DataGridView dgvDetalleVenta;
        private System.Windows.Forms.Label lblTotalVentaStatic;
        private System.Windows.Forms.Label lblTotalVentaValor;
        private System.Windows.Forms.Button btnAceptar;

        // NEW Controls for Payment Details
        private System.Windows.Forms.GroupBox grpPago;
        private System.Windows.Forms.Label lblCambioDetalleValor;
        private System.Windows.Forms.Label lblMontoPagadoDetalleValor;
        private System.Windows.Forms.Label lblMetodoPagoDetalleValor;
        private System.Windows.Forms.Label lblCambioDetalle;
        private System.Windows.Forms.Label lblMontoPagadoDetalle;
        private System.Windows.Forms.Label lblMetodoPagoDetalle;
    }
}