using UnityEngine;


namespace EditorExtensions
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle topTooltipButtonStyle;

		static ToolbarStyles()
		{
			topTooltipButtonStyle = new GUIStyle("Command")
			{
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.TextOnly,
				fixedWidth = 100f,
				fixedHeight = 19f,
			};
		}
	}
}