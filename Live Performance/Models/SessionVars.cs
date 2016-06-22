using System.Web;

namespace Live_Performance.Models
{
    /// <summary>
    ///     Variables that can be used in <see cref="HttpSessionStateBase">HttpContext.Session</see>.
    /// </summary>
    public struct SessionVars
    {
        /// <summary>
        ///     The logged in user.
        /// </summary>
        public const string User = "user";
    }
}