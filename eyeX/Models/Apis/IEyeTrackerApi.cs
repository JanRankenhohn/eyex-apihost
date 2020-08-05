using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eyeX.Models.Apis
{
    public interface IEyeTrackerApi
    {
        /// <summary>
        /// Establish a connection to the EyeTracker API
        /// </summary>
        /// <returns></returns>
        Task<ApiResponseData> ConnectAsync();

        /// <summary>
        /// Subscribe to GazeData that is broadcasted by the API
        /// </summary>
        /// <returns></returns>
        void SubscribeToGazeDataAsync();

        /// <summary>
        /// Subscribe to FixationData that is broadcasted by the API
        /// </summary>
        void SubscribeToFixationDataAsync();
    }
}