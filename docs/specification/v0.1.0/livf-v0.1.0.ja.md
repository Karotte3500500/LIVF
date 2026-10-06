# LIVF v0.1.0 仕様書

**日本語** | [English](livf-v0.1.0.md)

## 1. 概要

### 1.1 正式名称

**LIVF — Layered Image Variant Format**

### 1.2 ファイル拡張子

```text
.livf
```

### 1.3 暫定MIMEタイプ

```text
application/vnd.livf+zip
```

このMIMEタイプはLIVFプロジェクト内で使用する暫定的な値であり、IANAへの正式登録を意味しない。

### 1.4 目的

LIVFは、キャラクターの立ち絵などに使用される複数の画像差分を、一つのファイルとして管理・表示するための画像コンテナ形式である。

LIVFは、次の機能を提供する。

- 複数の画像レイヤー
- レイヤーの階層化
- レイヤーの表示・非表示
- 排他的な差分切り替え
- 複数レイヤーをまとめた名前付き状態
- 表情、衣装などの状態グループ
- ファイル全体のデフォルト状態
- アプリケーションからのIDによる操作
- Unity、Web、デスクトップアプリケーションでの利用

---

## 2. 想定用途

LIVFは、主に次の用途を想定する。

- ノベルゲームの立ち絵
- RPGやアドベンチャーゲームの会話画面
- キャラクター表示システム
- 配信ソフトの簡易アバター
- Web上のキャラクター表示
- 表情差分、衣装差分、装飾品差分の管理

LIVFは、Live2Dのような変形・ボーン・物理演算を目的としない。

LIVFの中心的な目的は、静止画像の組み合わせをアプリケーションから簡単に切り替えることである。

---

## 3. 用語

### 3.1 Document

一つのLIVFファイル全体を表す。

### 3.2 Canvas

最終的な画像を描画する領域を表す。

### 3.3 Node

LIVF内の階層構造を構成する要素。

Nodeには次の二種類がある。

- Layer
- Folder

### 3.4 Layer

実際に描画される画像を持つNode。

### 3.5 Folder

複数のNodeをまとめるNode。

Folderは、子Nodeに対する表示ルールを持つことができる。

### 3.6 State

複数のNode変更をまとめた、名前付きの操作。

例：

```text
笑顔
怒り顔
制服
戦闘状態
```

### 3.7 StateGroup

関連するStateをまとめるグループ。

例：

```text
表情
衣装
ポーズ
```

### 3.8 Default State

LIVFファイルの読み込み直後に適用される、ファイル全体の初期状態。

アプリケーションは、専用APIを使用していつでもDefault Stateへ戻すことができる。

### 3.9 Runtime State

ファイル読み込み後、アプリケーション上で保持されている現在のNode状態。

---

## 4. ファイル構造

LIVFはZIP形式を基礎としたコンテナファイルとする。

内部の基本構造は次のとおり。

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

### 4.1 必須ファイル

### manifest.json

LIVFファイル全体の構造と設定をJSON形式で記述する。

### 4.2 任意ファイル

### thumbnail.png

エディタやファイル一覧で使用するサムネイル画像。

### imagesディレクトリ

画像の格納場所として使用することを推奨する。

ただし、画像が必ず`images`直下に存在する必要はない。

次のような階層も許可する。

```text
images/body/base.png
images/face/eyes/smile.png
images/costumes/school.png
```

### 4.3 ファイル名と文字コード

- `manifest.json`はUTF-8で記述する
- ZIP内部のパス区切りには`/`を使用する
- ファイル名にはUTF-8文字を使用できる
- プログラムから参照するパスには英数字を推奨する
- ファイル名の大文字と小文字は区別する

---

## 5. manifest.json

LIVF v0.1.0の最上位構造は次のとおり。

```json
{
  "livfVersion": "0.1.0",
  "metadata": {},
  "canvas": {},
  "defaultState": "character.default",
  "nodes": [],
  "states": [],
  "stateGroups": []
}
```

