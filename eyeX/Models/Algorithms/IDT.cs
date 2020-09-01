using eyeX.Models.GazeData;
using eyeX.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.Interaction.Framework;

namespace eyeX.Models.Algorithms
{
    static class IDT
    {
        private static List<double> XValueList = new List<double>();
        private static List<double> YValueList = new List<double>();
        private static FixationData PreviousFixationData = null;

        static int counter = 0;

        public static FixationData Classify(GazeData.GazeData gazeData)
        {
            FixationData fixationData = gazeData.ToFixationData();
            if (XValueList.Count < Settings.Default.IDT_Window)
            {
                fixationData.Type = "SACCADES";
                XValueList.Add(fixationData.X_Median);
                YValueList.Add(fixationData.Y_Median);
            }
            else
            {
                XValueList.RemoveAt(0);
                YValueList.RemoveAt(0);
                XValueList.Add(fixationData.X_Median);
                YValueList.Add(fixationData.Y_Median);
                double dispersion = GetDispersion(fixationData);
                if (dispersion > Settings.Default.IDT_Threshold)
                {
                    if (PreviousFixationData.Type == "FIXATIONS")
                    {
                        fixationData.EventType = "END";
                    }
                    fixationData.EventType = "SACCADES";
                    fixationData.Type = "SACCADES";
                    //Console.WriteLine("-------------XXX--------------------");
                }
                else
                {
                    //Console.WriteLine("--------------------FIXATION--------------------");
                    fixationData.Type = "FIXATIONS";
                    if (PreviousFixationData.Type == "SACCADES")
                    {
                        fixationData.EventType = "BEGIN";
                        counter += 1;
                    }
                    else
                    {
                        fixationData.EventType = "DATA";
                    }
                    // For Fixations - compute centroid
                    fixationData.X_Median = XValueList.Average();
                    fixationData.Y_Median = YValueList.Average();
                }
            }
            //Console.WriteLine(counter);
            PreviousFixationData = fixationData;
            return fixationData;
        }

        private static double GetDispersion(FixationData fixationData)
        {
            try
            {
                return (XValueList.Max() - XValueList.Min()) + (YValueList.Max() - YValueList.Min());
            }
            catch(Exception ex)
            {
                // Enumeration Exception
                XValueList.Clear();
                YValueList.Clear();
                return 0;
            }
        }
    }
}
