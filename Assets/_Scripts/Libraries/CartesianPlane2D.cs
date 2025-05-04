using UnityEngine;
using Physics.Graphics;

namespace Physics.Planes
{
    internal class CartesianPlane2D
    {
        // ATTRIBUTES \\
        private GameObject parent;

        private Line xAxis;
        private Line yAxis;

        private float distanceUnitsX = 0;
        private float distanceUnitsY = 0;


        // CONSTRUCTOR \\
        internal CartesianPlane2D(float depth, float magnitudeX, float magnitudeY, float distanceX, float distanceY)
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
                    integerInAxis.Name = $"{integer}";
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
                    integerInAxis.Name = $"{integer}";
                }
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


        // SETTERS & GETTERS \\
        internal Line XAxis { get => this.xAxis; private set => this.xAxis = value; }
        internal Line YAxis { get => this.yAxis; private set => this.yAxis = value; }
        
        internal GameObject Plane { get => this.parent; private set => this.parent = value; }
    }
}
