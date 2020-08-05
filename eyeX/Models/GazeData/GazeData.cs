using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eyeX.Models.GazeData
{
    public class GazeData
    {
        public string Type { get; set; }
        public double Y_Left { get; set; }
        public double X_Left { get; set; }
        public double Y_Right { get; set; }
        public double X_Right { get; set; }
        public double Y_Median { get; set; }
        public double X_Median { get; set; }
        public bool GazePointValidity_Left { get; set; }
        public bool PupilValidity_Left { get; set; }
        public float PupilDiameter_Left { get; set; }
        public bool GazePointValidity_Right { get; set; }
        public bool PupilValidity_Right { get; set; }
        public float PupilDiameter_Right { get; set; }
        public long Timestamp { get; set; }
    }
}