using System;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using CupkekGames.TextPopup;


namespace CupkekGames.TextPopup.DamageNumbersPro
{
  public class DamageNumberManager : MonoBehaviour, IDamagePopup, IHealPopup, IStatusPopup, INumberPopupManager
  {
    [Serializable]
    public struct EffectPrefabEntry
    {
      public string Kind;
      public DamageNumber Prefab;
    }

    [Header("Damage Numbers")]
    [SerializeField]
    private Vector3 _damageNumberOffset = new Vector3(0, 4, 0);

    [SerializeField] private DamageNumber _prefabDamage;
    [SerializeField] private string _critPrefix = "";
    [SerializeField] private DamageNumber _prefabDamageWeak;
    [SerializeField] private string _critPrefixWeak = "";
    [SerializeField] private DamageNumber _prefabDamageStrong;
    [SerializeField] private string _critPrefixStrong = "";
    [SerializeField] private DamageNumber _prefabStatusEffectPositive;
    [SerializeField] private DamageNumber _prefabStatusEffectNegative;
    [SerializeField] private DamageNumber _prefabHeal;
    [SerializeField] private DamageNumber _prefabShield;

    [Header("Caller-defined effect prefabs (lookup by kind string)")]
    [SerializeField] private List<EffectPrefabEntry> _effectPrefabs = new List<EffectPrefabEntry>();

    private readonly Dictionary<string, DamageNumber> _effectMap = new Dictionary<string, DamageNumber>();

    // Runtime settings
    private float _scaleMaxDamage = 1000f;
    private const float _scaleMaxDamageMultiplier = 3f;

    private void Awake()
    {
      _prefabDamage.PrewarmPool();
      _prefabDamageWeak.PrewarmPool();
      _prefabDamageStrong.PrewarmPool();
      _prefabStatusEffectPositive.PrewarmPool();
      _prefabStatusEffectNegative.PrewarmPool();
      _prefabHeal.PrewarmPool();
      _prefabShield.PrewarmPool();

      foreach (EffectPrefabEntry entry in _effectPrefabs)
      {
        if (string.IsNullOrEmpty(entry.Kind) || entry.Prefab == null) continue;
        entry.Prefab.PrewarmPool();
        _effectMap[entry.Kind] = entry.Prefab;
      }
    }

    public void SetScaleMaxDamage(float maxDamage)
    {
      _scaleMaxDamage = maxDamage * _scaleMaxDamageMultiplier;
      Debug.Log($"Set DamageNumber scale max damage to {_scaleMaxDamage}");
    }

    public void ShowDamage(Vector3 center, int value, float elementMultiplier, bool isCrit)
    {
      Vector3 position = center + _damageNumberOffset;

      DamageNumber damageNumber;

      if (elementMultiplier < 1f)
      {
        damageNumber = _prefabDamageWeak.Spawn(position, value);
      }
      else if (elementMultiplier > 1f)
      {
        damageNumber = _prefabDamageStrong.Spawn(position, value);
      }
      else
      {
        damageNumber = _prefabDamage.Spawn(position, value);
      }

      damageNumber.scaleByNumberSettings.toNumber = _scaleMaxDamage;

      if (isCrit)
      {
        if (elementMultiplier < 1f)
        {
          damageNumber.leftText = _critPrefixWeak + "-";
        }
        else if (elementMultiplier > 1f)
        {
          damageNumber.leftText = _critPrefixStrong + "-";
        }
        else
        {
          damageNumber.leftText = _critPrefix + "-";
        }
      }
      else
      {
        damageNumber.leftText = "-";
      }
    }

    public void ShowStatusEffect(Vector3 center, bool positive, string leftText)
    {
      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber;
      if (positive)
      {
        damageNumber = _prefabStatusEffectPositive.Spawn(position);
      }
      else
      {
        damageNumber = _prefabStatusEffectNegative.Spawn(position);
      }

      damageNumber.leftText = leftText;
    }

    public void ShowHeal(Vector3 center, int value)
    {
      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber = _prefabHeal.Spawn(position, value);
      damageNumber.scaleByNumberSettings.toNumber = _scaleMaxDamage;
    }

    public void ShowShield(Vector3 center, int value)
    {
      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber = _prefabShield.Spawn(position, value);
      damageNumber.scaleByNumberSettings.toNumber = _scaleMaxDamage;
    }

    public void ShowEffect(Vector3 center, string kind, int value)
    {
      if (string.IsNullOrEmpty(kind)) return;
      if (!_effectMap.TryGetValue(kind, out DamageNumber prefab) || prefab == null) return;

      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber = prefab.Spawn(position, value);
      damageNumber.scaleByNumberSettings.toNumber = _scaleMaxDamage;
    }
  }
}
