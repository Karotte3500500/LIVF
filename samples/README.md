# LIVF sample works

[日本語](README.ja.md) | **English**

The v0.1.0 Livi sample is included here. A general-purpose CC0 sample without Livi is still in preparation.

## Layout and versioning

```text
samples/
├── README.ja.md
├── README.md
└── v0.1.0/
    ├── livi/
    │   ├── livi.livf
    │   └── livi.livf.license.txt
    └── cc0/
        ├── basic.livf                (planned)
        └── basic.livf.license.txt    (planned)
```

`v0.1.0/` identifies the LIVF **file format** version and corresponds to `livfVersion: "0.1.0"` in each file's `manifest.json`. It is not the release version of an implementation or app. Samples for future format versions will go in separate version directories. We plan to keep only `.livf` sample assets here, without separate source image files or other asset files. As a v0.1.0 exception, each `.livf` will have an accompanying external license document.

## Terms for each sample category

| Location | Contents | Terms |
|---|---|---|
| `v0.1.0/livi/livi.livf` | Sample featuring Livi, the LIVF mascot | **CC BY 4.0**, with additional waivers of the attribution conditions. Uses that are both free of charge and noncommercial, as well as technical uses primarily for development, research, or education, need no attribution even when those technical uses are paid or commercial. Other paid or commercial creative uses require attribution. See the [Livi guidelines](../docs/livi-guidelines.md) and [external license document](v0.1.0/livi/livi.livf.license.txt) for details. |
| `v0.1.0/cc0/` (planned) | General-purpose samples without Livi | Intended to be released under [CC0 1.0 Universal](https://creativecommons.org/publicdomain/zero/1.0/legalcode.en). You may modify, redistribute, and use them commercially without attribution. |

Livi's terms also cover the images, thumbnail, `manifest.json`, and other material inside `livi.livf`. If you prefer not to assess whether your use of Livi qualifies for an attribution waiver, you will be able to choose a sample from `cc0/` when one is available.

## License notices and rights checks

**As a temporary v0.1.0 approach**, each `.livf` has a corresponding `<filename>.license.txt` in the same directory. [Livi's external document](v0.1.0/livi/livi.livf.license.txt) states the additional waivers of the CC BY 4.0 attribution conditions. The project will provide the corresponding document whenever it distributes a sample. To make the terms discoverable if a `.livf` is shared alone, its `manifest.json` sets `metadata.license` to include the identifier for its base license (`CC-BY-4.0` for Livi or `CC0-1.0` for a future general-purpose sample). Livi's value also includes a public URL for the external document explaining the additional waivers.

**For file format versions after v0.1.0**, we plan to specify a way to include the license document inside the `.livf` file. The external document is a temporary arrangement for v0.1.0.

Before adding a sample, we verify that every asset in the file can be released under the applicable terms. The images and configuration data in the v0.1.0 Livi sample were created by its rights holder. CC0 samples may contain only contributors' own material that they can release under CC0 or existing CC0 material whose source and rights have been checked. Contributions will record which material is original and which is pre-existing, along with the source of pre-existing material.

Each external document describes the terms for its corresponding `.livf` and internal assets. The sample licenses above do not cover README documents, the external documents themselves, or other files outside the `.livf` containers. See the repository's [LICENSE](../LICENSE) for source code and specifications.
