# LIVF v0.1 Specification

[日本語](livf-v0.1.ja.md) | **English**

## 1. Overview

### 1.1 Official name

**LIVF — Layered Image Variant Format**

### 1.2 File extension

```text
.livf
```

### 1.3 Provisional MIME type

```text
application/vnd.livf+zip
```

This MIME type is a provisional value used within the LIVF project. It does not indicate formal registration with IANA.

### 1.4 Purpose

LIVF is an image container format for managing and displaying multiple image variants, especially character sprites, in a single file.

LIVF provides:

- Multiple image layers
- Layer hierarchy
- Layer visibility control
- Exclusive variant switching
- Named states that group multiple layer changes
- State groups for categories such as expressions and costumes
- A file-wide default state
- ID-based operations from applications
- Use from Unity, Web, and desktop applications

---

## 2. Intended use cases

LIVF is primarily intended for:

- Character sprites in visual novels
- Dialogue portraits in RPGs and adventure games
- Character display systems
- Simple avatars for streaming software
- Character displays on the Web
- Expression, costume, and accessory variant management

LIVF is not intended to provide deformation, bones, or physics simulation such as Live2D.

The central purpose of LIVF is to let applications switch combinations of static images easily.

---

## 3. Terminology

### 3.1 Document

Represents one complete LIVF file.

### 3.2 Canvas

Represents the final image area onto which layers are rendered.

### 3.3 Node

An element that forms the hierarchy inside a LIVF document.

There are two Node types:

- Layer
- Folder

### 3.4 Layer

A Node that contains an image to be rendered.

### 3.5 Folder

A Node that groups multiple Nodes.

A Folder may define visibility-selection rules for its child Nodes.

### 3.6 State

A named operation that groups multiple Node changes.

Examples:

```text
Smile
Angry
School uniform
Battle state
```

### 3.7 StateGroup

A group of related States.

Examples:

```text
Expression
Costume
Pose
```

### 3.8 Default State

The file-wide initial state applied immediately after a LIVF file is loaded.

Applications can use a dedicated API to return to the Default State at any time.

### 3.9 Runtime State

The current Node state maintained by an application after the file has been loaded.

---

## 4. Container structure

LIVF uses a ZIP-based container.

The basic internal structure is:

```text
character.livf
├─ manifest.json
├─ images/
│  ├─ body.png
│  ├─ eye_normal.png
│  ├─ eye_smile.png
│  ├─ mouth_normal.png
│  └─ mouth_smile.png
└─ thumbnail.png
```

### 4.1 Required files

#### manifest.json

Describes the LIVF document structure and settings in JSON format.

### 4.2 Optional files

#### thumbnail.png

A thumbnail image for editors and file browsers.

#### images directory

Using an `images` directory for image resources is recommended.

Images are not required to be placed directly under `images`.

The following hierarchy is valid:

```text
images/body/base.png
images/face/eyes/smile.png
images/costumes/school.png
```

### 4.3 File names and character encoding

- `manifest.json` must use UTF-8.
- `/` must be used as the path separator inside the ZIP container.
- UTF-8 characters may be used in file names.
- ASCII letters and digits are recommended for paths referenced by programs.
- File-name matching is case-sensitive.

---

## 5. manifest.json

The top-level structure of LIVF v0.1 is:

```json
{
  "livfVersion": "0.1",
  "metadata": {},
  "canvas": {},
  "defaultState": "character.default",
  "nodes": [],
  "states": [],
  "stateGroups": []
}
```

### 5.1 Top-level properties

| Name | Type | Required | Description |
|---|---|---:|---|
| livfVersion | string | Yes | LIVF specification version |
| metadata | object | Yes | File information |
| canvas | object | Yes | Canvas information |
| defaultState | string | No | File-wide Default State |
| nodes | Node[] | Yes | Root Node list |
| states | State[] | Yes | State list |
| stateGroups | StateGroup[] | Yes | StateGroup list |

Even when `states` or `stateGroups` is unused, an empty array is recommended.

---

## 6. metadata

```json
{
  "metadata": {
    "title": "Sample Character",
    "author": "Karotte",
    "description": "Character sprite for a game",
    "createdAt": "2026-07-26T20:00:00+09:00",
    "modifiedAt": "2026-07-26T20:00:00+09:00",
    "application": "LIVF Editor"
  }
}
```

