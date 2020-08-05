using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eyeX.Properties;
using System.Threading.Tasks;

namespace eyeX.Models.Apis
{
    public static class ApiManager
    {
        private static EyeTrackerApi Api { get; set; }

        public static async Task<ApiResponseData> InitializeApiAsync(string apiName)
        {
            // Add any new API / Eye-Tracker Model here as new switch-case
            switch (apiName)
            {
                case "TobiiCore":
                    Api = new TobiiCoreApi();
                    break;
                case "TobiiPro":
                    Api = new TobiiProApi();
                    break;
                case "EyeLink":
                    Api = new EyeLinkApi(); // not yet implemented
                    break;
                case "iViewXApi":
                    Api = new IViewXApi(); // not yet implemented
                    break;
                default:
                    throw new NotImplementedException("No implemented Connection for API " + Settings.Default.EyeTrackerApi);
            }

            return await Api.ConnectAsync();
        }

        public static async Task<ApiResponseData> CalibrateEyeTrackerAsync()
        {
            if (Api == null)
            {
                return new ApiResponseData { Message = "Calibration Error: No Api Connected" };
            }
            else
            {
                await Api.CalibrateEyeTrackerAsync();
                return new ApiResponseData { Success = true };
            }
        }

    }

}