using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Components")]
  
    public Rigidbody rigibody;
    public Collider collider;

    [Header("Explosion Settings")]
    [SerializeField] private float baseRadiusMultiplier = 1.3f;

    private bool isExploded = false;

    private void Start()
    {
      
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isExploded) return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Plane"))
        {
            return;
        }

      
        Explode();
    }

    private void Explode()
    {
        isExploded = true;

     
        float dynamicRadius = transform.localScale.x * baseRadiusMultiplier;

      
        Collider[] obstacles = Physics.OverlapSphere(transform.position, dynamicRadius);

        foreach (Collider obstacleCollider in obstacles)
        {
            if (obstacleCollider.CompareTag("Obstacle"))
            {
                Renderer rend = obstacleCollider.GetComponent<Renderer>();
                if (rend != null)
                {
                   
                    rend.material.color = Color.red;
                }
    
                Destroy(obstacleCollider.gameObject, 0.2f);
            }
        }

      
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * baseRadiusMultiplier);
    }
}