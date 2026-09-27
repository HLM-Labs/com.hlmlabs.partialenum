using System;
using HLMLabs.PartialEnum.Runtime;
using UnityEngine;

namespace HLMLabs.PartialEnum.Samples
{
    public class EnumCustomTypeDummy : MonoBehaviour
    {
        [SerializeField] private TypedEnum<EnumCustomType> _customType;

        private void Start()
        {
            // example of operator usage with TypedEnum
            if (_customType == EnumCustomType.ValueA)
                Debug.Log("Value A selected");
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Debug.Log("Currently selected Custom Type: " + _customType);
            }
        }
    }
}
