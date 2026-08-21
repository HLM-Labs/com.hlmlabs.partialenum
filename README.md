# 🔧 Installation
- Inside your Unity project go to Window -> Package Manager
- At the bottom left corner choose "+"
- Install package from git URL
- Paste `https://github.com/Hllib/com.hlmlabs.partialenum.git`  
- Click "Install"

---
# Partial Enum

Type-safe, serializable enum-like values that can be defined across multiple files **and across multiple assemblies**.

## Concepts

- **Tag** — the type that identifies the enum, used as `TypedEnum<Tag>`. It never needs to be `partial`.
- **Holder** — any `class` or `struct` marked with `[EnumHolder(typeof(Tag))]` that declares
  `public static readonly TypedEnum<Tag>` fields. A tag can have any number of holders,
  in any assembly, and all of their values are merged.

The tag is usually its own first holder, which keeps the common case short.

## How to use

1. Declare the tag type with `[EnumHolder(typeof(YourType))]` and add its values as
   `public static readonly TypedEnum<YourType>` fields.
2. To add values from somewhere else, declare another holder for the same tag. Do **not**
   redeclare the tag type — a `partial` type cannot span assemblies, and a second declaration
   in another namespace creates an unrelated type that will not compile against the original.
3. Serialize `TypedEnum<YourType>` on components — the Inspector shows a dropdown of all
   registered values from all holders.

Compare values with `==` / `!=`. Use `.Value` for the underlying `int`, or rely on `ToString()` for the field name.

## Setup example

**EnumCustomType.cs** — the tag and its own values

```csharp
using HLMLabs.PartialEnum.Runtime;

[EnumHolder(typeof(EnumCustomType))]
public struct EnumCustomType
{
    public static readonly TypedEnum<EnumCustomType> Default = new(0);
    public static readonly TypedEnum<EnumCustomType> ValueA = new(1);
}
```

**EnumCustomTypeExtension.cs** — additional values, in any other assembly

```csharp
using HLMLabs.PartialEnum.Runtime;
using UnityEngine.Scripting;

[Preserve]
[EnumHolder(typeof(EnumCustomType))]
public static class EnumCustomTypeExtension
{
    public static readonly TypedEnum<EnumCustomType> ValueB = new(1000);
    public static readonly TypedEnum<EnumCustomType> ValueC = new(1001);
}
```

**MyComponent.cs** — use in the Inspector

```csharp
using HLMLabs.PartialEnum.Runtime;
using UnityEngine;

public class MyComponent : MonoBehaviour
{
    [SerializeField] private TypedEnum<EnumCustomType> customType;

    private void Start()
    {
        if (customType == EnumCustomTypeExtension.ValueB)
            Debug.Log("Value B selected");
    }
}
```

Values from different holders are reached through their own holder
(`EnumCustomType.ValueA`, `EnumCustomTypeExtension.ValueB`), because C# has no way to merge
static members of separate types. If you prefer a single entry point, write a small facade in
your own code that re-exposes the values you use.

## Notes

- Each value must have a unique integer **across all holders of the tag**. Duplicates are
  reported as errors in the Console on domain reload. Reserving a range per assembly
  (for example, the owner uses 0-999 and consumers start at 1000) avoids collisions.
- Holders are discovered by reflection, once, and then cached. The scan skips Unity and
  system assemblies.
- A holder that is never referenced from code can be removed by managed code stripping.
  Mark such holders with `[Preserve]`, or preserve them via `link.xml`.
