using UnityEngine;
using System;


namespace Crosses
{
	[Serializable]
	public struct MethodicElement
	{
		#region Fields

		[SerializeField] private MethodicElementsParts _elementPart;
		[SerializeField] private string _highParameter;
		[SerializeField] private string _lowParameter;
		[SerializeField] private bool _isReversed;

		#endregion


		#region Properties

		public MethodicElementsParts ElementPart => _elementPart;
		public string HightParameter => _highParameter;
		public string LowParameter => _lowParameter;
		public bool IsReversed => _isReversed;

		#endregion
	}
}