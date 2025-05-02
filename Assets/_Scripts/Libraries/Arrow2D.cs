using UnityEngine;
using UnityEngine.UI;
using Physics.Arrow.Graphics;

namespace Physics.Arrow
{
    internal class Arrow2D
    {
        private const float TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH = 4;

        // Attributes \\
        private Vector2 originPoint = Vector2.zero;
        private float magnitude = 0;
        private float width = 0;
        private float theta = 0;
        private Quaternion quaternion = Quaternion.identity;

        private GameObject Line;
        private GameObject Triangle;

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
            // Create the line
            this.Line = new GameObject("UILine");
            this.Line.transform.SetParent(this.Canvas, false);

            RectTransform lineTransform = this.Line.AddComponent<RectTransform>();
            lineTransform.anchorMin = new Vector2(0.5f, 0.5f);
            lineTransform.anchorMax = new Vector2(0.5f, 0.5f);
            lineTransform.pivot = new Vector2(0.5f, 0f);
            lineTransform.sizeDelta = new Vector2(this.width, this.magnitude);
            lineTransform.anchoredPosition = this.originPoint;
            lineTransform.localRotation = this.quaternion;

            Image image = this.Line.AddComponent<Image>();
            image.color = Color.red;

            // Create the triangle
            float triangleSize = this.width * TRIANGLE_SIZE_WITH_RESPECT_TO_WIDTH;

            this.Triangle = new GameObject("Triangle");
            this.Triangle.transform.SetParent(this.Canvas, false);

            RectTransform triangleTransform = this.Triangle.AddComponent<RectTransform>();
            triangleTransform.anchorMin = new Vector2(0.5f, 0.5f);
            triangleTransform.anchorMax = new Vector2(0.5f, 0.5f);
            triangleTransform.pivot = new Vector2(0.5f, 0f);
            triangleTransform.localRotation = this.quaternion;
            triangleTransform.sizeDelta = new Vector2(triangleSize, triangleSize);
            triangleTransform.anchoredPosition = this.originPoint + this.magnitude * new Vector2(-Mathf.Sin(this.theta), Mathf.Cos(this.theta));

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
        internal Quaternion Quaternion
        {
            get => this.quaternion;
            private set => this.quaternion = value;
        }
    }
}