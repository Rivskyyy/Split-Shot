using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shotPoint;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(shotPoint.forward * 20f, ForceMode.Impulse);
            }
        }
    }
}
