using UnityEngine;
using UnityEngine.UI;

namespace Physics.Arrow.Graphics
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal class UITriangle : Graphic
    {
        private float width = 100f;
        private float height = 100f;

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();

            Vector2 point0 = new Vector2(0, height);
            Vector2 point1 = new Vector2(-width * 0.5f, 0);
            Vector2 point2 = new Vector2(width * 0.5f, 0);

            vertexHelper.AddVert(point0, color, Vector2.zero);
            vertexHelper.AddVert(point1, color, Vector2.zero);
            vertexHelper.AddVert(point2, color, Vector2.zero);

            vertexHelper.AddTriangle(0, 1, 2);
        }

        internal void SetSize(float width, float height)
        {
            this.width = width;
            this.height = height;

            SetVerticesDirty();
        }
    }
}
