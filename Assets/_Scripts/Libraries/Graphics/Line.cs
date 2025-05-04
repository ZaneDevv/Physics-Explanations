using UnityEngine;
using UnityEngine.UI;

namespace Physics.Graphics
{
    internal class Line
    {
        // ATTRIBUTES \\
        private GameObject parent;
        private GameObject line;

        private RectTransform rectTransform;
        private Image image;

        private string name = "Line";

        private float theta = 0;


        // CONSTRUCTOR \\
        internal Line(GameObject parent, float width, float magnitude, Vector2 position, Vector2 pivot, float angle)
        {
            this.parent = parent;
            this.theta = angle;

            this.line = new GameObject(this.name);
            this.line.transform.SetParent(this.parent.transform, false);

            this.rectTransform = this.line.AddComponent<RectTransform>();
            this.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            this.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            this.rectTransform.pivot = pivot;
            this.rectTransform.anchoredPosition = position;
            this.rectTransform.sizeDelta = new Vector2(magnitude, width);
            this.rectTransform.rotation = new Quaternion(0, 0, Mathf.Sin(angle * 0.5f), Mathf.Cos(angle * 0.5f));

            this.image = this.line.AddComponent<Image>();
            this.image.color = Color.white;
        }


        // METHODS \\
        private void SetColor(Color color)
        {
            image.color = color;
        }
        

        // SETTERS & GETTERS \\
        internal string Name {
            get => this.name;
            set
            {
                this.name = value;
                this.line.name = value;
            }
        }

        internal float Angle
        {
            get => this.theta;
            set
            {
                this.theta = value;
                this.rectTransform.rotation = new Quaternion(0, 0, Mathf.Sin(value * 0.5f), Mathf.Cos(value * 0.5f));
            }
        }

        internal GameObject Object { get => this.line; private set => this.line = value; }
        internal RectTransform Transform { get => this.rectTransform; private set => this.rectTransform = value; }
    }
}
