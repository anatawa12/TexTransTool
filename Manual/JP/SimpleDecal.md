# SimpleDecal について

## このコンポーネントの概要

このコンポーネントは、非破壊的に紋章やマークをメッシュに対して貼り付けることを目的とした物で、貼り付けるテクスチャー次第で幅広いことができるコンポーネントです。

## 使い方

### 始めに

TexTransTool/Runtime/Decal にある SimpleDecalAvatarTag.cs から、
またはインスペクターのコンポーネントを追加の TexTransTool/SimpleDecal から
ゲームオブジェクトに追加できます。

### デカールの張り方

- 適当な GameObject に上記の方法でコンポーネントを付け
- デカールを張りたいメッシュを持つレンダラーを TargetRenderer にセット
- DecalTexture に張りたいテクスチャーをセット
- GameObject の位置、Scale、MaxDistance などを調整し

Preview ボタンを押すとそのデカールをプレビューすることができます。

### リアルタイムプレビュー

これは、Preview をしなくてもテクスチャの変更やブレンドモードの変更、位置や向き、サイズの変更をリアルタイムに確認できる機能で、完全に Preview をした結果と同じになり、細かい調整に使えて「EnableRealTimePreview」から使用できます。

注意点として、複数のデカールのプレビューができません。

## プロパティ

### TargetRenderer

ターゲットとなるレンダラーをセットするプロパティ。

### Multi Renderer Mode

複数レンダラーにまたがるデカールを貼れるようにするプロパティです。

### DecalTexture

貼り付けるデカールをセットするプロパティ。

### Color

デカールのテクスチャに乗算する色のプロパティで、色調整や透明度を設定できます。

### BlendType

デカールを元画像と合成するときの合成モードを選択するプロパティです。[詳細](BlendType.md)

### TargetPropertyName

デカールを張るテクスチャーをマテリアルのどのプロパティのテクスチャーにするかを選択するプロパティです。

### Scale

貼り付けるデカールのサイズを調整できるプロパティで、そのゲームオブジェクトのトランスフォーㇺの Scale の X,Y を変更します。

### Fixed Aspect

これのチェックが入っている場合、Scale の値が一つの float となり、画像のアスペクト比と同じ比率に、デカールを張り付ける範囲が調整されます。

これのチェックが外れている場合、Scale の値が二つの float となり、画像のアスペクト比を無視し、X を幅、Y を高さとして、デカールを張り付ける範囲が調整できます。

### Max Distance

貼り付けるデカールの最大奥行きを調整できるプロパティで、そのゲームオブジェクトのトランスフォームの Scale の Z を変更します。
頬などに張り付ける場合白目のメッシュに誤って張られてしまうのを防止できます。

### Polygon Culling

polygon をカリングする条件を調整できるプロパティです。
特に理由がない場合 Vertex をお勧めします。

- Vertex 頂点ベースでカリングします。
- Edge 辺ベースでカリングします。デカールを張る範囲に頂点が入らない場合に。
- EdgeAndCenterRey 辺ベースのカリングに加え中央から疑似的なレイキャストを行いカリングします。辺が一つも入らないほどポリゴンに比べてデカールを張り付ける範囲が小さい場合に。

### Side Culling

チェックが入っている場合、デカールを張る範囲に対して裏面である場合そのポリゴンがカリングされ、デカールが張られなくなります。

チェックが入っていない場合、裏面にもデカールが張られます。

髪の毛に対してグラデーションを入れるときチェックを外すことを想定した設定です。

### Island Culling

UVのひとまとまりにだけ、デカールを張れるようにする設定で、元のUVに依存しますが髪の毛のグラデーションを入れるときに使用すれば一房だけにグラデーションを入れるといった使い方ができる設定です。

この設定には注意点があり、[AtlasTexture](AtlasTexture.md)と同じように、これを設定して初回のPreviewが非常に遅く、フリーズする場合もあります。

この設定にチェックを入れて、リアルタイムプレビューを使用するときは、開発者の環境では20fpsほどしか出なくなるほどに重いのでご注意ください。

#### IslandSelectorPos x,y

Islandを選ぶRayの位置を調整するプロパティです。

#### Island Selector Range

Islandを選ぶRayの最大の長さを制限するプロパティで、奥のポリゴンが選ばれないようにするための設定です。

### Advanced Option

あまり使うことのない細かい設定です。

#### FastMode

デフォルトでチェックが入っている設定で、チェックを外すと遅くなる代わりにPaddingの計算がきれいになります。

#### Padding

Paddingの大きさを調整できる設定です。

#### SeparateMaterialAndTexture

MaterialとTextureを分割する設定で、同じマテリアルとテクスチャーを参照してる別のレンダラーに影響しなくなりますが、先に張られるほかのデカールを消してしまう場合があるのでしようにはご注意。

この設定を使用するとテクスチャーが新しく別のものになるため、VRAMも増加します。必要のない場合は基本チェックを外すことが推奨です。