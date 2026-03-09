using System.Collections;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform leftHand_origin;
    [SerializeField] private Transform rightHand_origin;

    [SerializeField] private GameObject leftHand_attack_prefab;
    [SerializeField] private GameObject rightHand_attack_prefab;

    [SerializeField] private float firingCooldown = 0.5f;

    private bool canFire = true;
    private float firingTimer = 0f;
    private Transform targetPosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canFire)
        {
            GetClickPosition(out Vector3 direction);
            ProjectileObject projectile = CreateProjectilePrefab(leftHand_attack_prefab, leftHand_origin.position, direction);    

            if (projectile != null)
            {
                projectile.direction = direction;

                Vector3 pos = projectile.transform.position;
                pos.z = 0f;
                projectile.transform.position = pos;
            }

            StartCoroutine(FiringCooldown(firingCooldown));
        }

        if (Input.GetMouseButtonDown(1))
        {
            GetClickPosition(out Vector3 direction);
            ProjectileObject projectile = CreateProjectilePrefab(rightHand_attack_prefab, rightHand_origin.position, direction);

            if (projectile != null)
            {
                projectile.direction = direction;

                Vector3 pos = projectile.transform.position;
                pos.z = 0f;
                projectile.transform.position = pos;
            }

            StartCoroutine(FiringCooldown(firingCooldown));
        }
    }

    private ProjectileObject CreateProjectilePrefab(GameObject prefab, Vector3 origin, Vector3 _direction)
    {
        GameObject attackPrefab = Instantiate(
            prefab,
            origin,
            Quaternion.LookRotation(Vector3.forward, _direction)
            );

        ProjectileObject projectile = attackPrefab.GetComponent<ProjectileObject>();

        return projectile;
    }
    private IEnumerator FiringCooldown(float _firingCooldown)
    {
        firingTimer = _firingCooldown;
        canFire = false;

        while(firingTimer > 0f)
        {
            firingTimer -= Time.deltaTime;
            yield return null;
        }

        firingTimer = 0f;
        canFire = true;
    }

    private void GetClickPosition(out Vector3 _direction)
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = transform.position.z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector3 direction = (worldPosition - transform.position).normalized;

        _direction = direction;
    }
}
