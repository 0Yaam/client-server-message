using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapNhomMMT
{
    public partial class DangKy : Form
    {
        public DangKy()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Hide();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

        }
    }
}
