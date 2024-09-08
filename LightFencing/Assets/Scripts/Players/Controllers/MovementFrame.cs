using UnityEngine;

namespace LightFencing.Utils
{
    public class MovementFrame
    {
        public long Timestamp;
        public SerializableVector3 HeadPosition;
        public SerializableVector3 BodyPosition;
        public SerializableVector3 HeadRotation;
        public SerializableVector3 BodyRotation;
        public SerializableVector3 LeftHandPosition;
        public SerializableVector3 RightHandPosition;
        public SerializableVector3 LeftHandRotation;
        public SerializableVector3 RightHandRotation;

        public MovementFrame()
        {
        }

        public MovementFrame(
            long timestamp,
            SerializableVector3 headPosition,
            SerializableVector3 bodyPosition,
            SerializableVector3 headRotation,
            SerializableVector3 bodyRotation,
            SerializableVector3 leftHandPosition,
            SerializableVector3 rightHandPosition,
            SerializableVector3 leftHandRotation,
            SerializableVector3 rightHandRotation)
        {
            Timestamp = timestamp;
            HeadPosition = headPosition;
            BodyPosition = bodyPosition;
            HeadRotation = headRotation;
            BodyRotation = bodyRotation;
            LeftHandPosition = leftHandPosition;
            RightHandPosition = rightHandPosition;
            LeftHandRotation = leftHandRotation;
            RightHandRotation = rightHandRotation;
        }

        public MovementFrame(long timestamp, Transform referenceTransform, Transform headTransform, Transform bodyTransform, Transform leftHandTransform, Transform rightHandTransform)
        {
            Timestamp = timestamp;
            HeadPosition = referenceTransform.InverseTransformPoint(headTransform.position);
            BodyPosition = referenceTransform.InverseTransformPoint(bodyTransform.position);
            RightHandPosition = referenceTransform.InverseTransformPoint(rightHandTransform.position);
            LeftHandPosition = referenceTransform.InverseTransformPoint(leftHandTransform.position);

            HeadRotation = (Quaternion.Inverse(referenceTransform.rotation) * headTransform.rotation).eulerAngles;
            BodyRotation = (Quaternion.Inverse(referenceTransform.rotation) * bodyTransform.rotation).eulerAngles;
            RightHandRotation = (Quaternion.Inverse(referenceTransform.rotation) * rightHandTransform.rotation).eulerAngles;
            LeftHandRotation = (Quaternion.Inverse(referenceTransform.rotation) * leftHandTransform.rotation).eulerAngles;
        }
    }
}
