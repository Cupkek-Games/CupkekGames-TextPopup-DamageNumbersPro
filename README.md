# CupkekGames TextPopup — DamageNumbersPro Bridge

Concrete backend for [CupkekGames.TextPopup](https://github.com/Cupkek-Games/CupkekGames-TextPopup) on top of the [DamageNumbersPro](https://assetstore.unity.com/packages/tools/gui/damage-numbers-pro-150862) asset. Drop in a `DamageNumberManager` MonoBehaviour and the `IPopupManager` interface resolves through `ServiceLocator`.

## What's inside

**Runtime** (`CupkekGames.TextPopup.DamageNumbersPro.asmdef`)

- `DamageNumberManager` — MonoBehaviour implementation of `IPopupManager`. Holds a `List<PopupKindEntry>` mapping designer-defined kind strings to `DamageNumber` prefabs (with optional default left-text and crit-prefix per entry), and forwards `Show(kind, …)` calls to DamageNumbersPro.

## Dependencies

- `com.cupkekgames.textpopup` (UPM)
- DamageNumbersPro Asset Store package (project-level — bring your own; bridge will not compile without it)
