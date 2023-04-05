using UnityEngine;
using System;


namespace Crosses
{
	public sealed class CheatController
	{
        #region Constants

        public const float MESSAGE_DELAY_TIME = 3f;

        #endregion


        #region Events

        public event Action<GameSides> ChangedFirstTurn;
        public event Action RoundRestarted;
        public event Action DifficultyChanged;

        #endregion


        #region Constructor

        public CheatController()
        {
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
            GlobalController.Instance.OnUpdate += CheckCheat;
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging -= UnsubscribeEvents;
            GlobalController.Instance.OnUpdate -= CheckCheat;
        }

        private void CheckCheat()
        {
            CheckFirstTurnChange();
            CheckRoundRestart();
            CheckDifficultyChange();
        }

        private void CheckFirstTurnChange()
        {
            if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.T))
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    ChangedFirstTurn?.Invoke(GameSides.Player);
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    ChangedFirstTurn?.Invoke(GameSides.Computer);
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    ChangedFirstTurn?.Invoke(GameSides.None);
                }
            }
        }

        private void CheckRoundRestart()
        {
            if(Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    RoundRestarted?.Invoke();
                }              
            }
        }

        private void CheckDifficultyChange()
        {
            if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.I))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    DifficultyChanged?.Invoke();
                }
            }
        }

        #endregion
    }
}