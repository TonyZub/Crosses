#if UNITY_EDITOR
using UnityEngine;


namespace Crosses
{
    public sealed class ArrayElementTitleAttribute : PropertyAttribute
    {
        public string Varname;
        public ArrayElementTitleAttribute(string ElementTitleVar)
        {
            Varname = ElementTitleVar;
        }
    }
}
#endif