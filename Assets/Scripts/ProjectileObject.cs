using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    private Vector3 direction;

    private void Awake()
    {
        Destroy(this.gameObject, lifetime);
    }
    private void Update()
    {
        direction.z = 0f;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

}
