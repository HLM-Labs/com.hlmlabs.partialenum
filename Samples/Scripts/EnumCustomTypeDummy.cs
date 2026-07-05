using UnityEngine;

namespace HLMLabs.PartialEnum.Runtime
{
    public class EnumCustomTypeDummy: MonoBehaviour
    {
        [SerializeField] private TypedEnum<EnumCustomType> _customType;

        private void Start()
        {
            Debug.Log("Currently selected Subscription Type: " + _customType);
        }
    }
}