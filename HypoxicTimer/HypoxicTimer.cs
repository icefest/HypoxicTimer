using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Media;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using NPlot;

namespace HypoxicTimer
{
    public partial class HypoxicTimer : Form
    {
        private int ScreenSaverTimeout;
        private bool ScreenSaverActive;

        SoundPlayer StartStopSound = new SoundPlayer();
        //SoundPlayer beepsp_a = new SoundPlayer();
        //SoundPlayer beepsp_b = new SoundPlayer();
        //SoundPlayer beepsp_c = new SoundPlayer();
        Sound a = new Sound("phasesr_75_6");
        Sound b = new Sound("phasesr_50_3");
        Sound c = new Sound("phasesr2");
        Sound Temple12 = new Sound("Temple-12");
        Sound Temple = new Sound("Temple");
        Sound Beep = new Sound("Beep");
        Sound Beep1 = new Sound("Beep1");
        Sound Beep2 = new Sound("Beep2");
        Sound Beep3 = new Sound("Beep3");
        Sound Beep4 = new Sound("Beep4");
        Sound RedAlert = new Sound("RedAlert");
        Sound Industrial = new Sound("Industrial Alarm");


        class Sound
        {
            public static void Stop()
            {
                foreach (Sound s in us)
                {
                    s.sp.Stop();
                }
            }
            private static List<Sound> us = new List<Sound>();
            public Sound(string name)
            {
                sp.Stream = this.GetType().Assembly.GetManifestResourceStream("HypoxicTimer.Resources." + name + ".wav");
                Sound.us.Add(this);
            }
            SoundPlayer sp = new SoundPlayer();
            public void Play()
            {
                sp.Play();
            }
        }


        public HypoxicTimer()
        {
            LoadUserSettings();
            InitializeComponent();
            theOptions = new Options(); //must be done after loading user settings
            if (Properties.Settings.Default.Profiles)
            {
                ProfilesFolderBrowserDialog.Description = "Select folder for profile";
                ProfilesFolderBrowserDialog.SelectedPath = Properties.Settings.Default.saveTo;
                if (ProfilesFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    theOptions.ProfilePath = ProfilesFolderBrowserDialog.SelectedPath;
                }
            }
            //System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
            this.Text = "Hypoxic Timer V " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            //this.Text
            if (SystemInformation.UserName != "A560507" && SystemInformation.UserName != "fellrnr")
            {
                Debug.Visible = false;
                testModeBox.Visible = false;
            }
            SetPosition();


            Logger.init(theOptions.SaveTo);
            Logger.DebugLevel = theOptions.DebugLevel;

            finishTimer.Start();
            oximeterBox_CheckedChanged(null, null);
            SetupPlot(OxPlotSurface2D, 0, MAX_O2_POINTS, 75, 97, false);
            OxPlotSurface2D.XAxis2 = new LinearAxis(0, MAX_O2_POINTS / 60);
            ((LinearAxis)OxPlotSurface2D.XAxis2).LargeTickStep = 1;
            ((LinearAxis)OxPlotSurface2D.XAxis1).LargeTickStep = 60;

            int totalTimeInSec = (((theOptions.HypoxicTimeMin + theOptions.RecoveryTimeMin) * (theOptions.Sessions)) + theOptions.FirstTimeExtraMin) * 60;
            SetupPlot(OverviewPlotSurface2D1, 0, totalTimeInSec, 65, 97, false);
            ((LinearAxis)OverviewPlotSurface2D1.XAxis1).LargeTickStep = 60;
            OverviewPlotSurface2D1.XAxis2 = new LinearAxis(0, totalTimeInSec / 60);
            ((LinearAxis)OverviewPlotSurface2D1.XAxis2).LargeTickStep = 1;


            SetupPlot(pulsePlotSurface2D, 0, MAX_PULSE_POINTS, 0, 100, false);
            SetupPlot(distributionPlotSurface2D1, 60, 100, 0, 100, false);

            //!goal - initial creation
            SetupPlot(goalPlotSurface2D1, 0, (int)(theOptions.HTiGoal * 1.5), 0, 1, true);
            SetGoalAxis();

            TheTimeLeft.Text = "";
            Period.Text = "";
            TotalHypoxia.Text = "";
            FinishTime.Text = "";
            //Oximeter.Instance().RecievePacket = RecievePacketDelegate;
        }

        private void SetupPlot(NPlot.Windows.PlotSurface2D aPlotSurface2D, int minX, int maxX, int minY, int maxY, bool forGoal)
        {
            aPlotSurface2D.Clear();
            aPlotSurface2D.BackColor = this.BackColor;

            Grid grid = new Grid();
            grid.VerticalGridType = Grid.GridType.Fine;
            if (!forGoal)
            {
                grid.HorizontalGridType = Grid.GridType.Fine;
            }
            grid.MajorGridPen = new Pen(Color.LightGray, 1.0f);
            aPlotSurface2D.Add(grid);

            PointPlot aPointPlot = CreateInvisiblePointsToFixGrid(minX, maxX, minY, maxY);
            aPlotSurface2D.Add(aPointPlot);
        }

