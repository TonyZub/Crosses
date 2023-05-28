using UnityEngine;
using System.Linq;


namespace Crosses
{
	public sealed class ResearchDataService
	{
        #region Fields

        private readonly ResearchData _data;

        #endregion


        #region Properties

        public ResearchData Data => _data;

        #endregion


        #region Constructor

        public ResearchDataService()
        {
            _data = new ResearchData();
        }

        #endregion


        #region Methods

        public ResearchDataService SetUserName(string name)
        {
            _data.UserName = name;
            return this;
        }

        public ResearchDataService SetUserSex(string sex)
        {
            _data.UserSex = sex;
            return this;
        }

        public ResearchDataService SetUserAge(int age)
        {
            _data.UserAge = age;
            return this;
        }

        public ResearchDataService AddRoundInfo(GameSides winner, float roundTime)
        {
            _data.RoundsInfo.Add((winner, roundTime));
            return this;
        }

        public ResearchDataService CountRoundsInfo()
        {
            _data.RoundsAmount = _data.RoundsInfo.Count;
            _data.RoundsWon = _data.RoundsInfo.Where(x => x.Item1 == GameSides.Player).Count();
            _data.RoundsLost = _data.RoundsInfo.Where(x => x.Item1 == GameSides.Computer).Count();
            _data.RoundsDraw = _data.RoundsInfo.Where(x => x.Item1 == GameSides.None).Count();
            _data.TotalGameTime = _data.RoundsInfo.Select(x => x.Item2).Aggregate((a, b) => a + b);
            _data.AverageRoundTime = _data.TotalGameTime / _data.RoundsAmount;
            return this;
        }

        public ResearchDataService SetVideoFeedback(string feedback)
        {
            _data.HasWatchedVideo = true;
            _data.VideoFeedback = feedback;
            return this;
        }

        public ResearchDataService AddFirstMethodicAnswer(int answer)
        {
            _data.MainMethodicAnswers.Add(answer);
            return this;
        }

        public ResearchDataService SetFirstMethodicResults(float health, float activity, float mood)
        {
            _data.HealthResult = health;
            _data.ActivityResult = activity;
            _data.MoodResult = mood;
            return this;
        }

        public ResearchDataService AddSecondMethodicAnswer(int answer)
        {
            _data.SecondMethodicAnswers.Add(answer);
            return this;
        }

        public ResearchDataService SetEmail(string email)
        {
            _data.Email = email;
            return this;
        }

        public ResearchDataService SetTotalFeedback(string feedback)
        {
            _data.Feedback = feedback;
            return this;
        }

        public string GetDataJSON()
        {
            return JsonUtility.ToJson(Data);
        }

        #endregion
    }
}