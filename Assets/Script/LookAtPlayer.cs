using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform playerHead;

    void Start()
    {
        playerHead = Camera.main.transform;
    }

    void Update()
    {
        if (playerHead != null)
        {
            Vector3 direction = playerHead.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
