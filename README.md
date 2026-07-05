# 🔧 Installation
- Inside your Unity project go to Window -> Package Manager
- At the bottom left corner choose "+"
- Install package from git URL
- Paste `https://github.com/Hllib/com.hlmlabs.partialenum.git`  
- Click "Install"

---
# Partial Enum

Type-safe, serializable enum-like values that can be defined across multiple files.

## How to use

1. Declare a `partial struct` with `[EnumHolder(typeof(YourType))]`.
2. Add values as `public static readonly TypedEnum<YourType>` fields on that struct.
3. Extend the same `partial struct` in other files to add more values.
4. Serialize `TypedEnum<YourType>` on components — the Inspector shows a dropdown of all registered values.

Compare values with `==` / `!=`. Use `.Value` for the underlying `int`, or rely on `ToString()` for the field name.

## Setup example

**EnumCustomType.cs**

```csharp
using HLMLabs.PartialEnum.Runtime;

[EnumHolder(typeof(EnumCustomType))]
public partial struct EnumCustomType
{
    public static readonly TypedEnum<EnumCustomType> Default = new(0);
    public static readonly TypedEnum<EnumCustomType> ValueA = new(1);
}
```

**EnumCustomType.Impl.cs** — additional values in another file

```csharp
using HLMLabs.PartialEnum.Runtime;

public partial struct EnumCustomType
{
    public static readonly TypedEnum<EnumCustomType> ValueB = new(2);
    public static readonly TypedEnum<EnumCustomType> ValueC = new(3);
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
        if (customType == EnumCustomType.ValueB)
            Debug.Log("Value B selected");
    }
}
```

Each value must have a unique integer. Duplicate values are reported as errors in the Console on domain reload.
