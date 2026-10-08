# CupkekGames TextPopup — DamageNumbersPro Bridge

Concrete backend for [CupkekGames.TextPopup](https://github.com/Cupkek-Games/CupkekGames-TextPopup) on top of the [DamageNumbersPro](https://assetstore.unity.com/packages/tools/gui/damage-numbers-pro-150862) asset. Drop in a `DamageNumberManager` MonoBehaviour and the `IPopupManager` interface resolves through `ServiceLocator`.

## What's inside

**Runtime** (`CupkekGames.TextPopup.DamageNumbersPro.asmdef`)

- `DamageNumberManager` — MonoBehaviour implementation of `IPopupManager`. Holds a `List<PopupKindEntry>` mapping designer-defined kind strings to `DamageNumber` prefabs, each with an optional crit popup (best a prefab variant of the kind's prefab, so it keeps its look and changes only size and motion), and forwards `Show(kind, …)` calls to DamageNumbersPro. A prefab's own left text shows; a `TextPopupContext` replaces it for one call. Every spawn is tinted with the `DamagePopupContext` colour, or white: keep a prefab's own colours in its vertex gradient.

## Dependencies

- `com.cupkekgames.textpopup` (UPM)
- DamageNumbersPro Asset Store package (project-level — bring your own; bridge will not compile without it)
