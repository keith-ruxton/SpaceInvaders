using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDied(int points);
    public static event EnemyDied OnEnemyDied;
    
    public delegate void EnemyFired();
    public static event EnemyFired OnEnemyFired;
    
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 3f;
    public int health = 1;

    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = fireRate;
        }
    }

    void Fire()
    {
        Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        OnEnemyFired?.Invoke();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            OnEnemyDied?.Invoke(3);
            Destroy(gameObject);
        }
    }
}


