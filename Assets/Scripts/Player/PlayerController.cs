using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject currentProjectile;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float growthSpeed = 0.5f;
    [SerializeField] private float minScale = 0.2f;

    [Header("Movement settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float forwardForce = 15; // Трохи збільшив для впевненого руху вперед


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. Створюємо снаряд
        if (Input.GetMouseButtonDown(0))
        {
            currentProjectile = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation);

            // НЕ використовуємо SetParent, щоб уникнути конфліктів масштабу (Scale)
            Rigidbody projRb = currentProjectile.GetComponent<Rigidbody>();
            projRb.isKinematic = true;

            // Робимо тригером на час росту, щоб гравець не врізався в нього як у стіну
            currentProjectile.GetComponent<Collider>().isTrigger = true;
        }

        // 2. Снаряд росте, гравець худне, снаряд слідує за точкою пострілу
        if (Input.GetMouseButton(0) && currentProjectile != null)
        {
            // Примусово тримаємо снаряд у shotPoint (імітуємо SetParent без його мінусів)
            currentProjectile.transform.position = shotPoint.position;

            float growth = growthSpeed * Time.deltaTime;
            transform.localScale -= Vector3.one * growth;
            currentProjectile.transform.localScale += Vector3.one * growth;

            // Постійно ігноруємо колізію (про всяк випадок)
            Physics.IgnoreCollision(currentProjectile.GetComponent<Collider>(), GetComponent<Collider>());

            if (transform.localScale.x <= minScale)
            {
                Debug.Log("Game Over!");
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }

        // 3. Постріл та стрибок
        if (Input.GetMouseButtonUp(0) && currentProjectile != null)
        {
            // Залізобетонне ігнорування перед вмиканням фізики
            Physics.IgnoreCollision(currentProjectile.GetComponent<Collider>(), GetComponent<Collider>());

            currentProjectile.GetComponent<Collider>().isTrigger = false;
            Rigidbody projectileRb = currentProjectile.GetComponent<Rigidbody>();
            projectileRb.isKinematic = false;

            // Використовуємо shotPoint.forward для напрямку пострілу
            projectileRb.AddForce(shotPoint.forward * 20f, ForceMode.Impulse);

            currentProjectile = null;

            // Викликаємо стрибок
            MoveForward();
        }
    }

    private void MoveForward()
    {
        // Замість повного zero, давай скинемо тільки горизонтальну швидкість (X та Z)
        // Це дозволить кулі зберегти інерцію стрибка, якщо ти клікаєш часто
        Vector3 currentVel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0, currentVel.y, 0);
        rb.angularVelocity = Vector3.zero;

        // Робимо стрибок ВГОРУ сильнішим за рух ВПЕРЕД для ефекту арки
        // Спробуй jumpForce = 8, forwardForce = 5 в інспекторі
        Vector3 jumpVector = (Vector3.up * jumpForce) + (transform.forward * forwardForce);

        // Використовуємо Impulse для фізичного "поштовху"
        rb.AddForce(jumpVector, ForceMode.Impulse);

        Debug.Log("Jump! Power: " + jumpVector);
    }
}