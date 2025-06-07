using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; // Make sure this is included for .ToList() and .Sum()
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpTiendaElectronica
{
    public partial class FrmVentaDetalle : Form
    {
        private List<VentaDetalleDisplay> _detallesVenta;
        private string _nombreCliente;
        private decimal _totalVenta;

        public FrmVentaDetalle(List<VentaDetalleDisplay> detalles, string nombreCliente, decimal totalVenta)
        {
            InitializeComponent();
            _detallesVenta = detalles;
            _nombreCliente = nombreCliente;
            _totalVenta = totalVenta;
            this.Text = "Detalle de Venta Registrada";
        }

        public FrmVentaDetalle()
        {
            InitializeComponent();
        }

        private void FrmVentaDetalle_Load(object sender, EventArgs e)
        {
            if (lblClienteValor != null)
            {
                lblClienteValor.Text = _nombreCliente;
            }
            if (lblFechaVentaValor != null)
            {
                lblFechaVentaValor.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            if (lblTotalVentaValor != null)
            {
                lblTotalVentaValor.Text = $"Bs/. {_totalVenta:N2}";
            }

            if (dgvDetalleVenta != null)
            {
                dgvDetalleVenta.AutoGenerateColumns = false;
                dgvDetalleVenta.Columns.Clear();

                dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DescripcionProducto", HeaderText = "Producto", Width = 250, ReadOnly = true });
                dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cantidad", HeaderText = "Cantidad", Width = 100, ReadOnly = true });
                dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrecioUnitario", HeaderText = "Precio Unit.", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }, ReadOnly = true });
                dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subtotal", HeaderText = "Subtotal", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }, ReadOnly = true });

                dgvDetalleVenta.DataSource = _detallesVenta;

                dgvDetalleVenta.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dgvDetalleVenta.AllowUserToAddRows = false;
                dgvDetalleVenta.ReadOnly = true;
                dgvDetalleVenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    // The VentaDetalleDisplay class definition should be in FrmVenta.cs (or a common file)
    // and accessible here. Do NOT define it again here.
}