        private static PointPlot CreateInvisiblePointsToFixGrid(int minX, int maxX, int minY, int maxY)
        {
            PointPlot aPointPlot = new PointPlot();
            ArrayList forceX = new ArrayList();
            ArrayList forceY = new ArrayList();
            forceX.Add(minX);
            forceY.Add(minY);
            forceX.Add(maxX);
            forceY.Add(maxY);
            aPointPlot.AbscissaData = forceX;
            aPointPlot.OrdinateData = forceY;
            aPointPlot.Marker = new Marker(Marker.MarkerType.None);
            return aPointPlot;
        }
        private bool inTick = false;
        private void mainTimer_Tick(object sender, EventArgs e)
        {
            if (inTick)
                return;
            inTick = true;
            if (theOptions.KeepAwake)
                KeepAwake.Wakeup(); //don't let the PC go to sleep
            //Jiggler.Jiggle(1, 1);
            int timeInIntervalInMins = hypoxic ? theOptions.HypoxicTimeMin : theOptions.RecoveryTimeMin;
            if (hypoxic && sessions == 1)
                timeInIntervalInMins += theOptions.FirstTimeExtraMin;
            offsetBeginingToNow = DateTime.Now.Subtract(begining).Subtract(timePausedThisInterval);
            TimeSpan timeInIntervalSpan;
            if (testModeBox.Checked)
            {
                timeInIntervalSpan = new TimeSpan(0, 0, timeInIntervalInMins);
            }
            else
            {
                timeInIntervalSpan = new TimeSpan(0, timeInIntervalInMins, 0);
            }
            TimeSpan remainingTimeInInterval = timeInIntervalSpan.Subtract(offsetBeginingToNow);
            hypoxicTimeIncludingCurrent = hypoxicTotalForCompletedIntervals;
            overallTimeIncludingCurrent = overallTotalForCompletedIntervals;

            if (hypoxic)
                hypoxicTimeIncludingCurrent = hypoxicTimeIncludingCurrent.Add(offsetBeginingToNow);
            overallTimeIncludingCurrent = overallTimeIncludingCurrent.Add(offsetBeginingToNow);
            string ht = Utils.TimeSpanToString(hypoxicTimeIncludingCurrent);
            string tt = Utils.TimeSpanToString(overallTimeIncludingCurrent);
            TotalHypoxia.Text = "Total " + tt + " Hypoxic " + ht + " " + sessions + "/" + theOptions.Sessions;

            bool terminateEarly = false;
            if(sessions == theOptions.Sessions && !hypoxic && currentO2 > 90 && timeAbove90 > 5)
            {
                terminateEarly = true;
            }
            if (remainingTimeInInterval.Ticks < 0 || abort || terminateEarly)
            {
                mainTimer.Stop();

                if ((sessions == theOptions.Sessions && !hypoxic) || abort)
                {
                    finishTimer.Stop();
                    mainTimer.Stop();
                    oximeterTimer.Stop();
                    Oximeter.Instance().SaveData(theOptions.SaveTo, theOptions.BackupTo);
                    Oximeter.Instance().Stop();
                    Start.Enabled = true;

                    MessageBox.Show("Complete", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ShowDiary();

                    inTick = false;
                    return;
                }

                if (theOptions.PlaySounds)
                {
                    if (hypoxic && !string.IsNullOrEmpty(theOptions.EndHypoxicFile))
                    {
                        StartStopSound.SoundLocation = theOptions.EndHypoxicFile;
                        StartStopSound.Play();
                    }
                    else if (!hypoxic && !string.IsNullOrEmpty(theOptions.EndRecoveryFile))
                    {
                        StartStopSound.SoundLocation = theOptions.EndRecoveryFile;
                        StartStopSound.Play();
                    }
                    else
                    {
                        string sound = (hypoxic ? "rooster2.wav" : "wolf.wav");
                        StartStopSound.Stream = this.GetType().Assembly.GetManifestResourceStream("HypoxicTimer.Resources." + sound);
                        StartStopSound.Play();
                    }
                }
                DialogResult dr;
                if (theOptions.AutoClose)
                {
                    dr = Utils.ShowAutoClosingMessageBox("Times up " + (hypoxic ? "remove hypoxicator" : "start hypoxication"), "Times up", ((int)(theOptions.CloseTime * 1000)));
                }
                else
                {
                    dr = MessageBox.Show("Times up " + (hypoxic ? "remove hypoxicator" : "start hypoxication"), "Times up", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                }
                if (dr == DialogResult.Cancel)
                {
                    StartStopSound.Stop();
                    inTick = false;
                    return;
                }
                StartStopSound.Stop();
                mainTimer.Start();
                if (hypoxic)
                {
                    hypoxicTotalForCompletedIntervals = hypoxicTotalForCompletedIntervals.Add(offsetBeginingToNow);
                }
                else
                {
                    sessions++;
                }
                overallTotalForCompletedIntervals = overallTotalForCompletedIntervals.Add(offsetBeginingToNow);
                hypoxic = !hypoxic;
                AddVerticalLine(hypoxic);

                begining = DateTime.Now;
                timePausedThisInterval = new TimeSpan();
            }
            else
            {
                //string left = remainingTimeInInterval.ToString();
                //left = left.Substring(3, left.IndexOf('.') - 3);
                string left = remainingTimeInInterval.ToString(@"mm\:ss");
                TheTimeLeft.Text = left;
            }
            Period.Text = hypoxic ? "Hypoxia" : "Recovery";
            Period.ForeColor = hypoxic ? Color.Red : Color.Green;
            TheTimeLeft.ForeColor = hypoxic ? Color.Red : Color.Green;
            inTick = false;
        }

        TimeSpan hypoxicTotalForCompletedIntervals = new TimeSpan();
        TimeSpan hypoxicTimeIncludingCurrent = new TimeSpan();
        TimeSpan overallTotalForCompletedIntervals = new TimeSpan();
        TimeSpan overallTimeIncludingCurrent = new TimeSpan();
        TimeSpan offsetBeginingToNow = new TimeSpan();
        DateTime begining;
        bool hypoxic = true;
        int sessions = 1;
        private void Start_Click(object sender, EventArgs e)
        {
            Start.Enabled = false;
            hypoxicTotalForCompletedIntervals = new TimeSpan();
            overallTotalForCompletedIntervals = new TimeSpan();
            begining = DateTime.Now;
            AddVerticalLine(hypoxic);
            mainTimer.Start();
        }

        bool paused = false;
        TimeSpan totalTimePaused = new TimeSpan();
        TimeSpan timePausedThisInterval = new TimeSpan();
        DateTime startOfPause = new DateTime();
        private void Pause_Click(object sender, EventArgs e)
        {
            paused = !paused;
            if (paused)
            {
                mainTimer.Stop();
                startOfPause = DateTime.Now;
                Pause.Text = "Continue";
            }
            else
            {
                TimeSpan thisPause = DateTime.Now.Subtract(startOfPause);
                totalTimePaused = totalTimePaused.Add(thisPause);
                timePausedThisInterval = timePausedThisInterval.Add(thisPause);
                mainTimer.Start();
                Pause.Text = "Pause";
            }
        }

        public static void LoadUserSettings()
        {
            if (Properties.Settings.Default.DoUpgrade)
            {
                //MessageBox.Show("Upgrading properties, old save is " + Properties.Settings.Default.GetPreviousVersion("saveTo") + ", existing is " + Properties.Settings.Default.saveTo);

                Properties.Settings.Default.Upgrade();
                //Properties.Settings.Default.Reload();
                Properties.Settings.Default.DoUpgrade = false;
                //SaveUserSettings();
            }
        }
        public static void SaveUserSettings()
        {
            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveUserSettings();
            ScreenSaver.SetScreenSaverTimeout(ScreenSaverTimeout); //set to what it was
            ScreenSaver.SetScreenSaverActive(ScreenSaverActive ? 1 : 0);
        }
        private void HypoxicTimer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Oximeter.Instance().Stop();
            //Oximeter.Instance().SaveData();
        }

        private void finishTimer_Tick(object sender, EventArgs e)
        {
            int totalTimeInMins = (theOptions.HypoxicTimeMin + theOptions.RecoveryTimeMin) * (theOptions.Sessions) + theOptions.FirstTimeExtraMin; 
            TimeSpan totalTimeSpan = new TimeSpan(0, totalTimeInMins, 0);
            DateTime finishTime = DateTime.Now.Add(totalTimeSpan).Subtract(overallTimeIncludingCurrent).Add(totalTimePaused);
            FinishTime.Text = "End at: " + finishTime.ToString("h:mm:ss") + " Left: " + Utils.TimeSpanToString(totalTimeSpan.Subtract(overallTimeIncludingCurrent).Add(totalTimePaused));
        }

        private void oximeterBox_CheckedChanged(object sender, EventArgs e)
        {
            //OxPlotSurface2D.Enabled = oximeterBox.Checked;
            if (oximeterBox.Checked)
            {
                SpO2Label.Text = "...";
                oximeterTimer.Start();
                Oximeter.Instance().Start(theOptions.ComPort, theOptions.OximeterType);
            }
            else
            {
                SpO2Label.Text = "N/A";
                oximeterTimer.Stop();
                Oximeter.Instance().Stop();
            }
        }


        Plotter PulsePlotter = null;
        delegate void SetPointCallback(RealTimePacket aRealTimePacket);
        public void RecievePacketDelegate(RealTimePacket aRealTimePacket)
        {
            if (pulsePlotSurface2D.InvokeRequired)
            {
                SetPointCallback d = new SetPointCallback(RecievePacketDelegate);
                this.Invoke(d, new object[] { aRealTimePacket });
            }
            else
            {
                if (PulsePlotter == null)
                {
                    PulsePlotter = new Plotter(Color.Red, MAX_PULSE_POINTS, pulsePlotSurface2D);
                }
                AddPoint(aRealTimePacket.Pulse, PulsePlotter);
                PulsePlotter.aPlotSurface2D.Refresh();
            }
        }

        int HTindex1 = 0;
        int HTindex2 = 0;
        int currentO2 = 0;
        int timeAbove90 = 0;
        private DateTime lastTick = new DateTime();
        private DateTime nextBeep = DateTime.Now;
        private int lastrange;
        private int lastSpO2 = 99;
        int NoDataCounter = 0;
        int BadDataCounter = 0;
        private void oximeterTimer_Tick(object sender, EventArgs e)
        {
            if (!Oximeter.Instance().IsRunning)
            {
                oximeterBox.Checked = false;
            }
            RealTimePacket aRealPacket = Oximeter.Instance().GetLastRealTimePacket();
            if (aRealPacket == null)
            {
                BadDataCounter = 0;
                SpO2Label.Text = "ND " + (++NoDataCounter);
            }
            else if (aRealPacket.SpO2 == 0)
            {
                NoDataCounter = 0;
                SpO2Label.Text = "BD " + (++BadDataCounter);
            }
            else
            {
                NoDataCounter = 0;
                BadDataCounter = 0;
                //SpO2Label.Text = "SpO2Key " + aRealPacket.SpO2Key + "% HR " + aRealPacket.HeartRate;
                SpO2Label.Text = aRealPacket.SpO2 + "% " + aRealPacket.HeartRate;
                if (aRealPacket.SpO2 > 90)
                    SpO2Label.ForeColor = Color.Black;
                else if (aRealPacket.SpO2 > 75)
                    SpO2Label.ForeColor = Color.Green;
                else
                    SpO2Label.ForeColor = Color.Red;
                if (DateTime.Now.Second != lastTick.Second ||//once per second
                    (Oximeter.Instance().IsReplay && (DateTime.Now.Millisecond % 100) != (lastTick.Second % 100))) //replay 1/10th
                {
                    lastTick = DateTime.Now;
                    aRealPacket = Oximeter.Instance().GetNextRealTimePacket();
                    if (aRealPacket != null)
                    {
                        lastSpO2 = aRealPacket.SpO2;
                        DrawOximeter(aRealPacket, true, true);
                        CalculateHTi(aRealPacket, false);
                    }
                }

            }

            //if (theOptions.PlaySounds && lastSpO2 < 81)
            //{
            //    int timeinterval = 4250 - ((81 - lastSpO2) * 250);
            //    timeinterval = timeinterval < 250 ? 250 : timeinterval;
            //    DateTime nextbeep = nextBeep.AddMilliseconds(timeinterval);
            //    if (DateTime.Now > nextbeep)
            //    {
            //        Sound.Stop();
            //        if (lastSpO2 > 75)
            //        {
            //            beepsp_a.Play();
            //        }
            //        else if (lastSpO2 > 70)
            //        {
            //            beepsp_b.Play();
            //        }
            //        else 
            //        {
            //            beepsp_c.Play();
            //        }
            //        nextBeep = DateTime.Now;
            //    }
            //}
            //Sound Temple12 = new Sound("Temple-12");
            //Sound a = new Sound("phasesr_75_6");
            //Sound b = new Sound("phasesr_50_3");
            //Sound c = new Sound("phasesr2");
            //Sound Temple = new Sound("Temple");//3600
            //Sound Beep = new Sound("Beep"); //400
            //Sound Beep2 = new Sound("Beep2"); //450
            //Sound Industrial = new Sound("Industrial Alarm"); //400
            //Sound RedAlert = new Sound("RedAlert"); //1000


            if (theOptions.PlaySounds)
            {
                int newrange = (lastSpO2 / 5);
                if (newrange > 90 / 5)
                    newrange = 90 / 5;
                if (DateTime.Now > nextBeep || lastrange != newrange)
                {
                    Sound.Stop();
                    int timeint = 1000;
                    if (lastSpO2 < 70)
                    {
                        RedAlert.Play();
                        timeint = 400;
                    }
                    else if (lastSpO2 < 75)
                    {
                        Industrial.Play();
                        timeint = 1200;
                    }
                    else if (lastSpO2 < 80)
                    {
                        timeint = 1500;
                        Beep3.Play();
                    }
                    else if (lastSpO2 < 85)
                    {
                        timeint = 2000;
                        Beep2.Play();
                    }
                    else if (lastSpO2 < 90)
                    {
                        timeint = 3000;
                        Beep1.Play();
                    }
                    else
                    {
                        Temple12.Play();
                        timeint = 4000;
                    }
                    nextBeep = DateTime.Now.AddMilliseconds(timeint);
                    lastrange = (lastSpO2 / 5);
                    if (lastrange > 90 / 5)
                        lastrange = 90 / 5;
                }
            }

            List<RealTimePacket> outstanding = Oximeter.Instance().GetOutstandingRealTimePackets();
            if (theOptions.ShowPulse)
            {
                ShowPulse(outstanding);
            }
        }

        private DateTime nextPlotTime = DateTime.MinValue;
        private RealTimePacket lastPlottedPacket = null;
        private Queue<RealTimePacket> uiPlotQueue = new Queue<RealTimePacket>();

        private void ShowPulse(List<RealTimePacket> outstanding)
        {
            if (PulsePlotter == null)
            {
                PulsePlotter = new Plotter(Color.Red, MAX_PULSE_POINTS, pulsePlotSurface2D);
            }

            foreach (RealTimePacket p in outstanding)
            {
                uiPlotQueue.Enqueue(p);
            }

            // Cap the queue if the device sends faster than 60Hz to maintain real-time
            while (uiPlotQueue.Count > 15)
            {
                lastPlottedPacket = uiPlotQueue.Dequeue();
            }

            // Protect against timer pauses
            if (nextPlotTime == DateTime.MinValue || (DateTime.Now - nextPlotTime).TotalMilliseconds > 200)
            {
                nextPlotTime = DateTime.Now;
            }

            bool addedAny = false;
            while (DateTime.Now >= nextPlotTime)
            {
                nextPlotTime = nextPlotTime.AddMilliseconds(16.666);
                
                if (uiPlotQueue.Count > 0)
                {
                    lastPlottedPacket = uiPlotQueue.Dequeue();
                }

                if (lastPlottedPacket != null)
                {
                    int pulseraw = lastPlottedPacket.Pulse1;
                    if (pulseraw < MinPulseSeen) MinPulseSeen = pulseraw;
                    int pulseoffset = pulseraw - MinPulseSeen;
                    if (pulseoffset > MaxPulseSeen)
                    {
                        MaxPulseSeen = pulseoffset;
                        PulseScale = MaxPulseSeen > 0 ? 100.0 / MaxPulseSeen : 1.0;
                    }
                    int pulsescaled = (int)(PulseScale * pulseoffset);
                    if (pulsescaled <= 100 && pulsescaled >= 0)
                    {
                        AddPoint(pulsescaled, PulsePlotter);
                        addedAny = true;
                    }
                }
            }

            if (addedAny)
            {
                PulsePlotter.aPlotSurface2D.Refresh();
            }
        }

        private void CalculateHTi(RealTimePacket aRealPacket, bool overrideTimer)
        {
            if (mainTimer.Enabled || overrideTimer)
            {
                if (aRealPacket.SpO2 != 0)
                {
                    currentO2 = aRealPacket.SpO2;
                    if (aRealPacket.SpO2 > 90)
                        timeAbove90++;
                    else
                        timeAbove90 = 0;
                }
                if (aRealPacket.SpO2 < 90 && aRealPacket.SpO2 != 0)
                {
                    int SpO2 = aRealPacket.SpO2 > 75 ? aRealPacket.SpO2 : 75;
                    HTindex1 += (90 - SpO2);
                    HTindex2 += (90 - aRealPacket.SpO2);
                }
            }
            HTi = (HTindex1 / 60.0);
            HTi2 = (HTindex2 / 60.0);
            HypoxicTrainingIndex.Text = "HTi " + String.Format("{0:0,0.00}", HTi) + " %/min [" + String.Format("{0:0,0.00}", HTi2) + "]";
        }
        double HTi = 0;
        double HTi2 = 0;
        int MaxPulseSeen = int.MinValue;
        int MinPulseSeen = int.MaxValue;
        double PulseScale = 1.0;

        private void Debug_Click(object sender, EventArgs e)
        {
            Packet.Debug();
        }
        private const int MAX_O2_POINTS = 60 * 5;
        private const int MAX_PULSE_POINTS = 60*10; //60 per second
        class O2Plot
        {
            public Plotter spO2Plotter;
            public Plotter spO2HighRangePlotter;
            public Plotter spO2MidRangePlotter;
            public Plotter spO2LowRangePlotter;
            public Plotter hrPlotter;
            public FilledRegion highFilledRegion = null;
            public FilledRegion midFilledRegion = null;
            public FilledRegion lowFilledRegion = null;
            public NPlot.Windows.PlotSurface2D aPlotSurface2D;
            public List<VerticalLine> VerticalLines = new List<VerticalLine>();
            public O2Plot(NPlot.Windows.PlotSurface2D aPlotSurface2D, int MaxX)
            {
                this.aPlotSurface2D = aPlotSurface2D;
                spO2Plotter = new Plotter(Color.Red, MaxX, aPlotSurface2D);
                spO2HighRangePlotter = new Plotter(Color.Black, MaxX, aPlotSurface2D);
                spO2MidRangePlotter = new Plotter(Color.Black, MaxX, aPlotSurface2D);
                spO2LowRangePlotter = new Plotter(Color.Black, MaxX, aPlotSurface2D);
                hrPlotter = new Plotter(Color.Blue, MaxX, aPlotSurface2D);
            }
        }
        class Plotter
        {
            public ArrayList YAxis = new ArrayList();
            public LinePlot aLinePlot = null;
            public Color aColor;
            public NPlot.Windows.PlotSurface2D aPlotSurface2D;
            public Plotter(Color c, int x, NPlot.Windows.PlotSurface2D aPlotSurface2D) 
            { 
                aColor = c; 
                MaxX = x;
                this.aPlotSurface2D = aPlotSurface2D;
            }
            public int MaxX;
        };
        O2Plot currentO2Plot = null;
        O2Plot sessionO2Plot = null;
        private void DrawOximeter(RealTimePacket aRealPacket, bool includeCurrent, bool refresh)
        {
            int totalTimeInSec = (((theOptions.HypoxicTimeMin + theOptions.RecoveryTimeMin) * (theOptions.Sessions)) + theOptions.FirstTimeExtraMin) * 60;
            if (sessionO2Plot != null && sessionO2Plot.spO2Plotter.MaxX != totalTimeInSec)
            {
            }

            if (currentO2Plot == null)
                currentO2Plot = new O2Plot(OxPlotSurface2D, MAX_O2_POINTS);
            if (sessionO2Plot == null)
                sessionO2Plot = new O2Plot(OverviewPlotSurface2D1, totalTimeInSec);

            if (includeCurrent)
                DrawOximeter(aRealPacket, currentO2Plot, refresh);
            DrawOximeter(aRealPacket, sessionO2Plot, refresh);
        }

        private void AddVerticalLine(bool hypoxic)
        {
            AddVerticalLine(hypoxic, sessionO2Plot);
            AddVerticalLine(hypoxic, currentO2Plot);
        }

        private void AddVerticalLine(bool hypoxic, O2Plot aO2Plot)
        {
            if (aO2Plot != null && aO2Plot.spO2Plotter.YAxis.Count > 0)
            {
                VerticalLine aVerticalLine = new VerticalLine(aO2Plot.spO2Plotter.YAxis.Count - 1, (hypoxic ? Color.Red : Color.Green));
                aVerticalLine.Pen.Width = 2f;
                aO2Plot.aPlotSurface2D.Add(aVerticalLine);
                aO2Plot.VerticalLines.Add(aVerticalLine);
            }
        }

        private void DrawOximeter(RealTimePacket aRealPacket, O2Plot aO2Plot, bool refresh)
        {
            if (aO2Plot.spO2Plotter.YAxis.Count == aO2Plot.spO2Plotter.MaxX)
            {
                //scroll any vertical lines
                foreach (VerticalLine vl in aO2Plot.VerticalLines)
                {
                    vl.AbscissaValue--;
                }
            }
            int highRangeSpO2 = aRealPacket.SpO2 > 90 ? 90 : aRealPacket.SpO2;
            int midRangeSpO2 = aRealPacket.SpO2 > 90 ? aRealPacket.SpO2 : 90;
            int lowRangeSpO2 = aRealPacket.SpO2 > 75 ? aRealPacket.SpO2 : 75;
            ///
            if (theOptions.HighlightLowValues && aRealPacket.SpO2 < 75)
            {
                midRangeSpO2 = 90;
                lowRangeSpO2 = 90;
            }
            ///
            AddPoint(highRangeSpO2, aO2Plot.spO2HighRangePlotter);
            AddPoint(midRangeSpO2, aO2Plot.spO2MidRangePlotter);
            AddPoint(lowRangeSpO2, aO2Plot.spO2LowRangePlotter);
            AddPoint(aRealPacket.SpO2, aO2Plot.spO2Plotter);
            if (aO2Plot.highFilledRegion == null)
            {
                //aO2Plot.midFilledRegion = new FilledRegion(aO2Plot.spO2Plotter.aLinePlot, aO2Plot.spO2MidRangePlotter.aLinePlot);
                aO2Plot.highFilledRegion = new FilledRegion(aO2Plot.spO2HighRangePlotter.aLinePlot, aO2Plot.spO2MidRangePlotter.aLinePlot);
                aO2Plot.highFilledRegion.Brush = Brushes.LightGreen;
                aO2Plot.aPlotSurface2D.Add(aO2Plot.highFilledRegion);
            }
            if (aO2Plot.midFilledRegion == null)
            {
                //aO2Plot.midFilledRegion = new FilledRegion(aO2Plot.spO2Plotter.aLinePlot, aO2Plot.spO2MidRangePlotter.aLinePlot);
                aO2Plot.midFilledRegion = new FilledRegion(aO2Plot.spO2LowRangePlotter.aLinePlot, aO2Plot.spO2MidRangePlotter.aLinePlot);
                aO2Plot.midFilledRegion.Brush = Brushes.LightBlue;
                aO2Plot.aPlotSurface2D.Add(aO2Plot.midFilledRegion);
            }
            if (aO2Plot.lowFilledRegion == null)
            {
                aO2Plot.lowFilledRegion = new FilledRegion(aO2Plot.spO2Plotter.aLinePlot, aO2Plot.spO2LowRangePlotter.aLinePlot);
                aO2Plot.lowFilledRegion.Brush = Brushes.MistyRose;
                aO2Plot.aPlotSurface2D.Add(aO2Plot.lowFilledRegion);
            }
            AddPoint(aRealPacket.HeartRate, aO2Plot.hrPlotter);
            //seems to loose the X axis points
            //aO2Plot.aPlotSurface2D.Remove(aO2Plot.spO2Plotter.aLinePlot, true);
            //aO2Plot.aPlotSurface2D.Add(aO2Plot.spO2Plotter.aLinePlot);


            SetupHistorgramCounters();
            if (mainTimer.Enabled)
            {
                Plot.AddHistogramPoint(aRealPacket, lowCounts, medCounts, highCounts);
            }
            if (highDistributionHistogramPlot == null)
            {
                Plot.CreateHistorgramPlots(ref lowDistributionHistogramPlot, ref medDistributionHistogramPlot, ref highDistributionHistogramPlot, lowCounts, medCounts, highCounts);
            }
            if (goalDistributionHistogramPlot == null)
            {
                //!goal - lazy creation
                goalCounts = new ArrayList();
                goalCounts.Add(0);
                Plot.CreateGoalHistorgramPlot(ref goalDistributionHistogramPlot, goalCounts);
            }
            //!goal - update
            goalCounts[0] = HTi;
            goalDistributionHistogramPlot.Color = HTi > theOptions.HTiGoal ? Color.Green : Color.Blue;
            goalDistributionHistogramPlot.RectangleBrush = HTi > theOptions.HTiGoal ? RectangleBrushes.HorizontalCenterFade.FaintGreenFade : RectangleBrushes.HorizontalCenterFade.FaintBlueFade;

            goalPlotSurface2D1.Remove(goalDistributionHistogramPlot, true);
            goalPlotSurface2D1.Add(goalDistributionHistogramPlot);
            SetGoalAxis();

            if (refresh)
            {
                RefreshCharts(aO2Plot);
            }
        }

        private void RefreshCharts(O2Plot aO2Plot)
        {
            aO2Plot.aPlotSurface2D.Refresh();
            goalPlotSurface2D1.Refresh();
            distributionPlotSurface2D1.Remove(medDistributionHistogramPlot, true);
            distributionPlotSurface2D1.Remove(highDistributionHistogramPlot, true);
            distributionPlotSurface2D1.Remove(lowDistributionHistogramPlot, true);
            distributionPlotSurface2D1.Add(highDistributionHistogramPlot);
            distributionPlotSurface2D1.Add(medDistributionHistogramPlot);
            distributionPlotSurface2D1.Add(lowDistributionHistogramPlot);
            distributionPlotSurface2D1.Refresh();
        }

        private void SetGoalAxis()
        {
            HorizontalLine aHorizontalLine = new HorizontalLine(theOptions.HTiGoal, Color.Red);
            aHorizontalLine.Pen.Width = 2f;
            goalPlotSurface2D1.Add(aHorizontalLine);

            goalPlotSurface2D1.YAxis1.WorldMax = HTi > theOptions.HTiGoal ? HTi * 1.2 : theOptions.HTiGoal * 1.2;
            goalPlotSurface2D1.YAxis1.WorldMin = 0;
            //goalPlotSurface2D1.XAxis1.Hidden = true;
            goalPlotSurface2D1.XAxis1.HideTickText = true;
        }
        HistogramPlot lowDistributionHistogramPlot = null;
        HistogramPlot medDistributionHistogramPlot = null;
        HistogramPlot highDistributionHistogramPlot = null;
        HistogramPlot goalDistributionHistogramPlot = null;

        ArrayList lowCounts = null;
        ArrayList medCounts = null;
        ArrayList highCounts = null;
        ArrayList goalCounts = null;
        private void SetupHistorgramCounters()
        {
            Plot.InitializeHistorgramYAxis(ref lowCounts, Plot.LOW_MIN, Plot.LOW_MAX);
            Plot.InitializeHistorgramYAxis(ref medCounts, Plot.MED_MIN, Plot.MED_MAX);
            Plot.InitializeHistorgramYAxis(ref highCounts, Plot.HIGH_MIN, Plot.HIGH_MAX);
        }
        private void AddPoint(int value, Plotter aPlotter)
        {
            if (aPlotter.YAxis.Count < aPlotter.MaxX)
            {
                aPlotter.YAxis.Add(value);
            }
            else
            {
                for (int i = 0; i < aPlotter.MaxX - 1; i++)
                {
                    aPlotter.YAxis[i] = aPlotter.YAxis[i + 1];
                }
                aPlotter.YAxis[aPlotter.MaxX - 1] = value;
            }
            if (aPlotter.aLinePlot == null)
            {
                aPlotter.aLinePlot = new LinePlot();
                aPlotter.aLinePlot.OrdinateData = aPlotter.YAxis;
                aPlotter.aLinePlot.Pen = new Pen(aPlotter.aColor, 2.0f);
                aPlotter.aPlotSurface2D.Add(aPlotter.aLinePlot);
            }
            //aO2Plot.aPlotSurface2D.Remove(aPlotter.aLinePlot, false);
            //aO2Plot.aPlotSurface2D.Add(aPlotter.aLinePlot);
        }

        private void StartReplay_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry, you changed the type of the resources/packet_1.xml to not be copied - it was a meg!");
            //Oximeter.Instance().LoadData("Packets_1.xml");
            //oximeterTimer.Start();
        }

