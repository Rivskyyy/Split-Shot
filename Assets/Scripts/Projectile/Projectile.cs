using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] victims = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider victim in victims)
        {
            if (victim.CompareTag("Obstacle"))
            {
                
                Destroy(victim.gameObject);
            }
        }

    }



}
