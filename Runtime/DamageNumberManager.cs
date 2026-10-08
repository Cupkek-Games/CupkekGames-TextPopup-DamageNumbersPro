using System;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using CupkekGames.TextPopup;

namespace CupkekGames.TextPopup.DamageNumbersPro
{
    public class DamageNumberManager : MonoBehaviour, IPopupManager
    {
        [Serializable]
        public class PopupKindEntry
        {
            public string Kind;

            [Tooltip("The kind's popup. Its own left text shows (a TextPopupContext replaces it for one call).")]
            public DamageNumber Prefab;

            [Tooltip("A critical hit's popup (best a variant of the prefab, so it keeps the kind's look); empty uses the prefab.")]
            public DamageNumber CritPrefab;

            [Tooltip("The killing blow's popup (best a variant of the prefab); empty falls back to the crit's or the prefab.")]
            public DamageNumber KillPrefab;

            [Tooltip("The killing blow's top text when it was an overkill; empty keeps the kill popup's own top text.")]
            public string OverkillTopText = "";
        }

        [Header("Damage Numbers")]
        [SerializeField] private Vector3 _offset = new Vector3(0, 4, 0);

        [Header("Popup kinds (kind, prefab, optional crit and kill variants)")]
        [SerializeField] private List<PopupKindEntry> _entries = new List<PopupKindEntry>();

        private readonly Dictionary<string, PopupKindEntry> _map = new Dictionary<string, PopupKindEntry>();

        // Runtime damage scaling (set by combat manager when max damage is known)
        private float _scaleMaxValue = 1000f;
        private const float ScaleMaxValueMultiplier = 3f;

        private void Awake()
        {
            foreach (PopupKindEntry entry in _entries)
            {
                if (string.IsNullOrEmpty(entry.Kind) || entry.Prefab == null)
                    continue;
                entry.Prefab.PrewarmPool();
                if (entry.CritPrefab != null) entry.CritPrefab.PrewarmPool();
                if (entry.KillPrefab != null) entry.KillPrefab.PrewarmPool();
                _map[entry.Kind] = entry;
            }
        }

        /// <summary>
        /// Damage popups scale their visual size by value relative to a max — set the max
        /// once max enemy HP is known so visuals don't clip on big hits.
        /// </summary>
        public void SetScaleMaxValue(float maxValue)
        {
            _scaleMaxValue = maxValue * ScaleMaxValueMultiplier;
        }

        public void Show(string kind, Vector3 center, int value = 0, IPopupContext context = null)
        {
            if (string.IsNullOrEmpty(kind)) return;
            if (!_map.TryGetValue(kind, out PopupKindEntry entry))
            {
                Debug.LogError($"[DamageNumberManager] No popup entry for kind '{kind}'. Add it to the entries list.", this);
                return;
            }

            DamagePopupContext damage = context as DamagePopupContext;
            DamageNumber prefab = PrefabFor(entry, damage);

            Vector3 position = center + _offset;
            // Spawn(position, number) switches the number on; a text-only prefab
            // (enableNumber off) keeps it off so no stray value trails the text.
            DamageNumber damageNumber = prefab.enableNumber
                ? prefab.Spawn(position, value)
                : prefab.Spawn(position);
            damageNumber.scaleByNumberSettings.toNumber = _scaleMaxValue;

            // A pooled popup keeps what the last spawn set, so a kill's top text is set every time.
            if (prefab == entry.KillPrefab)
            {
                damageNumber.topText = damage != null && damage.IsOverkill && !string.IsNullOrEmpty(entry.OverkillTopText)
                    ? entry.OverkillTopText
                    : prefab.topText;
            }

            if (context is TextPopupContext text && text.LeftText != null)
                damageNumber.leftText = text.LeftText;
        }

        /// <summary>The popup a call shows: the kill's for a killing blow, the crit's for a crit, else the kind's own.</summary>
        public static DamageNumber PrefabFor(PopupKindEntry entry, DamagePopupContext damage)
        {
            if (damage != null && damage.IsKill && entry.KillPrefab != null) return entry.KillPrefab;
            if (damage != null && damage.IsCrit && entry.CritPrefab != null) return entry.CritPrefab;
            return entry.Prefab;
        }
    }
}
