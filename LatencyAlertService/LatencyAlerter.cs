using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LatencyAlertService
{
    public partial class LatencyAlerter : ServiceBase
    {
        public LatencyAlerter()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
            PingTest initialPing = new PingTest();
            initialPing.PingCheck();
            timer1.Interval = Properties.Settings.Default.pingTimer;
            timer1.Start();
        }

        protected override void OnStop()
        {
            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PingTest ping = new PingTest();
            ping.PingCheck();
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
           
        }
    }
}
