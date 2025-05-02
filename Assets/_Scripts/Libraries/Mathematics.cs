namespace Physics.Math
{
    internal struct Mathematics
    {
        // LERPING \\

        /// <summary>
        /// Lerps a number from a to b
        /// </summary>
        /// <param name="a">Start value</param>
        /// <param name="b">Final value</param>
        /// <param name="t">How much advanced</param>
        /// <returns>Where the number goes</returns>
        internal static float Lerp(float a, float b, float t) => a + (b - a) * t;

        internal static float PowerLerp(float t, int power)
        {
            float result = t;

            for (int i = 0; i < power; i++)
            {
                result *= result;
            }

            return result;
        }
        internal static float InversePowerLerp(float t, int power)
        {
            float n = 1 - t;

            for (int i = 0; i < power; i++)
            {
                n *= n;
            }

            return 1 - n;
        }

        internal static float CubicAlphaLerp(float t) => PowerLerp(t, 3);
        internal static float InverseCubicAlphaLerp(float t) => InversePowerLerp(t, 3);

        internal static float SquareAlphaLerp(float t) => PowerLerp(t, 2);
        internal static float InverseSquareAlphaLerp(float t) => InversePowerLerp(t, 2);
    }
}
