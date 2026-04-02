using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject currentProjectile;

    private Rigidbody projectileRb;
    private Collider projectileCollider;
    private Collider playerCollider;

    [SerializeField] private float projectileStartScale = 0.4f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float growthSpeed = 0.7f;
    [SerializeField] private float minScale = 0.3f;
    [SerializeField] private float shootForce = 15f;
    [SerializeField] private float shootCooldown = 0.5f;
    private float lastShootTime;

    [Header("Movement settings")]
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float forwardForce = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        HandleInput();
        HandleGrowth();
        HandleRelease();
    }

    private void HandleInput()
    {
        if (Time.time - lastShootTime < shootCooldown)
            return;

        if (currentProjectile != null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            currentProjectile = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);

            currentProjectile.transform.localScale = Vector3.one * projectileStartScale;

            projectileRb = currentProjectile.GetComponent<Rigidbody>();
            projectileCollider = currentProjectile.GetComponent<Collider>();

            projectileRb.isKinematic = true;
            projectileCollider.isTrigger = true;

            Physics.IgnoreCollision(projectileCollider, playerCollider);
        }
    }


    private void HandleGrowth()
    {
        if (Input.GetMouseButton(0) && currentProjectile != null)
        {
            currentProjectile.transform.position = shotPoint.position;

            float growth = growthSpeed * Time.deltaTime;

            transform.localScale -= Vector3.one * growth;
            currentProjectile.transform.localScale += Vector3.one * growth;

            if (transform.localScale.x <= minScale)
            {
                GameOver();
            }
        }
    }

 
    private void HandleRelease()
    {
        if (Input.GetMouseButtonUp(0) && currentProjectile != null)
        {
            projectileCollider.isTrigger = false;
            projectileRb.isKinematic = false;

            projectileRb.AddForce(shotPoint.forward * shootForce, ForceMode.Impulse);

            currentProjectile = null;
            lastShootTime = Time.time;
            MoveForward();
        }
    }

    private void MoveForward()
    {
        Vector3 currentVel = rb.linearVelocity;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 jumpVector = (Vector3.up * jumpForce) + (transform.forward * forwardForce);

        rb.AddForce(jumpVector, ForceMode.Impulse);
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}