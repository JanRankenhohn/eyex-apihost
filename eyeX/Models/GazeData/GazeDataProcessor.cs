using eyeX.Properties;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using static eyeX.Models.Globals.Constants;
using System.Threading.Tasks;
using eyeX.Models.Globals;
using Tobii.Interaction;

namespace eyeX.Models.GazeData
{
    public static class GazeDataProcessor
    {
        private static GazeData GazeData;
        private static FixationData FixationData;

        /// <summary>
        /// Takes gaze data from the different APis in a GazeData Object and processes it further
        /// </summary>
        /// <param name="gazeData"></param>
        public static void ProcessGazeData(GazeData gazeData)
        {
            GazeData = gazeData;
            SendGazeData();
        }

        /// <summary>
        /// Takes either Gaze Data that is processed further to compute fixations or exisiting Fixation Data
        /// </summary>
        /// <param name="gazeData"></param>
        /// <param name="fixationData"></param>
        public static void ProcessFixationData(FixationData fixationData = null, GazeData gazeData = null)
        {
            if(gazeData != null)
            {
                ComputeFixationData(Settings.Default.FixationAlgorithm, gazeData);
            }
            if(fixationData != null)
            {
                FixationData = fixationData;
            }
            SendFixationData();
        }
        
        /// <summary>
        /// Validates and sends the gaze data to the subscribers
        /// </summary>
        public static void SendGazeData()
        {
            string jsonString = "";
            try
            {
                // Valid Gaze data - serialize to json string for transmission
                jsonString = JsonSerializer.Serialize(GazeData);
            }
            catch(Exception ex) {
                // Validation failure, send unvalid GazeDataObject to Clients
                jsonString = JsonSerializer.Serialize(new GazeData { 
                    Type = "gazepoint",
                    X_Left = 0,
                    Y_Left = 0,
                    GazePointValidity_Left = false,
                    GazePointValidity_Right = false,
                    PupilValidity_Left = false,
                    PupilValidity_Right = false,
                    Timestamp = GazeData.Timestamp
                });
            }

            foreach (var client in Globals.Globals.Clients)
            {
                // for performance, a new thread for each client
                Task.Run(() => sendJsonData(client.GazeDataSocket, jsonString));
            }   
        }

        /// <summary>
        /// Sends the fixation data to the subscribers
        /// </summary>
        private static void SendFixationData()
        {
            string jsonString = "";

            try
            {
                jsonString = JsonSerializer.Serialize(new FixationData
                {
                    Type = "fixation",
                    X = FixationData.X,
                    Y = FixationData.Y,
                    EventType = FixationData.EventType,
                    Timestamp = FixationData.Timestamp
                });

                foreach (var client in Globals.Globals.Clients)
                {
                    // for performance, a new thread for each client
                    Task.Run(() => sendJsonData(client.FixationDataSocket, jsonString));
                }
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Sends json Object to a Client Socket
        /// </summary>
        /// <param name="client"></param>
        private static void sendJsonData(Socket socket, string json)
        {
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(json);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(json);
            socket.Send(toSendBytes);
        }

        /// <summary>
        /// ´Computes Fixations from the gaze data
        /// </summary>
        /// <param name="algorithm"></param>
        private static void ComputeFixationData(string algorithm, GazeData gazeData)
        {
            switch (algorithm)
            {
                case "default":
                    break;
            }
        }
    }
}