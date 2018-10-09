using System;

namespace InformationHiding.Math
{
    public class RemainderService: IRemainder
    {
        public int FindRemainder(int numerator, int denominator)
        {
            if (denominator != 0)
            {
                return numerator % denominator;
            }
            else
            {
                return 0;
            }
        }
    }
}
