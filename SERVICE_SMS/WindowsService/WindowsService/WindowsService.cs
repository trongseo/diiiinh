using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using SMSapplication;
using System.IO.Ports;
using System.Timers;
namespace WindowsService
{
    class WindowsService : ServiceBase
    {
        private System.ComponentModel.IContainer components;
        Timer timer1a = new System.Timers.Timer();
        /// <summary>
        /// Public Constructor for WindowsService.
        /// - Put all of your Initialization code here.
        /// </summary>
        public WindowsService()
        {
            this.ServiceName = "My Windows Service";
            this.EventLog.Source = "My Windows Service";
            this.EventLog.Log = "Application";
            
            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;

            if (!EventLog.SourceExists("My Windows Service"))
                EventLog.CreateEventSource("My Windows Service", "Application");
        }
       
        /// <summary>
        /// The Main Thread: This is where your Service is Run.
        /// </summary>
        static void Main()
        {
            ServiceBase.Run(new WindowsService());
        }

        /// <summary>
        /// Dispose of objects that need it here.
        /// </summary>
        /// <param name="disposing">Whether or not disposing is going on.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        /// <summary>
        /// OnStart: Put startup code here
        ///  - Start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            timer1a.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
            timer1a.Interval = 5000;
            timer1a.Enabled = true;
            timer1a.Start();

            string[] ports = SerialPort.GetPortNames();
            string portsui = "";
            // Add all port names to the combo box:
            foreach (string portw in ports)
            {

                portsui = portw;
            }

            port = new SerialPort();
            objclsSMS = new clsSMS();
            this.port = objclsSMS.OpenPort(portsui, 9600, 8, 3000, 3000);
           

        }
        private void timer1_Elapsed(object sender, EventArgs e)
        {
            string strCommand = "AT+CMGL=\"ALL\"";
            
            string smsStr = "";
           

                objShortMessageCollection = objclsSMS.ReadSMS(this.port, strCommand);
               
                for (int i = 0; i < objShortMessageCollection.Count; i++)
                {
                    ShortMessage smes = objShortMessageCollection[i];
                    //smsStr += "-" + smes.Sender + ":" + smes.Message;
                    string[] splitAr = smes.Message.Split("-".ToCharArray());
                    try
                    {
                        if (splitAr.Length == 3)
                        {
                            string connectionString = "Data Source=USER-PC;Initial Catalog=TRACKER;Integrated Security=True";
                            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                            using (System.Data.SqlClient.SqlCommand command = connection.CreateCommand())
                            {
                                string valueInsert = splitAr[0] + "," + splitAr[1] + ",'" + splitAr[2] + "','" + smes.Sender + "'";
                                command.CommandText = "INSERT INTO my_tracker (lat,long,post_date,id_info) VALUES (" + valueInsert + ")";
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }

                    }
                    catch(Exception exkd)
                    {
                        smsStr += exkd.ToString();
                    }



                }

                 strCommand = "AT+CMGD=1,3";
                if (objclsSMS.DeleteMsg(this.port, strCommand))
                {
                    //MessageBox.Show("Messages has deleted successfuly");
                    smsStr += "Messages has deleted successfuly";
                }
                else
                {
                    //MessageBox.Show("Failed to delete messages ");
                    smsStr += "Failed to delete messages";
                }

                if (smsStr != "")
                {
                    //Some awesome code!
                    string folderPath = @"D:\SMS_SERVICE";

                    if (!System.IO.Directory.Exists(folderPath))
                        System.IO.Directory.CreateDirectory(folderPath);

                    FileStream fs = new FileStream(folderPath + "\\SMS_LOG.txt",
                                        FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter m_streamWriter = new StreamWriter(fs);
                    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);

                    m_streamWriter.WriteLine(smsStr + "\n");
                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                }
           

        }

        /// <summary>
        /// OnStop: Put your stop code here
        /// - Stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            timer1a.Enabled = false;
        }

        /// <summary>
        /// OnPause: Put your pause code here
        /// - Pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// OnContinue: Put your continue code here
        /// - Un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// OnCustomCommand(): If you need to send a command to your
        ///   service without the need for Remoting or Sockets, use
        ///   this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary Integer between 128 & 256</param>
        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:
            //#  int command = 128; //Some Arbitrary number between 128 & 256
            //#  ServiceController sc = new ServiceController("NameOfService");
            //#  sc.ExecuteCommand(command);

            base.OnCustomCommand(command);
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcase Status (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// OnSessionChange(): To handle a change event from a Terminal Server session.
        ///   Useful if you need to determine when a user logs in remotely or logs off,
        ///   or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription"></param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            //this.timer1 = new System.Windows.Forms.Timer(this.components);
            //// 
            //// timer1
            //// 
            //this.timer1.Enabled = true;
            //this.timer1.Interval = 5000;
            //this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            
        }
    }
}
