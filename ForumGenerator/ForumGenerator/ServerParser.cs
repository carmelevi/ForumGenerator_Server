using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;


namespace ForumGenerator
{
    class ServerParser
    {
        private static ServerParser instance;
        public static ServerParser Instance
        {
            get
            {
                if (instance == null) { instance = new ServerParser(); }
                return instance;
            }
        }

        public string getAllForums(MySqlDataReader reader)
        {
            string ans = "Get_All_Forums_Response$";
            while (reader.Read())
                ans += reader.GetString(0) + "$" + reader.GetString(1) + "#";
            reader.Close();
            ans = ans.Substring(0, ans.Length-1);
            return ans;
        }

        public string getSubForumByForumID(MySqlDataReader reader)
        {
            string ans = "Get_Sub_Forums_Response$";
            while (reader.Read())
                ans += reader.GetString(0) + "$" + reader.GetString(1) + "#";
            reader.Close();
            ans = ans.Substring(0, ans.Length-1);
            return ans;
        }

        public string getMainThreadsBySubForumID(MySqlDataReader reader)
        {
            string ans = "Get_Threads_Response$";
            while (reader.Read())
                ans += reader.GetString(0) + "$" + reader.GetString(1) + "$" + reader.GetString(2) + "$" + reader.GetString(3) + "$" + reader.GetString(4) + "$" + reader.GetString(5) + "#";
            reader.Close();
            ans = ans.Substring(0, ans.Length - 1);
            return ans;
        }

        public string getRepliesByThreadID(MySqlDataReader reader)
        {
            string ans = "Get_Replies_Response$";
            while (reader.Read())
                ans += reader.GetString(0) + "$" + reader.GetString(1) + "$" + reader.GetString(2) + "$" + reader.GetString(3) + "$" + reader.GetString(4) + "$" + reader.GetString(5) + "#";
            reader.Close();
            ans = ans.Substring(0, ans.Length - 1);
            return ans;
        }

        public string getUserByUserEmail(MySqlDataReader reader)
        {
            string ans = "Get_User_Response$";
            while (reader.Read())
                ans += reader.GetString(0) + "$" + reader.GetString(1) + "$" + reader.GetString(2) + "$" + reader.GetString(3) + "#";//email,userName,birthday,gender
            reader.Close();
            ans = ans.Substring(0, ans.Length - 1);
            return ans;
        }

        public string singInParser(MySqlDataReader reader)
        {
            string ans = "Sign_In_Response$";
            while (reader.Read())
                ans += reader.GetString(0) + "$" + reader.GetString(1) + "$" + reader.GetString(2) + "$" + reader.GetString(3) + "#";//email,password,birthday,gender
            ans = ans.Substring(0, ans.Length - 1);
            return ans;
        }



    }
}
