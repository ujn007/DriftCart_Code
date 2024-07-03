using UnityEngine;

public class GroundCaster : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 size;

    public bool CheckGround()
    {
        return Physics.CheckBox(transform.position, size, Quaternion.identity, whatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
