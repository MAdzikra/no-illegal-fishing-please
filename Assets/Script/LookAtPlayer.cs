using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player != null)
        {
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
    }
}
