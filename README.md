# CupkekGames Pool

Object pooling primitives for Unity. Extracted from `com.cupkekgames.core` so consumers can opt in without depending on the broader Core grab-bag.

## What's inside

**Runtime** (`CupkekGames.Pool.asmdef`)

- `IObjectPool<T>` — generic pool interface with create/take/return/destroy events
- `ObjectPoolBase<T>` — abstract base wrapping `UnityEngine.Pool.ObjectPool<T>`
- `ObjectPoolSO<T>` — `ScriptableObject`-flavored pool
- `GameObjectPool` / `GameObjectPoolBase` / `GameObjectPoolList` / `GameObjectPoolPooled` / `MonoGameObjectPool` — GameObject-typed pools

## Dependencies

None.

## Installation

Embedded package — install via the CupkekGames UPM scoped registry (`https://www.docs.cupkek.games/upm`) or as a local `file:` path during development.
