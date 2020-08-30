using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace eyeX.Models.Globals
{
    public static class Globals
    {
        public static int fixationCount = 0;
        public static List<Client> Clients = new List<Client>();
    }
    public class Client
    {
        public string Name { get; set; }
        public IPEndPoint IPAddress { get; set; }
        public Socket GazeDataSocket { get; set; }
        public Socket FixationDataSocket { get; set; }
    }

}