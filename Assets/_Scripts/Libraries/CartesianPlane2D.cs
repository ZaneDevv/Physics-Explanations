using UnityEngine;
using Unity.VisualScripting;
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

        internal delegate float Expression(float x);
        #endregion

        #region Attributes
        private GameObject parent;

        private Line xAxis;
        private Line yAxis;

        private float distanceUnitsX = 0;
        private float distanceUnitsY = 0;

        private List<(Line, Line)> integersInXAxis = new List<(Line, Line)>();
        private List<(Line, Line)> integersInYAxis = new List<(Line, Line)>();

        private Color color;
        #endregion


        // CONSTRUCTOR \\
        internal CartesianPlane2D(float thickness, float magnitudeX, float magnitudeY, float distanceX, float distanceY, Color color, Callback? callback)
        {
            this.parent = new GameObject("Cartesian plane");
            this.parent.transform.SetParent(GameObject.Find("Canvas").transform, false);

            this.distanceUnitsX = distanceX;
            this.distanceUnitsY = distanceY;

            this.color = color;

            // Creating x axis
            this.xAxis = new Line(
                parent: this.parent,
                width: thickness,
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
                        width: thickness,
                        magnitude: thickness * 5,
                        position: new Vector2(integer * this.distanceUnitsX, 0),
                        pivot: Vector2.one * 0.5f,
                        angle: Mathf.PI * 0.5f
                    );
                    integerInAxis.SetColor(color);
                    integerInAxis.Name = $"{integer}";

                    Line integerFullLine = new Line(
                        parent: integerInAxis.Object,
                        width: thickness * 0.5f,
                        magnitude: magnitudeX,
                        position: Vector2.zero,
                        pivot: Vector2.one * 0.5f,
                        angle: Mathf.PI * 0.5f
                    );
                    integerFullLine.SetColor(new Color(color.r, color.g, color.b, color.a * 0.25f));

                    integersInXAxis.Add((integerInAxis, integerFullLine));
                }
            }


            // Creating y axis
            this.yAxis = new Line(
                parent: this.parent,
                width: thickness,
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
                       width: thickness,
                       magnitude: thickness * 5,
                       position: new Vector2(integer * this.distanceUnitsY, 0),
                       pivot: Vector2.one * 0.5f,
                       angle: 0
                    );
                    integerInAxis.SetColor(color);
                    integerInAxis.Name = $"{integer}";

                    Line integerFullLine = new Line(
                       parent: integerInAxis.Object,
                       width: thickness * 0.5f,
                       magnitude: magnitudeY,
                       position: Vector2.zero,
                       pivot: Vector2.one * 0.5f,
                       angle: 0
                    );
                    integerFullLine.SetColor(new Color(color.r, color.g, color.b, color.a * 0.25f));

                    integersInYAxis.Add((integerInAxis, integerFullLine));
                }
            }

            integersInXAxis = integersInXAxis.OrderBy(item => int.Parse(item.Item1.Name)).ToList();
            integersInYAxis = integersInYAxis.OrderBy(item => int.Parse(item.Item1.Name)).ToList();

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

            foreach ((Line, Line) line in this.IntegersInXAxisLines)
            {
                line.Item1.SetColor(color);
                line.Item2.SetColor(color);
            }

            foreach ((Line, Line) line in this.IntegersInYAxisLines)
            {
                line.Item1.SetColor(color);
                line.Item2.SetColor(color);
            }

            this.color = color;
        }

        /// <summary>
        /// Gets the color of the plane
        /// </summary>
        /// <returns>Color of the plane</returns>
        internal Color GetColor() => this.color;

        /// <summary>
        /// Generates a graph
        /// </summary>
        /// <param name="function">Function that the graph must follow</param>
        /// <param name="color">Color of the graph</param>
         /// <returns>Object of the graph</returns>
        internal async void AddGraph(Expression function, Color color, float width, float speed)
        {
            int waitingSecondsAnimation = Mathf.FloorToInt(1 / speed * (float)1e3);

            GameObject graph = new GameObject("Graph");
            graph.transform.SetParent(this.parent.transform, false);

            RectTransform graphTransform = graph.AddComponent<RectTransform>();
            graphTransform.anchorMin = new Vector2(0.5f, 0.5f);
            graphTransform.anchorMax = new Vector2(0.5f, 0.5f);
            graphTransform.pivot = new Vector2(0.5f, 0.5f);
            graphTransform.anchoredPosition = this.GetPositionInPlane(0, 0);

            UIPath path = graphTransform.AddComponent<UIPath>();
            path.color = color;
            path.Width = width;

            List<Vector2> keyPositions = new List<Vector2>();

            float index = 0;
            for (float x = -10; x <= 10; x += 0.05f)
            {
                if (index == 0 && speed > 0)
                {
                    await Task.Delay(waitingSecondsAnimation);
                }

                float y = function(x);
                if (float.IsNaN(y) || Mathf.Abs(y) > integersInXAxis.Count * 0.5f)
                {
                    if (path.GetPointsAmout() > 0)
                    {
                        keyPositions.Clear();

                        graph = new GameObject("Graph");
                        graph.transform.SetParent(this.parent.transform, false);

                        graphTransform = graph.AddComponent<RectTransform>();
                        graphTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        graphTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        graphTransform.pivot = new Vector2(0.5f, 0.5f);
                        graphTransform.anchoredPosition = this.GetPositionInPlane(0, 0);

                        path = graphTransform.AddComponent<UIPath>();
                        path.color = color;
                        path.Width = width;
                    }

                    continue;
                }

                Vector2 position = this.GetPositionInPlane(-y, x);
                keyPositions.Add(position);

                path.SetPoints(keyPositions.ToArray());

                index++;
                index %= 10;
            }

        }

        // SETTERS & GETTERS \\
        internal Line XAxis { get => this.xAxis; private set => this.xAxis = value; }
        internal Line YAxis { get => this.yAxis; private set => this.yAxis = value; }

        internal List<(Line, Line)> IntegersInXAxisLines { get => this.integersInXAxis; private set => this.integersInXAxis = value; }
        internal List<(Line, Line)> IntegersInYAxisLines { get => this.integersInYAxis; private set => this.integersInYAxis = value; }

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
                foreach ((Line, Line) line in plane.IntegersInXAxisLines)
                {
                    await Task.Delay(60);
                    xAxisLine(line);
                }
            }
            async void xAxisLine((Line, Line) line)
            {
                float origin = line.Item1.Position.y;
                float target = origin + 20;

                for (float t = 0; t <= 1; t += 0.0333f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    line.Item1.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, alpha));
                    line.Item1.Position = new Vector2(line.Item1.Position.x, Mathematics.Lerp(origin, target, alpha));

                    line.Item2.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, Mathematics.Lerp(0, 0.25f, alpha)));

                    await Task.Delay(16);
                }
            }

            async void yAxisLines()
            {
                foreach ((Line, Line) line in plane.IntegersInYAxisLines)
                {
                    await Task.Delay(60);
                    yAxisLine(line);
                }
            }
            async void yAxisLine((Line, Line) line)
            {
                float origin = line.Item1.Position.y;
                float target = origin - 20;


                for (float t = 0; t <= 1; t += 0.0333f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    line.Item1.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, alpha));
                    line.Item1.Position = new Vector2(line.Item1.Position.x, Mathematics.Lerp(origin, target, alpha));

                    line.Item2.SetColor(new Color(planeColor.r, planeColor.g, planeColor.b, Mathematics.Lerp(0, 0.25f, alpha)));

                    await Task.Delay(16);
                }
            }

            foreach ((Line, Line) line in plane.IntegersInXAxisLines)
            {
                line.Item1.SetColor(new Color(1, 1, 1, 0));
                line.Item1.Position = new Vector2(line.Item1.Position.x, line.Item1.Position.y - 20);

                line.Item2.SetColor(new Color(1, 1, 1, 0));
            }

            foreach ((Line, Line) line in plane.IntegersInYAxisLines)
            {
                line.Item1.SetColor(new Color(1, 1, 1, 0));
                line.Item1.Position = new Vector2(line.Item1.Position.x, line.Item1.Position.y + 20);

                line.Item2.SetColor(new Color(1, 1, 1, 0));
            }

            plane.XAxis.SetColor(new Color(0, 0, 0, 0));
            plane.YAxis.SetColor(new Color(0, 0, 0, 0));

            xAxisLines();
            yAxisLines();

            Axes();
        }
    }
}