using UnityEngine;

public class CarIcon : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Quaternion.Euler(90, -90, 0);
    }
}
