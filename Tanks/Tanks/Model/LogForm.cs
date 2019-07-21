using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Model
{
    public partial class LogForm : Form
    {
        private static bool isClosed = true;
        public static bool IsClosed { set => isClosed= value; get => isClosed; }
        public LogForm(BindingList<Log> logs)
        {
            InitializeComponent();
            dgvLog.DataSource = logs;
            isClosed = false;
        }

        public void RefreshDgv(BindingList<Log> logs)
        {
            dgvLog.DataSource = null;
            dgvLog.DataSource = logs;
        }

        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
        }
    }
}
