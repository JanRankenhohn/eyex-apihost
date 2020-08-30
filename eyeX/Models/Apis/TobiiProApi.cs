using eyeX.Models.GazeData;
using eyeX.Models.GUI;
using eyeX.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Tobii.Research;
using static eyeX.Models.Globals.Constants;

namespace eyeX.Models.Apis
{
    public class TobiiProApi : EyeTrackerApi
    {
        private string IPAdress;
        private IEyeTracker EyeTracker;

        /// <summary>
        /// Connects to Tobii Pro Eye-Tracker
        /// </summary>
        /// <returns>Connection Success Message</returns>
        public override async Task<ApiResponseData> ConnectAsync()
        {
            IPAdress = "tet-tcp://" + Settings.Default.EyeTrackerIP + "/";

            try
            {
                EyeTracker = EyeTrackingOperations.GetEyeTracker(IPAdress);
            }
            catch(Exception ex)
            {
                // Likely wrong URI Format
                return new ApiResponseData { Message = ex.Message };
            }

            if (EyeTracker != null)
            {
                // Connection established
                Logger.Debug("Connected to " + EyeTracker.DeviceName + " width IP " + IPAdress);
                SubscribeToGazeDataAsync();
                IsConnected = true;
                return new ApiResponseData { Success = true };
            }
            else
            {
                return new ApiResponseData { Message = "Unable to connect to EyeTracker with IP " + IPAdress };
            }
        }

        /// <summary>
        /// Calibrates the Screen Based Tobii Eye Tracker using a Windows Form
        /// Based on http://devtobiipro.azurewebsites.net/tobii.research/dotnet/reference/1.7.2.7-alpha-g369155c3/class_tobii_1_1_research_1_1_screen_based_calibration.html
        /// </summary>
        /// <returns></returns>
        public override async Task<ApiResponseData> CalibrateEyeTrackerAsync()
        {
            //EyeTracker == null ? new ApiResponseData { Message = "No connection to an eye tracker has been established. Connect to an eye tracker first." } : null;

            throw new NotImplementedException();
            var calibration = new ScreenBasedCalibration(EyeTracker);
            await calibration.EnterCalibrationModeAsync();

            // Define the points on screen we should calibrate at.
            // The coordinates are normalized, i.e. (0.0f, 0.0f) is the upper left corner and (1.0f, 1.0f) is the lower right corner.
            var pointsToCalibrate = new NormalizedPoint2D[] {
                new NormalizedPoint2D(0.5f, 0.5f),
                new NormalizedPoint2D(0.1f, 0.1f),
                new NormalizedPoint2D(0.1f, 0.9f),
                new NormalizedPoint2D(0.9f, 0.1f),
                new NormalizedPoint2D(0.9f, 0.9f),
            };

            // Display Windows Form for Calibration Drawing
            CalibrationForm calibrationForm = new CalibrationForm();
            calibrationForm.Show();

            System.Threading.Thread.Sleep(3000);

            // Get screen resolution
            Screen myScreen = Screen.FromControl(calibrationForm);
            float screenResX = myScreen.WorkingArea.Width;
            float screenResY = myScreen.WorkingArea.Height;

            // Collect data.
            foreach (var point in pointsToCalibrate)
            {
                // Get screen coodrinates from normalized coordinates
                float x = point.X * screenResX;
                float y = point.Y * screenResY;

                // Show an image on screen where you want to calibrate.
                Console.WriteLine("Show point on screen from UI thread at ({0}, {1})", x, y);

                calibrationForm.Paint += (sender, e) =>
                {
                    e.Graphics.DrawEllipse(Pens.Red, x, y, 100, 100);
                };

                // Wait a little for user to focus.
                System.Threading.Thread.Sleep(700);
                // Collect data.
                CalibrationStatus status = await calibration.CollectDataAsync(point);
                if (status != CalibrationStatus.Success)
                {
                    // Try again if it didn't go well the first time.
                    // Not all eye tracker models will fail at this point, but instead fail on ComputeAndApplyAsync.
                    await calibration.CollectDataAsync(point);
                }
            }
            // Compute and apply the calibration.
            CalibrationResult calibrationResult = await calibration.ComputeAndApplyAsync();
            Console.WriteLine("Compute and apply returned {0} and collected at {1} points.",
                calibrationResult.Status, calibrationResult.CalibrationPoints.Count);
            // Analyze the data and maybe remove points that weren't good.
            calibration.DiscardData(new NormalizedPoint2D(0.1f, 0.1f));
            // Redo collection at the discarded point.
            Console.WriteLine("Show point on screen from UI thread at ({0}, {1})", 0.1f, 0.1f);
            await calibration.CollectDataAsync(new NormalizedPoint2D(0.1f, 0.1f));
            // Compute and apply again.
            calibrationResult = await calibration.ComputeAndApplyAsync();
            Console.WriteLine("Second compute and apply returned {0} and collected at {1} points.",
                calibrationResult.Status, calibrationResult.CalibrationPoints.Count);
            // See that you're happy with the result.
            // The calibration is done. Leave calibration mode.
            await calibration.LeaveCalibrationModeAsync();
        }

