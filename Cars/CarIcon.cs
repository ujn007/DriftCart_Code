using UnityEngine;

public class CarIcon : MonoBehaviour
{
    [SerializeField] private Vector3 objVector;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(objVector);   
    }
}