        Options theOptions;
        private void OptionsButton_Click(object sender, EventArgs e)
        {
            theOptions.ShowDialog();
            Logger.DebugLevel = theOptions.DebugLevel;
        }

        private void ReplayFileButtong_Click(object sender, EventArgs e)
        {
            replayOpenFileDialog.InitialDirectory = theOptions.SaveTo;
            replayOpenFileDialog.Filter = "Packet Files|Packet*.xml";
            if (replayOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream replayFile = replayOpenFileDialog.OpenFile();
                Oximeter.Instance().LoadData(replayFile);
                Start_Click(sender, e); //do all the work
                oximeterTimer.Start();
            }
        }

        Diary aDiary = null;
        private void ShowDiary()
        {
            if(aDiary == null)
                aDiary = Diary.GetDiary(theOptions.SaveTo);
            DiaryForm aDiaryForm = new DiaryForm(aDiary, theOptions.SaveTo, theOptions.BackupTo);
            aDiaryForm.Show();
            aDiary.Debug();
        }

        private void DiaryButton_Click(object sender, EventArgs e)
        {
            ShowDiary();
        }

        bool abort = false;
        private void AbortButton_Click(object sender, EventArgs e)
        {
            abort = true;
        }

        //private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //{
        //    lastSpO2 = (int)numericUpDown1.Value;
        //}