### 5.1 最上位プロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| livfVersion | string | 必須 | LIVF仕様のバージョン |
| metadata | object | 必須 | ファイル情報 |
| canvas | object | 必須 | キャンバス情報 |
| defaultState | string | 任意 | ファイル全体のDefault State |
| nodes | Node[] | 必須 | ルートNode一覧 |
| states | State[] | 必須 | State一覧 |
| stateGroups | StateGroup[] | 必須 | StateGroup一覧 |

`states`または`stateGroups`を使用しない場合でも、空配列を記述することを推奨する。

---

## 6. metadata

```json
{
  "metadata": {
    "title": "Sample Character",
    "author": "Karotte",
    "description": "ゲーム用キャラクター立ち絵",
    "createdAt": "2026-07-26T20:00:00+09:00",
    "modifiedAt": "2026-07-26T20:00:00+09:00",
    "application": "LIVF Editor"
  }
}
```

### 6.1 プロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| title | string | 必須 | ファイルの名称 |
| author | string | 任意 | 制作者 |
| description | string | 任意 | ファイルの説明 |
| createdAt | string | 任意 | 作成日時 |
| modifiedAt | string | 任意 | 更新日時 |
| application | string | 任意 | 作成アプリケーション |
| license | string | 任意 | ライセンス情報 |
| tags | string[] | 任意 | 検索・分類用タグ |

日時にはISO 8601形式を使用する。

---

## 7. Canvas

LIVFファイルは、一つのCanvasを持つ。

```json
{
  "canvas": {
    "width": 1024,
    "height": 2048
  }
}
```

### 7.1 プロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| width | integer | 必須 | Canvasの横幅 |
| height | integer | 必須 | Canvasの縦幅 |

### 7.2 制約

- `width`は1以上
- `height`は1以上
- 単位はピクセル
- Canvas外の領域は描画しない
- 背景は透明として初期化する

---

## 8. 座標系

LIVFは左上を原点とする座標系を使用する。

```text
(0, 0) ─────────→ X
  │
  │
  │
  ↓
  Y
```

- X座標は右方向に増加する
- Y座標は下方向に増加する
- 座標には負の値を指定できる
- Layerの一部がCanvas外にはみ出すことを許可する

Folderはv0.1.0では独自の座標変換を持たない。

そのため、すべてのLayer座標はCanvas基準の絶対座標として扱う。

---

## 9. ID

すべてのNode、State、StateGroupは一意なIDを持つ。

### 9.1 推奨形式

```text
body.base
face.eyes.normal
face.eyes.smile
face.mouth.normal
expression.smile
costume.school
```

### 9.2 推奨文字

```text
a-z
0-9
.
-
_
```

### 9.3 制約

- IDは空文字列にできない
- IDはファイル内で重複してはならない
- 大文字と小文字は区別する
- IDはアプリケーションから参照されるため、公開後の変更を避ける
- 人間向けの表示名には`name`を使用する

LIVF v0.1.0では、Node、State、StateGroupを含め、すべてのIDをファイル全体で一意とする。

---

## 10. Node共通仕様

NodeにはLayerとFolderがある。

すべてのNodeは、次の共通プロパティを持つ。

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| type | string | 必須 | `layer`または`folder` |
| id | string | 必須 | 一意なID |
| name | string | 必須 | 人間向けの表示名 |
| visible | boolean | 必須 | 宣言時の表示状態 |
| opacity | number | 任意 | 不透明度 |
| locked | boolean | 任意 | エディタ上での編集禁止 |
| tags | string[] | 任意 | 分類用タグ |

### 10.1 opacity

`opacity`には0.0から1.0までの数値を指定する。

```text
0.0 = 完全に透明
1.0 = 完全に不透明
```

省略時は`1.0`とする。

### 10.2 visible

`visible`は、manifest上で宣言された基本表示状態である。

Runtime State上では、Stateやアプリケーション操作によって変更される。

---

## 11. Layer

Layerは、実際に描画する画像を表す。

```json
{
  "type": "layer",
  "id": "face.eyes.normal",
  "name": "通常の目",
  "source": "images/eye_normal.png",
  "visible": true,
  "x": 0,
  "y": 0,
  "opacity": 1.0
}
```

