using UnityEngine;
using UnityEngine.UI;
using Physics.Graphics;

namespace Physics.Arrow
{
    internal class Arrow2D
    {
        private const float TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH = 4;

        // Attributes \\
        private float magnitude = 0;
        private float width = 0;
        private float theta = 0;

        private Vector2 originPoint = Vector2.zero;
        private Quaternion quaternion = Quaternion.identity;

        private GameObject arrow;
        private GameObject Line;
        private GameObject Triangle;

        private RectTransform lineTransform;
        private RectTransform triangleTransform;

        private Transform Canvas;


        // Constructors \\
        internal Arrow2D(Vector2 originPoint, float magnitude, float theta, float width)
        {
            this.originPoint = originPoint;
            this.magnitude = magnitude;
            this.width = width;
            this.theta = theta;

            this.quaternion = new Quaternion(0, 0, Mathf.Sin(theta * 0.5f), Mathf.Cos(theta * 0.5f));
            this.Canvas = GameObject.Find("Canvas").transform;

            this.SetGraphics();
        }


        internal Arrow2D(Vector2 originPoint, Vector2 targetPoint, float width)
        {
            this.originPoint = originPoint;
            this.magnitude = Vector2.Distance(originPoint, targetPoint);
            this.width = width;

            Vector2 direction = targetPoint - originPoint;
            float theta = Mathf.Atan2(direction.y, direction.x);

            this.theta = theta;

            this.quaternion = new Quaternion(0, 0, Mathf.Sin(theta * 0.5f), Mathf.Cos(theta * 0.5f));
            this.Canvas = GameObject.Find("Canvas").transform;

            this.SetGraphics();
        }


        // Methods \\

        /// <summary>
        /// Creates the graphics for the line
        /// </summary>
        private void SetGraphics()
        {
            float triangleSize = this.width * TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH;

            // Create the line
            this.arrow = new GameObject("Arrow");
            this.arrow.transform.SetParent(this.Canvas, false);

            this.Line = new GameObject("Line");
            this.Line.transform.SetParent(this.arrow.transform, false);

            this.lineTransform = this.Line.AddComponent<RectTransform>();
            this.lineTransform.anchorMin = new Vector2(0.5f, 0.5f);
            this.lineTransform.anchorMax = new Vector2(0.5f, 0.5f);
            this.lineTransform.pivot = new Vector2(0.5f, 0f);
            this.lineTransform.sizeDelta = new Vector2(this.width, this.magnitude - triangleSize);
            this.lineTransform.anchoredPosition = this.originPoint;
            this.lineTransform.localRotation = this.quaternion;

            Image image = this.Line.AddComponent<Image>();
            image.color = Color.red;

            // Create the triangle
            this.Triangle = new GameObject("Triangle");
            this.Triangle.transform.SetParent(this.arrow.transform, false);

            this.triangleTransform = this.Triangle.AddComponent<RectTransform>();
            this.triangleTransform.anchorMin = new Vector2(0.5f, 0.5f);
            this.triangleTransform.anchorMax = new Vector2(0.5f, 0.5f);
            this.triangleTransform.pivot = new Vector2(0.5f, 0f);
            this.triangleTransform.localRotation = this.quaternion;
            this.triangleTransform.sizeDelta = new Vector2(triangleSize, triangleSize);
            this.triangleTransform.anchoredPosition = this.originPoint + (this.magnitude - triangleSize) * new Vector2(-Mathf.Sin(this.theta), Mathf.Cos(this.theta));

            UITriangle triangleGraphics = this.Triangle.AddComponent<UITriangle>();
            triangleGraphics.color = Color.red;
            triangleGraphics.Width = triangleGraphics.Height = triangleSize;

            triangleGraphics.SetAllDirty();
            this.Triangle.transform.SetAsLastSibling();
        }


        /// <summary>
        /// Sets the color of the arrow
        /// </summary>
        /// <param name="color">Color to apply</param>
        internal void SetColor(Color color) {
            this.Line.GetComponent<Image>().color = color;
            this.Triangle.GetComponent<UITriangle>().color = color;
        }

        

        // Setters & getters \\
        internal Vector2 OriginPoint
        {
            get => this.originPoint;
            private set => this.originPoint = value;
        }
        internal Quaternion Rotation
        {
            get => this.quaternion;
            private set => this.quaternion = value;
        }

        internal float Angle
        {
            get => this.theta;
            set
            {
                this.theta = value;
                this.quaternion = this.quaternion = new Quaternion(0, 0, Mathf.Sin(theta * 0.5f), Mathf.Cos(theta * 0.5f));
                this.lineTransform.localRotation = this.quaternion;
                this.triangleTransform.localRotation = this.quaternion;
                this.triangleTransform.anchoredPosition = this.originPoint + (this.magnitude - this.width * TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH) * new Vector2(-Mathf.Sin(this.theta), Mathf.Cos(this.theta));
            }
        }
        internal float Magnitude
        {
            get => this.magnitude;
            set
            {
                this.magnitude = value;
                this.lineTransform.sizeDelta = new Vector2(this.width, this.magnitude - this.width * TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH);
                this.triangleTransform.anchoredPosition = this.originPoint + (this.magnitude - this.width * TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH) * new Vector2(-Mathf.Sin(this.theta), Mathf.Cos(this.theta));
            }
        }

        internal GameObject Object { get => this.arrow; private set => this.arrow = value; }

        internal Transform Parent
        {
            get => this.Canvas;
            set
            {
                this.Canvas = value;

                this.lineTransform.SetParent(value, false);
                this.triangleTransform.SetParent(value, false);
            }
        }
    }
}