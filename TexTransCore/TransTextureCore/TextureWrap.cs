using System;
using UnityEngine;

namespace net.rs64.TexTransCore.TransTextureCore
{
    public class TextureWrap
    {
        public WrapMode Mode;
        public Vector2? WarpRange;
        public enum WrapMode
        {
            Stretch = TextureWrapMode.Clamp,
            Loop = TextureWrapMode.Repeat,
        }

        public TextureWrap(WrapMode mode, Vector2? warpRange)
        {
            this.Mode = mode;
            this.WarpRange = warpRange;
        }

        public TextureWrapMode ConvertTextureWrapMode => (TextureWrapMode)Mode;

        public static TextureWrap NotWrap => new TextureWrap(WrapMode.Stretch, Vector2.zero);
        public static TextureWrap Loop => new TextureWrap(WrapMode.Loop, null);
        public static TextureWrap Stretch => new TextureWrap(WrapMode.Stretch, null);
    }

}