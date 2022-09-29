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
    public partial class SwitchWizard : Form
    {
        private string switchGroupNameToUse;
        private string switchGroupIDToUse;
        private List<Dictionary<string, object>> AMHeirarchy;
        private string SwitchContainerMakeParentID;
        static private List<string> SwitchNames = new List<string>();
        static private List<string> SwitchIds = new List<string>();
        static private List<string> FilesToImport = new List<string>();
        public SwitchWizard(string switchGroupId, string switchGroupName) 
        {
            InitializeComponent();
            label1.Text = switchGroupName;
            switchGroupIDToUse = switchGroupId;
            switchGroupNameToUse = switchGroupName;
            AMHeirarchy = ak.wwise.core.Object.MakeDirectWAQLGetCallList("\"\\Actor-Mixer Hierarchy\" select descendants");
           for (int i = 0; i < AMHeirarchy.Count; ++i)
            {
                Dictionary<string, object> Entry = AMHeirarchy[i];
                comboBox1.Items.Add(Entry["name"]);
            }
            comboBox1.SelectedIndex = 0;
            listBox1.AllowDrop = true;
            listBox1.DragDrop += new DragEventHandler(this.SwitchWizard_DragDrop);
            listBox1.DragEnter += new DragEventHandler(this.SwitchWizard_DragEnter);
        }
        private void SwitchWizard_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filename in files)
            {
                Console.WriteLine("Dropped in: " + filename);
                if (!FilesToImport.Contains(filename))
                {
                    FilesToImport.Add(filename);
                }
            }
            listBox1.Items.Clear();
            foreach(string file in FilesToImport)
            {
                listBox1.Items.Add(file);
            }
           
        }
        private void SwitchWizard_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("User dragged file(s) in");
           
         string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
         foreach(string filename in files)
         {
             if (!filename.EndsWith(".wav"))
             {
                 MessageBox.Show("Can only drag in wavs");
                 return;
             }
         }
            e.Effect = DragDropEffects.Copy;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<string, object> Entry = AMHeirarchy[comboBox1.SelectedIndex];
            SwitchContainerMakeParentID = Entry["id"].ToString();
         //   Console.WriteLine("New switch container will go to: " + SwitchContainerMakeParentID);
        }

        private void MakeSwitchTooltip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> NewSwitchContainer = ak.wwise.core.Object.Create(SwitchContainerMakeParentID, 
                                                                                        "SwitchContainer",
                                                                                        switchGroupNameToUse, WaapiCS.CustomValues.WwiseValues.OnNameConflict.rename);
           string RandomContainerParent = NewSwitchContainer["id"].ToString();
           IDictionary<string, object> args = ak.wwise.core.Object.SetReference(RandomContainerParent, "SwitchGroupOrStateGroup", switchGroupIDToUse);
           foreach (var pair in args)
            {
             //   Console.WriteLine("Key: " + pair.Key + ", Value: " + pair.Value);
            }
            string WAQLCall = "from object \"" + switchGroupIDToUse + "\" select descendants";
          //  Console.WriteLine(WAQLCall);
            List <Dictionary<string, object> > Switches = ak.wwise.core.Object.MakeDirectWAQLGetCallList(WAQLCall);
            foreach(object ObjOut in Switches)
            {
                CacheResults(ObjOut);
            }
            foreach (string entry in SwitchNames)
            {
               // Console.WriteLine(entry);
            }
            foreach (string entry in SwitchIds)
            {
                Console.WriteLine(entry);
            }
            int loops = SwitchNames.Count;
            for (int i = 0; i < loops; ++i)
            {
                string NewRCName = SwitchNames[i];
                Dictionary<string, object> NewRandomContainer = ak.wwise.core.Object.Create(RandomContainerParent,
                                                                                 "RandomSequenceContainer",
                                                                            NewRCName);
            }
        }

        static void CacheResults(object results)
        {
            foreach (var pair in (Dictionary<string, object>)results)
            {
                if (pair.Key == "name")
                {
                    SwitchNames.Add(pair.Value.ToString());
                }
                if (pair.Key == "id")
                {
                    SwitchIds.Add(pair.Value.ToString());
                }
            }
        }
    }
}
