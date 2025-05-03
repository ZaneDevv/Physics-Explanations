using UnityEngine;
using System.Threading.Tasks;
using Physics.Arrow;
using System.Collections.Generic;

#nullable enable

namespace Physics.Fields
{
    internal class VectorField2D
    {
        // Attributes \\

        internal delegate Vector2 FunctionVectorField(Vector2 point);
        internal delegate void ShowUpAnimation(Arrow2D arrow, int index);
        internal delegate void Callback(Arrow2D arrow);
        internal delegate void AnimationOnFieldChange(Arrow2D arrow, float originAngle, float targetAngle);

        private FunctionVectorField function;
        private ShowUpAnimation? animation;
        private Callback? callback;
        private AnimationOnFieldChange? animationOnChange;

        private Vector2 center = Vector2.zero;

        private int XIterations = 0;
        private int YIterations = 0;
        private float gap = 0;

        private GameObject parent;

        private Arrow2D[,] arrowsGrid;


        // Constructors \\

        internal VectorField2D(FunctionVectorField function, int xIterations, int yIterations, float gap, ShowUpAnimation? animation, Callback? callback, Vector2 center, AnimationOnFieldChange? animationOnChange)
        {
            this.parent = new GameObject("Vector field");
            this.parent.transform.SetParent(GameObject.Find("Canvas").transform, false);

            this.function = function;
            this.animation = animation;
            this.callback = callback;
            this.animationOnChange = animationOnChange;

            this.center = center;

            this.XIterations = xIterations;
            this.YIterations = yIterations;

            this.gap = gap;

            this.arrowsGrid = new Arrow2D[XIterations, YIterations];

            this.ShowUp();
        }


        // Methods \\

        /// <summary>
        /// Generates the whole field
        /// </summary>
        private void ShowUp()
        {
            for (int i = -(int)Mathf.Floor(this.XIterations * 0.5f); i < (int)Mathf.Floor(this.XIterations * 0.5f); i++)
            {
                for (int j = -(int)Mathf.Floor(this.YIterations * 0.5f); j < (int)Mathf.Floor(this.YIterations * 0.5f); j++)
                {

                    Vector2 currentPosition = new Vector2(i, j) * this.gap;
                    Vector2 functionFieldResult = this.function(currentPosition);
                    Vector2 targetPoint = currentPosition + new Vector2(-functionFieldResult.y, functionFieldResult.x);

                    Arrow2D arrow = new Arrow2D(currentPosition, targetPoint, 2);
                    arrow.Object.transform.SetParent(this.parent.transform, false);

                    this.arrowsGrid[i + this.XIterations / 2, j + this.YIterations / 2] = arrow;

                    if (this.callback is null) continue;

                    this.callback(arrow);
                }
            }

            if (this.animation is null) return;

            this.ShowUpAnimate();
        }

        /// <summary>
        /// Animates the arrows when they show up from the left-bottom corner to the top-right corner
        /// </summary>
        private async void ShowUpAnimate()
        {
            if (this.animation is null || this.arrowsGrid == null) return;

            int rows = this.arrowsGrid.GetLength(0);
            int columns = this.arrowsGrid.GetLength(1);

            int currentIndex = 0;

            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            List<(int, int)> current = new List<(int, int)> { (1, 1) };

            while (current.Count > 0)
            {
                await Task.Delay(60);

                List<(int, int)> next = new List<(int, int)>();

                foreach (var (row, col) in current)
                {
                    if (row >= rows || col >= columns || visited.Contains((row, col))) continue;

                    visited.Add((row, col));
                    this.animation(this.arrowsGrid[row, col], currentIndex);

                    next.Add((row + 1, col));
                    next.Add((row, col + 1));
                }

                currentIndex++;
                current = next;
            }
        }

        internal async void ChangeField(FunctionVectorField function)
        {
            this.function = function;

            int rows = this.arrowsGrid.GetLength(0);
            int columns = this.arrowsGrid.GetLength(1);

            int currentIndex = 0;

            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            List<(int, int)> current = new List<(int, int)> { (1, 1) };

            while (current.Count > 0)
            {
                await Task.Delay(60);

                List<(int, int)> next = new List<(int, int)>();

                foreach (var (row, col) in current)
                {
                    if (row >= rows || col >= columns || visited.Contains((row, col))) continue;

                    visited.Add((row, col));

                    Arrow2D arrow = this.arrowsGrid[row, col];

                    Vector2 position = arrow.OriginPoint;
                    Vector2 newTarget = this.function(position);
                    Vector2 difference = newTarget - position;

                    float theta = Mathf.Atan2(difference.y, difference.x);

                    if (this.animationOnChange is not null)
                    {
                        this.animationOnChange(arrow, arrow.Angle, theta);
                    }
                    else
                    {
                        arrow.Angle = theta;
                    }

                    next.Add((row + 1, col));
                    next.Add((row, col + 1));
                }

                currentIndex++;
                current = next;
            }
        }
    }
}