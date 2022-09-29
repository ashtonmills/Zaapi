using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaapiCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SwitchGroupSelector SwitchGroupForm = new SwitchGroupSelector();
            SwitchGroupForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        static void PrintResults(object results)
        {
            foreach (var pair in (Dictionary<string, object>)results)
            {
                Console.WriteLine("Key: " + pair.Key + ", Value: " + pair.Value);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
