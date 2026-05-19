using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HypoxicTimer
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            if(string.IsNullOrEmpty(Properties.Settings.Default.saveTo))
            {
                //saveToBrowseButton_Click(null, null);
                String PersonalFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = PersonalFolder + "\\" + Oximeter.PACKET_FOLDER;
                Properties.Settings.Default.saveTo = path;
                saveToBox.Text = path;
            }
            String[] ports = System.IO.Ports.SerialPort.GetPortNames();
            ComPortBox.Items.Clear();
            foreach (string port in ports)
            {
                ComPortBox.Items.Add(port);
            }
            if (ComPortBox.Items.Count > 0 && string.IsNullOrEmpty(ComPortBox.Text))
            {
                ComPortBox.SelectedIndex = 0;
            }
            UpdateDisplay();
            UpdateOximeterVisibility();
        }

        public int DebugLevel { get { return (int)DebugUpDown.Value; } }
        public bool PlaySounds { get { return playSounds.Checked; } }
        public bool HighlightLowValues { get { return HighlightLowValuesBox.Checked; } }
        public bool ShowPulse { get { return showPulseBox.Checked; } }
        public bool AutoClose { get { return AutoCloseCheckBox.Checked; } }
        public bool KeepAwake { get { return KeepAwakeCheckBox.Checked; } }
        public Decimal CloseTime { get { return AutoCloseNumericUpDown.Value; } }
        public int HypoxicTimeMin { get { return (int)HypoxicTime.Value; } }
        public int HTiGoal { get { return (int)GoalNumericUpDown.Value; } }
        public int FirstTimeExtraMin { get { return (int)firstSessionBox.Value; } }
        public int RecoveryTimeMin { get { return (int)RecoveryTime.Value; } }
        public int Sessions { get { return (int)sessionsBox.Value; } }
        public string ComPort { get { return ComPortBox.Text; } }
        public string EndRecoveryFile { get { return EndRecovery.Text; } }
        public string EndHypoxicFile { get { return EndHypoxic.Text; } }
        public string SaveToPath { get { return saveToBox.Text; } }
        public string BackupTo { get { return backupToBox.Text; } }
        public string OximeterType { get { return OximeterTypeComboBox.Text; } }
        private string profilePath = null;
        public string ProfilePath { set { profilePath = value; } }
        public string SaveTo { get { return string.IsNullOrEmpty(profilePath) ? SaveToPath : profilePath; }  }

        private void UpdateDisplay()
        {
            int totalTimeInMins = (HypoxicTimeMin + RecoveryTimeMin) * (Sessions) + FirstTimeExtraMin;
            TimeSpan totalTimeSpan = new TimeSpan(0, totalTimeInMins, 0);
            DateTime finishTime = DateTime.Now.Add(totalTimeSpan);
            TotalTime.Text = "End at: " + finishTime.ToString("h:mm:ss") + " Time: " + Utils.TimeSpanToString(totalTimeSpan);

            int hypoxicTimeInMins = (HypoxicTimeMin) * (Sessions) + FirstTimeExtraMin;
            TimeSpan hypoxicTimeSpan = new TimeSpan(0, hypoxicTimeInMins, 0);
            HypoxicTimeLabel.Text = "Hypoxic Time: " + Utils.TimeSpanToString(hypoxicTimeSpan);

            int HTi75 = (90 - 75) * hypoxicTimeInMins;
            int HTi80 = (90 - 80) * hypoxicTimeInMins;
            int HTi85 = (90 - 85) * hypoxicTimeInMins;
            HTiLabel.Text = "Theoretical HTi: " + HTi85 + " (85%), " + HTi80 + " (80%), " + HTi75 + " (75%)";
        }

        
        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void saveToBrowseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = saveToBox.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                saveToBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void backupToBrowseButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(backupToBox.Text))
            {
                folderBrowserDialog1.SelectedPath = saveToBox.Text;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = backupToBox.Text;
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                saveToBox.Text = folderBrowserDialog1.SelectedPath;

        }

        private void HypoxicTime_ValueChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void RecoveryTime_ValueChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void sessionsBox_ValueChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void firstSessionBox_ValueChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void HypoxicBrowse_Click(object sender, EventArgs e)
        {
            SoundOpenFileDialog.FileName = EndHypoxic.Text;
            if (SoundOpenFileDialog.ShowDialog() == DialogResult.OK)
                EndHypoxic.Text = SoundOpenFileDialog.FileName;

        }

        private void RecoveryBrowse_Click(object sender, EventArgs e)
        {
            SoundOpenFileDialog.FileName = EndRecovery.Text;
            if (SoundOpenFileDialog.ShowDialog() == DialogResult.OK)
                EndRecovery.Text = SoundOpenFileDialog.FileName;
        }

        private void OximeterTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOximeterVisibility();
        }

        private void UpdateOximeterVisibility()
        {
            bool isHid = OximeterType == "CMS 50E (HID)";
            ComPortBox.Visible = !isHid;
            label4.Visible = !isHid;
        }
    }
}
