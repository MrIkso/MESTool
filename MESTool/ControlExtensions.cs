using System.Reflection;
using System.Windows.Forms;

namespace MESTool
{
    public static class ControlExtensions
    {
        public static void SetDoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (doubleBufferPropertyInfo != null)
            {
                doubleBufferPropertyInfo.SetValue(control, enable, null);
            }
        }
    }
}
