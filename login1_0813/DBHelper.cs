using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace login1_0811
{
    internal class DBHelper
    {
        private static string connStr = 
            "Server=localhost;Database=djing;Uid=root;Pwd=1111;";
        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }

        // 전달받은 SQL을 실행하고 변경된 데이터 개수를 반환
        public static int Execute(string query, params MySqlParameter[] parameters)
        {
            // 데이터베이스 연결 생성
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open(); // 데이터베이스 연결 열기

                // 실행할 SQL과 연결 정보 설정
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // SQL에 필요한 값들을 추가
                cmd.Parameters.AddRange(parameters);

                // SQL 실행 후 변경된 행의 개수 반환
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
