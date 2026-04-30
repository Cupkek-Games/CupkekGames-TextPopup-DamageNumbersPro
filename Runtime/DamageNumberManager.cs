using UnityEngine;
using DamageNumbersPro;
using CupkekGames.TextPopup;


namespace CupkekGames.TextPopup.DamageNumbersPro
{
  public class DamageNumberManager : MonoBehaviour, IDamagePopup, IHealPopup, IStatusPopup, INumberPopupManager
  {
    [Header("Damage Numbers")]
    [SerializeField]
    private Vector3 _damageNumberOffset = new Vector3(0, 4, 0);

    [SerializeField] private DamageNumber _prefabDamage;
    [SerializeField] private string _critPrefix = "<sprite name=\"Damage\">";
    [SerializeField] private DamageNumber _prefabDamageWeak;
    [SerializeField] private string _critPrefixWeak = "<sprite name=\"DamageWeak\">";
    [SerializeField] private DamageNumber _prefabDamageStrong;
    [SerializeField] private string _critPrefixStrong = "<sprite name=\"DamageStrong\">";
    [SerializeField] private DamageNumber _prefabStatusEffectPositive;
    [SerializeField] private DamageNumber _prefabStatusEffectNegative;
    [SerializeField] private DamageNumber _prefabHeal;
    [SerializeField] private DamageNumber _prefabShield;
    [SerializeField] private DamageNumber _prefabPoison;
    [SerializeField] private DamageNumber _prefabBleed;
    [SerializeField] private DamageNumber _prefabMoodPositive;

    [SerializeField] private DamageNumber _prefabMoodNegative;

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
      _prefabPoison.PrewarmPool();
      _prefabBleed.PrewarmPool();
      _prefabMoodPositive.PrewarmPool();
      _prefabMoodNegative.PrewarmPool();
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

    public void ShowPoison(Vector3 center, int value)
    {
      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber = _prefabPoison.Spawn(position, value);
      damageNumber.scaleByNumberSettings.toNumber = _scaleMaxDamage;
    }

    public void ShowBleed(Vector3 center, int value)
    {
      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber = _prefabBleed.Spawn(position, value);
      damageNumber.scaleByNumberSettings.toNumber = _scaleMaxDamage;
    }

    public void ShowMood(Vector3 center, bool positive, int value)
    {
      Vector3 position = center + _damageNumberOffset;
      DamageNumber damageNumber;
      if (positive)
      {
        damageNumber = _prefabMoodPositive.Spawn(position, value);
      }
      else
      {
        damageNumber = _prefabMoodNegative.Spawn(position, value);
      }
    }
  }
}
