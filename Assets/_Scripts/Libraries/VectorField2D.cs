using UnityEngine;
using System.Threading.Tasks;
using Physics.Arrow;
using System.Collections.Generic;

#nullable enable

namespace Physics.Fields
{
    enum AnimationDirection
    {
        BottomLeft_TopRight,
        Center,
    }

    internal class VectorField2D
    {
        // Attributes \\

        #region Functions definitions
        internal delegate Vector2 FunctionVectorField(Vector2 point);
        internal delegate void FunctionPerArrow(Arrow2D arrow, int index);
        internal delegate void Callback(Arrow2D arrow);
        internal delegate void AnimationOnFieldChange(Arrow2D arrow, float originAngle, float targetAngle);
        #endregion

        #region Attributes
        private FunctionVectorField function;
        private FunctionPerArrow? animation;
        private Callback? callback;
        private AnimationOnFieldChange? animationOnChange;

        private Vector2 center = Vector2.zero;

        private int XIterations = 0;
        private int YIterations = 0;
        private float gap = 0;

        private GameObject parent;

        private Arrow2D[,] arrowsGrid;
        #endregion

        // Constructors \\

        internal VectorField2D(
            FunctionVectorField function, FunctionPerArrow? animation, Callback? callback, AnimationOnFieldChange? animationOnChange,
            int xIterations, int yIterations, float gap, Vector2 center
        )
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
            int iIndex = 0;

            for (int i = -Mathf.FloorToInt(this.XIterations * 0.5f); i < Mathf.FloorToInt(this.XIterations * 0.5f); i++)
            {
                int jIndex = 0;

                for (int j = -Mathf.FloorToInt(this.YIterations * 0.5f); j < Mathf.FloorToInt(this.YIterations * 0.5f); j++)
                {

                    Vector2 currentPosition = new Vector2(i, j) * this.gap;
                    Vector2 functionFieldResult = this.function(currentPosition);
                    Vector2 targetPoint = currentPosition + new Vector2(-functionFieldResult.y, functionFieldResult.x);

                    Arrow2D arrow = new Arrow2D(currentPosition, targetPoint, 2);
                    arrow.Object.transform.SetParent(this.parent.transform, false);

                    this.arrowsGrid[iIndex, jIndex] = arrow;

                    if (this.callback is null) continue;

                    this.callback(arrow);

                    jIndex++;
                }

                iIndex++;
            }

            this.ShowUpAnimate();
        }

        /// <summary>
        /// Animates the arrows when they show up from the left-bottom corner to the top-right corner
        /// </summary>
        private void ShowUpAnimate()
        {
            if (this.animation is null || this.arrowsGrid == null) return;

            this.RunCodeOverVectors(AnimationDirection.Center, true, this.animation);
        }

        /// <summary>
        /// Changes the function of this vector field
        /// </summary>
        /// <param name="function">Now function that defined the field</param>
        internal void ChangeField(FunctionVectorField function, AnimationDirection direction)
        {
            this.function = function;

            this.RunCodeOverVectors(direction, true, (Arrow2D arrow, int index) =>
            {
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
            });
        }

        /// <summary>
        /// Executes a code for each arrow in a specific order
        /// </summary>
        /// <param name="direction">Direction of the animation</param>
        /// <param name="animation">Execute an animation in a certain order</param>
        /// <param name="functionToExecute">Function to be executed</param>
        private async void RunCodeOverVectors(AnimationDirection direction, bool animation, FunctionPerArrow functionToExecute)
        {
            int delayMiliseconds = animation ? 60 : 0;
            int currentIndex = 0;

            int rows = this.arrowsGrid.GetLength(0);
            int columns = this.arrowsGrid.GetLength(1);

            int startingX = 0;
            int startingY = 0;

            if (direction == AnimationDirection.Center)
            {
                startingX = Mathf.FloorToInt(this.XIterations * 0.5f);
                startingY = Mathf.FloorToInt(this.YIterations * 0.5f);
            }

            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            List<(int, int)> current = new List<(int, int)> { (startingX, startingY) };

            while (current.Count > 0)
            {
                await Task.Delay(delayMiliseconds);

                List<(int, int)> next = new List<(int, int)>();

                foreach (var (row, col) in current)
                {
                    if (row < 0 || col < 0 || row >= rows || col >= columns || visited.Contains((row, col))) continue;
                    visited.Add((row, col));

                    Arrow2D arrow = this.arrowsGrid[row, col];
                    if (arrow is null) continue;

                    functionToExecute(arrow, currentIndex);

                    if (direction == AnimationDirection.Center)
                    {
                        next.Add((row + 1, col));
                        next.Add((row - 1, col));
                        next.Add((row, col + 1));
                        next.Add((row, col - 1));
                    }
                    else
                    {
                        next.Add((row + 1, col));
                        next.Add((row, col + 1));
                    }
                }

                currentIndex++;
                current = next;
            }
        }


        // GETTERS & SETTERS \\
        internal GameObject Parent
        {
            get => this.parent;
            private set => this.parent = value;
        }
    }
}