using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int spawnInterval = 5;
    public int spawnLimit = 20;
    public int despawnRadius = 100;

    private List<GameObject> _monsters = new List<GameObject>(100);
    private Player _playerEntity;

    void Start()
    {
        InvokeRepeating(nameof(SpawningLoop), spawnInterval, spawnInterval);
        _playerEntity = GameManager.Instance.player.GetComponent<Player>();
    }

    private void SpawningLoop()
    {
        var monstersToDespawn = _monsters
            .Where(x =>
            {
                var monsterPosition = x.GetComponent<MonsterMovementComponent>().Position;
                return Vector2.Distance(_playerEntity.Position, monsterPosition) > despawnRadius;
            })
            .ToList();

        foreach (var monster in monstersToDespawn)
        {
            monster.SetActive(false);
            _monsters.Remove(monster);
        }


        var howManyMonstersToSpawn = spawnLimit - _monsters.Count();
        // TODO: Tris caller le poisson
        var spawnPositions = new List<Vector2> { new Vector2(0, 0) };
        for (int i = 0; i < howManyMonstersToSpawn; i++)
        {
            var monster = ObjectPoolManager.Instance.GetNextPooledObjectByTag("Enemy");
            if (monster == null)
                break;

            monster.GetComponent<MonsterAI>().Initialize();
            monster.GetComponent<MonsterMovementComponent>().TeleportTo(spawnPositions[0]);
            _monsters.Add(monster);
            monster.SetActive(true);
        }
    }
}