### 6.1 Properties

| Name | Type | Required | Description |
|---|---|---:|---|
| title | string | Yes | File title |
| author | string | No | Author |
| description | string | No | Description |
| createdAt | string | No | Creation date and time |
| modifiedAt | string | No | Modification date and time |
| application | string | No | Application that created the file |
| license | string | No | License information |
| tags | string[] | No | Search and classification tags |

Dates and times must use ISO 8601 format.

---

## 7. Canvas

A LIVF document has one Canvas.

```json
{
  "canvas": {
    "width": 1024,
    "height": 2048
  }
}
```

### 7.1 Properties

| Name | Type | Required | Description |
|---|---|---:|---|
| width | integer | Yes | Canvas width |
| height | integer | Yes | Canvas height |

### 7.2 Constraints

- `width` must be at least 1.
- `height` must be at least 1.
- Units are pixels.
- Areas outside the Canvas must not be rendered.
- The Canvas is initialized as transparent.

---

## 8. Coordinate system

LIVF uses the top-left corner as the origin.

```text
(0, 0) ─────────→ X
  │
  │
  │
  ↓
  Y
```

- X increases to the right.
- Y increases downward.
- Coordinates may be negative.
- A Layer may extend partially outside the Canvas.

Folders do not have independent coordinate transformations in v0.1.

All Layer coordinates are absolute Canvas coordinates.

---

## 9. IDs

Every Node, State, and StateGroup has a unique ID.

### 9.1 Recommended format

```text
body.base
face.eyes.normal
face.eyes.smile
face.mouth.normal
expression.smile
costume.school
```

### 9.2 Recommended characters

```text
a-z
0-9
.
-
_
```

### 9.3 Constraints

- An ID must not be empty.
- IDs must not be duplicated within the file.
- ID matching is case-sensitive.
- IDs are referenced by applications and should not be changed after publication.
- `name` should be used for human-readable labels.

In LIVF v0.1, IDs are globally unique across Nodes, States, and StateGroups.

---

## 10. Common Node properties

There are two Node types: Layer and Folder.

All Nodes have the following common properties.

| Name | Type | Required | Description |
|---|---|---:|---|
| type | string | Yes | `layer` or `folder` |
| id | string | Yes | Unique ID |
| name | string | Yes | Human-readable name |
| visible | boolean | Yes | Visibility declared in the manifest |
| opacity | number | No | Opacity |
| locked | boolean | No | Prevent editing in editors |
| tags | string[] | No | Classification tags |

### 10.1 opacity

`opacity` must be between 0.0 and 1.0.

```text
0.0 = fully transparent
1.0 = fully opaque
```

The default value is `1.0`.

### 10.2 visible

`visible` is the base visibility declared in the manifest.

In Runtime State, it may be changed by States or application operations.

---

## 11. Layer

A Layer represents an image that is actually rendered.

```json
{
  "type": "layer",
  "id": "face.eyes.normal",
  "name": "Normal eyes",
  "source": "images/eye_normal.png",
  "visible": true,
  "x": 0,
  "y": 0,
  "opacity": 1.0
}
```

### 11.1 Layer properties

| Name | Type | Required | Description |
|---|---|---:|---|
| type | string | Yes | Always `layer` |
| id | string | Yes | Layer ID |
| name | string | Yes | Display name |
| source | string | Yes | Image path |
| visible | boolean | Yes | Base visibility |
| x | integer | Yes | X coordinate |
| y | integer | Yes | Y coordinate |
| opacity | number | No | Opacity |
| locked | boolean | No | Prevent editing in editors |
| tags | string[] | No | Classification tags |

### 11.2 Supported image format

A LIVF v0.1-compliant runtime must support PNG.

```text
image/png
```

PNG images may use an alpha channel.

WebP, JPEG, AVIF, and other formats are not required by LIVF v0.1.

---

## 12. Folder

A Folder groups multiple Nodes.

```json
{
  "type": "folder",
  "id": "face.eyes",
  "name": "Eyes",
  "visible": true,
  "selectionMode": "exclusive",
  "defaultChild": "face.eyes.normal",
  "children": []
}
```

### 12.1 Folder properties

