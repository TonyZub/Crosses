using UnityEngine;


namespace Crosses
{
    public sealed partial class Data
    {
        #region Fields

        private static Data _data;
        private static UIData _uiData;

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

        #endregion


        #region Constructor

        private Data() { }

        #endregion
    }
}

