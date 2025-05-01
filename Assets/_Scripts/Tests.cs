using UnityEngine;
using Physics.Arrow;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private void Start()
    {
        Arrow2D arrow = new Arrow2D(
            originPoint: new Vector2(0, 0),
            magnitude: 20, 
            theta: 45, 
            width: 10
        );

        arrow.SetColor(Color.red);
    }

    private void Update()
    {
        
    }
}