| Name | Type | Required | Description |
|---|---|---:|---|
| type | string | Yes | Always `folder` |
| id | string | Yes | Folder ID |
| name | string | Yes | Display name |
| visible | boolean | Yes | Base visibility |
| opacity | number | No | Opacity applied to descendants |
| selectionMode | string | No | Child-selection mode |
| defaultChild | string | No | Initially selected child Node |
| children | Node[] | Yes | Child Node list |
| locked | boolean | No | Prevent editing in editors |
| tags | string[] | No | Classification tags |

### 12.2 Folder visibility

When a Folder is hidden, none of its descendants are rendered.

A Node is effectively visible only when:

```text
the Node itself is visible
and
all ancestor Folders are visible
```

### 12.3 Folder opacity

A Folder's opacity is multiplied into every descendant Layer.

Example:

```text
Folder opacity  = 0.5
Layer opacity   = 0.8
Effective value = 0.4
```


---

## 13. selectionMode

A Folder may define how its child Nodes are selected by using `selectionMode`.

LIVF v0.1 defines two modes:

- `multiple`
- `exclusive`

The default is `multiple`.

### 13.1 multiple

Multiple child Nodes may be visible at the same time.

```json
{
  "selectionMode": "multiple"
}
```

Example:

```text
Accessories
├─ Glasses
├─ Hat
└─ Necklace
```

All of these may be visible simultaneously.

### 13.2 exclusive

At most one child Node may be visible.

```json
{
  "selectionMode": "exclusive"
}
```

Example:

```text
Eyes
├─ Normal
├─ Smile
├─ Closed
└─ Surprised
```

When one child Node is made visible, all other child Nodes in the same Folder are automatically hidden.

### 13.3 Exclusive processing

Assume the following operation is performed.

```csharp
SetVisible("face.eyes.smile", true);
```

Before:

```text
face.eyes.normal = true
face.eyes.smile  = false
face.eyes.closed = false
```

After:

```text
face.eyes.normal = false
face.eyes.smile  = true
face.eyes.closed = false
```

If the direct parent Folder is hidden, making the child Node visible also makes that parent Folder visible.

Higher ancestor Folders are not automatically made visible.

### 13.4 No selected child

`exclusive` means “at most one,” not “exactly one.”

The currently selected child may be explicitly hidden, leaving no visible children.

```csharp
SetVisible("face.eyes.normal", false);
```

### 13.5 defaultChild

`defaultChild` specifies the ID of a direct child Node used as the Folder's initial selection.

```json
{
  "selectionMode": "exclusive",
  "defaultChild": "face.eyes.normal"
}
```

The referenced Node must be a direct child of the Folder.

`defaultChild` may also be specified for `multiple`, but it is primarily intended for `exclusive`.

---

## 14. State

A State is a named operation that groups multiple Node changes.

```json
{
  "id": "expression.smile",
  "name": "Smile",
  "changes": [
    {
      "target": "face.eyes.smile",
      "visible": true
    },
    {
      "target": "face.mouth.smile",
      "visible": true
    }
  ]
}
```

### 14.1 State properties

| Name | Type | Required | Description |
|---|---|---:|---|
| id | string | Yes | State ID |
| name | string | Yes | Display name |
| changes | Change[] | Yes | Changes to apply |
| tags | string[] | No | Classification tags |

### 14.2 Change

```json
{
  "target": "face.eyes.smile",
  "visible": true
}
```

| Name | Type | Required | Description |
|---|---|---:|---|
| target | string | Yes | Target Node ID |
| visible | boolean | No | Visibility |
| opacity | number | No | Opacity |

A Change must define at least one of `visible` or `opacity`.

### 14.3 Applying a State

A State applies its Changes to the current Runtime State.

Nodes not mentioned by the State remain unchanged.

```csharp
character.SetState("expression.smile");
```

If the State does not mention glasses, the current glasses visibility is preserved.

### 14.4 Atomic application

All Changes in a State are applied as one atomic operation.

An implementation must not render an intermediate state such as:

```text
1. Only the eyes change to a smile.
2. A frame is rendered.
3. The mouth changes to a smile.
```

The correct process is:

```text
1. Apply all Changes to a temporary state.
2. Resolve exclusive rules.
3. Commit the complete state to Runtime State.
4. Redraw.
```

### 14.5 Change order

Changes are processed from the beginning of the `changes` array.

