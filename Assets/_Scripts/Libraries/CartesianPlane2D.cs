using UnityEngine;
using Physics.Graphics;
using System.Collections.Generic;
using System.Linq;
using Physics.Math;
using System.Threading.Tasks;

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

        private Color color;
        #endregion


        // CONSTRUCTOR \\
        internal CartesianPlane2D(float depth, float magnitudeX, float magnitudeY, float distanceX, float distanceY, Color color, Callback? callback)
        {
            this.parent = new GameObject("Cartesian plane");
            this.parent.transform.SetParent(GameObject.Find("Canvas").transform, false);

            this.distanceUnitsX = distanceX;
            this.distanceUnitsY = distanceY;

            this.color = color;

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

            integersInXAxis = integersInXAxis.OrderBy(item => int.Parse(item.Name)).ToList();
            integersInYAxis = integersInYAxis.OrderBy(item => int.Parse(item.Name)).ToList();

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

            this.color = color;
        }

        /// <summary>
        /// Gets the color of the plane
        /// </summary>
        /// <returns>Color of the plane</returns>
        internal Color GetColor() => this.color;

        // SETTERS & GETTERS \\
        internal Line XAxis { get => this.xAxis; private set => this.xAxis = value; }
        internal Line YAxis { get => this.yAxis; private set => this.yAxis = value; }

        internal List<Line> IntegersInXAxisLines { get => this.integersInXAxis; private set => this.integersInXAxis = value; }
        internal List<Line> IntegersInYAxisLines { get => this.integersInYAxis; private set => this.integersInYAxis = value; }

        internal GameObject Plane { get => this.parent; private set => this.parent = value; }


        // STATIC METHODS

        internal static void ShowUpAnimated(CartesianPlane2D plane)
        {
            Color planeColor = plane.GetColor();

            async void Axes()
            {
                for (float t = 0; t <= 1; t += 0.006f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    plane.XAxis.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, alpha));
                    plane.YAxis.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, alpha));

                    await Task.Delay(30);
                }
            }

            async void xAxisLines()
            {
                foreach (Line line in plane.IntegersInXAxisLines)
                {
                    await Task.Delay(60);
                    xAxisLine(line);
                }
            }
            async void xAxisLine(Line line)
            {
                float origin = line.Position.y;
                float target = origin + 20;

                for (float t = 0; t <= 1; t += 0.0333f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    line.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, alpha));
                    line.Position = new Vector2(line.Position.x, Mathematics.Lerp(origin, target, alpha));

                    await Task.Delay(16);
                }
            }

            async void yAxisLines()
            {
                foreach (Line line in plane.IntegersInYAxisLines)
                {
                    await Task.Delay(60);
                    yAxisLine(line);
                }
            }
            async void yAxisLine(Line line)
            {
                float origin = line.Position.y;
                float target = origin - 20;

                for (float t = 0; t <= 1; t += 0.0333f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    line.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, alpha));
                    line.Position = new Vector2(line.Position.x, Mathematics.Lerp(origin, target, alpha));

                    await Task.Delay(16);
                }
            }

            foreach (Line line in plane.IntegersInXAxisLines)
            {
                line.SetColor(new Color(1, 1, 1, 0));
                line.Position = new Vector2(line.Position.x, line.Position.y - 20);
            }

            foreach (Line line in plane.IntegersInYAxisLines)
            {
                line.SetColor(new Color(1, 1, 1, 0));
                line.Position = new Vector2(line.Position.x, line.Position.y + 20);
            }

            plane.XAxis.SetColor(new Color(0, 0, 0, 0));
            plane.YAxis.SetColor(new Color(0, 0, 0, 0));

            xAxisLines();
            yAxisLines();

            Axes();
        }
    }
}