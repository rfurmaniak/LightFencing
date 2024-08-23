using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LightFencing.Core.Interactions
{
    public class XRControllerProvider : MonoBehaviour, IDeviceTransformProvider
    {
        [SerializeField]
        private XRBaseController leftController;

        [SerializeField]
        private XRBaseController rightController;

        [SerializeField]
        private Camera deviceCamera;

        public Transform GetControllerTransform(Handedness handedness)
        {
            return handedness == Handedness.Left ? leftController.transform : rightController.transform;
        }

        public Transform GetHeadTransform()
        {
            return deviceCamera.transform;
        }
    }
}