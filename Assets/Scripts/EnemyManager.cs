using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public static event Action OnEnemyDestroyed;

    public GameObject enemyPrefab;
    public Transform[] spawnPositions;
    public float moveSpeed = 1f;
    public float descentAmount = 0.5f;
    public float speedIncreaseFactor = 1.2f;

    private List<Enemy> enemies = new List<Enemy>();
    private float direction = 1f;

    void Start()
    {
        SpawnEnemies();
        Enemy.OnEnemyDied += HandleEnemyKilled;
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= HandleEnemyKilled;
    }

    void SpawnEnemies()
    {
        foreach (Transform pos in spawnPositions)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, pos.position, Quaternion.identity);
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
    }
    
    void update() 
    {
        MoveEnemies();
    }

    void MoveEnemies()
    {
        Debug.Log("moving enemies");
        float maxX = float.MinValue;
        float minX = float.MaxValue;

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Vector3 pos = enemy.transform.position;
                pos.x += moveSpeed * direction * Time.deltaTime;
                enemy.transform.position = pos;

                maxX = Mathf.Max(maxX, pos.x);
                minX = Mathf.Min(minX, pos.x);
            }
        }

        if (maxX >= 7 || minX <= -7)
        {
            direction *= -1;
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.transform.position += new Vector3(0, -descentAmount, 0);
                }
            }
        }
    }

    void HandleEnemyKilled(int points)
    {
        moveSpeed *= speedIncreaseFactor;
        OnEnemyDestroyed?.Invoke();
    }
}

