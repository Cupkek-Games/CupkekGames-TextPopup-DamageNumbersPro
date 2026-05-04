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
            public DamageNumber Prefab;

            [Tooltip("Default left-text for this kind (e.g. '-' for damage). Used when no TextPopupContext.LeftText is provided.")]
            public string DefaultLeftText = "";

            [Tooltip("Optional prefix prepended to left-text when DamagePopupContext.IsCrit is true (e.g. '★').")]
            public string CritPrefix = "";
        }

        [Header("Damage Numbers")]
        [SerializeField] private Vector3 _offset = new Vector3(0, 4, 0);

        [Header("Popup kinds (kind, prefab, optional left-text + crit prefix)")]
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
            if (!_map.TryGetValue(kind, out PopupKindEntry entry) || entry?.Prefab == null) return;

            Vector3 position = center + _offset;
            DamageNumber damageNumber = entry.Prefab.Spawn(position, value);
            damageNumber.scaleByNumberSettings.toNumber = _scaleMaxValue;

            string leftText = ResolveLeftText(entry, context);
            if (leftText != null)
                damageNumber.leftText = leftText;
        }

        private static string ResolveLeftText(PopupKindEntry entry, IPopupContext context)
        {
            switch (context)
            {
                case TextPopupContext text:
                    return text.LeftText;
                case DamagePopupContext damage when damage.IsCrit:
                    return entry.CritPrefix + entry.DefaultLeftText;
                default:
                    return string.IsNullOrEmpty(entry.DefaultLeftText) ? null : entry.DefaultLeftText;
            }
        }
    }
}
