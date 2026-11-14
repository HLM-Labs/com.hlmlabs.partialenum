using HLMLabs.PartialEnum.Runtime;
using TMPro;
using UnityEngine;

namespace HLMLabs.PartialEnum.Samples
{
    public class EnumCustomTypeDummy : MonoBehaviour
    {
        [SerializeField] private TypedEnum<EnumCustomType> _customType;
        [SerializeField] private TMP_Text _sampleText;

        private void Start()
        {
            //use operators != or ==
            bool equalsA = _customType == EnumCustomType.ValueA;
            // get string value
            string getStringName = _customType.ToString();
            //access int value 
            int getIntValue = _customType.Value;
            
            string sampleText = $"Custom type == ValueA : {equalsA}, Custom type value :  {getIntValue},  Custom type name : {getStringName}";
            Debug.Log(sampleText);
            _sampleText.text = sampleText;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                string str  = "Currently selected Custom Type: " + _customType;
                Debug.Log(str);
                _sampleText.text = str;
            }
        }
    }
}
