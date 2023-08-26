#if UNITY_EDITOR
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace net.rs64.TexTransTool.Island
{
    [Serializable]
    public class IslandCacheObject
    {
        public byte[] Hash;
        public List<Island> Islands;

        public IslandCacheObject(byte[] hash, List<Island> islands)
        {
            Hash = hash;
            Islands = islands;
        }
        public IslandCacheObject(List<TraiangleIndex> Triange, List<Vector2> UV, List<Island> Island)
        {
            SetData(Triange, UV, Island);
        }
        public void SetData(List<TraiangleIndex> Triange, List<Vector2> UV, List<Island> Island)
        {
            Islands = Island;

            Hash = GenereatHash(Triange, UV);
        }

        public static byte[] GenereatHash(IReadOnlyList<TraiangleIndex> Triange, IReadOnlyList<Vector2> UV)
        {
            var datajson = JsonUtility.ToJson(new TrainagleAndUVpeas(new List<TraiangleIndex>(Triange), UV));
            byte[] data = System.Text.Encoding.UTF8.GetBytes(datajson);

            return SHA1.Create().ComputeHash(data);
        }

        [Serializable]
        class TrainagleAndUVpeas
        {
            public List<TraiangleIndex> Triange;
            public List<Vector2> UV;

            public TrainagleAndUVpeas(List<TraiangleIndex> triange, List<Vector2> uV)
            {
                Triange = triange;
                UV = uV;
            }
            public TrainagleAndUVpeas(IReadOnlyList<TraiangleIndex> triange, IReadOnlyList<Vector2> uV)
            {
                Triange = triange.ToList();
                UV = uV.ToList();
            }
        

        }
    }


    public class IslandCache : ScriptableObject
    {
        public IslandCacheObject CacheObject;
    }
}
#endif