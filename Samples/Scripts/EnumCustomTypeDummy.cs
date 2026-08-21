using HLMLabs.PartialEnum.Runtime;
using UnityEngine;

namespace HLMLabs.PartialEnum.Samples
{
    public class EnumCustomTypeDummy : MonoBehaviour
    {
        [SerializeField] private TypedEnum<EnumCustomType> _customType;

        private void Start()
        {
            Debug.Log("Currently selected Custom Type: " + _customType);

            if (_customType == EnumCustomType.ValueA)
                Debug.Log("Value A selected");
        }
    }
}
