using UnityEngine;
using Physics.Planes;
using Physics.Graphics;
using System.Threading.Tasks;
using Physics.Math;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private async void Start()
    {
        await Task.Delay(1000);

        CartesianPlane2D plane = new CartesianPlane2D(
            depth: 3,
            magnitudeX: 600,
            magnitudeY: 600,
            distanceX: 30,
            distanceY: 30,
            callback: (CartesianPlane2D plane) =>
            {
                foreach (Line line in plane.IntegersInXAxisLines)
                {
                    line.SetColor(new Color(1, 1, 1, 0));
                    line.Position = new Vector2(line.Position.x, line.Position.y - 20);
                }

                foreach (Line line in plane.IntegersInYAxisLines)
                {
                    line.SetColor(new Color(1, 1, 1, 0));
                    line.Position = new Vector2(line.Position.x, line.Position.y + 20);
                }

                plane.XAxis.SetColor(new Color(0, 0, 0, 0));
                plane.YAxis.SetColor(new Color(0, 0, 0, 0));

                xAxisLines(plane);
                yAxisLines(plane);

                Axes(plane);
            },
            color: new Color(0, 0, 0, 0)
        );
    }

    private async void Axes(CartesianPlane2D plane)
    {
        for (float t = 0; t <= 1; t += 0.02f)
        {
            float alpha = Mathematics.InverseCubicAlphaLerp(t);

            plane.XAxis.SetColor(new Color(1, 1, 1, alpha));
            plane.YAxis.SetColor(new Color(1, 1, 1, alpha));

            await Task.Delay(16);
        }
    }

    private async void xAxisLines(CartesianPlane2D plane)
    {
        foreach (Line line in plane.IntegersInXAxisLines)
        {
            await Task.Delay(60);
            xAxisLine(line);
        }
    }

    private async void xAxisLine(Line line)
    {
        float origin = line.Position.y;
        float target = origin + 20;

        for (float t = 0; t <= 1; t += 0.0333f)
        {
            float alpha = Mathematics.InverseCubicAlphaLerp(t);

            line.SetColor(new Color(1, 1, 1, alpha));
            line.Position = new Vector2(line.Position.x, Mathematics.Lerp(origin, target, alpha));

            await Task.Delay(16);
        }
    }

    private async void yAxisLines(CartesianPlane2D plane)
    {
        foreach (Line line in plane.IntegersInYAxisLines)
        {
            await Task.Delay(60);
            yAxisLine(line);
        }
    }

    private async void yAxisLine(Line line)
    {
        float origin = line.Position.y;
        float target = origin - 20;

        for (float t = 0; t <= 1; t += 0.0333f)
        {
            float alpha = Mathematics.InverseCubicAlphaLerp(t);

            line.SetColor(new Color(1, 1, 1, alpha));
            line.Position = new Vector2(line.Position.x, Mathematics.Lerp(origin, target, alpha));

            await Task.Delay(16);
        }
    }
}
