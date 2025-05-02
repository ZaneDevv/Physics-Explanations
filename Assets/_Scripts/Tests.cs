using UnityEngine;
using Physics.Arrow;

[DisallowMultipleComponent] internal sealed class Tests : MonoBehaviour
{
    private void Start()
    {
        Arrow2D arrow0 = new Arrow2D(
            originPoint: new Vector2(0, 0),
            magnitude: 20, 
            width: 10,
            theta: 45 
        );

        arrow0.SetColor(Color.green);

        Arrow2D arrow1 = new Arrow2D(Vector2.zero, Vector2.right * 200, 3);
        arrow1.SetColor(Color.red);
    }

    private void Update()
    {
        
    }
}