When multiple Changes modify the same Node, the later value wins.

When multiple Nodes in one `exclusive` Folder are made visible, the last one made visible remains selected.

---

## 15. StateGroup

A StateGroup groups related States.

```json
{
  "id": "expression",
  "name": "Expression",
  "selectionMode": "exclusive",
  "defaultState": "expression.normal",
  "targets": [
    "face.eyes",
    "face.mouth"
  ],
  "states": [
    "expression.normal",
    "expression.smile",
    "expression.angry"
  ]
}
```

### 15.1 StateGroup properties

| Name | Type | Required | Description |
|---|---|---:|---|
| id | string | Yes | StateGroup ID |
| name | string | Yes | Display name |
| selectionMode | string | Yes | State-selection mode |
| defaultState | string | No | Initial State |
| targets | string[] | No | Nodes managed by the group |
| states | string[] | Yes | IDs of member States |

### 15.2 selectionMode

A StateGroup supports:

- `multiple`
- `exclusive`

#### multiple

Multiple States may be applied independently.

#### exclusive

One State is selected at a time.

This is intended for categories such as expressions and costumes.

### 15.3 targets

`targets` defines the Node range managed by the StateGroup.

When a Folder is specified, its descendant Nodes are also included.

```json
{
  "targets": [
    "face.eyes",
    "face.mouth"
  ]
}
```

This StateGroup manages the eyes Folder, the mouth Folder, and their descendants.

### 15.4 Switching an exclusive StateGroup

When a State in an `exclusive` StateGroup is selected, the runtime performs:

1. Restore Nodes in `targets` to the Group Baseline.
2. Apply the selected State's Changes.
3. Resolve Folder exclusive rules.
4. Update the active State.
5. Commit the complete result atomically.

This prevents effects from the previously selected State from remaining.

### 15.5 Group Baseline

The Group Baseline is constructed from:

1. Each Node's declared `visible`.
2. Each Node's declared `opacity`.
3. Folder `defaultChild` values.
4. Folder exclusive rules.

A StateGroup's `defaultState` and the file-wide `defaultState` are not included in the Group Baseline.

### 15.6 Omitting targets

`targets` may be omitted when `selectionMode` is `multiple`.

`targets` is required when `selectionMode` is `exclusive`.

### 15.7 Conflicts between StateGroups

Two or more `exclusive` StateGroups must not manage the same Node.

A conflict also exists when Folder target expansion causes descendant Nodes to overlap.

This is an error in LIVF v0.1.

---

## 16. File-wide Default State

A LIVF file may define a special State as its file-wide initial display.

```json
{
  "defaultState": "character.default"
}
```

`defaultState` references the ID of a State in `states`.

### 16.1 Purpose

The Default State is used for:

- Display immediately after loading
- Restoring the initial state from an application
- Standard preview in an editor
- Fully resetting runtime changes

### 16.2 Example definition

```json
{
  "defaultState": "character.default",
  "states": [
    {
      "id": "character.default",
      "name": "Default",
      "changes": [
        {
          "target": "face.eyes.normal",
          "visible": true
        },
        {
          "target": "face.mouth.normal",
          "visible": true
        },
        {
          "target": "costume.standard",
          "visible": true
        },
        {
          "target": "accessory.glasses",
          "visible": false
        }
      ]
    }
  ]
}
```

### 16.3 Special behavior

The State referenced as the Default State has the same structure as any ordinary State.

However, applying that State normally differs from resetting to the default.

```csharp
character.SetState("character.default");
```

This only applies that State's Changes to the current Runtime State.

By contrast:

```csharp
character.ResetToDefault();
```

discards the current Runtime State and restores the complete initial state.

### 16.4 Implicit Default State

When top-level `defaultState` is omitted, the runtime constructs an implicit Default State from:

1. Node `visible`.
2. Node `opacity`.
3. Folder `defaultChild`.
4. StateGroup `defaultState`.
5. Folder exclusive rules.

When an explicit file-wide Default State exists, it is applied last.

---

## 17. Initialization

When loading a LIVF file, the runtime performs:

