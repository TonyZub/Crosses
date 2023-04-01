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

        #endregion


        #region Fields

        private readonly bool _isListening;

        #endregion


        #region Constructor

        public CheatController(bool isListening)
        {
            _isListening = isListening;
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
            if(_isListening && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.T))
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

        #endregion
    }
}