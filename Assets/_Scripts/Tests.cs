using UnityEngine;
using Physics.Planes;
using Physics.Arrow;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private void Start()
    {
        CartesianPlane2D plane = new CartesianPlane2D(
            depth: 3,
            magnitudeX: 600,
            magnitudeY: 400,
            distanceX: 30,
            distanceY: 30
        );

        Arrow2D arrow = new Arrow2D(
            plane.GetPositionInPlane(0, 0),
            plane.GetPositionInPlane(3, -2),
            5
        );
    }
}
