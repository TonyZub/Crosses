namespace Crosses
{
	public sealed class GlobalServices : IDispose
	{
		#region Fields

		private readonly ScreenOrientationService _screenOrientationService;
		private readonly ResearchDataService _researchDataService;

		#endregion


		#region Properties

		public ScreenOrientationService ScreenOrientationService => _screenOrientationService;
		public ResearchDataService ResearchDatService => _researchDataService;

		#endregion


		#region Constructor

		public GlobalServices()
        {
			_screenOrientationService = new ScreenOrientationService();
			_researchDataService = new ResearchDataService();
		}

        #endregion


        #region IDispose

		public void OnDispose()
        {
			_screenOrientationService.OnDispose();
        }

        #endregion
    }
}