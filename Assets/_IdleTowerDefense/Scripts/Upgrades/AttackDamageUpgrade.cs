using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Damage Upgrade", menuName = "Idle Tower Defense/Upgrades/Attack Damage")]
public class AttackDamageUpgrade : UpgradeBase
{
    private EcsFilter weaponFilter;

    public override void Init()
    {
        weaponFilter = GameManager.Instance.World.Filter<TowerWeapon>().End();
    }

    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Gold, UpgradeManager.Instance.UpgradeCounts[Title]+1
            }
        };
    }

    public override bool CanUpgrade()
    {
        return GameManager.Instance.Currency.HasAtLeast(GetCost());
    }

    public override void Upgrade()
    {
        Debug.Log($"{nameof(AttackDamageUpgrade)}.{nameof(Upgrade)}() - Upgrading!");
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());
        UpgradeManager.Instance.UpgradeCounts[Title] += 1;

        // Handle upgrade
        EcsPool<TowerWeapon> weaponPool = GameManager.Instance.World.GetPool<TowerWeapon>();
        foreach (int entity in weaponFilter)
        {
            ref TowerWeapon towerWeapon = ref weaponPool.Get(entity);
            towerWeapon.AttackDamage += 1;
        }
    }
}