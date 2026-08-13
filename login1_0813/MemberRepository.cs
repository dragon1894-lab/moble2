using MySql.Data.MySqlClient;

namespace login1_0811
{
    // member 테이블과 관련된 SQL을 한곳에서 관리하는 클래스
    internal static class MemberRepository
    {
        // 현재 비밀번호가 맞으면 새 비밀번호로 변경하고 변경된 행 수를 반환
        public static int ChangePassword(string userID, string currentPassword, string newPassword)
        {
            string query = @"UPDATE member
                             SET password = @newPassword
                             WHERE user_id = @userID
                             AND password = @currentPassword";

            return DBHelper.Execute(
                query,
                new MySqlParameter("@newPassword", newPassword),
                new MySqlParameter("@userID", userID),
                new MySqlParameter("@currentPassword", currentPassword)
            );
        }

        // 로그인한 사용자의 휴대폰 번호를 변경하고 변경된 행 수를 반환
        public static int ChangePhone(string userID, string phoneNumber)
        {
            string query = @"UPDATE member
                             SET phone = @phone
                             WHERE user_id = @userID";

            return DBHelper.Execute(
                query,
                new MySqlParameter("@phone", phoneNumber),
                new MySqlParameter("@userID", userID)
            );
        }
    }
}
