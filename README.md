# LIVF — Layered Image Variant Format

[日本語](README.ja.md) | **English**

LIVF is an experimental layered image container format designed for
character sprites and other images composed of switchable variants.

A LIVF file stores image layers, folders, visibility rules, named states,
and a reproducible default state in a single `.livf` container.

## Project Status

> [!WARNING]
> LIVF is currently under development and is not yet recommended for
> production use.

- Initial LIVF specification: v0.1.0
- C# reference implementation: under development
- Unity runtime: planned
- Web runtime: planned
- Editor and PSD importer: planned

## Goals

LIVF aims to make it easy for applications to manage layered character
sprites without directly depending on image filenames or editor-specific
formats such as PSD.

Typical use cases include:

- Visual novel character sprites
- RPG and adventure game dialogue portraits
- Character expression and costume variants
- Simple streaming avatars
- Layered character images on the Web

LIVF is not intended to replace animation systems such as Live2D.
Version 0.1.0 focuses on switching and combining static image layers.

## LIVF v0.1.0 Features

- ZIP-based `.livf` container
- UTF-8 `manifest.json`
- PNG image layers
- Layer and folder hierarchy
- Visibility and opacity control
- `multiple` and `exclusive` folder selection
- Named states
- State groups
- File-wide default state
- Default snapshot and reset behavior
- Deterministic layer ordering
- Normal alpha compositing
- ID-based runtime operations

## Example

```csharp
using Livf.Serialization;
using Livf.Runtime;
using Livf.Rendering;

var document = LivfDocumentLoader.Load("character.livf");
var character = LivfRuntime.Create(document);

character.SetGroupState(
    "expression",
    "expression.smile"
);

character.SetVisible(
    "accessory.glasses",
    true
);

character.ResetToDefault();

var image = LivfRenderer.Render(character);
```

> The API shown above represents the planned reference API and is not yet
> available.

## Repository Structure

```text
livf/
├─ docs/       Specification and development documents
├─ schemas/    JSON schemas
├─ src/        LIVF libraries
├─ tools/      CLI and development tools
└─ tests/      Automated tests and LIVF fixtures
```

## Planned Components

- `Livf.Core`
- `Livf.Serialization`
- `Livf.Validation`
- `Livf.Runtime`
- `Livf.Rendering`
- `Livf.Cli`
- Unity runtime
- Web runtime
- LIVF editor
- PSD importer

## Versioning

LIVF file-format versions use `major.minor.patch`, for example `0.1.0`,
`0.1.1`, `0.2.0`, and `1.0.0`. The `livfVersion` value omits the `v` prefix:

```json
{
  "livfVersion": "0.1.0"
}
```

## Contributing

Contribution guidelines will be added as the reference implementation
develops.

Specification proposals, implementation feedback, test cases, and bug
reports will be welcome.

## License

LIVF source code, specifications, JSON Schema, and ordinary documentation are
licensed under the [Apache License 2.0](LICENSE). The [Livi sample `.livf`](samples/v0.1.0/livi/livi.livf)
is available under CC BY 4.0 with additional attribution waivers. A general-purpose
CC0 sample without Livi is still in preparation. See the [samples README](samples/README.md)
for the layout and terms.

A supplementary Japanese explanation is available in
[LICENSE.ja.md](LICENSE.ja.md).

For Livi, see the [creative and usage guidelines](docs/livi-guidelines.md).
