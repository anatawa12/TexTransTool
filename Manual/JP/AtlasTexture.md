# AtlasTexture について

## このコンポーネントの概要

このコンポーネントは、複数のテクスチャー必要な部分だけまとめた一枚のテクスチャーにすることができ、いろいろなところから衣装を引っ張ってきて、テクスチャーの使用していない部分が多い時に必要な部分だけをまとめて、見た目に大きな影響を出さずに、VRAM 容量を削減することを目的としたコンポーネントです。

## 使い方

### 始めに

TexTransTool/Runtime/TextureAtlas にある AtlasTexture.cs から、
またはインスペクターのコンポーネントを追加の TexTransTool/AtlasTexture から
ゲームオブジェクトに追加できます。

### 基本的な設定

- アトラス化したい対象の親を TargetRoot に追加
- アトラス化したいテクスチャーを持つマテリアルのリストにチェックを入れる
- それぞれのマテリアルのサイズのオフセットを Automatic OffSet Setting で自動設定するか手動で調整
- AtlasSettingsを適宜調整。

### アトラス化する対象について

TargetRoot に追加したゲームオブジェクトの子にあるすべての SkindMeshRenderer と MeshRenderer の IsTarget のチェックが入ったマテリアルが対象となり、アトラス化されます。

## 注意点

このコンポーネントはVRAM容量を削減することを主な機能としていますが、場合や設定によっては増やしてしまう可能性があるため、取り扱いは少々上級者向けです。

このコンポーネントは初回のPreviewやUseIslandCash(下記参照)をオフにしてPreviewやアバタービルドを行う場合は非常に遅いため、フリーズします。

対象となるアバターのメッシュの重さに依存しますが、長いものだと一分以上かかる場合もあります。

## プロパティ

### TargetRoot

レンダラーの親のオブジェクトをセットするプロパティ。

### MaterialSelector And Scale , Channel

左から アトラス化の対象かどうか、そのテクスチャの大きさの調整、アトラス化するチャンネル。

存在しないはずのマテリアルが表示されている場合や、存在するはずのマテリアルがない場合は「Refresh Materials」を実行しましょう。

### Offset

マテリアルに対してテクスチャの大きさにオフセットをかける値で、小さく表示させるものや元のテクスチャが小さい場合それに応じて値を小さくするための調整に使います。

上に表示されている「Automatic OffSet Setting」を実行すれば、それらのテクスチャーの解像度に基づいた比率になり、特に理由がない場合はこれを使用することを推奨します。

### Channel

### *注意 ! : この機能は廃止予定です [Channel 1]以上を使用していた場合次期バージョンでコンポーネントの状態が大きく書き換えられるマイグレーションがされます。*

アトラス化のチャンネルというような感じで、そのマテリアルたちから二枚のテクスチャにアトラス化などをするためにあります。

通常一枚にまとめる使い方がメインなのであまり使うことはないかもしれません。

### UseIslandCash

メッシュの UV とトライアングルから UV のまとまり（アイランド）を計算するとき、キャッシュを使うかどうかのプロパティで、
基本的に、オンを推奨しますが、キャッシュを使用することで問題がある場合はオフにしてください。

キャッシュを使わないのではなく削除したい場合は、Assets/TexTransToolGenerates にある「IslandCash ~」と名前のついているオブジェクトを削除してください。


### AtlasTextureSize

アトラス化したテクスチャの解像度で X が横 Y 縦になります。

### IsMergeMaterial

チェックを入れると、強制的にマテリアルを一つに統合することができます。

マテリアル数の削減はできるため SetPassCall などの削減になりますが、マテリアルスロット数の削減はできないのでご注意ください。
スロット数を削減したい場合は[AvatarOptimizer](https://github.com/anatawa12/AvatarOptimizer)との併用をお勧めします。

見た目への影響が大きいので上級者向けで、使用の際はご注意ください。

#### Property Bake Setting

マテリアルをマージするとき、マテリアルの色変更などがマテリアルごとに違うとき、色変更をテクスチャに焼きこみ、見た目を維持したままマージをしやすくする設定です。

- NotBake 何もしない、IsMergeMaterialのチェックが外れているときは常にこれになります。
- Bake 通常設定、新しいテクスチャを生成しない範囲でプロパティをマージするために焼きこみを行います。
- BakeAllProperty すべてのプロパティをできるだけマージするために、新しいテクスチャを生成して見た目をできるだけ維持する設定です。ただし、VRAM容量が増加する可能性が高いため、注意が必要です。

#### MergeReferenceMaterial

新しく割り当てるマテリアルを指定することができるプロパティです。

### ForceSetTexture

チェックを入れると、マテリアルに対してアトラス化されたテクスチャーをすべて強制的にセットします。

チェックが入っていないときは、マテリアルのプロパティーにすでにテクスチャーが入っている場合そのプロパティーはアトラス化されたテクスチャーがセットされます。

### Padding

UV 同士の距離です。

### SortingType

アトラス化するときの自動生成 UV の並べ方を指定します。

- EvenlySpaced - 適当な順番でマス目にしたがって並べます。(非推奨)
- NextFitDecreasingHeight - 高さ順に左下から敷き詰めます。(非推奨)
- NextFitDecreasingHeightPlusFloorCeiling - 高さ順に左下から敷き詰め、左下からはいらなくなったら右上から入るものをできるだけ詰めます。(推奨)

もっと効率的な UV の並び替えアルゴリズムを実装できる方はは Issues を立てるか PullRequest を投げてくれると助かります。

### TextureFineTuning

[TextureFineTuningのマニュアル](TextureFineTuning.md)