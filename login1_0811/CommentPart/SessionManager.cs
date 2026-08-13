using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentPart
{
    internal class SessionManager
    {
        public static string CurrentUserId { get; set; }
        public static string CurrentUserName { get; set; }

        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(CurrentUserId);
        }
    }
}
