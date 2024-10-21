#region Using statements

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Bitgem.VFX.StylisedWater
{
    public class WateverVolumeFloater : MonoBehaviour
    {
        #region Public fields

        public WaterVolumeHelper WaterVolumeHelper = null;
        public bool ShouldFollowWaterSurface = true;
        #endregion

        #region MonoBehaviour events

        void Update()
        {
            var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper.Instance;
            if (!instance)
            {
                return;
            }

            if(ShouldFollowWaterSurface)
                transform.position = new Vector3(transform.position.x, instance.GetHeight(transform.position) ?? transform.position.y, transform.position.z);
        }

        #endregion
    }
}