        private static void CalibrationForm_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves Tobii Pro Api Gaze Data and sends it further to the Data Processor
        /// </summary>
        /// <returns></returns>
        public async override void SubscribeToGazeDataAsync()
        {
            EyeTracker.GazeDataReceived += (sender, gazeDataEventArgs) =>
            {
                // Converting Tobii Pro GazeData to EyeX GazeData Format
                GazeData.GazeData gazeData = new GazeData.GazeData
                {
                    Type = nameof(GazeDataTypes.GAZEPOINTS),
                    PupilDiameter_Left = gazeDataEventArgs.LeftEye.Pupil.PupilDiameter,
                    PupilDiameter_Right = gazeDataEventArgs.RightEye.Pupil.PupilDiameter,
                    Timestamp = gazeDataEventArgs.SystemTimeStamp,
                    Y_Left = gazeDataEventArgs.LeftEye.GazePoint.PositionOnDisplayArea.Y,
                    Y_Right = gazeDataEventArgs.RightEye.GazePoint.PositionOnDisplayArea.Y,
                    X_Left = gazeDataEventArgs.LeftEye.GazePoint.PositionOnDisplayArea.X,
                    X_Right = gazeDataEventArgs.RightEye.GazePoint.PositionOnDisplayArea.X,
                };

                // Map 3D Eye Coordinates
                var originR = gazeDataEventArgs.RightEye.GazeOrigin.PositionInTrackBoxCoordinates;
                var originL = gazeDataEventArgs.LeftEye.GazeOrigin.PositionInTrackBoxCoordinates;
                gazeData.GazeOrigin_X_Median = (originR.X + originL.X) / 2.0;
                gazeData.GazeOrigin_Y_Median = (originR.Y + originL.Y) / 2.0;
                gazeData.GazeOrigin_Y_Median = (originR.Z + originL.Z) / 2.0;
                gazeData.X_3D_Median = gazeDataEventArgs.RightEye.GazePoint.PositionInUserCoordinates.X;
                gazeData.Y_3D_Median = gazeDataEventArgs.RightEye.GazePoint.PositionInUserCoordinates.Y;
                gazeData.Z_3D_Median = gazeDataEventArgs.RightEye.GazePoint.PositionInUserCoordinates.Z;

                // Map Validity Values
                gazeData.GazePointValidity_Left = gazeDataEventArgs.LeftEye.GazePoint.Validity == Validity.Valid ? true : false;
                gazeData.GazePointValidity_Right = gazeDataEventArgs.RightEye.GazePoint.Validity == Validity.Valid ? true : false;
                gazeData.PupilValidity_Left = gazeDataEventArgs.LeftEye.Pupil.Validity == Validity.Valid ? true : false;
                gazeData.PupilValidity_Right = gazeDataEventArgs.RightEye.Pupil.Validity == Validity.Valid ? true : false;

                // Compute Median Gaze Points
                gazeData.Y_Median = (gazeData.Y_Left + gazeData.Y_Right) / 2;
                gazeData.X_Median = (gazeData.X_Left + gazeData.X_Right) / 2;

                if (!double.IsNaN(gazeData.X_Median))
                {
                    GazeDataProcessor.ProcessGazeData(gazeData);
                    GazeDataProcessor.ProcessFixationData(null, gazeData);
                }
            };
        }
    }
}