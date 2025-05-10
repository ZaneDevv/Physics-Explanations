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

        plane.AddGraph((float x) => Mathf.Sin(x), new Color(0.75f, 0.1f, 0.25f));
    }
}
