using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eyeX.Models.Apis
{
    public abstract class EyeTrackerApi : IEyeTrackerApi
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public virtual Task<ApiResponseData> ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<ApiResponseData> DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public virtual void SubscribeToGazeDataAsync()
        {
            throw new NotImplementedException();
        }

        public virtual void SubscribeToFixationDataAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<ApiResponseData> CalibrateEyeTrackerAsync()
        {
            throw new NotImplementedException();
        }

    }
}