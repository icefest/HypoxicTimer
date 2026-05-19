namespace HypoxicTimer
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.okayButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveToBrowseButton = new System.Windows.Forms.Button();
            this.backupToBrowseButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.TotalTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.HTiLabel = new System.Windows.Forms.Label();
            this.HypoxicTimeLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.RecoveryBrowse = new System.Windows.Forms.Button();
            this.HypoxicBrowse = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.SoundOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label13 = new System.Windows.Forms.Label();
            this.GoalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.KeepAwakeCheckBox = new System.Windows.Forms.CheckBox();
            this.EndRecovery = new System.Windows.Forms.TextBox();
            this.EndHypoxic = new System.Windows.Forms.TextBox();
            this.AutoCloseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.AutoCloseCheckBox = new System.Windows.Forms.CheckBox();
            this.ProfilesCheckBox = new System.Windows.Forms.CheckBox();
            this.DebugUpDown = new System.Windows.Forms.NumericUpDown();
            this.OximeterTypeComboBox = new System.Windows.Forms.ComboBox();
            this.firstSessionBox = new System.Windows.Forms.NumericUpDown();
            this.backupToBox = new System.Windows.Forms.TextBox();
            this.saveToBox = new System.Windows.Forms.TextBox();
            this.HighlightLowValuesBox = new System.Windows.Forms.CheckBox();
            this.sessionsBox = new System.Windows.Forms.NumericUpDown();
            this.RecoveryTime = new System.Windows.Forms.NumericUpDown();
            this.HypoxicTime = new System.Windows.Forms.NumericUpDown();
            this.showPulseBox = new System.Windows.Forms.CheckBox();
            this.playSounds = new System.Windows.Forms.CheckBox();
            this.ComPortBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.GoalNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoCloseNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebugUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstSessionBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecoveryTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HypoxicTime)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Sessions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Recovery Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Hypoxic Time";
            // 
            // okayButton
            // 
            this.okayButton.Location = new System.Drawing.Point(12, 462);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(75, 23);
            this.okayButton.TabIndex = 31;
            this.okayButton.Text = "OK";
            this.okayButton.UseVisualStyleBackColor = true;
            this.okayButton.Click += new System.EventHandler(this.okayButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 329);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "COM Port (no :)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 359);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Save to";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 393);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Backup to";
            // 
            // saveToBrowseButton
            // 
            this.saveToBrowseButton.Location = new System.Drawing.Point(370, 358);
            this.saveToBrowseButton.Name = "saveToBrowseButton";
            this.saveToBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.saveToBrowseButton.TabIndex = 40;
            this.saveToBrowseButton.Text = "Browse...";
            this.saveToBrowseButton.UseVisualStyleBackColor = true;
            this.saveToBrowseButton.Click += new System.EventHandler(this.saveToBrowseButton_Click);
            // 
            // backupToBrowseButton
            // 
            this.backupToBrowseButton.Location = new System.Drawing.Point(370, 388);
            this.backupToBrowseButton.Name = "backupToBrowseButton";
            this.backupToBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.backupToBrowseButton.TabIndex = 41;
            this.backupToBrowseButton.Text = "Browse...";
            this.backupToBrowseButton.UseVisualStyleBackColor = true;
            this.backupToBrowseButton.Click += new System.EventHandler(this.backupToBrowseButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "1st Session Extra";
            // 
            // TotalTime
            // 
            this.TotalTime.AutoSize = true;
            this.TotalTime.Location = new System.Drawing.Point(151, 139);
            this.TotalTime.Name = "TotalTime";
            this.TotalTime.Size = new System.Drawing.Size(60, 13);
            this.TotalTime.TabIndex = 44;
            this.TotalTime.Text = "Total Time:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 427);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "Oximeter type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(106, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 48;
            this.label9.Text = "Debug";
            // 
            // HTiLabel
            // 
            this.HTiLabel.AutoSize = true;
            this.HTiLabel.Location = new System.Drawing.Point(151, 194);
            this.HTiLabel.Name = "HTiLabel";
            this.HTiLabel.Size = new System.Drawing.Size(27, 13);
            this.HTiLabel.TabIndex = 49;
            this.HTiLabel.Text = "HTi:";
            // 
            // HypoxicTimeLabel
            // 
            this.HypoxicTimeLabel.AutoSize = true;
            this.HypoxicTimeLabel.Location = new System.Drawing.Point(151, 164);
            this.HypoxicTimeLabel.Name = "HypoxicTimeLabel";
            this.HypoxicTimeLabel.Size = new System.Drawing.Size(74, 13);
            this.HypoxicTimeLabel.TabIndex = 50;
            this.HypoxicTimeLabel.Text = "Hypoxic Time:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(198, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 54;
            this.label10.Text = "seconds";
            // 
            // RecoveryBrowse
            // 
            this.RecoveryBrowse.Location = new System.Drawing.Point(370, 288);
            this.RecoveryBrowse.Name = "RecoveryBrowse";
            this.RecoveryBrowse.Size = new System.Drawing.Size(75, 23);
            this.RecoveryBrowse.TabIndex = 60;
            this.RecoveryBrowse.Text = "Browse...";
            this.RecoveryBrowse.UseVisualStyleBackColor = true;
            this.RecoveryBrowse.Click += new System.EventHandler(this.RecoveryBrowse_Click);
            // 
            // HypoxicBrowse
            // 
            this.HypoxicBrowse.Location = new System.Drawing.Point(370, 258);
            this.HypoxicBrowse.Name = "HypoxicBrowse";
            this.HypoxicBrowse.Size = new System.Drawing.Size(75, 23);
            this.HypoxicBrowse.TabIndex = 59;
            this.HypoxicBrowse.Text = "Browse...";
            this.HypoxicBrowse.UseVisualStyleBackColor = true;
            this.HypoxicBrowse.Click += new System.EventHandler(this.HypoxicBrowse_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 293);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 56;
            this.label11.Text = "End Recovery Sound";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 259);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 13);
            this.label12.TabIndex = 55;
            this.label12.Text = "End Hypoxic Sound";
            // 
            // SoundOpenFileDialog
            // 
            this.SoundOpenFileDialog.Filter = "WAV Sound Files|*.wav";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(239, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 62;
            this.label13.Text = "HTi Goal";
            // 
            // GoalNumericUpDown
            // 
            this.GoalNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "HTiGoal", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.GoalNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.GoalNumericUpDown.Location = new System.Drawing.Point(311, 9);
            this.GoalNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.GoalNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.GoalNumericUpDown.Name = "GoalNumericUpDown";
            this.GoalNumericUpDown.Size = new System.Drawing.Size(78, 20);
            this.GoalNumericUpDown.TabIndex = 63;
            this.GoalNumericUpDown.Value = global::HypoxicTimer.Properties.Settings.Default.HTiGoal;
            // 
            // KeepAwakeCheckBox
            // 
            this.KeepAwakeCheckBox.AutoSize = true;
            this.KeepAwakeCheckBox.Checked = global::HypoxicTimer.Properties.Settings.Default.KeepAwake;
            this.KeepAwakeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeepAwakeCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HypoxicTimer.Properties.Settings.Default, "KeepAwake", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.KeepAwakeCheckBox.Location = new System.Drawing.Point(110, 12);
            this.KeepAwakeCheckBox.Name = "KeepAwakeCheckBox";
            this.KeepAwakeCheckBox.Size = new System.Drawing.Size(104, 17);
            this.KeepAwakeCheckBox.TabIndex = 61;
            this.KeepAwakeCheckBox.Text = "Keep PC Awake";
            this.KeepAwakeCheckBox.UseVisualStyleBackColor = true;
            // 
            // EndRecovery
            // 
            this.EndRecovery.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::HypoxicTimer.Properties.Settings.Default, "RecoverySound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EndRecovery.Location = new System.Drawing.Point(119, 290);
            this.EndRecovery.Name = "EndRecovery";
            this.EndRecovery.Size = new System.Drawing.Size(245, 20);
            this.EndRecovery.TabIndex = 58;
            this.EndRecovery.Text = global::HypoxicTimer.Properties.Settings.Default.RecoverySound;
            // 
            // EndHypoxic
            // 
            this.EndHypoxic.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::HypoxicTimer.Properties.Settings.Default, "HypoxicSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EndHypoxic.Location = new System.Drawing.Point(119, 256);
            this.EndHypoxic.Name = "EndHypoxic";
            this.EndHypoxic.Size = new System.Drawing.Size(245, 20);
            this.EndHypoxic.TabIndex = 57;
            this.EndHypoxic.Text = global::HypoxicTimer.Properties.Settings.Default.HypoxicSound;
            // 
            // AutoCloseNumericUpDown
            // 
            this.AutoCloseNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "CloseTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AutoCloseNumericUpDown.DecimalPlaces = 1;
            this.AutoCloseNumericUpDown.Location = new System.Drawing.Point(151, 103);
            this.AutoCloseNumericUpDown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.AutoCloseNumericUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.AutoCloseNumericUpDown.Name = "AutoCloseNumericUpDown";
            this.AutoCloseNumericUpDown.Size = new System.Drawing.Size(41, 20);
            this.AutoCloseNumericUpDown.TabIndex = 53;
            this.AutoCloseNumericUpDown.Value = global::HypoxicTimer.Properties.Settings.Default.CloseTime;
            // 
            // AutoCloseCheckBox
            // 
            this.AutoCloseCheckBox.AutoSize = true;
            this.AutoCloseCheckBox.Checked = global::HypoxicTimer.Properties.Settings.Default.AutoClose;
            this.AutoCloseCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HypoxicTimer.Properties.Settings.Default, "AutoClose", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AutoCloseCheckBox.Location = new System.Drawing.Point(12, 104);
            this.AutoCloseCheckBox.Name = "AutoCloseCheckBox";
            this.AutoCloseCheckBox.Size = new System.Drawing.Size(140, 17);
            this.AutoCloseCheckBox.TabIndex = 52;
            this.AutoCloseCheckBox.Text = "Auto Close Dialogs After";
            this.AutoCloseCheckBox.UseVisualStyleBackColor = true;
            // 
            // ProfilesCheckBox
            // 
            this.ProfilesCheckBox.AutoSize = true;
            this.ProfilesCheckBox.Checked = global::HypoxicTimer.Properties.Settings.Default.Profiles;
            this.ProfilesCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HypoxicTimer.Properties.Settings.Default, "Profiles", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProfilesCheckBox.Location = new System.Drawing.Point(12, 81);
            this.ProfilesCheckBox.Name = "ProfilesCheckBox";
            this.ProfilesCheckBox.Size = new System.Drawing.Size(96, 17);
            this.ProfilesCheckBox.TabIndex = 51;
            this.ProfilesCheckBox.Text = "Enable Profiles";
            this.ProfilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // DebugUpDown
            // 
            this.DebugUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "DebugLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DebugUpDown.Location = new System.Drawing.Point(151, 32);
            this.DebugUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DebugUpDown.Name = "DebugUpDown";
            this.DebugUpDown.Size = new System.Drawing.Size(41, 20);
            this.DebugUpDown.TabIndex = 47;
            this.DebugUpDown.Value = global::HypoxicTimer.Properties.Settings.Default.DebugLevel;
            // 
            // OximeterTypeComboBox
            // 
            this.OximeterTypeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::HypoxicTimer.Properties.Settings.Default, "OximeterType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.OximeterTypeComboBox.FormattingEnabled = true;
            this.OximeterTypeComboBox.Items.AddRange(new object[] {
            "CMS 50E",
            "CMS 60C",
            "CMS 50E (HID)"});
            this.OximeterTypeComboBox.Location = new System.Drawing.Point(110, 427);
            this.OximeterTypeComboBox.Name = "OximeterTypeComboBox";
            this.OximeterTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.OximeterTypeComboBox.TabIndex = 46;
            this.OximeterTypeComboBox.Text = global::HypoxicTimer.Properties.Settings.Default.OximeterType;
            this.OximeterTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.OximeterTypeComboBox_SelectedIndexChanged);
            // 
            // firstSessionBox
            // 
            this.firstSessionBox.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "FirstOffset", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.firstSessionBox.Location = new System.Drawing.Point(109, 223);
            this.firstSessionBox.Name = "firstSessionBox";
            this.firstSessionBox.Size = new System.Drawing.Size(36, 20);
            this.firstSessionBox.TabIndex = 42;
            this.firstSessionBox.Value = global::HypoxicTimer.Properties.Settings.Default.FirstOffset;
            this.firstSessionBox.ValueChanged += new System.EventHandler(this.firstSessionBox_ValueChanged);
            // 
            // backupToBox
            // 
            this.backupToBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::HypoxicTimer.Properties.Settings.Default, "backupToBox", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.backupToBox.Location = new System.Drawing.Point(119, 390);
            this.backupToBox.Name = "backupToBox";
            this.backupToBox.Size = new System.Drawing.Size(245, 20);
            this.backupToBox.TabIndex = 38;
            this.backupToBox.Text = global::HypoxicTimer.Properties.Settings.Default.backupToBox;
            // 
            // saveToBox
            // 
            this.saveToBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::HypoxicTimer.Properties.Settings.Default, "saveTo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.saveToBox.Location = new System.Drawing.Point(119, 356);
            this.saveToBox.Name = "saveToBox";
            this.saveToBox.Size = new System.Drawing.Size(245, 20);
            this.saveToBox.TabIndex = 37;
            this.saveToBox.Text = global::HypoxicTimer.Properties.Settings.Default.saveTo;
            // 
            // HighlightLowValuesBox
            // 
            this.HighlightLowValuesBox.AutoSize = true;
            this.HighlightLowValuesBox.Checked = global::HypoxicTimer.Properties.Settings.Default.HighlightLowValues;
            this.HighlightLowValuesBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HypoxicTimer.Properties.Settings.Default, "HighlightLowValues", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.HighlightLowValuesBox.Location = new System.Drawing.Point(12, 58);
            this.HighlightLowValuesBox.Name = "HighlightLowValuesBox";
            this.HighlightLowValuesBox.Size = new System.Drawing.Size(125, 17);
            this.HighlightLowValuesBox.TabIndex = 34;
            this.HighlightLowValuesBox.Text = "Highlight Low Values";
            this.HighlightLowValuesBox.UseVisualStyleBackColor = true;
            // 
            // sessionsBox
            // 
            this.sessionsBox.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "Sessions", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.sessionsBox.Location = new System.Drawing.Point(109, 192);
            this.sessionsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sessionsBox.Name = "sessionsBox";
            this.sessionsBox.Size = new System.Drawing.Size(36, 20);
            this.sessionsBox.TabIndex = 30;
            this.sessionsBox.Value = global::HypoxicTimer.Properties.Settings.Default.Sessions;
            this.sessionsBox.ValueChanged += new System.EventHandler(this.sessionsBox_ValueChanged);
            // 
            // RecoveryTime
            // 
            this.RecoveryTime.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "RecoveryTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RecoveryTime.Location = new System.Drawing.Point(109, 164);
            this.RecoveryTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RecoveryTime.Name = "RecoveryTime";
            this.RecoveryTime.Size = new System.Drawing.Size(36, 20);
            this.RecoveryTime.TabIndex = 28;
            this.RecoveryTime.Value = global::HypoxicTimer.Properties.Settings.Default.RecoveryTime;
            this.RecoveryTime.ValueChanged += new System.EventHandler(this.RecoveryTime_ValueChanged);
            // 
            // HypoxicTime
            // 
            this.HypoxicTime.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HypoxicTimer.Properties.Settings.Default, "HypoicTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.HypoxicTime.Location = new System.Drawing.Point(109, 137);
            this.HypoxicTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.HypoxicTime.Name = "HypoxicTime";
            this.HypoxicTime.Size = new System.Drawing.Size(36, 20);
            this.HypoxicTime.TabIndex = 25;
            this.HypoxicTime.Value = global::HypoxicTimer.Properties.Settings.Default.HypoicTime;
            this.HypoxicTime.ValueChanged += new System.EventHandler(this.HypoxicTime_ValueChanged);
            // 
            // showPulseBox
            // 
            this.showPulseBox.AutoSize = true;
            this.showPulseBox.Checked = global::HypoxicTimer.Properties.Settings.Default.ShowPulse;
            this.showPulseBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showPulseBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HypoxicTimer.Properties.Settings.Default, "ShowPulse", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showPulseBox.Location = new System.Drawing.Point(12, 35);
            this.showPulseBox.Name = "showPulseBox";
            this.showPulseBox.Size = new System.Drawing.Size(82, 17);
            this.showPulseBox.TabIndex = 24;
            this.showPulseBox.Text = "Show Pulse";
            this.showPulseBox.UseVisualStyleBackColor = true;
            // 
            // playSounds
            // 
            this.playSounds.AutoSize = true;
            this.playSounds.Checked = global::HypoxicTimer.Properties.Settings.Default.PlaySounds;
            this.playSounds.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HypoxicTimer.Properties.Settings.Default, "PlaySounds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.playSounds.Location = new System.Drawing.Point(12, 12);
            this.playSounds.Name = "playSounds";
            this.playSounds.Size = new System.Drawing.Size(85, 17);
            this.playSounds.TabIndex = 23;
            this.playSounds.Text = "Play Sounds";
            this.playSounds.UseVisualStyleBackColor = true;
            // 
            // ComPortBox
            // 
            this.ComPortBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComPortBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::HypoxicTimer.Properties.Settings.Default, "ComPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ComPortBox.FormattingEnabled = true;
            this.ComPortBox.Location = new System.Drawing.Point(119, 323);
            this.ComPortBox.Name = "ComPortBox";
            this.ComPortBox.Size = new System.Drawing.Size(121, 21);
            this.ComPortBox.TabIndex = 64;
            this.ComPortBox.Text = global::HypoxicTimer.Properties.Settings.Default.ComPort;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 507);
            this.Controls.Add(this.ComPortBox);
            this.Controls.Add(this.GoalNumericUpDown);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.KeepAwakeCheckBox);
            this.Controls.Add(this.RecoveryBrowse);
            this.Controls.Add(this.HypoxicBrowse);
            this.Controls.Add(this.EndRecovery);
            this.Controls.Add(this.EndHypoxic);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AutoCloseNumericUpDown);
            this.Controls.Add(this.AutoCloseCheckBox);
            this.Controls.Add(this.ProfilesCheckBox);
            this.Controls.Add(this.HypoxicTimeLabel);
            this.Controls.Add(this.HTiLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.DebugUpDown);
            this.Controls.Add(this.OximeterTypeComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TotalTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.firstSessionBox);
            this.Controls.Add(this.backupToBrowseButton);
            this.Controls.Add(this.saveToBrowseButton);
            this.Controls.Add(this.backupToBox);
            this.Controls.Add(this.saveToBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.HighlightLowValuesBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.sessionsBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RecoveryTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HypoxicTime);
            this.Controls.Add(this.showPulseBox);
            this.Controls.Add(this.playSounds);
            this.Name = "Options";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GoalNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoCloseNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebugUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstSessionBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecoveryTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HypoxicTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showPulseBox;
        private System.Windows.Forms.CheckBox playSounds;
        private System.Windows.Forms.NumericUpDown sessionsBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown RecoveryTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown HypoxicTime;
        private System.Windows.Forms.Button okayButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox HighlightLowValuesBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox saveToBox;
        private System.Windows.Forms.TextBox backupToBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button saveToBrowseButton;
        private System.Windows.Forms.Button backupToBrowseButton;
        private System.Windows.Forms.NumericUpDown firstSessionBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label TotalTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox OximeterTypeComboBox;
        private System.Windows.Forms.NumericUpDown DebugUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label HTiLabel;
        private System.Windows.Forms.Label HypoxicTimeLabel;
        private System.Windows.Forms.CheckBox ProfilesCheckBox;
        private System.Windows.Forms.CheckBox AutoCloseCheckBox;
        private System.Windows.Forms.NumericUpDown AutoCloseNumericUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button RecoveryBrowse;
        private System.Windows.Forms.Button HypoxicBrowse;
        private System.Windows.Forms.TextBox EndRecovery;
        private System.Windows.Forms.TextBox EndHypoxic;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.OpenFileDialog SoundOpenFileDialog;
        private System.Windows.Forms.CheckBox KeepAwakeCheckBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown GoalNumericUpDown;
        private System.Windows.Forms.ComboBox ComPortBox;
    }
}