# Changelog

## [1.1.0] - 2026-08-21

### Core
- `EnumHolderAttribute` now declares the tag it contributes values to, and can be applied to
  classes as well as structs.
- Values for a tag are collected from every holder in every loaded assembly, so an enum can be
  extended from outside the assembly that declares it. Extending via `partial` no longer applies.
- Holder discovery is indexed once and cached, and skips Unity and system assemblies.

### Editor
- Duplicate value validation now covers all holders of a tag and reports the holders involved.

### Samples
- Replaced the same-assembly `partial` example with a holder in a separate assembly.

## [1.0.1] - 2026-07-09
- Renamed asmdef to a more used standard.

## [1.0.0] - 2026-07-05

### Core
- Inital release version.
### Documentation
- Changelod & readme added.
