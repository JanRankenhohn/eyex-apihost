using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyeX.Models.Globals
{
    static class Utils
    {
        public static string ToString615(string s)
        {
            while(s.Length < 615)
            {
                s += " ";
            }
            return s;
        }

        public static string ToString625(string s)
        {
            while (s.Length < 625)
            {
                s += " ";
            }
            return s;
        }
    }
}
