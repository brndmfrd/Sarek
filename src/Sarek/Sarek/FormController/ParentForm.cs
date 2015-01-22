using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sarek.FormController
{
    public partial class ParentForm : Form
    {
        MainWindow mw;

        public ParentForm()
        {
            InitializeComponent();
            mw = new MainWindow();
            mw.Show();
        }

        private void ParentForm_Load(object sender, EventArgs e)
        {
            contextMenuStrip1.ShowImageMargin = false;
            
        }
        
        
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mw.Show();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mw.bttn_sign_out_Click(null, null);
        }







    }// end ParentForm
}// end namespace
