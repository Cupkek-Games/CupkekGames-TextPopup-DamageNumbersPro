# CupkekGames TextPopup — DamageNumbersPro Bridge

Concrete backend for [CupkekGames.TextPopup](https://github.com/Cupkek-Games/CupkekGames-TextPopup) on top of the [DamageNumbersPro](https://assetstore.unity.com/packages/tools/gui/damage-numbers-pro-150862) asset. Drop in a `DamageNumberManager` MonoBehaviour and the TextPopup interfaces (`IDamagePopup` / `IHealPopup` / `IStatusPopup` / `INumberPopupManager`) resolve through `ServiceLocator`.

## What's inside

**Runtime** (`CupkekGames.TextPopup.DamageNumbersPro.asmdef`)

- `DamageNumberManager` — single MonoBehaviour that implements all four TextPopup popup interfaces; holds prefab references for damage/heal/status numbers and forwards spawn calls to `DamageNumbersPro`.

## Dependencies

- `com.cupkekgames.textpopup` (UPM)
- DamageNumbersPro Asset Store package (project-level — bring your own; bridge will not compile without it)
