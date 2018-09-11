using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewGG.Cheats.GUI
{
    public partial class Log : Form
    {
        public bool kek=false;
        int errors = 0;
        public Log()
        {
            InitializeComponent();
        }
        public void Add(String Message)
        {
            //if (label1.Text.Length >= 1800) label1.Text = "";
           listBox1.Items.Add(errors +". "+ Message);
            errors++;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            kek = false;
            listBox1.Items.Add(errors + ". " + "Закрыта Консоль ");
            errors++;
            Hide();
        }
    }
}
