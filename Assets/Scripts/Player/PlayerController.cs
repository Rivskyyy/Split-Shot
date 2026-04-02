using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject currentProjectile;
    private Rigidbody projectileRb;
    private Collider projectileCollider;
    private Collider playerCollider;

    [Header("Projectile Settings")]
    [SerializeField] private float projectileStartScale = 0.4f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float growthSpeed = 0.7f;
    [SerializeField] private float minScale = 0.3f;
    [SerializeField] private float shootForce = 15f;
    [SerializeField] private float shootCooldown = 0.5f;
    private float lastShootTime;

    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float forwardForce = 3f;


    private bool isHolding = false;
    private bool isReleased = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    private void Update()
    {
      
        HandleInput();
    }

    private void FixedUpdate()
    {
       
        if (isHolding) HandleGrowth();
        if (isReleased) HandleRelease();
    }

    private void HandleInput()
    {
        if (Time.time - lastShootTime < shootCooldown) return;
        if (currentProjectile != null && !isHolding) return;

       
        if (Input.GetMouseButtonDown(0) && currentProjectile == null)
        {
            currentProjectile = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);
            Projectile projScript = currentProjectile.GetComponent<Projectile>();

            if (projScript != null)
            {
                projectileRb = projScript.rigibody;
                projectileCollider = projScript.collider;

                currentProjectile.transform.localScale = Vector3.one * projectileStartScale;
                projectileRb.isKinematic = true;
                projectileCollider.isTrigger = true;

                Physics.IgnoreCollision(projectileCollider, playerCollider);

                isHolding = true;
                isReleased = false;
            }
        }

       
        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            isHolding = false; 
            isReleased = true; 
        }
    }

    private void HandleGrowth()
    {
        if (currentProjectile != null)
        {
            currentProjectile.transform.position = shotPoint.position;

          
            float growth = growthSpeed * Time.fixedDeltaTime;

            transform.localScale -= Vector3.one * growth;
            currentProjectile.transform.localScale += Vector3.one * growth;

            if (transform.localScale.x <= minScale)
            {
                GameManager.Instance.ShowGameOver();
                this.enabled = false;
            }
        }
    }

    private void HandleRelease()
    {
        if (currentProjectile != null)
        {
            projectileCollider.isTrigger = false;
            projectileRb.isKinematic = false;
            projectileRb.AddForce(shotPoint.forward * shootForce, ForceMode.Impulse);

            currentProjectile = null;
            lastShootTime = Time.time;

            MoveForward();
        }
        isReleased = false; 
    }

    private void MoveForward()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 jumpVector = (Vector3.up * jumpForce) + (transform.forward * forwardForce);
        rb.AddForce(jumpVector, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.ShowWin();
            this.enabled = false;
        }
    }
}