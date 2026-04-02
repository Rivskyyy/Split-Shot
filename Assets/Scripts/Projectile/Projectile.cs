using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float explosionRadius;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Plane"))
        {
            return;
        }
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Explode();
        }
        
    }

   private void Explode()
    {

        float dynamicRadius = transform.localScale.x * 1.3f;

        Collider[] obstacles = Physics.OverlapSphere(transform.position, dynamicRadius);

        foreach (Collider col in obstacles)
        {
            Renderer rend = col.GetComponent<Renderer>();
            if (col.CompareTag("Obstacle"))
            {
                if(rend != null)
                {
                    rend.material.color = Color.red;
                }
                Destroy(col.gameObject, 0.2f);
            }
        }

        Destroy(gameObject);
    }



}
