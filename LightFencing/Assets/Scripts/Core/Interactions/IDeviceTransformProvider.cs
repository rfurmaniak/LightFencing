using Unity.XR.CoreUtils;
using UnityEngine;

namespace LightFencing.Core.Interactions
{
    public interface IDeviceTransformProvider
    {
        XROrigin Origin { get; }
        Transform GetHeadTransform();
        Transform GetControllerTransform(Handedness handedness);
    }
}
