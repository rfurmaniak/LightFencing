using Newtonsoft.Json;
using System;
using UnityEngine;

namespace LightFencing.Utils
{
    public struct SerializableVector3
    {
        public const int DecimalPlaces = 4;

        /// <summary>
        /// x component
        /// </summary>
        [JsonConverter(typeof(DecimalTruncateConverter))]
        public float x;

        /// <summary>
        /// y component
        /// </summary>
        [JsonConverter(typeof(DecimalTruncateConverter))]
        public float y;

        /// <summary>
        /// z component
        /// </summary>
        [JsonConverter(typeof(DecimalTruncateConverter))]
        public float z;

        /// <summary>
        /// Creates new serializable vector. By default it will be rounded to <paramref name="decimalPlaces"/> 
        /// Pass negative number to keep whole value.
        /// </summary>
        /// <param name="decimalPlaces">Number of decimal places to round the number to.
        /// Pass negative number to keep whole value</param>
        public SerializableVector3(float rX, float rY, float rZ, int decimalPlaces = DecimalPlaces)
        {
            if (decimalPlaces >= 0)
            {
                x = (float)Math.Round(rX, decimalPlaces);
                y = (float)Math.Round(rY, decimalPlaces);
                z = (float)Math.Round(rZ, decimalPlaces);
            }
            else
            {
                x = rX;
                y = rY;
                z = rZ;
            }
        }

        /// <summary>
        /// Creates new serializable vector. By default it will be rounded to <paramref name="decimalPlaces"/> 
        /// Pass negative number to keep whole value.
        /// </summary>
        /// <param name="decimalPlaces">Number of decimal places to round the number to.
        /// Pass negative number to keep whole value</param>
        public SerializableVector3(Vector3 vector3, int decimalPlaces = DecimalPlaces) : this(vector3.x, vector3.y, vector3.z, decimalPlaces)
        {
        }

        /// <summary>
        /// Converts formatted vector string into serializable vector class. <br></br>
        /// Components need to be separated by space
        /// </summary>
        public SerializableVector3(string vectorString)
        {
            var data = vectorString.Split(' ');
            x = float.Parse(data[0], NumberFormats.StandardNumberFormat);
            y = float.Parse(data[1], NumberFormats.StandardNumberFormat);
            z = float.Parse(data[2], NumberFormats.StandardNumberFormat);
        }

        /// <summary>
        /// Constructor from color - RGB is mapped into XYZ
        /// </summary>
        public SerializableVector3(Color color)
        {
            x = color.r;
            y = color.g;
            z = color.b;
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        public override string ToString()
        {
            return string.Format(NumberFormats.StandardNumberFormat, "{0} {1} {2}", x, y, z);
        }

        /// <summary>
        /// Automatic conversion from SerializableVector3 to Vector3
        /// </summary>
        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SerializableVector3
        /// </summary>
        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3(rValue.x, rValue.y, rValue.z);
        }

        /// <summary>
        /// Automatic conversion from string to SerializableVector3
        /// </summary>
        public static explicit operator SerializableVector3(string vectorString)
        {
            return new SerializableVector3(vectorString);
        }

        /// <summary>
        /// Automatic conversion from SerializableVector3 to color. XYZ is mapped into RGB components
        /// </summary>
        public static implicit operator Color(SerializableVector3 vec)
        {
            return new Color(vec.x, vec.y, vec.z);
        }

        public static implicit operator Quaternion(SerializableVector3 vec)
        {
            return Quaternion.Euler(vec);
        }

        public static SerializableVector3 VecToSerial(Vector3 vec, int decimalPlaces = DecimalPlaces)
        {
            return new SerializableVector3(vec.x, vec.y, vec.z, decimalPlaces);
        }
    }
}