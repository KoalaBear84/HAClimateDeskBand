namespace HAClimateDeskband
{
    using System.Windows.Forms;

    /// <summary>
    /// Class used to execute code on different thread, mostly used to execute code on form thread
    /// </summary>
    /// <example>
    /// // Synchronous, ex. for retrieving data from form
    /// ControlsHelper.SyncInvoke(this, delegate
    /// {
    ///     variableName = this.Textbox.Text();
    /// });
    /// // Asynchronous, ex. for executing 'display' tasks on form
    /// ControlsHelper.SyncBeginInvoke(this, delegate
    /// {
    ///     this.LabelDisplay.Text = "Progress";
    /// });
    /// </example>
    public static class ControlsHelper
    {
        /// <summary>
        /// Asynchronously execute code on different thread, mostly used to execute code on form thread
        /// </summary>
        /// <param name="control">The control you need to access, mostly 'this'</param>
        /// <param name="methodInvoker">The delegate you want to access, anonymous is also possible</param>
        public static void SyncBeginInvoke(Control control, MethodInvoker methodInvoker)
        {
            if (control != null && control.InvokeRequired)
            {
                control.BeginInvoke(methodInvoker, null);
            }
            else
            {
                methodInvoker();
            }
        }

        /// <summary>
        /// Synchronously execute code on different thread, mostly used to execute code on form thread
        /// </summary>
        /// <param name="control">The control you need to access, mostly 'this'</param>
        /// <param name="methodInvoker">The delegate you want to access, anonymous is also possible</param>
        public static void SyncInvoke(Control control, MethodInvoker methodInvoker)
        {
            if (control != null && control.InvokeRequired)
            {
                control.Invoke(methodInvoker, null);
            }
            else
            {
                methodInvoker();
            }
        }
    }
}