        private void SetPosition()
        {
            Screen[] screens = Screen.AllScreens;
            Screen target = screens[0];
            this.StartPosition = FormStartPosition.Manual;
            this.Left = target.Bounds.X + (target.Bounds.Width - this.Width) / 2;
            this.Top = target.Bounds.Y + (target.Bounds.Height - this.Height) / 2;
            ScreenSaverTimeout = ScreenSaver.GetScreenSaverTimeout();
            ScreenSaverActive = ScreenSaver.GetScreenSaverActive();
            ScreenSaver.SetScreenSaverActive(0);
            //beepsp_a.Stream = this.GetType().Assembly.GetManifestResourceStream("HypoxicTimer.Resources.phasesr_75_6.wav");
            //beepsp_b.Stream = this.GetType().Assembly.GetManifestResourceStream("HypoxicTimer.Resources.phasesr_50_3.wav");
            //beepsp_c.Stream = this.GetType().Assembly.GetManifestResourceStream("HypoxicTimer.Resources.phasesr2.wav");

        }

        Masimo CurrentMasimo = null;
        private void ReadButton_Click(object sender, EventArgs e)
        {
            if (readCsvFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Masimo aMasimo = Masimo.ReadFile(readCsvFileDialog.FileName);
                foreach (RealTimePacket aRealPacket in aMasimo.Packets)
                {
                    DrawOximeter(aRealPacket, false, false);
                    CalculateHTi(aRealPacket, true);
                    Plot.AddHistogramPoint(aRealPacket, lowCounts, medCounts, highCounts);
                }
                RefreshCharts(sessionO2Plot);
                CurrentMasimo = aMasimo;
            }
        }

        private void AddToDiaryButton_Click(object sender, EventArgs e)
        {
            if (aDiary == null)
                aDiary = Diary.GetDiary(theOptions.SaveTo);
            if(CurrentMasimo != null)
                aDiary.AddMasimo(CurrentMasimo);
            aDiary.SaveXML(theOptions.SaveTo, theOptions.BackupTo);
        }
    }
}
