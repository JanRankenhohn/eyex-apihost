using eyeX.Models.GazeData;
using eyeX.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Tobii.EyeX.Client;
using Tobii.Interaction;
using Tobii.Interaction.Framework;
using static eyeX.Models.Globals.Constants;

namespace eyeX.Models.Apis
{
    public class TobiiCoreApi : EyeTrackerApi
    {
        public Host Host;

        public async override Task<ApiResponseData> ConnectAsync()
        {
            try
            {
                Host = new Host();
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                return new ApiResponseData { Message = ex.Message };
            }

            // Connection established
            Logger.Debug("Connected to " + await Host.States.GetEyeTrackingDeviceStatusAsync());

            SubscribeToGazeDataAsync();

            IsConnected = true;

            return new ApiResponseData { Success = true };
        }

        public async override void SubscribeToGazeDataAsync()
        {
            var gazePointDataStream = Host.Streams.CreateGazePointDataStream();

            gazePointDataStream.GazePoint((x, y, ts) => handleGazeData(x, y, ts));

            void handleGazeData(double x, double y, double ts)
            {
                // Converting Tobii Core GazeData to EyeX GazeData Format
                GazeData.GazeData gazeData = new GazeData.GazeData
                {
                    Type = nameof(GazeDataTypes.GAZEPOINTS),
                    Timestamp = (long)ts,
                    X_Median = x / 3840,
                    Y_Median = y / 2160
                };

                GazeDataProcessor.ProcessGazeData(gazeData);

                switch (Settings.Default.FixationAlgorithm)
                {
                    // Use implemented IDT Algorithm
                    case "IDT":
                        GazeDataProcessor.ProcessFixationData(null, gazeData);
                        break;
                                            // Use Tobii Core Default Fixation Alogrithm
                    default:
                        SubscribeToFixationDataAsync();
                        break;
                }
            }
        }

        public async override void SubscribeToFixationDataAsync()
        {
            // Initialize Fixation data stream.
            var fixationDataStream = Host.Streams.CreateFixationDataStream();

            fixationDataStream.Next += (o, fixation) =>
            {
                // Converting Tobii Core FixationData to EyeX GazeData Format
                System.Drawing.Rectangle resolution = Screen.PrimaryScreen.Bounds;
                GazeData.FixationData fixationData = new GazeData.FixationData
                {
                    Type = nameof(Globals.Constants.GazeDataTypes.FIXATIONS),
                    Timestamp = (long)fixation.Data.Timestamp,
                    X_Median = (fixation.Data.X / 3840),
                    Y_Median = (fixation.Data.Y / 2160),
                    EventType = Enum.GetName(typeof(FixationDataEventType), fixation.Data.EventType).ToUpper()
                };
                if (!double.IsNaN(fixation.Data.X))
                {
                    GazeDataProcessor.ProcessFixationData(fixationData);
                }
                //if (fixationData.EventType == FixationDataEventType.Begin)
                //{
                //    Globals.Globals.fixationCount += 1;
                //}
            };
        }

        public async override Task<ApiResponseData> DisconnectAsync()
        {
            Host.DisableConnection();
            return new ApiResponseData { Success = true };
        }
    }
}