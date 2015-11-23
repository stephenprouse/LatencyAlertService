using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace LatencyAlertService
{
    class PingTest
    {
        public void PingCheck()
        {
            Ping ping = new Ping();
            PingReply reply;
            string[] host = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "hosts.ini");
            long result;
            long[] p = new long[10];

            for (int h = 0; h < host.Length; h++)
            {
                for (int i = 0; i <= Properties.Settings.Default.pingCount; i++)
                {
                    try
                    {
                        reply = ping.Send(host[h], 3000);
                        if (reply.Status == IPStatus.Success)
                        {
                            result = reply.RoundtripTime;
                            p[i] = result;
                            Console.WriteLine(host[h] + " Ping #" + i + " time = " + p[i] + " ms");

                            #region evaluate and send email if latency is high
                            if (i == 9)
                            {
                                Console.WriteLine("Average ping time = " + p.Average() + "ms");
                                EventLog log = new EventLog();
                                log.Source = "Latency Alerter";
                                log.WriteEntry(host[h] + " Average ping time = " + p.Average() + "ms");

                                if (p.Average() >= Properties.Settings.Default.pingThreshold)
                                {
                                    Console.WriteLine(DateTime.Now.ToShortTimeString());
                                    Console.WriteLine("Latency too high sending alert now");
                                    SendEmail alertMail = new SendEmail();
                                    alertMail.SendEmailMsg(
                                        "uchexchangehmh",
                                        "ping@uchs.org",
                                        "sprouse@uchs.org",
                                        host[h] + " latency alert",
                                        "Average latency = " + result + " ms");
                                }
                            }
                            #endregion
                        }
                        else { Console.WriteLine(host[h] + " " + reply.Status); }
                    }

                    catch (PingException ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                        SendEmail alertMail = new SendEmail();
                        alertMail.SendEmailMsg(
                            "uchexchangehmh",
                            "ping@uchs.org",
                            "sprouse@uchs.org",
                            host[h] + " latency alert",
                            ex.InnerException.Message);
                        EventLog log = new EventLog();
                        log.Source = "Latency Alerter";
                        log.WriteEntry(host[h] + " latency alert" + ex.InnerException.Message);
                        break;
                    }
                }
            }
        }
    }
}
