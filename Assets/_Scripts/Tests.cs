using UnityEngine;
using Physics.Planes;
using Physics.Graphics;
using System.Threading.Tasks;
using Physics.Math;
using Physics.Arrow;
using Unity.VisualScripting;
using System.Collections.Generic;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private async void Start()
    {
        await Task.Delay(1000);

        CartesianPlane2D plane = new CartesianPlane2D(
            thickness: 3,
            magnitudeX: 600,
            magnitudeY: 600,
            distanceX: 30,
            distanceY: 30,
            callback: CartesianPlane2D.ShowUpAnimated,
            color: new Color(1, 1, 1, 0)
        );

        await Task.Delay(1000);

        GameObject graph = new GameObject("Graph");
        graph.transform.SetParent(GameObject.Find("Canvas").transform, false);

        RectTransform graphTransform = graph.AddComponent<RectTransform>();
        graphTransform.anchorMin = new Vector2(0.5f, 0.5f);
        graphTransform.anchorMax = new Vector2(0.5f, 0.5f);
        graphTransform.pivot = new Vector2(0.5f, 0.5f);
        graphTransform.anchoredPosition = plane.GetPositionInPlane(0, 0);

        UIPath path = graphTransform.AddComponent<UIPath>();

        List<Vector2> keyPositions = new List<Vector2>();

        for (float x = -3; x <= 3; x += 0.05f)
        {
            Vector2 position = plane.GetPositionInPlane(x, Function(x));
            keyPositions.Add(new Vector2(-position.y, position.x));
        }

        path.SetPoints(keyPositions.ToArray());
    }

    private float Function(float x)
    {
        return Mathf.Sin(x);
    }
}