### 11.1 Layerプロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| type | string | 必須 | 常に`layer` |
| id | string | 必須 | LayerのID |
| name | string | 必須 | 表示名 |
| source | string | 必須 | 画像ファイルのパス |
| visible | boolean | 必須 | 基本表示状態 |
| x | integer | 必須 | X座標 |
| y | integer | 必須 | Y座標 |
| opacity | number | 任意 | 不透明度 |
| locked | boolean | 任意 | エディタ上での編集禁止 |
| tags | string[] | 任意 | 分類用タグ |

### 11.2 対応画像形式

LIVF v0.1.0準拠ランタイムは、PNG形式を必ずサポートしなければならない。

```text
image/png
```

PNG画像はアルファチャンネルを使用できる。

WebP、JPEG、AVIFなどは、LIVF v0.1.0の必須対応形式には含めない。

---

## 12. Folder

Folderは複数のNodeをまとめる。

```json
{
  "type": "folder",
  "id": "face.eyes",
  "name": "目",
  "visible": true,
  "selectionMode": "exclusive",
  "defaultChild": "face.eyes.normal",
  "children": []
}
```

### 12.1 Folderプロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| type | string | 必須 | 常に`folder` |
| id | string | 必須 | FolderのID |
| name | string | 必須 | 表示名 |
| visible | boolean | 必須 | 基本表示状態 |
| opacity | number | 任意 | 子孫へ適用する不透明度 |
| selectionMode | string | 任意 | 子Nodeの選択方法 |
| defaultChild | string | 任意 | 初期選択する子Node |
| children | Node[] | 必須 | 子Node一覧 |
| locked | boolean | 任意 | エディタ上での編集禁止 |
| tags | string[] | 任意 | 分類用タグ |

### 12.2 Folderの可視性

Folderが非表示の場合、その子孫Nodeはすべて描画されない。

Nodeが実際に描画される条件は次のとおり。

```text
自身が表示状態
かつ
すべての親Folderが表示状態
```

### 12.3 Folderのopacity

Folderのopacityは、すべての子孫Layerに乗算される。

例：

```text
Folder opacity = 0.5
Layer opacity  = 0.8
実効opacity    = 0.4
```

---

## 13. selectionMode

Folderは、子Nodeの表示方法を`selectionMode`で指定できる。

LIVF v0.1.0では、次の二種類を定義する。

- `multiple`
- `exclusive`

省略時は`multiple`とする。

### 13.1 multiple

複数の子Nodeを同時に表示できる。

```json
{
  "selectionMode": "multiple"
}
```

使用例：

```text
アクセサリー
├─ メガネ
├─ 帽子
└─ ネックレス
```

これらは同時に表示できる。

### 13.2 exclusive

子Nodeのうち、最大一つだけを表示できる。

```json
{
  "selectionMode": "exclusive"
}
```

使用例：

```text
目
├─ 通常
├─ 笑顔
├─ 閉じ目
└─ 驚き
```

一つの子Nodeを表示した場合、同じFolder内のほかの子Nodeは自動的に非表示になる。

### 13.3 排他処理

次の操作を行ったとする。

```csharp
SetVisible("face.eyes.smile", true);
```

実行前：

```text
face.eyes.normal = true
face.eyes.smile  = false
face.eyes.closed = false
```

実行後：

```text
face.eyes.normal = false
face.eyes.smile  = true
face.eyes.closed = false
```

対象の親Folderが非表示だった場合、対象の子Nodeを表示すると同時に、その親Folderも表示状態にする。

ただし、さらに上位の祖先Folderまでは自動的に表示しない。

### 13.4 子Nodeが0個になる状態

`exclusive`は「最大一つ」を意味する。

そのため、選択中の子Nodeを明示的に非表示にし、表示中の子Nodeが0個になることを許可する。

```csharp
SetVisible("face.eyes.normal", false);
```

### 13.5 defaultChild

`defaultChild`には、Folderの初期選択として使用する直接の子NodeのIDを指定する。

```json
{
  "selectionMode": "exclusive",
  "defaultChild": "face.eyes.normal"
}
```

`defaultChild`は、対象Folderの直接の子でなければならない。

`selectionMode`が`multiple`の場合でも指定できるが、主に`exclusive`での利用を想定する。

---

## 14. State

