using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 10f;

    void Update()
    {
        direction.z = 0f;
        transform.position += direction * speed * Time.deltaTime;
    }
}
