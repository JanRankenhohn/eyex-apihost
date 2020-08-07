using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyeX.Models.Globals
{
    public static class Constants
    {
        public const string RightEye = "right";
        public const string LeftEye = "left";
        public enum Apis { TOBIICORE, TOBIIPRO, EYELINK, IVIEWX};
        public enum GazeDataTypes { GAZEPOINTS, FIXATIONS };
    }
}