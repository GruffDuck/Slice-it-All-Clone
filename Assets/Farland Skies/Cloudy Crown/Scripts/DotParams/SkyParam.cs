using System;
using Borodar.FarlandSkies.Core.DotParams;
using UnityEngine;

namespace Borodar.FarlandSkies.CloudyCrownPro.DotParams
{
    [Serializable]
    public class SkyParam : DotParam
    {
        public Color TopColor = Color.gray;
        public Color BottomColor = Color.gray;
    }
}