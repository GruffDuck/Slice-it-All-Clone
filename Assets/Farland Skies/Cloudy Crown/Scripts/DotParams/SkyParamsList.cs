using System;
using Borodar.FarlandSkies.Core.DotParams;
using UnityEngine;

namespace Borodar.FarlandSkies.CloudyCrownPro.DotParams
{
    [Serializable]
    public class SkyParamsList : SortedParamsList<SkyParam>
    {
        public SkyParam GetParamPerTime(float currentTime)
        {
            if (SortedParams.Count <= 0)
            {
                Debug.LogWarning("Sky params list is empty");
                SortedParams.Add(0, new SkyParam());
            }

            var index = SortedParams.FindIndexPerTime(currentTime);

            if (index < 1) index = SortedParams.Count;

            var timeKey1 = SortedParams.Keys[index - 1];
            var value = SortedParams.Values[index - 1];
            var topColor1 = value.TopColor;
            var bottomColor1 = value.BottomColor;

            if (index >= SortedParams.Count) index = 0;
             
            var timeKey2 = SortedParams.Keys[index];
            value = SortedParams.Values[index];
            var topColor2 = value.TopColor;
            var bottomColor2 = value.BottomColor;

            var t1 = (currentTime > timeKey1) ?  currentTime - timeKey1 : currentTime + (100f - timeKey1);
            var t2 = (timeKey1 < timeKey2) ? timeKey2 - timeKey1 : 100f + timeKey2 - timeKey1;
            var t = t1/t2;

            var currentParam = new SkyParam
            {
                TopColor = Color.Lerp(topColor1, topColor2, t),
                BottomColor = Color.Lerp(bottomColor1, bottomColor2, t),
            };

            return currentParam;
        }
    }
}