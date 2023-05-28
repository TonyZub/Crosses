using System.Collections.Generic;
using System;


namespace Crosses
{
	[Serializable]
	public sealed class ResearchData
	{
		#region Fields

		// Presonal data
		public DateTime TimeStamp;
		public string UserName;
		public string UserSex;
		public int UserAge;

		// Rounds data
		public List<(GameSides, float)> RoundsInfo;
		public int RoundsAmount;
		public int RoundsWon;
		public int RoundsLost;
		public int RoundsDraw;
		public float TotalGameTime;
		public float AverageRoundTime;

		// Transitional data
		public bool HasWatchedVideo;
		public string VideoFeedback;

		// Methodic data
		public List<int> MainMethodicAnswers;
		public float HealthResult;
		public float ActivityResult;
		public float MoodResult;
		public List<int> SecondMethodicAnswers;

		// Feedback data
		public string Email;
		public string Feedback;

		#endregion


        #region Constructor

		public ResearchData()
        {
			TimeStamp = DateTime.Now;
			RoundsInfo = new List<(GameSides, float)>();
			MainMethodicAnswers = new List<int>();
			SecondMethodicAnswers = new List<int>();;
		}

        #endregion
	}
}