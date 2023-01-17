using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public static class FloatExtensions
    {
        public static bool InRange(this float value, float compareTo, float scatterValue)
        {
            float comparableWithScatter = compareTo * scatterValue;
            float maxRangeValue = compareTo + comparableWithScatter;
            float minRangeValue = compareTo - comparableWithScatter;

            if (value >= minRangeValue & value <= maxRangeValue)
            {
                return true;
            }
            return false;
        }
    }
}


