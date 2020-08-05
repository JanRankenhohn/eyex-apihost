using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tobii.Interaction.Framework;

namespace eyeX.Models.GazeData
{
    public class FixationData
    {
        public string Type { get; set; }
        public FixationDataEventType EventType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Timestamp { get; set; }
    }
}