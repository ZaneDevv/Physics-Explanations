using UnityEngine;
using UnityEngine.UI;
using Physics.Graphics;

namespace Physics.Planes
{
    internal class CartesianPlane2D
    {
        // ATTRIBUTES \\
        private GameObject parent;

        private Line xAxis;
        private Line yAxis;

        // CONSTRUCTOR \\
        internal CartesianPlane2D(float depth, float magnitudeX, float magnitudeY, float distanceX, float distanceY)
        {
            this.parent = new GameObject("Cartesian plane");
            this.parent.transform.SetParent(GameObject.Find("Canvas").transform, false);

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
                    currentX += distanceX * i;

                     Line integerInAxis = new Line(
                        parent: this.xAxis.Object,
                        width: depth,
                        magnitude: depth * 5,
                        position: new Vector2(integer * distanceX, 0),
                        pivot: Vector2.one * 0.5f,
                        angle: Mathf.PI * 0.5f
                    );
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
                    currentY += distanceY * i;

                    Line integerInAxis = new Line(
                       parent: this.yAxis.Object,
                       width: depth,
                       magnitude: depth * 5,
                       position: new Vector2(integer * distanceY, 0),
                       pivot: Vector2.one * 0.5f,
                       angle: 0
                   );
                }
            }
        }


        // SETTERS & GETTERS \\
        internal Line XAxis { get => this.xAxis; private set => this.xAxis = value; }
        internal Line YAxis { get => this.yAxis; private set => this.yAxis = value; }
    }
}
