using UnityEngine;

public class FinishDoor : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float openDistance = 5f;
    [SerializeField] float openSpeed = 2f;
    [SerializeField] float lowerLimit = 1.5f;

    private bool shouldOpen = false;

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < openDistance)
        {
            shouldOpen = true;
        }

        if(shouldOpen)
        {
            Vector3 targetPos = new Vector3(transform.position.x, lowerLimit, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, downSpeed * Time.deltaTime);
        }
    }
}