1. Read `manifest.json`.
2. Validate the version.
3. Validate IDs and references.
4. Load images.
5. Construct the base state from Node declarations.
6. Apply Folder `defaultChild` values.
7. Apply StateGroup `defaultState` values.
8. Apply the file-wide `defaultState`.
9. Resolve exclusive rules.
10. Store the result as the Default Snapshot.
11. Copy the Default Snapshot to Runtime State.
12. Complete loading in a renderable state.

Immediately after loading, Runtime State must be identical to the Default Snapshot.

---

## 18. Default State precedence

Initial-state precedence is:

```text
file-wide defaultState
>
StateGroup defaultState
>
Folder defaultChild
>
Node visible and opacity
```

When settings conflict, the higher-priority setting wins.

Example:

```text
Node.visible            = true
Folder.defaultChild     = normal
StateGroup.defaultState = smile
File.defaultState       = angry
```

The final result is `angry`.

---

## 19. ResetToDefault

A LIVF runtime must provide a dedicated operation that restores the Default State.

```csharp
void ResetToDefault();
```

### 19.1 Behavior

`ResetToDefault()` restores the Default Snapshot to Runtime State.

It discards all of the following:

- Visibility changes made by the application
- Opacity changes made by the application
- Changes made by ordinary States
- StateGroup selection changes
- Temporary runtime changes

### 19.2 Guarantees

These two states must be identical:

```text
immediately after loading
```

```text
immediately after ResetToDefault()
```

Calling `ResetToDefault()` repeatedly must always produce the same result.

### 19.3 Atomicity

Restoring the Default Snapshot must be atomic.

Intermediate states must not be rendered.

---

## 20. IsDefaultState

A runtime may provide a property that reports whether the current Runtime State equals the Default Snapshot.

```csharp
bool IsDefaultState { get; }
```

`IsDefaultState` must not be determined only by remembering the last API call.

It must compare the current effective Node state with the Default Snapshot.

Example:

1. Change to a smile.
2. Restore the original eyes and mouth through direct operations.
3. The result equals the Default Snapshot.

In this case, `IsDefaultState` is `true`.

---

## 21. Runtime operations

A LIVF runtime must provide at least the following operations.

### 21.1 Get a Node

```csharp
LivfNode GetNode(string nodeId);
```

### 21.2 Set visibility

```csharp
void SetVisible(string nodeId, bool visible);
```

When a Node in an `exclusive` Folder is made visible, other child Nodes are automatically hidden.

### 21.3 Get visibility

```csharp
bool GetVisible(string nodeId);
```

This returns the Node's own Runtime State `visible` value.

To include ancestor Folder visibility, use a separate operation.

```csharp
bool GetEffectiveVisible(string nodeId);
```

### 21.4 Set opacity

```csharp
void SetOpacity(string nodeId, float opacity);
```

### 21.5 Get opacity

```csharp
float GetOpacity(string nodeId);
```

### 21.6 Apply a State

```csharp
void SetState(string stateId);
```

### 21.7 Select a StateGroup State

```csharp
void SetGroupState(string stateGroupId, string stateId);
```

The selected State must belong to the specified StateGroup.

### 21.8 Get the active State

```csharp
string? GetActiveState(string stateGroupId);
```

When a Node managed by a StateGroup is directly modified, the selected State becomes indeterminate.

In that case, this method returns `null`.

### 21.9 Restore the Default State

```csharp
void ResetToDefault();
```

### 21.10 Render

```csharp
LivfRenderedImage Render();
```

---

## 22. Direct operations and StateGroups

When an application directly modifies a Node included in a StateGroup's `targets`, that StateGroup's active selection is cleared.

```csharp
character.SetGroupState("expression", "expression.smile");
character.SetVisible("face.eyes.closed", true);
```

The expression group may now represent a combination that is not defined by any State.

Therefore:

```csharp
character.GetActiveState("expression");
```

returns `null`.

The actual Node visibility values remain unchanged.

---

## 23. Rendering order

Nodes are rendered from the beginning of each `nodes` or `children` array.

Nodes listed later are rendered in front of earlier Nodes.

```json
{
  "nodes": [
    {
      "id": "body.base"
    },
    {
      "id": "costume.standard"
    },
    {
      "id": "face"
    }
  ]
}
```

Rendering order:

```text
1. body.base
2. costume.standard
3. face
```

In this example, `face` appears in front.

Folders recursively render their children using the same rule.

---

## 24. Rendering process

LIVF v0.1 requires only normal alpha compositing.

