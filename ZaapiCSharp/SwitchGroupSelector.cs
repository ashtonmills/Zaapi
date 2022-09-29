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
    
    public partial class SwitchGroupSelector : Form
    {
        List<Dictionary<string, object>> SwitchGroupList;
        public SwitchGroupSelector()
        {
            InitializeComponent();
            RefreshSwitchGroupList();
    
           
         //   PrintResults(results[0]);
        }
        private void RefreshSwitchGroupList()
        {
            listBox1.Items.Clear();
            SwitchGroupList = ak.wwise.core.Object.GetAllObjectsOfType("SwitchGroup");
            for (int i = 0; i < SwitchGroupList.Count; ++i)
            {
                Dictionary<string, object> Entry = SwitchGroupList[i];
                listBox1.Items.Add(Entry["name"]);
                PrintResults(SwitchGroupList[i]);
            }
        }
        static void PrintResults(object results)
        {
            foreach (var pair in (Dictionary<string, object>)results)
            {
                Console.WriteLine("Key: " + pair.Key + ", Value: " + pair.Value);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("User selected {0}", listBox1.SelectedItem.ToString());
            Dictionary<string, object> Entry = SwitchGroupList[listBox1.SelectedIndex];
            string SwitchGroupID = Entry["id"].ToString();
            string SwitchGroupName = Entry["name"].ToString();
            SwitchWizard switchWizard = new SwitchWizard(SwitchGroupID, SwitchGroupName);
            switchWizard.Show();
            this.Hide();
       
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SwitchGroupCreator switchGroupCreator = new SwitchGroupCreator();
            switchGroupCreator.Show();
            this.Hide();
        }
    }

}
