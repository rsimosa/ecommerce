namespace InformationHiding.Math
{
    public interface IRemainder
    {
        /// <summary>
        /// Returns the remainder after dividing the two inputs
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        int FindRemainder(int numerator, int denominator);
    }
}