Rendering proceeds as follows:

1. Initialize the Canvas as transparent.
2. Process root Nodes in array order.
3. Skip descendants of hidden Folders.
4. Recursively process Folder children.
5. Draw each Layer image at its specified coordinates.
6. Composite using the image alpha and effective opacity.
7. Clip areas outside the Canvas.

### 24.1 Effective opacity

A Layer's effective opacity is:

```text
Layer opacity
×
all ancestor Folder opacities
```

### 24.2 Blend mode

LIVF v0.1 uses only:

```text
normal
```

Multiply, additive, screen, and other blend modes are reserved for future versions.


---

## 25. Complete manifest example

```json
{
  "livfVersion": "0.1",
  "metadata": {
    "title": "Sample Character",
    "author": "Karotte",
    "description": "LIVF sample character",
    "createdAt": "2026-07-26T20:00:00+09:00",
    "application": "LIVF Editor"
  },
  "canvas": {
    "width": 1024,
    "height": 2048
  },
  "defaultState": "character.default",
  "nodes": [
    {
      "type": "layer",
      "id": "body.base",
      "name": "Body",
      "source": "images/body.png",
      "visible": true,
      "x": 0,
      "y": 0,
      "opacity": 1.0
    },
    {
      "type": "folder",
      "id": "face",
      "name": "Face",
      "visible": true,
      "opacity": 1.0,
      "selectionMode": "multiple",
      "children": [
        {
          "type": "folder",
          "id": "face.eyes",
          "name": "Eyes",
          "visible": true,
          "selectionMode": "exclusive",
          "defaultChild": "face.eyes.normal",
          "children": [
            {
              "type": "layer",
              "id": "face.eyes.normal",
              "name": "Normal eyes",
              "source": "images/eye_normal.png",
              "visible": true,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.eyes.smile",
              "name": "Smiling eyes",
              "source": "images/eye_smile.png",
              "visible": false,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.eyes.angry",
              "name": "Angry eyes",
              "source": "images/eye_angry.png",
              "visible": false,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            }
          ]
        },
        {
          "type": "folder",
          "id": "face.mouth",
          "name": "Mouth",
          "visible": true,
          "selectionMode": "exclusive",
          "defaultChild": "face.mouth.normal",
          "children": [
            {
              "type": "layer",
              "id": "face.mouth.normal",
              "name": "Normal mouth",
              "source": "images/mouth_normal.png",
              "visible": true,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.mouth.smile",
              "name": "Smiling mouth",
              "source": "images/mouth_smile.png",
              "visible": false,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.mouth.angry",
              "name": "Angry mouth",
              "source": "images/mouth_angry.png",
              "visible": false,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            }
          ]
        }
      ]
    },
    {
      "type": "folder",
      "id": "costume",
      "name": "Costume",
      "visible": true,
      "selectionMode": "exclusive",
      "defaultChild": "costume.standard",
      "children": [
        {
          "type": "layer",
          "id": "costume.standard",
          "name": "Standard costume",
          "source": "images/costume_standard.png",
          "visible": true,
          "x": 0,
          "y": 0,
          "opacity": 1.0
        },
        {
          "type": "layer",
          "id": "costume.school",
          "name": "School uniform",
          "source": "images/costume_school.png",
          "visible": false,
          "x": 0,
          "y": 0,
          "opacity": 1.0
        }
      ]
    },
    {
      "type": "layer",
      "id": "accessory.glasses",
      "name": "Glasses",
      "source": "images/glasses.png",
      "visible": false,
      "x": 0,
      "y": 0,
      "opacity": 1.0
    }
  ],
  "states": [
    {
      "id": "expression.normal",
      "name": "Normal",
      "changes": [
        {
          "target": "face.eyes.normal",
          "visible": true
        },
        {
          "target": "face.mouth.normal",
          "visible": true
        }
      ]
    },
    {
      "id": "expression.smile",
      "name": "Smile",
      "changes": [
        {
          "target": "face.eyes.smile",
          "visible": true
        },
        {
          "target": "face.mouth.smile",
          "visible": true
        }
      ]
    },
    {
      "id": "expression.angry",
      "name": "Angry",
      "changes": [
        {
          "target": "face.eyes.angry",
          "visible": true
        },
        {
          "target": "face.mouth.angry",
          "visible": true
        }
      ]
    },
    {
      "id": "costume.standard.state",
      "name": "Standard costume",
      "changes": [
        {
          "target": "costume.standard",
          "visible": true
        }
      ]
    },
    {
      "id": "costume.school.state",
      "name": "School uniform",
      "changes": [
        {
          "target": "costume.school",
          "visible": true
        }
      ]
    },
    {
      "id": "character.default",
      "name": "Default",
      "changes": [
        {
          "target": "face.eyes.normal",
          "visible": true
        },
        {
          "target": "face.mouth.normal",
          "visible": true
        },
        {
          "target": "costume.standard",
          "visible": true
        },
        {
          "target": "accessory.glasses",
          "visible": false
        }
      ]
    }
  ],
  "stateGroups": [
    {
      "id": "expression",
      "name": "Expression",
      "selectionMode": "exclusive",
      "defaultState": "expression.normal",
      "targets": [
        "face.eyes",
        "face.mouth"
      ],
      "states": [
        "expression.normal",
        "expression.smile",
        "expression.angry"
      ]
    },
    {
      "id": "costume.states",
      "name": "Costume",
      "selectionMode": "exclusive",
      "defaultState": "costume.standard.state",
      "targets": [
        "costume"
      ],
      "states": [
        "costume.standard.state",
        "costume.school.state"
      ]
    }
  ]
}
```

