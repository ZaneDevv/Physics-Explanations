using UnityEngine;
using Physics.Fields;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private void Start()
    {
        VectorField2D field = new VectorField2D(
            function: VectorFieldFunction,
            xIterations: 20,
            yIterations: 20,
            gap: 20
        );
    }

    private Vector2 VectorFieldFunction(Vector2 point)
    {
        return -point.normalized * 10;
    }
}
