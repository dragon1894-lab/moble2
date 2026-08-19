using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJing
{
    public static class UserSession
    {
        // 로그인 성공 시 세션에 저장하여 전역에서 사용할 유저 ID
        public static string UserId { get; set; } = "";
        public static bool IsGuest { get; set; } = false;
    }
}