---

## 26. Validation

### 26.1 Errors

A LIVF file is invalid in any of the following cases:

- `manifest.json` does not exist.
- `livfVersion` does not exist.
- The major version is unsupported.
- Canvas width or height is zero or negative.
- A required property is missing.
- IDs are duplicated.
- A Layer's `source` does not exist.
- A `source` path refers outside the container.
- A State `target` does not exist.
- `defaultState` refers to a State that does not exist.
- A StateGroup refers to a State that does not exist.
- A StateGroup `defaultState` is not a member of that StateGroup.
- A Folder `defaultChild` is not a direct child.
- The Node hierarchy contains a cycle.
- An `exclusive` StateGroup has no `targets`.
- The managed ranges of multiple `exclusive` StateGroups overlap.
- An opacity value is below 0.0 or above 1.0.

### 26.2 Warnings

Loading may continue, but a warning is produced in the following cases:

- A Layer is completely outside the Canvas.
- An image resource is unused.
- A Layer is not referenced by any State.
- Multiple children of an `exclusive` Folder are initially visible.
- A State belongs to no StateGroup.
- The Default State changes nothing.
- Applying the Default State leaves every Layer hidden.
- The same State changes the same Node multiple times.
- The same State makes multiple exclusive Nodes visible.

### 26.3 Initial conflicts in an exclusive Folder

When multiple child Nodes of an `exclusive` Folder are initially visible, resolve the conflict as follows:

1. If `defaultChild` is visible, keep it visible.
2. Otherwise, keep the last visible Node in array order.
3. Hide the other child Nodes.
4. Record a warning.

---

## 27. Error handling

A LIVF runtime should distinguish error categories.

```text
InvalidManifest
UnsupportedVersion
DuplicateId
MissingImage
InvalidReference
InvalidCanvasSize
InvalidNode
InvalidState
InvalidStateGroup
InvalidPath
ResourceLimitExceeded
```

A C# implementation may define a dedicated exception.

```csharp
public sealed class LivfFormatException : Exception
{
    public LivfErrorCode ErrorCode { get; }

    public LivfFormatException(
        LivfErrorCode errorCode,
        string message
    ) : base(message)
    {
        ErrorCode = errorCode;
    }
}
```

---

## 28. Loading modes

A runtime may provide strict and lenient loading modes.

```csharp
LivfLoadMode.Strict
LivfLoadMode.Lenient
```

### 28.1 Strict

- Invalid references cause loading to fail.
- Missing images cause loading to fail.
- Specification violations are treated as errors.

### 28.2 Lenient

- Missing images may be treated as transparent images.
- Repairable exclusive conflicts may be corrected automatically.
- Problems may be returned as warnings.

Structural errors that cannot be resolved must still fail in Lenient mode.

---

## 29. Versioning

Version numbers use:

```text
major.minor
```

Examples:

```text
0.1
1.0
1.1
2.0
```

