using UnityEngine;


namespace Crosses
{
    public sealed partial class Data
    {
        #region Fields

        private static Data _data;
        private static UIData _uiData;
        private static MethodicData _methodicData;

        #endregion


        #region Properties

        public static Data Instance
        {
            get
            {
                if (_data == null)
                {
                    _data = new Data();
                }
                return _data;
            }
        }

        public static UIData UIData
        {
            get
            {
                if (_uiData == null)
                {
                    _uiData = Resources.Load<UIData>(typeof(UIData).Name);
                }
                return _uiData;
            }
        }

        public static MethodicData MethodicData
        {
            get
            {
                if(_methodicData == null)
                {
                    _methodicData = Resources.Load<MethodicData>(typeof(MethodicData).Name);
                }
                return _methodicData;
            }
        }

        #endregion


        #region Constructor

        private Data() { }

        #endregion
    }
}

