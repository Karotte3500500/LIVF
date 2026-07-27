# LIVF — Layered Image Variant Format

**日本語** | [English](README.md)

LIVFは、キャラクターの立ち絵など、複数の差分を切り替えて表示する画像向けに設計された、実験的なレイヤー画像コンテナ形式です。

LIVFファイルは、画像レイヤー、フォルダー、表示ルール、名前付き状態、再現可能なデフォルト状態を、一つの`.livf`コンテナにまとめて保存します。

## プロジェクトの状態

> [!WARNING]
> LIVFは現在開発中であり、まだ本番環境での利用は推奨していません。

- LIVF v0.1仕様：凍結済み
- C#参照実装：開発中
- Unityランタイム：開発予定
- Webランタイム：開発予定
- エディターおよびPSDインポーター：開発予定

初期参照実装の開発と検証が完了するまで、v0.1仕様は変更しない方針です。

## 目標

LIVFは、アプリケーションが画像ファイル名やPSDなどの編集ソフト固有形式へ直接依存せず、レイヤー付きのキャラクター画像を簡単に管理できるようにすることを目標としています。

主な用途として、次を想定しています。

- ノベルゲームの立ち絵
- RPGやアドベンチャーゲームの会話用キャラクター画像
- 表情差分や衣装差分の管理
- 簡易的な配信用アバター
- Web上で使用するレイヤー付きキャラクター画像

LIVFは、Live2Dのようなアニメーションシステムを置き換えるものではありません。
v0.1では、静止画像レイヤーの切り替えと組み合わせに重点を置きます。

## LIVF v0.1の機能

- ZIPベースの`.livf`コンテナ
- UTF-8の`manifest.json`
- PNG画像レイヤー
- LayerとFolderによる階層構造
- 表示・非表示と不透明度の制御
- `multiple`および`exclusive` Folder
- 名前付きState
- StateGroup
- ファイル全体のDefault State
- Default Snapshotとリセット処理
- 決定的なレイヤー描画順
- 通常アルファ合成
- IDを使用したランタイム操作

## 使用例

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

> 上記のAPIは、予定している参照APIの例です。現在はまだ実装されていません。

## リポジトリ構成

```text
livf/
├─ docs/       仕様書および開発文書
├─ schemas/    JSON Schema
├─ src/        LIVFライブラリ
├─ tools/      CLIおよび開発ツール
└─ tests/      自動テストおよびLIVFテストデータ
```

## 開発予定のコンポーネント

- `Livf.Core`
- `Livf.Serialization`
- `Livf.Validation`
- `Livf.Rendering`
- `Livf.Cli`
- Unityランタイム
- Webランタイム
- LIVFエディター
- PSDインポーター

## バージョン管理

LIVFファイル形式のバージョンと、参照実装のバージョンは別々に管理します。

ファイル形式バージョンの例：

```json
{
  "livfVersion": "0.1"
}
```

参照実装バージョンの例：

```text
0.1.0-alpha.1
```

## コントリビューション

参照実装の開発が進んだ段階で、コントリビューションガイドラインを追加する予定です。

仕様への提案、実装へのフィードバック、テストケース、バグ報告などを受け付けられるプロジェクトを目指します。

## ライセンス

LIVFは、[Apache License 2.0](LICENSE)の下で公開されています。

日本語での補助説明は、[LICENSE.ja.md](LICENSE.ja.md)を参照してください。
正式なライセンス条件は、英語の`LICENSE`が優先されます。
