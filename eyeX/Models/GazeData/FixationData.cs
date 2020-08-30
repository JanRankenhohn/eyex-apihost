using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tobii.Interaction.Framework;

namespace eyeX.Models.GazeData
{
    public class FixationData : GazeData
    {
        public string EventType { get; set; }
    }
}