using UnityEngine;
using Physics.Arrow;

namespace Physics.Fields
{
    internal class VectorField2D
    {
        internal delegate Vector2 FunctionVectorField(Vector2 point);

        FunctionVectorField function;
        int XIterations = 0;
        int YIterations = 0;
        float gap = 0;

        internal VectorField2D(FunctionVectorField function, int xIterations, int yIterations, float gap)
        {
            this.function = function;

            this.XIterations = xIterations;
            this.YIterations = yIterations;

            this.gap = gap;

            this.ShowUp();
        }

        private void ShowUp()
        {
            for (int i = -(int)Mathf.Floor(this.XIterations * 0.5f); i < (int)Mathf.Floor(this.XIterations * 0.5f); i++)
            {
                for (int j = -(int)Mathf.Floor(this.YIterations * 0.5f); j < (int)Mathf.Floor(this.YIterations * 0.5f); j++)
                {
                    Vector2 currentPosition = new Vector2(i, j) * gap;
                    Vector2 targetPoint = currentPosition + function(currentPosition);

                    Arrow2D arrow = new Arrow2D(currentPosition, targetPoint, 2);
                }
            }
        }
    }
}