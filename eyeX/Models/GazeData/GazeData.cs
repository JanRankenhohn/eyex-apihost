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
        public double GazeOrigin_X_Median { get; set; }
        public double GazeOrigin_Y_Median { get; set; }
        public double GazeOrigin_Z_Median { get; set; }
        public double X_3D_Median { get; set; }
        public double Y_3D_Median { get; set; }
        public double Z_3D_Median { get; set; }
    
        public FixationData ToFixationData()
        {
            return new FixationData
            {
                Type = this.Type,
                X_Left = this.X_Left,
                X_Right = this.X_Right,
                X_Median = this.X_Median,
                X_3D_Median = this.X_3D_Median,
                GazeOrigin_X_Median = this.GazeOrigin_X_Median,
                Y_Left = this.Y_Left,
                Y_Right = this.Y_Right,
                Y_Median = this.Y_Median,
                Y_3D_Median = this.Y_3D_Median,
                GazeOrigin_Y_Median = this.GazeOrigin_Y_Median,
                GazeOrigin_Z_Median = this.GazeOrigin_Z_Median,
                GazePointValidity_Left = this.GazePointValidity_Left,
                GazePointValidity_Right = this.GazePointValidity_Right,
                PupilDiameter_Left = this.PupilDiameter_Left,
                PupilDiameter_Right = this.PupilDiameter_Right,
                PupilValidity_Left = this.PupilValidity_Left,
                PupilValidity_Right = this.PupilValidity_Right,
                Timestamp = this.Timestamp,
                Z_3D_Median = this.Z_3D_Median
            };
        }
    }
}