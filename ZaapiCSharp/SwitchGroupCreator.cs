using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace ZaapiCSharp
{

    public partial class SwitchGroupCreator : Form
    {
        public List<Dictionary<string, object>> SwitchWorkUnits;
        private Dictionary<string, object> WorkUnitToUse;
        public Dictionary<string, object> SwitchGroupToUse;
        private string NewSwitchGroupID;
        private string NewSwitchGroupName;
        public SwitchGroupCreator()
        {
          InitializeComponent();
          SwitchWorkUnits = ak.wwise.core.Object.MakeDirectWAQLGetCallList("\"\\Switches\" select descendants where type = \"WorkUnit\"");
           for (int i = 0; i < SwitchWorkUnits.Count; ++i)
           {
               Dictionary<string, object> Entry = SwitchWorkUnits[i];
               comboBox1.Items.Add(Entry["name"]);
               PrintResults(SwitchWorkUnits[i]);
           }
            comboBox1.SelectedItem = comboBox1.Items[0];
            WorkUnitToUse = SwitchWorkUnits[0];
        }
        static void PrintResults(object results)
        {
            foreach (var pair in (Dictionary<string, object>)results)
            {
                Console.WriteLine("Key: " + pair.Key + ", Value: " + pair.Value);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Can't add a switch with empty name!");
                return;
            }
            if (char.IsDigit(textBox2.Text[0]))
            {
                MessageBox.Show("Name cannot start with a number");
                return;
            }
            if (checkedListBox1.Items.Contains(textBox2.Text))
            {
                MessageBox.Show( "'" + textBox2.Text + "' switch value already exists");
                return;
            }

            if (textBox2.Text == "WAAPI")
            {
                Dictionary<string, object> results = ak.wwise.core.GetInfo();
                PrintResults(results);
            }
            if (textBox2.Text == "Create")
            {
               Dictionary<string, object> results = ak.wwise.core.Object.AshtonCreate("{F728071A-94F4-4FF7-9214-21CE70519952}", "Sound", "Roibos");
               PrintResults(results);
            }
            
            checkedListBox1.Items.Add(textBox2.Text);
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
          List<int> IndexesToRemove = new List<int>();
          for (int i = 0; i < checkedListBox1.Items.Count; ++i)
          {
                if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
                {
                    IndexesToRemove.Add(i);

                }
                Console.WriteLine("Loop {0}", i);
            }
          for (int i = IndexesToRemove.Count; i > 0; --i)
          {
                checkedListBox1.Items.RemoveAt(IndexesToRemove[i-1]);
          }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkUnitToUse = SwitchWorkUnits[comboBox1.SelectedIndex];
            Console.WriteLine("Using Work Unit: " + WorkUnitToUse["name"]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("The new Switch Group needs a name");
            }
            SwitchGroupToUse = ak.wwise.core.Object.Create(WorkUnitToUse["id"].ToString(), "SwitchGroup", textBox1.Text);
            NewSwitchGroupID = SwitchGroupToUse["id"].ToString();
            NewSwitchGroupName = SwitchGroupToUse["name"].ToString();
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            {
                string switchNameToAdd = checkedListBox1.Items[i].ToString();
                Dictionary<string, object> Switch = ak.wwise.core.Object.Create(NewSwitchGroupID, "Switch", switchNameToAdd);
            }
            SwitchWizard switchWizard = new SwitchWizard(NewSwitchGroupID, NewSwitchGroupName);
            switchWizard.Show();
            this.Hide();
        }
    }
}
