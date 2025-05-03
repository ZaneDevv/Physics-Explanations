using System.Threading.Tasks;
using UnityEngine;
using Physics.Fields;
using Physics.Math;
using Physics.Arrow;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private async void Start()
    {
        await Task.Delay(1000);

        VectorField2D field = new VectorField2D(
            function: VectorFieldFunction,
            callback: (Arrow2D arrow) => arrow.SetColor(new Color(0, 0, 0, 0)),
            animation: async (Arrow2D arrow, int index) => {
                arrow.SetColor(new Color(0, 0, 0, 0));

                for (float t = 0; t <= 1; t += 0.033f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    arrow.SetColor(new Color(1f, index / 100f, index / 100f, Mathematics.Lerp(0f, 1f, alpha)));
                    arrow.Object.transform.rotation = Quaternion.Euler(0, 0, Mathematics.Lerp(5f, 0f, alpha));

                    await Task.Delay(20);
                }
            },
            animationOnChange: async (Arrow2D arrow, float originAngle, float targetAngle) => {
                for (float t = 0; t <= 1; t += 0.033f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    arrow.Angle = Mathematics.Lerp(originAngle, targetAngle, alpha);

                    await Task.Delay(20);
                }
            },
            xIterations: 20,
            yIterations: 20,
            gap: 20,
            center: Vector2.zero
        );

        await Task.Delay(3500);

        field.ChangeField(VectorFieldFunction2);
    }

    private void Update()
    {

    }

    private Vector2 VectorFieldFunction(Vector2 point)
    {
        return -point.normalized * 10;
    }

    private Vector2 VectorFieldFunction2(Vector2 point)
    {
        return new Vector2(point.normalized.y, -point.normalized.x);
    }
}
