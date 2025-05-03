using System.Threading.Tasks;
using UnityEngine;
using Physics.Fields;
using Physics.Math;
using Physics.Arrow;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private bool start = false;

    [SerializeField] private RectTransform electron;
    private VectorField2D field;

    private async void Start()
    {
        await Task.Delay(1000);

        field = new VectorField2D(
            function: VectorFieldFunction,
            callback: (Arrow2D arrow) => arrow.SetColor(new Color(0, 0, 0, 0)),
            animation: async (Arrow2D arrow, int index) => {
                arrow.SetColor(new Color(0, 0, 0, 0));

                for (float t = 0; t <= 1; t += 0.033f)
                {
                    float alpha = Mathematics.InverseCubicAlphaLerp(t);

                    arrow.SetColor(new Color(255, index / 100f, index / 60f, Mathematics.Lerp(0f, 1f, alpha)));
                    arrow.Object.transform.rotation = Quaternion.Euler(0, 0, Mathematics.Lerp(5f, 0f, alpha));

                    await Task.Delay(20);
                }
            },
            animationOnChange: null,
            xIterations: 30,
            yIterations: 30,
            gap: 20,
            center: Vector2.zero
        );

        await Task.Delay(3000);

        start = true;
    }

    private void Update()
    {
        if (!start) return;
        field.ChangeField(VectorFieldFunction, AnimationDirection.AllAtOnce); 
    }

    private Vector2 VectorFieldFunction(Vector2 point)
    {
        return (electron.anchoredPosition - point).normalized * 10;
    }
}
