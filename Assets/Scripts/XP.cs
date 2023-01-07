using UnityEngine;

public class XP : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane2"))
        {
            Destroy(gameObject);
        }
    }
}