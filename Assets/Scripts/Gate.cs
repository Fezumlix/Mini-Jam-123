using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane2"))
        {
            Destroy(gameObject);
        }
    }
}