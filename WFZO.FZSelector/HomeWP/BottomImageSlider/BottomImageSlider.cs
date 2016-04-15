using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace WFZO.FZSelector.HomeWP.BottomImageSlider
{
    [ToolboxItemAttribute(false)]
    public class BottomImageSlider : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WFZO.FZSelector.HomeWP/BottomImageSlider/BottomImageSliderUserControl.ascx";

        public const int DefaultTime = 1000;
        public static int TimeB;
        [Category("Slider Time Interval"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        DefaultValue(DefaultTime),
        WebDisplayName("Time Interval"),
        WebDescription("This Accepts Time Interval Input")]
        public int _TimeInterval
        {
            get { return TimeB; }
            set { TimeB = value; }
        }
        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