Stateは、複数のNode変更をまとめた名前付き操作である。

```json
{
  "id": "expression.smile",
  "name": "笑顔",
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

### 14.1 Stateプロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| id | string | 必須 | StateのID |
| name | string | 必須 | 表示名 |
| changes | Change[] | 必須 | 適用する変更 |
| tags | string[] | 任意 | 分類用タグ |

### 14.2 Change

```json
{
  "target": "face.eyes.smile",
  "visible": true
}
```

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| target | string | 必須 | 対象NodeのID |
| visible | boolean | 任意 | 表示状態 |
| opacity | number | 任意 | 不透明度 |

一つのChangeには、`visible`または`opacity`の少なくとも一方を指定しなければならない。

### 14.3 Stateの適用

Stateは、現在のRuntime Stateに対して変更を適用する。

Stateに記載されていないNodeは変更しない。

例：

```csharp
character.SetState("expression.smile");
```

このStateにメガネの設定が含まれていなければ、現在のメガネ表示状態は維持される。

### 14.4 アトミックな適用

State内のすべてのChangeは一括して適用する。

次のような途中状態を画面に表示してはならない。

```text
1. 目だけ笑顔になる
2. 一度描画される
3. 口が笑顔になる
```

正しい処理は次のとおり。

```text
1. すべてのChangeを一時状態に適用
2. 排他ルールを解決
3. Runtime Stateへ一括反映
4. 再描画
```

### 14.5 Changeの適用順

`changes`配列の先頭から順番に処理する。

同じNodeに複数のChangeが存在する場合、後に記載された値を優先する。

同じ`exclusive` Folder内の複数Nodeを表示した場合、最後に表示されたNodeを残す。

---

## 15. StateGroup

StateGroupは、関連するStateをまとめる。

```json
{
  "id": "expression",
  "name": "表情",
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

### 15.1 StateGroupプロパティ

| 名前 | 型 | 必須 | 説明 |
|---|---|---:|---|
| id | string | 必須 | StateGroupのID |
| name | string | 必須 | 表示名 |
| selectionMode | string | 必須 | Stateの選択方式 |
| defaultState | string | 任意 | 初期State |
| targets | string[] | 任意 | Groupが管理するNode |
| states | string[] | 必須 | 所属StateのID一覧 |

### 15.2 selectionMode

StateGroupでは次の値を使用できる。

- `multiple`
- `exclusive`

### multiple

複数のStateを独立して適用できる。

### exclusive

一度に一つのStateを選択する。

表情や衣装のように、一つだけを選択する分類に使用する。

### 15.3 targets

`targets`は、そのStateGroupが管理するNodeの範囲を表す。

Folderを指定した場合、その子孫Nodeも対象に含まれる。

例：

```json
{
  "targets": [
    "face.eyes",
    "face.mouth"
  ]
}
```

このStateGroupは、目Folderと口Folder以下のNodeを管理する。

### 15.4 exclusive StateGroupの切り替え

`exclusive` StateGroup内のStateを適用する場合、次の順番で処理する。

1. `targets`内のNodeをGroup Baselineへ戻す
2. 指定されたStateのChangeを適用する
3. Folderの排他ルールを解決する
4. 選択中Stateを更新する
5. 変更を一括反映する

これにより、以前のStateの影響が残ることを防ぐ。

### 15.5 Group Baseline

Group Baselineは、次の情報から構築する。

1. Node自身の`visible`
2. Node自身の`opacity`
3. Folderの`defaultChild`
4. Folderの排他ルール

StateGroupの`defaultState`や、ファイル全体の`defaultState`はGroup Baselineに含めない。

### 15.6 targetsを省略した場合

`selectionMode`が`multiple`の場合は省略できる。

`selectionMode`が`exclusive`の場合は、`targets`の指定を必須とする。

### 15.7 StateGroup間の競合

二つ以上の`exclusive` StateGroupが同じNodeを管理してはならない。

Folderを指定した結果、子孫Nodeが重複する場合も競合とする。

これはLIVF v0.1.0ではエラーとして扱う。

---

## 16. ファイル全体のDefault State

LIVFファイルは、ファイル全体の初期表示を特別なDefault Stateとして定義できる。

```json
{
  "defaultState": "character.default"
}
```

`defaultState`は、`states`に存在するStateのIDを参照する。

### 16.1 Default Stateの目的

Default Stateは、次の用途に使用する。

- ファイル読み込み直後の表示
- アプリケーションからの初期状態への復元
- エディタ上での標準プレビュー
- 変更内容の完全なリセット

### 16.2 Default Stateの定義例

```json
{
  "defaultState": "character.default",
  "states": [
    {
      "id": "character.default",
      "name": "デフォルト",
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

### 16.3 Default Stateの特殊性

Default Stateとして指定されたStateは、Stateそのものの構造は通常のStateと同じである。

ただし、Default Stateへの復元操作は、通常の`SetState`とは異なる。

```csharp
character.SetState("character.default");
```

この処理は、現在のRuntime Stateに対してStateのChangeだけを適用する。

一方、次の処理は現在の状態を破棄して初期状態を再構築する。

```csharp
character.ResetToDefault();
```

### 16.4 暗黙的なDefault State

トップレベルの`defaultState`が省略されている場合、LIVFランタイムは暗黙的なDefault Stateを構築する。

暗黙的なDefault Stateは次の情報から決定する。

1. Nodeの`visible`
2. Nodeの`opacity`
3. Folderの`defaultChild`
4. StateGroupの`defaultState`
5. Folderの排他ルール

明示的なDefault Stateが存在する場合は、そのStateを最後に適用する。

---

## 17. 初期化処理

LIVFファイルの読み込み時には、次の処理を行う。

1. `manifest.json`を読み込む
2. バージョンを検証する
3. IDと参照関係を検証する
4. 画像を読み込む
5. Nodeの宣言値から基本状態を構築する
6. Folderの`defaultChild`を適用する
7. StateGroupの`defaultState`を適用する
8. ファイル全体の`defaultState`を適用する
9. 排他ルールを解決する
10. 解決結果をDefault Snapshotとして保存する
11. Default SnapshotをRuntime Stateへコピーする
12. 描画可能な状態として読み込みを完了する

読み込み直後のRuntime Stateは、必ずDefault Snapshotと同じでなければならない。

---

## 18. Default Stateの優先順位

初期状態を決定する際の優先順位は次のとおり。

```text
ファイル全体のdefaultState
>
StateGroupのdefaultState
>
FolderのdefaultChild
>
Nodeのvisible・opacity
```

上位の設定が下位の設定と競合した場合、上位の値を優先する。

例：

```text
Node.visible            = true
Folder.defaultChild     = normal
StateGroup.defaultState = smile
File.defaultState       = angry
```

最終的には`angry`が優先される。

---

## 19. ResetToDefault

LIVFランタイムは、Default Stateへ復元するための専用操作を提供しなければならない。

```csharp
void ResetToDefault();
```

### 19.1 処理内容

`ResetToDefault()`は、読み込み時に構築したDefault SnapshotをRuntime Stateへ復元する。

次の情報はすべて破棄される。

- アプリケーションから行った表示変更
- アプリケーションから行ったopacity変更
- 通常のStateによる変更
- StateGroupの選択変更
- Runtime上の一時的な変更

### 19.2 保証

次の二つの状態は同一でなければならない。

```text
ファイル読み込み直後
```

```text
ResetToDefault()実行直後
```

`ResetToDefault()`は何度実行しても同じ結果になる。

### 19.3 アトミック性

Default Snapshotへの復元は一括で行う。

途中の状態を描画してはならない。

---

## 20. IsDefaultState

ランタイムは、現在のRuntime StateがDefault Snapshotと同じか確認する機能を提供してもよい。

```csharp
bool IsDefaultState { get; }
```

`IsDefaultState`は、最後に呼び出されたAPIだけで判定してはならない。

現在の有効なNode状態とDefault Snapshotを比較して判定する。

例：

1. 笑顔に変更する
2. 個別操作で元の目と口へ戻す
3. 結果がDefault Snapshotと一致する

この場合、`IsDefaultState`は`true`となる。

---

## 21. Runtime操作

LIVFランタイムは、最低限次の操作を提供する。

### 21.1 Nodeの取得

```csharp
LivfNode GetNode(string nodeId);
```

### 21.2 表示状態の変更

```csharp
void SetVisible(string nodeId, bool visible);
```

`exclusive` Folder内のNodeを表示した場合、ほかの子Nodeを自動的に非表示にする。

### 21.3 表示状態の取得

```csharp
bool GetVisible(string nodeId);
```

これはNode自身のRuntime上の`visible`を返す。

親Folderの状態を含む実効表示状態を取得する場合は、別のAPIを使用する。

```csharp
bool GetEffectiveVisible(string nodeId);
```

### 21.4 opacityの変更

```csharp
void SetOpacity(string nodeId, float opacity);
```

### 21.5 opacityの取得

```csharp
float GetOpacity(string nodeId);
```

### 21.6 Stateの適用

```csharp
void SetState(string stateId);
```

### 21.7 StateGroupの選択

```csharp
void SetGroupState(string stateGroupId, string stateId);
```

指定されたStateは、対象StateGroupに所属していなければならない。

### 21.8 選択中Stateの取得

```csharp
string? GetActiveState(string stateGroupId);
```

StateGroupの管理対象Nodeを直接変更した場合、そのStateGroupの選択中Stateは未確定となる。

その場合は`null`を返す。

### 21.9 Default Stateへの復元

```csharp
void ResetToDefault();
```

### 21.10 描画

```csharp
LivfRenderedImage Render();
```

---

## 22. 直接操作とStateGroup

StateGroupの`targets`に含まれるNodeをアプリケーションが直接変更した場合、そのStateGroupの選択状態は解除される。

例：

```csharp
character.SetGroupState("expression", "expression.smile");
character.SetVisible("face.eyes.closed", true);
```

この場合、表情GroupはStateとして定義されていない組み合わせになった可能性がある。

したがって、次の値は`null`となる。

```csharp
character.GetActiveState("expression");
```

実際のNode表示状態は維持される。

---

## 23. 描画順序

Nodeは、`nodes`および`children`配列の先頭から順番に描画する。

後に記載されたNodeほど手前に表示される。

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

描画順は次のとおり。

```text
1. body.base
2. costume.standard
3. face
```

この場合、`face`が最も手前に描画される。

Folderは、子Nodeを同じ規則で再帰的に描画する。

---

## 24. 描画処理

LIVF v0.1.0では、通常のアルファ合成のみを必須対応とする。

描画処理は次の順番で行う。

1. Canvasを透明色で初期化する
2. ルートNodeを配列順に処理する
3. 非表示Folderの子孫を除外する
4. Folderの場合は子Nodeを再帰処理する
5. Layerの場合は画像を指定座標へ描画する
6. Layerのアルファ値と実効opacityで合成する
7. Canvas外の領域を切り取る

### 24.1 実効opacity

Layerの実効opacityは次のように求める。

```text
Layer自身のopacity
×
すべての親Folderのopacity
```

### 24.2 ブレンドモード

LIVF v0.1.0では、次のブレンドモードのみを使用する。

```text
normal
```

乗算、加算、スクリーンなどは将来仕様とする。

---

## 25. 完全なmanifest例

```json
{
  "livfVersion": "0.1.0",
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
      "name": "体",
      "source": "images/body.png",
      "visible": true,
      "x": 0,
      "y": 0,
      "opacity": 1.0
    },
    {
      "type": "folder",
      "id": "face",
      "name": "顔",
      "visible": true,
      "opacity": 1.0,
      "selectionMode": "multiple",
      "children": [
        {
          "type": "folder",
          "id": "face.eyes",
          "name": "目",
          "visible": true,
          "selectionMode": "exclusive",
          "defaultChild": "face.eyes.normal",
          "children": [
            {
              "type": "layer",
              "id": "face.eyes.normal",
              "name": "通常の目",
              "source": "images/eye_normal.png",
              "visible": true,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.eyes.smile",
              "name": "笑顔の目",
              "source": "images/eye_smile.png",
              "visible": false,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.eyes.angry",
              "name": "怒った目",
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
          "name": "口",
          "visible": true,
          "selectionMode": "exclusive",
          "defaultChild": "face.mouth.normal",
          "children": [
            {
              "type": "layer",
              "id": "face.mouth.normal",
              "name": "通常の口",
              "source": "images/mouth_normal.png",
              "visible": true,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.mouth.smile",
              "name": "笑顔の口",
              "source": "images/mouth_smile.png",
              "visible": false,
              "x": 0,
              "y": 0,
              "opacity": 1.0
            },
            {
              "type": "layer",
              "id": "face.mouth.angry",
              "name": "怒った口",
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
      "name": "衣装",
      "visible": true,
      "selectionMode": "exclusive",
      "defaultChild": "costume.standard",
      "children": [
        {
          "type": "layer",
          "id": "costume.standard",
          "name": "標準衣装",
          "source": "images/costume_standard.png",
          "visible": true,
          "x": 0,
          "y": 0,
          "opacity": 1.0
        },
        {
          "type": "layer",
          "id": "costume.school",
          "name": "制服",
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
      "name": "メガネ",
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
      "name": "通常",
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
      "name": "笑顔",
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
      "name": "怒り",
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
      "name": "標準衣装",
      "changes": [
        {
          "target": "costume.standard",
          "visible": true
        }
      ]
    },
    {
      "id": "costume.school.state",
      "name": "制服",
      "changes": [
        {
          "target": "costume.school",
          "visible": true
        }
      ]
    },
    {
      "id": "character.default",
      "name": "デフォルト",
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
      "name": "表情",
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
      "name": "衣装",
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

## 26. バリデーション

### 26.1 エラー

次の場合、LIVFファイルを不正とする。

- `manifest.json`が存在しない
- `livfVersion`が存在しない
- 未対応のメジャーバージョン
- Canvasサイズが0以下
- 必須プロパティが存在しない
- IDが重複している
- Layerの`source`が存在しない
- `source`がコンテナ外を参照している
- Stateの`target`が存在しない
- `defaultState`が存在しないStateを参照している
- StateGroupが存在しないStateを参照している
- StateGroupの`defaultState`が所属Stateではない
- Folderの`defaultChild`が直接の子ではない
- Node階層が循環している
- `exclusive` StateGroupに`targets`が存在しない
- 複数の`exclusive` StateGroupの管理範囲が重複している
- opacityが0.0未満または1.0より大きい

### 26.2 警告

次の場合は読み込みを継続できるが、警告を発生させる。

- Canvas外に完全にはみ出しているLayer
- 使用されていない画像
- どのStateからも参照されないLayer
- `exclusive` Folder内で複数の子が初期表示になっている
- Default Stateが何も変更しない
- Default State適用後にすべてのLayerが非表示になる
- 同じState内で同じNodeを複数回変更している
- 同じState内で排他的な複数Nodeを表示している

### 26.3 exclusive Folderの初期競合

`exclusive` Folder内で複数の子Nodeが表示状態だった場合、次の順番で解決する。

1. `defaultChild`が表示状態なら、それを残す
2. それ以外の場合、配列上で最後の表示Nodeを残す
3. ほかの子Nodeを非表示にする
4. 警告を記録する

---

## 27. エラー処理

LIVFランタイムは、エラーを種類別に識別できることを推奨する。

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

C#では、次のような専用例外を定義できる。

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

## 28. 読み込みモード

ランタイムは、厳格な読み込みと寛容な読み込みを提供してもよい。

```csharp
LivfLoadMode.Strict
LivfLoadMode.Lenient
```

### 28.1 Strict

- 不正な参照で読み込み失敗
- 不足画像で読み込み失敗
- 仕様違反をエラーとして扱う

### 28.2 Lenient

- 不足画像を透明画像として扱う
- 修正可能な排他競合を自動修正する
- 問題を警告として返す

構造的に解決できないエラーは、Lenientでも読み込みを失敗させる。

---

## 29. バージョン管理

LIVFのバージョン番号は次の形式とする。

```text
メジャー.マイナー.パッチ
```

例：

```text
0.1.0
0.1.1
0.2.0
1.0.0
```

### 29.1 メジャーバージョン

大きな機能追加、互換性のない仕様変更、破壊的変更などで増加する。

例：

- 座標系の変更
- 描画順序の変更
- manifest構造の大幅な変更
- State適用ルールの変更

### 29.2 マイナーバージョン

同じメジャーバージョン内で互換性を維持した機能追加・仕様拡張などで増加する。

例：

- 任意プロパティの追加
- 新しい画像形式
- 新しいブレンドモード
- 新しいmetadata項目

### 29.3 パッチバージョン

仕様の意図を変えない軽微な修正、誤記修正、明確化などで増加する。

### 29.4 未知のプロパティ

同じメジャーバージョン内では、ランタイムは未知の任意プロパティを無視できる。

未知の必須機能が必要な場合は、将来の仕様で機能宣言方法を定義する。

---

## 30. セキュリティ

### 30.1 外部参照

LIVF v0.1.0では、外部URLやコンテナ外のファイル参照を禁止する。

禁止例：

```text
../secret.png
C:/Users/example/image.png
https://example.com/image.png
file:///home/example/image.png
```

許可例：

```text
images/body.png
images/face/eye.png
```

### 30.2 ZIP Slip対策

展開先の外部へファイルを書き出すパスを拒否しなければならない。

### 30.3 リソース制限

ランタイムは、次の上限を設定できるものとする。

- 最大Canvasサイズ
- 最大Layer数
- 最大Node数
- 最大State数
- 最大階層深度
- 最大画像サイズ
- 最大展開後ファイルサイズ
- 最大ファイル数
- 最大メモリ使用量

制限を超えた場合は、`ResourceLimitExceeded`として読み込みを中止できる。

### 30.4 スクリプト

LIVF v0.1.0は、実行可能なスクリプトを含まない。

---

## 31. LIVF v0.1.0の必須対応機能

LIVF v0.1.0準拠ランタイムは、次の機能をサポートしなければならない。

- ZIPベースのコンテナ
- UTF-8の`manifest.json`
- PNG Layer
- Canvas
- Layer
- Folder
- Node階層
- 表示・非表示
- opacity
- `multiple` Folder
- `exclusive` Folder
- `defaultChild`
- State
- Stateのアトミック適用
- StateGroup
- StateGroupの排他選択
- ファイル全体のDefault State
- 暗黙的なDefault State
- Default Snapshot
- `ResetToDefault`
- 通常アルファ合成
- IDによるNode・State操作
- パス検証

---

## 32. LIVF v0.1.0で対応しない機能

次の機能はLIVF v0.1.0の対象外とする。

- レイヤーアニメーション
- フレームアニメーション
- ボーンアニメーション
- メッシュ変形
- Live2D形式の変形
- 物理演算
- マスク
- クリッピング
- 乗算・加算などのブレンドモード
- ベクター画像
- 動画
- 音声
- スクリプト
- 外部URL画像
- 暗号化
- DRM
- PSD編集機能の完全な置き換え

---

## 33. 推奨C#クラス構成

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

推奨プロジェクト構成：

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

## 34. 推奨API例

```csharp
using var character = LivfDocument.Load("character.livf");

// 読み込み直後はDefault State
Console.WriteLine(character.IsDefaultState); // true

// 表情を笑顔に変更
character.SetGroupState(
    "expression",
    "expression.smile"
);

// メガネを表示
character.SetVisible(
    "accessory.glasses",
    true
);

// 現在はDefault Stateではない
Console.WriteLine(character.IsDefaultState); // false

// 初期状態へ完全に戻す
character.ResetToDefault();

// 再びDefault State
Console.WriteLine(character.IsDefaultState); // true
```

---

## 35. LIVFの中心概念

LIVF形式の中心となる概念は、次の五つである。

### Layer

実際に描画する画像。

### Folder

LayerやFolderを階層化し、表示ルールを設定する要素。

### State

複数のNode変更をまとめた名前付き操作。

### StateGroup

表情や衣装など、関連するStateを管理するグループ。

### Default State

ファイルの標準表示を定義し、アプリケーションからいつでも完全に復元できる特別な状態。

これにより、アプリケーションは画像ファイル名や内部のLayer構造を直接意識せず、意味のあるIDで立ち絵を操作できる。

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
