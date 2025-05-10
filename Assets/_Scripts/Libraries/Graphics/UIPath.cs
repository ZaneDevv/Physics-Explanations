using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Physics.Graphics
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal class UIPath : Graphic
    {
        [SerializeField] private List<Vector2> positions = new List<Vector2>();
        [SerializeField] private float width = 5f;

        internal float Width { get => width; set 
            {
                width = value;
                SetVerticesDirty();
            }
        }

        internal void SetPoints(Vector2[] keyPositions)
        {
            positions.Clear();
            positions.AddRange(keyPositions);

            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();

            if (positions.Count < 2) return;

            for (int i = 0; i < positions.Count - 1; i++)
            {
                Vector2 currentPoint = positions[i];
                Vector2 nextPoint = positions[i + 1];

                Vector2 direction = (nextPoint - currentPoint).normalized;
                Vector2 normal = new Vector2(-direction.y, direction.x) * width * 0.5f;

                Vector2 vertex0 = currentPoint + normal;
                Vector2 vertex1 = currentPoint - normal;
                Vector2 vertex2 = nextPoint - normal;
                Vector2 vertex3 = nextPoint + normal;

                int index = vertexHelper.currentVertCount;

                vertexHelper.AddVert(vertex0, color, Vector2.zero);
                vertexHelper.AddVert(vertex1, color, Vector2.zero);
                vertexHelper.AddVert(vertex2, color, Vector2.zero);
                vertexHelper.AddVert(vertex3, color, Vector2.zero);

                vertexHelper.AddTriangle(index, index + 1, index + 2);
                vertexHelper.AddTriangle(index + 2, index + 3, index);
            }
        }
    }
}
