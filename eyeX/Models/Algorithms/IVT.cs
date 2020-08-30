using eyeX.Models.GazeData;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Tobii.Interaction.Framework;

namespace eyeX.Models.Algorithms
{
    static class IVT
    {
        private static FixationData PreviousFixationData;
        public static FixationData Classify(GazeData.GazeData gazeData, double threshold)
        {
            FixationData fixationData = gazeData.ToFixationData();
            if (PreviousFixationData == null)
            {
                fixationData.Type = "SACCADES";
            }
            else
            {
                double distance = GetDistance(PreviousFixationData, gazeData);
                if(distance > 600)
                {
                    if(PreviousFixationData.Type == "FIXATIONS")
                    {
                        fixationData.EventType = "END";
                    }
                    fixationData.Type = "SACCADES";
                    //Console.WriteLine("-------------XXX--------------------");
                }
                else
                {
                    //Console.WriteLine("--------------------FIXATION--------------------");
                    fixationData.Type = "FIXATIONS";
                    if(PreviousFixationData.Type == "SACCADES")
                    {
                        fixationData.EventType = "BEGIN";
                    }
                    else
                    {
                        fixationData.EventType = "DATA";
                    }
                }
            }
            PreviousFixationData = fixationData;
            return fixationData;
        }

        private static double GetDistance(GazeData.GazeData previousGazeData, GazeData.GazeData gazeData)
        {
            Vector3D origin = new Vector3D(previousGazeData.GazeOrigin_X_Median, previousGazeData.GazeOrigin_Y_Median, previousGazeData.GazeOrigin_Z_Median);
            Vector3D p1 = new Vector3D(previousGazeData.X_3D_Median, previousGazeData.Y_3D_Median, previousGazeData.Z_3D_Median);
            Vector3D p2 = new Vector3D(gazeData.X_3D_Median, gazeData.Y_3D_Median, gazeData.Z_3D_Median);

            Vector3D originToP1 = Vector3D.Subtract(p1, origin);
            Vector3D originToP2 = Vector3D.Subtract(p2, origin);

            return Vector3D.AngleBetween(originToP1, originToP2) * 600;
        }
    }
}
