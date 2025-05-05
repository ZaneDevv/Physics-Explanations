# Physics explanations [Currenctly in process]

When I decided to make a YouTube channel to explain in a not rigurous way at all some maths and physics stuff, like most of the people, my first thought to make the animations was [`Manim`](https://github.com/3b1b/manim). Nevertheless, everybody uses it.

# Arrows

Arrow2D class allows you to create an arrow in a super simple way. It has two constructors, you can specify the origin and target position of the arrow or specify the origin, magnitude and angle of the arrow.
It can be used not only to sign something, but also to represent vectors.

# Vector fields

Vector fields are quite a useful in maths and physics, so it is essential to implement that for animations in a easier way to optimize the time.

<div align="center">
  <img src="https://github.com/user-attachments/assets/ff22e958-3f35-4efd-a9b8-04aa2d4d218a" width="40%" />
  <img src="https://github.com/user-attachments/assets/73777cfe-074b-43f9-bc32-354bf85eb5f9" width="40%" />
</div>

# Cartesian plane

Cartesian planes are super important in all possible fields of mathematics, so it was important to make it as easy and customizable as possible.

<img src="https://github.com/user-attachments/assets/e9c5bffc-89b9-48d3-9ace-080fe8168cac" width="70%" />

The constructor counts with several parameters to customize the plane such as:

<ul>
  <li>Depth: Thickness of the axes.</li>
  <li>Magnitude X: How long you want the X axis to be.</li>
  <li>Magnitude Y: How long you want the Y axis to be.</li>
  <li>Distance X: How long is a unit in the X axis.</li>
  <li>Distance Y: How long is a unit in the Y axis.</li>
  <li>Color: Color you want to use for the axes.</li>
  <li>Callback: Optional function that will be automatically executed when the plane is done.</li>
</ul>

Example of code:

```cs
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
            distanceY: 30,
            callback: null,
            color: new Color(1, 1, 1, 1)
        );
    }
}
``` 