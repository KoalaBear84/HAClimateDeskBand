using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HAClimateDeskband
{
    [ComVisible(true)]
    [Guid("CE5E3E5C-C896-4963-99C9-0DF948BA092B")]
    [CSDeskBand.CSDeskBandRegistration(Name = "HA Climate DeskBand", ShowDeskBand = true)]
    public class HAClimateDeskBand : CSDeskBand.CSDeskBandWin
    {
        HAClimateUserControl HomeAssistantUserControl { get; set; } = new HAClimateUserControl();

        public HAClimateDeskBand()
        {
            Options.HorizontalSize.Width = Options.MinHorizontalSize.Width = HomeAssistantUserControl.Width;
            Options.VerticalSize.Width = Options.MinVerticalSize.Width = HomeAssistantUserControl.Height;
        }

        protected override Control Control => HomeAssistantUserControl;
    }
}
