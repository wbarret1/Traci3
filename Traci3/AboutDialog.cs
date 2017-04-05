using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Traci3
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
                            
            this.label5.Text = "Version: " + this.GetType().Assembly.GetName().Version.ToString();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
