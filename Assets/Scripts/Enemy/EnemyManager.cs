using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemies;

    [SerializeField] private Enemy enemyPrefab;

    public delegate void OnEnemiesEmpty();
    public OnEnemiesEmpty onEnemiesEmpty;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetEnemyList()
    {
        enemies.Clear();
    }


    public void SpawnEnemies(List<EnemySpawn> spawnPoints)
    {
        foreach(EnemySpawn p in spawnPoints)
        {
            Enemy e = Instantiate(enemyPrefab);
            e.onDeath += OnEnemyDeath;
            e.Init(p.pos, p.moves > 0 ? p.moves : 3);
            enemies.Add(e);
        }
    }

    public void MoveAllEnemies()
    {
        foreach(Enemy e in enemies)
        {
            e.Move();
        }
    }

    public void ClearEnemies()
    {
        foreach (Enemy e in enemies)
        {
            Destroy(e.gameObject);
        }
        enemies.Clear();
    }

    private void OnEnemyDeath(Enemy d)
    {
        enemies.Remove(d);
        if(enemies.Count == 0)
        {
            onEnemiesEmpty?.Invoke();
        }
    }
}
