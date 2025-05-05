using UnityEngine;
using Physics.Graphics;
using System.Collections.Generic;

#nullable enable

namespace Physics.Planes
{
    internal class CartesianPlane2D
    {
        // ATTRIBUTES \\

        #region Function definitions
        internal delegate void Callback(CartesianPlane2D plane);
        #endregion

        #region Attributes
        private GameObject parent;

        private Line xAxis;
        private Line yAxis;

        private float distanceUnitsX = 0;
        private float distanceUnitsY = 0;

        private List<Line> integersInXAxis = new List<Line>();
        private List<Line> integersInYAxis = new List<Line>();
        #endregion


        // CONSTRUCTOR \\
        internal CartesianPlane2D(float depth, float magnitudeX, float magnitudeY, float distanceX, float distanceY, Color color, Callback? callback)
        {
            this.parent = new GameObject("Cartesian plane");
            this.parent.transform.SetParent(GameObject.Find("Canvas").transform, false);

            this.distanceUnitsX = distanceX;
            this.distanceUnitsY = distanceY;

            // Creating x axis
            this.xAxis = new Line(
                parent: this.parent,
                width: depth,
                magnitude: magnitudeX,
                position: Vector2.zero,
                pivot: Vector2.one * 0.5f,
                angle: 0
            );
            this.xAxis.Name = "X-Axis";
            this.XAxis.SetColor(color);

            for (int i = -1; i <= 1; i += 2)
            {
                float currentX = 0;
                int integer = 0;

                while (i < 0 ? -currentX < magnitudeX * 0.5f : currentX < magnitudeX * 0.5f)
                {
                    integer += i;
                    currentX += this.distanceUnitsX * i;

                     Line integerInAxis = new Line(
                        parent: this.xAxis.Object,
                        width: depth,
                        magnitude: depth * 5,
                        position: new Vector2(integer * this.distanceUnitsX, 0),
                        pivot: Vector2.one * 0.5f,
                        angle: Mathf.PI * 0.5f
                    );
                    integerInAxis.SetColor(color);
                    integerInAxis.Name = $"{integer}";
                    integersInXAxis.Add(integerInAxis);
                }
            }


            // Creating y axis
            this.yAxis = new Line(
                parent: this.parent,
                width: depth,
                magnitude: magnitudeY,
                position: Vector2.zero,
                pivot: Vector2.one * 0.5f,
                angle: Mathf.PI * 0.5f
            );
            this.yAxis.Name = "Y-Axis";
            this.yAxis.SetColor(color);

            for (int i = -1; i <= 1; i += 2)
            {
                float currentY = 0;
                int integer = 0;

                while (i < 0 ? -currentY < magnitudeY * 0.5f : currentY < magnitudeY * 0.5f)
                {
                    integer += i;
                    currentY += this.distanceUnitsY * i;

                    Line integerInAxis = new Line(
                       parent: this.yAxis.Object,
                       width: depth,
                       magnitude: depth * 5,
                       position: new Vector2(integer * this.distanceUnitsY, 0),
                       pivot: Vector2.one * 0.5f,
                       angle: 0
                    );
                    integerInAxis.SetColor(color);
                    integerInAxis.Name = $"{integer}";
                    integersInYAxis.Add(integerInAxis);
                }
            }

            if (callback is not null)
            {
                callback(this);
            }
        }


        // METHODS \\
        /// <summary>
        /// Calculates the given position in the plan but in the UI coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        internal Vector2 GetPositionInPlane(float x, float y) => new Vector2(this.distanceUnitsX * y, this.distanceUnitsY * -x);

        /// <summary>
        /// Sets a color for each line that make the axes
        /// </summary>
        /// <param name="color">Color to set for the axes</param>
        internal void SetColor(Color color)
        {
            this.XAxis.SetColor(color);
            this.YAxis.SetColor(color);

            foreach (Line line in this.IntegersInXAxisLines)
            {
                line.SetColor(color);
            }

            foreach (Line line in this.IntegersInYAxisLines)
            {
                line.SetColor(color);
            }
        }

        // SETTERS & GETTERS \\
        internal Line XAxis { get => this.xAxis; private set => this.xAxis = value; }
        internal Line YAxis { get => this.yAxis; private set => this.yAxis = value; }

        internal List<Line> IntegersInXAxisLines { get => this.integersInXAxis; private set => this.integersInXAxis = value; }
        internal List<Line> IntegersInYAxisLines { get => this.integersInYAxis; private set => this.integersInYAxis = value; }

        internal GameObject Plane { get => this.parent; private set => this.parent = value; }
    }
}