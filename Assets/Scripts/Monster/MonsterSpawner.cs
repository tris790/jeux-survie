using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int spawnInterval = 5;
    public int spawnLimit = 20;
    public int despawnRadius = 25;

    private List<GameObject> _monsters = new List<GameObject>(100);
    private Player _playerEntity;

    void Start()
    {
        InvokeRepeating(nameof(SpawningLoop), 1, spawnInterval);
        _playerEntity = GameManager.Instance.player.GetComponent<Player>();
    }

    private void SpawningLoop()
    {
        var monstersToDespawn = _monsters
            .Where(x =>
            {
                var monsterPosition = x.GetComponent<MonsterMovementComponent>().Position;
                return !x.activeInHierarchy || Vector2.Distance(_playerEntity.Position, monsterPosition) > despawnRadius;
            })
            .ToList();

        foreach (var monster in monstersToDespawn)
        {
            monster.SetActive(false);
            _monsters.Remove(monster);
        }

        var playerPosition = GameManager.Instance.player.GetComponent<Player>().Position;

        var howManyMonstersToSpawn = spawnLimit - _monsters.Count();

        var spawnPositions = Enumerable
            .Range(0, howManyMonstersToSpawn)
            .Select(x => new Vector2(playerPosition.x + Random.Range(-20, 20), playerPosition.y - Random.Range(-20, 20)))
            .Where(x => Vector2.Distance(playerPosition, x) > 5)
            .ToList();

        var actualSpawnCount = Mathf.Min(howManyMonstersToSpawn, spawnPositions.Count);
        Debug.Log($"Trying to spawn {actualSpawnCount} monsters");
        for (int i = 0; i < actualSpawnCount; i++)
        {
            var monster = ObjectPoolManager.Instance.GetNextPooledObjectByTag("Enemy");
            if (monster == null)
            {
                Debug.Log($"Couldn't spawn a monster, {i}/{actualSpawnCount}");
                break;
            }

            monster.GetComponent<MonsterAI>().Initialize();
            monster.GetComponent<MonsterMovementComponent>().TeleportTo(spawnPositions[i]);
            _monsters.Add(monster);
            monster.SetActive(true);
        }
    }
}
