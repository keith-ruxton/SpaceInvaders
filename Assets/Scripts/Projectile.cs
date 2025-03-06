using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float speed = 20f;
    public System.Action destroyed;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (destroyed != null)
        {
            destroyed.Invoke();
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        DestroyProjectile(); 
    }
}
