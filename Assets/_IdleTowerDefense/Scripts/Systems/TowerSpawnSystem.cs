using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerSpawnSystem : IEcsPreInitSystem, IEcsInitSystem
{
    private EcsWorld world;
    private SharedData sharedData;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }

    public void Init(EcsSystems systems)
    {
        // Create Entity, add components
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Tower> towerPool = world.GetPool<Tower>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();
        EcsPool<TowerTargetSelector> towerTargetingPool = world.GetPool<TowerTargetSelector>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        ref Tower tower = ref towerPool.Add(entity);
        ref TowerWeapon towerWeapon = ref towerWeaponPool.Add(entity);
        ref TowerTargetSelector towerTargetSelector = ref towerTargetingPool.Add(entity);
        ref Health towerHealth = ref healthPool.Add(entity);

        // Setup View
        TowerView towerView = GameObject.Instantiate(sharedData.Settings.TowerView, Vector3.zero, Quaternion.identity);

        // Init components
        towerHealth.MaxHealth = towerView.StartingHealth;
        towerHealth.CurrentHealth = towerView.StartingHealth;
        towerHealth.HealthRegeneration = towerView.StartingHealthRegeneration;
        towerHealth.OnDamaged += () => towerView.transform.DOPunchPosition(Random.insideUnitCircle / 10f, 0.1f, 3, 1, false)
            .OnComplete(() => towerView.transform.position = Vector3.zero);
        towerHealth.OnKilled += () => GameManager.Instance.OnTowerKilled();
            towerWeapon.AttackCooldown = towerView.StartingAttackCooldown;
        towerWeapon.AttackDamage = towerView.StartingAttackDamage;
        towerTargetSelector.TargetingRange = towerView.StartingTargetingRange;
        towerTargetSelector.MaxTargets = towerView.StartingMaxTargets;


        // Init View
        towerView.PackedEntity = packedEntity;
        towerView.World = world;
        sharedData.TowerView = towerView;
    }
}