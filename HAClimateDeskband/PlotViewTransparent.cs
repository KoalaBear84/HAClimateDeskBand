using OxyPlot.WindowsForms;
using System.Windows.Forms;

namespace HAClimateDeskband
{
    /// <summary>
    /// See https://github.com/oxyplot/oxyplot/issues/1670
    /// </summary>
    public class PlotViewTransparent : PlotView
    {
        public PlotViewTransparent() : base()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
