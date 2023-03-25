using UnityEngine;


namespace Crosses
{
	public sealed class PrimitiveFactory
	{
		#region Methods

		public static Object CreateEmptyObject()
        {
			return new Object();
        }

		public static GameObject CreateEmptyGameObject()
		{
			return new GameObject();
		}

		public static Object Instantiate(Object objectToCreate)
        {
			return Object.Instantiate(objectToCreate);
        }

		public static Object Instantiate(Object objectToCreate, Transform parent)
        {
			return Object.Instantiate(objectToCreate, parent);
		}

		#endregion
	}
}