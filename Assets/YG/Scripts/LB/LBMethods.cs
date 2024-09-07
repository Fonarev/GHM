using System;

namespace Assets.YG.Scripts.LB
{
    public static class LBMethods 
    {
        public static string TimeTypeConvertStatic(int score, int decimalSize)
        {
            if (score < 1000)
                return "00:00";

            if (decimalSize == 1)
                return TimeSpan.FromMilliseconds(score).ToString("mm':'ss'.'f");
            else if (decimalSize == 2)
                return TimeSpan.FromMilliseconds(score).ToString("mm':'ss'.'ff");
            else if (decimalSize == 3)
                return TimeSpan.FromMilliseconds(score).ToString("mm':'ss'.'fff");
            else
                return TimeSpan.FromMilliseconds(score).ToString("mm':'ss");
        }

        public static string TimeTypeConvertStatic(int score)
        {
            return TimeTypeConvertStatic(score, 0);
        }

        public static string AnonimName(string origName)
        {
            if (origName != "anonymous") return origName;
            else return "None";
        }
    }
}