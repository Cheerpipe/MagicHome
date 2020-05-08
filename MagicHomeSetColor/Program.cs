using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MagicHomeSetColor
{
    class Program
    {
        //private static RgbFusion _fusion;
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
                args = "192.168.1.30 222 0 0".Split(' ');
            bool error = false;
            MagicHome.MagicHome strips = new MagicHome.MagicHome();
            strips.TurnOnWhenConnected = false;
            if (!IPAddress.TryParse(args[0], out strips.Ip))
            {
                Console.WriteLine("Invalid IP Address");
                error = true;
            }
            int R = 0;
            int G = 0;
            int B = 0;


            if (!int.TryParse(args[1], out R))
            {
                Console.WriteLine("Invalid Red value. Put something between 0 and 255");
                error = true;
            }
            if (!int.TryParse(args[2], out G))
            {
                Console.WriteLine("Invalid Green value. Put something between 0 and 255");
                error = true;
            }
            if (!int.TryParse(args[3], out B))
            {
                Console.WriteLine("Invalid Blue value. Put something between 0 and 255");
                error = true;
            }
            if (error)
            {
                Console.WriteLine("There are some errors. Check what is wrong and try again.");
                return;
            }
            Color color = Color.FromArgb(R, G, B);
            strips.Connect();
            System.Timers.Timer timeout = new System.Timers.Timer(3000);
            timeout.Elapsed += Timeout_Elapsed;
            timeout.Start();
            while (!strips.IsReallyConnected && timeout.Enabled)
            {
                Thread.Sleep(100);
            }
            if (!strips.IsReallyConnected)
            {
                Console.WriteLine("Controller not found.");
                return;
            }

            strips.Disconnect();
        }

        private static void Timeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ((System.Timers.Timer)sender).Enabled = false;
        }
    }
}
