using UnityEngine;

namespace LightFencing.Core.Interactions
{
    public interface IDeviceTransformProvider
    {
        Transform GetHeadTransform();
        Transform GetControllerTransform(Handedness handedness);
    }
}
