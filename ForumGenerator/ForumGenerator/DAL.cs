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
    public class DAL
    {
        static MySqlConnection conn;
        static string connectionString;
        static MySqlCommand cmd;
        static int nextForumID;
        static int nextSubForumID;
        static int nextPostID;
        static int nextMessageID;
        static ServerParser parser;

        public DAL()
        {
            MySqlDataReader resultSet;
            connectionString = "server=127.0.0.1;uid=root;" + "pwd=12345;database=forumgenerator;";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                Console.WriteLine("Error");
            }

            // Init nextForumID
            cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Get_Max_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;
            
            resultSet = cmd.ExecuteReader();
            while (resultSet.Read())
            {
                nextForumID = Int32.Parse(resultSet.GetString(0)) + 1;
            }
            resultSet.Close();

            // Init nexSubForumID
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Get_Max_SubForumID";
            cmd.CommandType = CommandType.StoredProcedure;
            resultSet = cmd.ExecuteReader();
            while (resultSet.Read())
            {
                nextSubForumID = Int32.Parse(resultSet.GetString(0)) + 1;
            }
            resultSet.Close();

            
            // Init nextPostID
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Get_Max_PostID";
            cmd.CommandType = CommandType.StoredProcedure;
            resultSet = cmd.ExecuteReader();
            while (resultSet.Read())
            {
                nextPostID = Int32.Parse(resultSet.GetString(0)) + 1;
            }
            resultSet.Close();


            // Init nextMessageID
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Get_Max_MessageID";
            cmd.CommandType = CommandType.StoredProcedure;
            resultSet = cmd.ExecuteReader();
            while (resultSet.Read())
            {
                nextMessageID = Int32.Parse(resultSet.GetString(0)) + 1;
            }
            resultSet.Close();

            parser = new ServerParser();

        }
        
        public string SendMessage(string msg)
        {
            
            char[] separatingChars = {'$'};
            string[] query = msg.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string queryAns= "";

            switch(query[0])
            {
                case "Add_Forum":
                    queryAns = addForum(query);
                    break;
                case "Add_SubForum":
                    queryAns = addSubForum(query);
                    break;
                case "Add_Thread":
                    queryAns = addThread(query);
                    break;
                case "Add_Reply":
                    queryAns = addReply(query);
                    break;
                case "Add_User":
                    queryAns = addUser(query);
                    break;
                case "Get_All_Forums":
                    queryAns= getAllForums();
                    break;
                case "Get_Sub_Forums":
                    queryAns = getSubForumsByForumID(query);
                    break;
                case "Get_Threads":
                    queryAns = getMainThreadsBySubForumID(query);
                    break;
                case "Get_Replies":
                    queryAns = getRepliesByFatherPostID(query);
                    break;
                case "Get_User":
                    queryAns = getUserByEmail(query);
                    break;
                case "Delete_User":
                    queryAns = removeUserByEmail(query);
                    break;
                case "Delete_Post":
                    queryAns = removePostByID(query);
                    break;
                case "Delete_Forum":
                    queryAns = removeForumByID(query);
                    break;
                case "Delete_Sub_Forum":
                    queryAns = removeSubForumBySubForumID(query);
                    break;
                case "Is_User_Exist_Email_ForumID":
                    queryAns = isUserExistByEmailForumID(query);
                    break;
                case "Is_Admin":
                    queryAns = isAdminEmailForumID(query);
                    break;
                case "Is_Init":
                    queryAns = isInit(query);
                    break;
                case "Is_Moderator":
                    queryAns = isModeratorEmailSubForumID(query);
                    break;
                case "Is_Super_Admin":
                    queryAns = isSuperAdmin(query);
                    break;
                case "Sign_In":
                    queryAns = getUserEmailPassForumID(query);
                    break;

            }

            return queryAns;
        }

        // -------------ADD-------------
        public static string addForum(string[] query)
        {
            string ans = "";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Add_Forum";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ForumID", ""+nextForumID);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumName", query[1]);
            cmd.Parameters["@_ForumName"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Add_Forum_Response$" + nextForumID;
                nextForumID++;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Add_Forum_Response$Error";
            }
            return ans;
        }

        public static string addSubForum(string[] query)
        {
            string ans = "";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Add_SubForum";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_SubForumID", ""+nextSubForumID);
            cmd.Parameters["@_SubForumID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_SubForumName", query[1]);
            cmd.Parameters["@_SubForumName"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumID", query[2]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Add_SubForum_Response$" + nextSubForumID;
                nextSubForumID++;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Add_SubForum_Response$Error";
            }
            return ans;
        }

        public static string addThread(string[] query)
        {
            string ans = "";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Add_Post";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_PostID", "" + nextPostID);
            cmd.Parameters["@_PostID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_Topic", "" + query[1]);
            cmd.Parameters["@_Topic"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_Content", "" + query[2]);
            cmd.Parameters["@_Content"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_SubForumID", "" + query[3]);
            cmd.Parameters["@_SubForumID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ParentPostID", null);
            cmd.Parameters["@_ParentPostID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_CreationTime", "" + query[4]);
            cmd.Parameters["@_CreationTime"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_UserEmail", "" + query[5]);
            cmd.Parameters["@_UserEmail"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Add_Thread_Response$" + nextPostID;
                nextPostID++;

            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Add_Thread_Response$Error";
            }
            return ans;
        }

        public static string addReply(string[] query)
        {
            string ans = "";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Add_Post";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_PostID", "" + nextPostID);
            cmd.Parameters["@_PostID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_Topic", "" + query[1]);
            cmd.Parameters["@_Topic"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_Content", "" + query[2]);
            cmd.Parameters["@_Content"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_SubForumID", "" + query[3]);
            cmd.Parameters["@_SubForumID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ParentPostID", "" + query[4]);
            cmd.Parameters["@_ParentPostID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_CreationTime", "" + query[5]);
            cmd.Parameters["@_CreationTime"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_UserEmail", "" + query[6]);
            cmd.Parameters["@_UserEmail"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Add_Reply_Response$" + nextPostID;
                nextPostID++;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Add_Reply_Response$Error";
            }
            return ans;
        }

        public static string addUser(string[] query)
        {
            string ans = "";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            if(!isUserExistByEmailBool(query[1],query[6]))
            {

            cmd.CommandText = "Add_User";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_BirthDay", "" + query[4]);
            cmd.Parameters["@_BirthDay"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_Gender", "" + query[5]);
            cmd.Parameters["@_Gender"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Add_User_Response$SUCCESS";
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Add_User_Response$ALREADY_REG";
            }
            }

            cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Add_Forum_User";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[6]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_UserEmail", "" + query[1]);
            cmd.Parameters["@_UserEmail"].Direction = ParameterDirection.Input;

            DateTime dt = DateTime.Now;
            cmd.Parameters.AddWithValue("@_RegisterDate", "" + dt.Year + "-" + dt.Month + "-" + dt.Day);
            cmd.Parameters["@_RegisterDate"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_PasswordMod", "" + dt.Year + "-" + dt.Month + "-" + dt.Day);
            cmd.Parameters["@_PasswordMod"].Direction = ParameterDirection.Input;
            
            cmd.Parameters.AddWithValue("@_Password", "" + query[3]);
            cmd.Parameters["@_Password"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_userName", "" + query[2]);
            cmd.Parameters["@_userName"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Add_User_Response$SUCCESS";
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Add_User_Response$ALREADY_REG";
            }

            return ans;
        }

        // -------------GET-------------
        public static string getAllForums()
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Get_All_Forums";
            cmd.CommandType = CommandType.StoredProcedure;
            resultSet = cmd.ExecuteReader();
            return parser.getAllForums(resultSet);
        }

        public static string getSubForumsByForumID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Get_SubForum_by_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[1]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;
            
            resultSet = cmd.ExecuteReader();
            return parser.getSubForumByForumID(resultSet);
        }

        public static string getMainThreadsBySubForumID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Get_Main_Posts_in_SubForum_By_SubForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_SubForumID", "" + query[1]);
            cmd.Parameters["@_SubForumID"].Direction = ParameterDirection.Input;
            
            resultSet = cmd.ExecuteReader();
            return parser.getMainThreadsBySubForumID(resultSet);
        }

        public static string getRepliesByFatherPostID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Get_Posts_by_Father_PostID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ParentPostID", "" + query[1]);
            cmd.Parameters["@_ParentPostID"].Direction = ParameterDirection.Input;
            
            resultSet = cmd.ExecuteReader();
            return parser.getRepliesByThreadID(resultSet);
        }

        public static string getUserByEmail(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Get_User_by_Email_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[2]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;
            
            resultSet = cmd.ExecuteReader();
            return parser.getUserByUserEmail(resultSet);
        }

        // -------------REMOVE-------------
        public static string removeUserByEmail(string[] query)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Remove_User_by_Email";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_UserEmail", "" + query[1]);
            cmd.Parameters["@_UserEmail"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Delete_User_Response$SUCCESS";
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Delete_User_Response$Error";
            }
            return ans;
        }

        public static string removePostByID(string[] query)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Remove_Post_by_PostID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_PostID", "" + query[1]);
            cmd.Parameters["@_PostID"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Delete_Post_Response$SUCCESS";
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Delete_Post_Response$Error";
            }
            return ans;
        }

        public static string removeForumByID(string[] query)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Remove_Forum_by_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[1]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Delete_Forum_Response$SUCCESS";
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Delete_Forum_Response$Error";
            }
            return ans;
        }

        public static string removeSubForumBySubForumID(string[] query)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Remove_SubForum_By_SubForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_SubForumID", "" + query[1]);
            cmd.Parameters["@_SubForumID"].Direction = ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
                ans = "Delete_SubForum_Response$SUCCESS";
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                ans = "Delete_SubForum_Response$Error";
            }
            return ans;
        }
        
        // -------------IS EXIST-------------
        public static string isUserExistByEmailForumID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Get_User_by_Email_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[2]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            try
            {
                resultSet = cmd.ExecuteReader();
                if (resultSet.HasRows)
                    ans = "Get_User_by_Email_Response$true";
                else
                    ans = "Get_User_by_Email_Response$false";
                resultSet.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            return ans;
        }

        public static Boolean isUserExistByEmailBool(string email, string forumID)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            Boolean ans = false;

            cmd.CommandText = "Get_User_by_Email_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + email);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumID", "" + forumID);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            try
            {
                resultSet = cmd.ExecuteReader();
                if (resultSet.HasRows)
                    ans = true;
                else
                    ans = false;
                resultSet.Close();
               
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }
            return ans;
        }

        public static string isAdminEmailForumID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "is_Admin_Email_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[2]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            try
            {
                resultSet = cmd.ExecuteReader();
                if (resultSet.HasRows)
                    ans = "Is_Admin_Response$true";
                else
                    ans = "Is_Admin_Response$false";
                resultSet.Close();
            
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            return ans;
        }

        public static string isModeratorEmailSubForumID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "is_Moderator_Email_SubForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_SubForumID", "" + query[2]);
            cmd.Parameters["@_SubForumID"].Direction = ParameterDirection.Input;

            try
            {
                resultSet = cmd.ExecuteReader();
                if (resultSet.HasRows)
                    ans = "Is_Moderator_Response$true";
                else
                    ans = "Is_Moderator_Response$false";
                resultSet.Close();
           
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            return ans;
        }

        public static string isSuperAdmin(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "is_SuperAdmin_Email";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            try
            {
                resultSet = cmd.ExecuteReader();
                if (resultSet.HasRows)
                    ans = "Is_Super_Admin_Response$true";
                else
                    ans = "Is_Super_Admin_Response$false";
                resultSet.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            return ans;
        }

        public static string isInit(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Get_All_Users";
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                resultSet = cmd.ExecuteReader();
                if (resultSet.HasRows)
                    ans = "Is_Init_Response$true";
                else
                    ans = "Is_Init_Response$false";
                resultSet.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            return ans;
        }

        public static string getUserEmailPassForumID(string[] query)
        {
            MySqlDataReader resultSet;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string ans = "";

            cmd.CommandText = "Get_User_by_Email_Pass_ForumID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_Email", "" + query[1]);
            cmd.Parameters["@_Email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_Password", "" + query[2]);
            cmd.Parameters["@_Password"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@_ForumID", "" + query[3]);
            cmd.Parameters["@_ForumID"].Direction = ParameterDirection.Input;

            try
            {
                resultSet = cmd.ExecuteReader();
                if(resultSet.HasRows)
                    ans = parser.singInParser(resultSet);
                else
                    ans = "Sign_In_Response$Error";
                resultSet.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
           {
           }

            return ans;
        }
       
        public static string isForumExistByForumID(string[] query)
        {
            if (query[0].Equals("Get_Forum_By_ID_t"))
                return "Get_Forum_By_ID_Response$true";
            return "Get_Forum_By_ID_Response$false";
        }

        public static string isSubForumExistBySubForumID(string[] query)
        {
            if (query[0].Equals("Get_SubForum_By_ID_t"))
                return "Get_Sub_Forum_By_ID_Response$true";
            return "Get_Sub_Forum_By_ID_Response$false";
        }

        public static string isThreadExistByThreadID(string[] query)
        {
            if (query[0].Equals("Get_Post_By_ID_t"))
                return "Get_Thread_By_ID_Response$true";
            return "Get_Thread_By_ID_Response$false";
        }

        
    }
}
