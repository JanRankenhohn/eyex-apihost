using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using eyeX.Models.Apis;
using eyeX.Models.Globals;
using Microsoft.AspNetCore.Mvc;
using static eyeX.Models.Globals.Constants;

namespace eyeX.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// Load the EyeTrackerApi by Client Call
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> LoadApi(string apiName)
        {
            var result = await ApiManager.InitializeApiAsync(apiName);
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        /// <summary>
        /// Unload the EyeTrackerApi by Client Call
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> UnloadApi()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes eye tracker calibration process
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Calibrate()
        {
            var response = await ApiManager.CalibrateEyeTrackerAsync();

            if (response.Success)
            {
                return Ok();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Subscribes the client for an Api Socket Connection to recieve the Gaze Data
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public ActionResult Subscribe(string ip, int port, string name, string dataType)
        {
            // Create Client if it doesn't exist
            Client client = Globals.Clients.Where(c => c.Name == name).FirstOrDefault();
            if (client == null)
            {
                client = new Client
                {
                    Name = name,
                    IPAddress = new IPEndPoint(IPAddress.Parse(ip), port),
                    GazeDataSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                    FixationDataSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                };
                Globals.Clients.Add(client);
            }

            // Subscribe Client to data
            try
            {
                switch (dataType)
                {
                    case nameof(GazeDataTypes.GAZEPOINTS):
                        client.GazeDataSocket.Connect(client.IPAddress);
                        break;
                    case nameof(GazeDataTypes.FIXATIONS):
                        client.FixationDataSocket.Connect(client.IPAddress);
                        break;
                }
            }
            catch(Exception e)
            {
                // already subscribed
            }

            return Ok();
        }

        /// <summary>
        /// Unsubscribes the client
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult Unsubscribe(string client)
        {
            if (Globals.Clients.Any(c => c.Name == client) && client != null)
            {
                Globals.Clients.Remove(Globals.Clients.Where(c => c.Name == client).First());
                return Ok();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}