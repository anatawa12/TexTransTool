# TextureFineTuning について

## 存在意義

AtlasTexture でアトラス化したテクスチャーをそのまま使うといろいろと困ることがあります

例えば、VRAM 容量が増加するとか MipMap 必要ないテクスチャーにミップマップが生成されるなど。

そういった細かい調整を行うための物です。

## 共通することの多い設定

### TargetPropertyName

それら設定を行うマテリアルのプロパティの選択。

これは FineSetting だけの特別設定ですが、UseCustomProperty を使用してスペース区切りで複数の Property を指定することができます。

### Select;

Equal , Not Equal のどちらかを選ぶことができ、Equal だと TargetPropertyName で指定した物を対象に、Not Equal の場合 TargetPropertyName でしていたもの以外を対象にすることができます。

## Resize

指定したテクスチャの改造を変更する設定。

メインテクスチャー以外のテクスチャーはあまり大きな解像度がなくても問題がないことが多いのでテクスチャーのサイズを小さくしたほうが VRAM 容量の圧縮にもなるため、そのような場合に使うことを想定した設定です。

- Size リサイズ後のサイズ、 512 や 128 などの二のべき乗の値で適切な値を。

## Compress

指定したテクスチャの圧縮を指定できる設定。

アトラス化したテクスチャはデフォルトで NormalQuality のフォーマットと圧縮を適応しますが、メインテクスチャーは高品質に、ほかは低品質設定する、などの使い方を想定していますが、圧縮設定はVRAM容量に大きく影響するので取り扱いには注意。

### FormatQuality , CompressionQuality

完全に同じではないですが、UnityのTexture2Dのインポート設定と同じような設定にしています。[参照](https://docs.unity3d.com/ja/2019.4/Manual/class-TextureImporterOverride.html)

## ReferenceCopy

sourceとtargetを指定し、targetにsourceのテクスチャを割り当てる設定です。

メインテクスチャーをアウトラインテクスチャーに割り当てればアウトラインテクスチャーの分VRAMが節約できる...のような場合に使う設定です。

### Source Property Name , Target Property Name

それぞれコピーする元と、コピーされる先です。

ちなみにこの設定では、スペース区切りのプロパティの複数指定を行うことはできません。

## Remove

指定したテクスチャを消す設定です。

テクスチャを消せばVRAM削減になりますよね！使う場合はほんとに消して問題ないものなのかをしっかり考えてから設定しましょう。

## MipMapRemove

指定したテクスチャのミップマップを削除します。

アトラス化したテクスチャはデフォルトでミップマップが生成されますが、マスクテクスチャなどのテクスチャはミップマップが無くても見た目に影響がないことが多いのでそういった場合削除したほうがVRAM削減になりますよね！って使い方を想定した設定です。