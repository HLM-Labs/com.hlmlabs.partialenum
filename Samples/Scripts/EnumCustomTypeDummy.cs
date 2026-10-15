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
            //use operators != or ==
            bool equalsA = _customType == EnumCustomType.ValueA;
            // get string value
            string getStringName = _customType.ToString();
            //access int value 
            int getIntValue = _customType.Value;
            
            Debug.Log($"Custom type == ValueA : {equalsA}, Custom type value :  {getIntValue},  Custom type name : {getStringName}");
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
