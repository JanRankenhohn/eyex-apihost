using eyeX.Models.GazeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyeX.Models.Algorithms
{
    static class WeightedMean
    {
        private static List<GazeData.GazeData> GazeDataList = new List<GazeData.GazeData>();

        public static GazeData.GazeData Compute(GazeData.GazeData gazeData)
        {
            try
            {
                if (GazeDataList.Count < 2)
                {
                    GazeDataList.Add(gazeData);
                    return gazeData;
                }
                else
                {
                    GazeDataList.Add(gazeData);
                    double x_sum = 0.0;
                    double y_sum = 0.0;
                    foreach (var fd in GazeDataList)
                    {
                        x_sum += fd.X_Median;
                        y_sum += fd.Y_Median;
                    }
                    gazeData.X_Median = x_sum / 3.0;
                    gazeData.Y_Median = y_sum / 3.0;
                    GazeDataList.Remove(GazeDataList.First());
                    return gazeData;
                }
            }
            catch(Exception ex)
            {
                GazeDataList = new List<GazeData.GazeData>();
                return gazeData;
                // pass Enumeration error
            }
        }
    }
}
