using UnityEngine;
using UnityEngine.UI;
using Physics.Arrow.Graphics;

namespace Physics.Arrow
{
    internal class Arrow2D
    {
        // Attributes \\
        private Vector2 originPoint = Vector2.zero;
        private float magnitude = 0;
        private float width = 0;
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
            this.quaternion = new Quaternion(0, 0, Mathf.Sin(theta), Mathf.Cos(theta));

            this.Canvas = GameObject.Find("Canvas").transform;

            this.Render();
        }

        internal Arrow2D(Vector2 originPoint, Vector2 targetPoint, float width)
        {
            this.originPoint = originPoint;
            this.magnitude = Vector2.Distance(originPoint, targetPoint);
            this.width = width;

            float theta = Vector2.Angle(originPoint, targetPoint) * Mathf.Deg2Rad;

            this.quaternion = new Quaternion(0, 0, Mathf.Sin(theta), Mathf.Cos(theta));

            this.Canvas = GameObject.Find("Canvas").transform;

            this.Render();
        }


        // Methods \\

        /// <summary>
        /// Renders the arrow
        /// </summary>
        internal void Render()
        {
            // Create the line
            this.Line = new GameObject("UILine");
            RectTransform rectTransform = this.Line.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(this.width, this.magnitude);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.rotation = this.quaternion;

            Image image = this.Line.AddComponent<Image>();
            image.color = Color.red;

            this.Line.transform.SetParent(this.Canvas, false);

            // Create the triangle
            this.Triangle = new GameObject("Triangle");

            RectTransform triRect = this.Triangle.AddComponent<RectTransform>();
            triRect.anchoredPosition = new Vector2(0, this.magnitude * 0.5f);
            triRect.rotation = this.quaternion;

            this.Triangle.transform.SetParent(this.Canvas, false);

            UITriangle triangleGraphics = this.Triangle.AddComponent<UITriangle>();
            triangleGraphics.color = Color.red;
        }


        /// <summary>
        /// Sets the color of the arrow
        /// </summary>
        /// <param name="color"></param>
        internal void SetColor(Color color) {
            this.Line.GetComponent<Image>().color = color;
           // this.Triangle.GetComponent<Image>().color = color;
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