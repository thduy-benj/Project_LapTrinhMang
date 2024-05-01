using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    internal static class Program
    {
        public static Random Random = new Random();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ServerChatForm());
        }
    }



}