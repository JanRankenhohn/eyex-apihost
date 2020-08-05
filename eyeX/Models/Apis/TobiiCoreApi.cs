using eyeX.Models.GazeData;
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

            SubscribeToFixationDataAsync();

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
                    Timestamp = (long)ts,
                    X_Median = x,
                    Y_Median = y
                };

                GazeDataProcessor.ProcessGazeData(gazeData);
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
                    Timestamp = (long)fixation.Data.Timestamp,
                    X = (fixation.Data.X / 3840),
                    Y = (fixation.Data.Y / 2160),
                    EventType =fixation.Data.EventType
                };
                GazeDataProcessor.ProcessFixationData(fixationData);
            };
        }

        public async override Task<ApiResponseData> DisconnectAsync()
        {
            Host.DisableConnection();
            return new ApiResponseData { Success = true };
        }
    }
}