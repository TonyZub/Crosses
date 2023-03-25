using UnityEngine;


namespace Crosses
{
	public sealed class ObjectFinder
	{
        #region Methods

        public static T FindObjectOfType<T>(bool includeInactive = false) where T : Behaviour
        {
            return GameObject.FindObjectOfType<T>(includeInactive);
        }

        public static T[] FindObjectsOfType<T>(bool includeInactive = false) where T : Behaviour
        {
            return GameObject.FindObjectsOfType<T>(includeInactive);
        }

        #endregion
    }
}