### 29.1 Major version

The major version increases for incompatible changes.

Examples:

- Changing the coordinate system
- Changing rendering order
- Substantially changing the manifest structure
- Changing State application rules

### 29.2 Minor version

The minor version increases for backward-compatible additions.

Examples:

- Adding an optional property
- Adding an image format
- Adding a blend mode
- Adding a metadata field

### 29.3 Unknown properties

Within the same major version, a runtime may ignore unknown optional properties.

A future specification will define a feature-declaration mechanism for unknown required functionality.

---

## 30. Security

### 30.1 External references

LIVF v0.1 prohibits external URLs and references outside the container.

Invalid examples:

```text
../secret.png
C:/Users/example/image.png
https://example.com/image.png
file:///home/example/image.png
```

Valid examples:

```text
images/body.png
images/face/eye.png
```

### 30.2 ZIP Slip protection

An implementation must reject paths that would write outside the intended extraction location.

### 30.3 Resource limits

A runtime may enforce limits on:

- Maximum Canvas size
- Maximum Layer count
- Maximum Node count
- Maximum State count
- Maximum hierarchy depth
- Maximum image size
- Maximum uncompressed file size
- Maximum file count
- Maximum memory usage

When a limit is exceeded, loading may stop with `ResourceLimitExceeded`.

### 30.4 Scripts

LIVF v0.1 does not contain executable scripts.

---

## 31. Required LIVF v0.1 features

A LIVF v0.1-compliant runtime must support:

- ZIP-based containers
- UTF-8 `manifest.json`
- PNG Layers
- Canvas
- Layer
- Folder
- Node hierarchy
- Visibility control
- Opacity
- `multiple` Folders
- `exclusive` Folders
- `defaultChild`
- States
- Atomic State application
- StateGroups
- Exclusive StateGroup selection
- File-wide Default State
- Implicit Default State
- Default Snapshot
- `ResetToDefault`
- Normal alpha compositing
- ID-based Node and State operations
- Path validation

---

## 32. Features excluded from LIVF v0.1

LIVF v0.1 does not define:

- Layer animation
- Frame animation
- Bone animation
- Mesh deformation
- Live2D-style deformation
- Physics simulation
- Masks
- Clipping
- Multiply, additive, or other blend modes
- Vector images
- Video
- Audio
- Scripts
- External URL images
- Encryption
- DRM
- Complete replacement of PSD editing functionality

---

## 33. Recommended C# class structure

```text
LivfDocument
├─ LivfMetadata
├─ LivfCanvas
├─ LivfNode
│  ├─ LivfLayer
│  └─ LivfFolder
├─ LivfState
├─ LivfChange
├─ LivfStateGroup
├─ LivfRuntimeState
└─ LivfDefaultSnapshot
```

Recommended project structure:

```text
Livf.Core
Livf.Serialization
Livf.Validation
Livf.Rendering
Livf.Unity
Livf.Web
Livf.Editor
```

---

## 34. Recommended API example

```csharp
using var character = LivfDocument.Load("character.livf");

// Immediately after loading, the character is in the Default State.
Console.WriteLine(character.IsDefaultState); // true

// Change the expression to a smile.
character.SetGroupState(
    "expression",
    "expression.smile"
);

// Show glasses.
character.SetVisible(
    "accessory.glasses",
    true
);

// The character is no longer in the Default State.
Console.WriteLine(character.IsDefaultState); // false

// Fully restore the initial state.
character.ResetToDefault();

// The character is in the Default State again.
Console.WriteLine(character.IsDefaultState); // true
```

---

## 35. Core concepts of LIVF

The five core concepts of LIVF are:

### Layer

An image that is actually rendered.

### Folder

An element that organizes Layers and Folders hierarchically and defines visibility rules.

### State

A named operation that groups multiple Node changes.

### StateGroup

A group that manages related States, such as expressions and costumes.

### Default State

A special state that defines the standard display and can always be fully restored by an application.

These concepts let applications operate character sprites through meaningful IDs without directly depending on image file names or the internal Layer hierarchy.

```csharp
character.SetGroupState(
    "expression",
    "expression.smile"
);

character.SetGroupState(
    "costume.states",
    "costume.school.state"
);

character.SetVisible(
    "accessory.glasses",
    true
);

character.ResetToDefault();
```
