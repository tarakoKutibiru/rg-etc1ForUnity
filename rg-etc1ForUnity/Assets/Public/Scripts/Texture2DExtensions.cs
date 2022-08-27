using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TarakoKutibiru.RG_ETC1.Runtime
{
    public static class Texture2DExtensions
    {
        public static byte[] EncodeToETC(this Texture2D self, Quality quality = Quality.Med, bool dithering = false)
        {
            return RgEtc1.EncodeToETC(self.GetPixels32(), self.width, self.height, quality, dithering);
        }
    }
}
