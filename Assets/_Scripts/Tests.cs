using UnityEngine;
using Physics.Planes;
using Physics.Arrow;
using System.Threading.Tasks;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private async void Start()
    {
        await Task.Delay(0);

        CartesianPlane2D plane = new CartesianPlane2D(
            depth: 3,
            magnitudeX: 600,
            magnitudeY: 400,
            distanceX: 30,
            distanceY: 30,
            callback: null,
            color: new Color(1, 1, 1, 1)
        );
    }
}
