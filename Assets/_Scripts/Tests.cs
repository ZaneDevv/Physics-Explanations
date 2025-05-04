using UnityEngine;
using Physics.Planes;

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
    }
}
