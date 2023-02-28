using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemySpawnSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private float spawnTimeRemaining = 0;
    private EcsWorld world;

    public void PreInit(EcsSystems systems)
    {
        // Will be called once during EcsSystems.Init() call and before IEcsInitSystem.Init().
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }

    public void Run(EcsSystems systems)
    {
        spawnTimeRemaining -= Time.deltaTime;
        if (spawnTimeRemaining <= 0)
        {
            SpawnEnemy();
            spawnTimeRemaining = sharedData.Settings.EnemySpawnDelay;
        }
    }

    private void SpawnEnemy()
    {
        // Create Entity, add components
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Enemy> enemyPool = world.GetPool<Enemy>();
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        ref Enemy enemy = ref enemyPool.Add(entity);
        ref Position position = ref positionPool.Add(entity);
        ref Movement movement = ref movementPool.Add(entity);

        // Setup View
        GameObject enemyViewGameObject = GameObject.Instantiate(sharedData.Settings.EnemyPrefab);
        EnemyView enemyView = enemyViewGameObject.AddComponent<EnemyView>();
        enemyView.packedEntity = packedEntity;
        enemyView.world = world;

        // Give Entity a random starting position
        Vector2 randomPosition = Random.insideUnitCircle.normalized * sharedData.Settings.SpawnRadius;

        // Init Components
        position.x = randomPosition.x;
        position.y = randomPosition.y;

        movement.Speed = 1;
        movement.StopRadius = 3;
        
        // Init View
        enemyView.transform.position = randomPosition;
    }
}