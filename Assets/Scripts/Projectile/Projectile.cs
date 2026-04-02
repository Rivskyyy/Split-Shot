using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;

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
   
        float dynamicRadius = transform.localScale.x * 2f;

        // ¬¿∆À»¬Œ: œÂÂ‰‡∫ÏÓ Ò‡ÏÂ dynamicRadius Û OverlapSphere
        Collider[] obstacles = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in obstacles)
        {
            Renderer rend = col.GetComponent<Renderer>();
            if (col.CompareTag("Obstacle"))
            {
                if(rend != null)
                {
                    rend.material.color = Color.red;
                }
                Destroy(col.gameObject, 0.3f);
            }
        }

        Destroy(gameObject);
    }



}
