using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumGenerator;

namespace DALTest
{

    [TestClass]
    public class UnitTest1
    {
        DAL DAL = new DAL();

        [TestMethod]
        public void addUserTest()
        {
            string[] add_User_Parameters = {"addUser", "ploni.almoni@gmail.com","Ploni", "123456", "1990-09-09", "Male","1"};
            DAL.addUser(add_User_Parameters);
            string[] is_User_Exist_Parameters = { "Get_User_by_Email", "ploni.almoni@gmail.com","1" };
            Assert.AreEqual(DAL.isUserExistByEmailForumID(is_User_Exist_Parameters), "Get_User_by_Email_Response$true");
            string[] remove_User_Parameters = { "Remove_User_by_Email", "ploni.almoni@gmail.com" };
            DAL.removeUserByEmail(remove_User_Parameters);
        }

        [TestMethod]
        public void addForumTest()
        {
            string[] add_Forum_Parameters = { "addForum", "AkunaMatata" };
            string ans = DAL.addForum(add_Forum_Parameters);
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_Forum_Exist_Parameters = { "Get_Forum_By_ID_t", split_ans[1] };
            Assert.AreEqual(DAL.isForumExistByForumID(is_Forum_Exist_Parameters), "Get_Forum_By_ID_Response$true");
            string[] remove_Forum_Parameters = { "removeForum", split_ans[1] };
            DAL.removeForumByID(remove_Forum_Parameters);
        }
        [TestMethod]
        public void addSubForumTest()
        {
            string[] add_SubForum_Parameters = { "addSubForum", "YAKAZUMBA", "1" };
            string ans = DAL.addSubForum(add_SubForum_Parameters);
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_SubForum_Exist_Parameters = { "Get_SubForum_By_ID_t", split_ans[1] };
            Assert.AreEqual(DAL.isSubForumExistBySubForumID(is_SubForum_Exist_Parameters), "Get_Sub_Forum_By_ID_Response$true");
            string[] remove_SubForum_Parameters = { "removeSubForum", split_ans[1] };
            DAL.removeSubForumBySubForumID(remove_SubForum_Parameters);
        }

        [TestMethod]
        public void addThreadTest()
        {
            string[] add_Thread_Parameters = { "addTread", "Tutit", "HAMUDA-METUKA", "1", "1990-09-15 22:30:30", "Carmel@bgu.co.il" };
            string ans = DAL.addThread(add_Thread_Parameters);
            //string ans = "reply$1";
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_Thread_Exist_Parameters = { "Get_Post_By_ID_t", split_ans[1] };
            Assert.AreEqual(DAL.isThreadExistByThreadID(is_Thread_Exist_Parameters), "Get_Thread_By_ID_Response$true");
            string[] remove_Thread_Parameters = { "removeTread", split_ans[1] };
            DAL.removePostByID(remove_Thread_Parameters);
        }
        [TestMethod]
        public void addReplyTest()
        {
            string[] add_Reply_Parameters = { "addReply", "Ice-Kafaj", "BLABLABLA", "1", "1", "1990-09-15 22:30:30", "Carmel@bgu.co.il" };
            string ans = DAL.addThread(add_Reply_Parameters);
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_Thread_Exist_Parameters = { "Get_Post_By_ID_t", split_ans[1] };
            Assert.AreEqual(DAL.isThreadExistByThreadID(is_Thread_Exist_Parameters), "Get_Thread_By_ID_Response$true");
            string[] remove_Thread_Parameters = { "removeTread", split_ans[1] };
            DAL.removePostByID(remove_Thread_Parameters);
        }

        [TestMethod]
        public void getSubForumsByForumIDTest()
        {
            string[] get_SubForum_Parameters = { "Get_SubForum_by_ForumID", "1" };
            string ans = DAL.getSubForumsByForumID(get_SubForum_Parameters);
            bool res = false;
            if (ans.Contains("Get_Sub_Forums_Response$"))
                res = true;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void getMainThreadsBySubForumIDTest()
        {
            string[] get_SubForum_Parameters = { "Get_Main_Posts_in_SubForum_By_SubForumID", "1" };
            string ans = DAL.getMainThreadsBySubForumID(get_SubForum_Parameters);
            bool res = false;
            if (ans.Contains("Get_Threads_Response$"))
                res = true;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void getRepliesByFatherPostIDTest()
        {
            string[] get_SubForum_Parameters = { "Get_Posts_by_Father_PostID", "1" };
            string ans = DAL.getRepliesByFatherPostID(get_SubForum_Parameters);
            bool res = false;
            if (ans.Contains("Get_Replies_Response$"))
                res = true;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void getUserByEmailForumIDTest()
        {
            string[] get_SubForum_Parameters = { "Get_User_by_Email","Gili@bgu.co.il", "1" };
            string ans = DAL.getUserByEmail(get_SubForum_Parameters);
            bool res = false;
            if (ans.Contains("Get_User_Response$"))
                res = true;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void removeUserByEmailTest()
        {
            string[] add_User_Parameters = { "addUser", "ploni.almoni@gmail.com", "Ploni", "123456", "1990-09-09", "Male", "1" };
            DAL.addUser(add_User_Parameters);
            string[] remove_User_Parameters = { "Remove_User_by_Email", "ploni.almoni@gmail.com" };
            DAL.removeUserByEmail(remove_User_Parameters);
            string[] is_User_Exist_Parameters = { "Get_User_by_Email", "ploni.almoni@gmail.com","1" };
            Assert.AreEqual(DAL.isUserExistByEmailForumID(is_User_Exist_Parameters), "Get_User_by_Email_Response$false");
        }

        [TestMethod]
        public void removeForumByIDTest()
        {
            string[] add_Forum_Parameters = { "addForum", "AkunaMatata" };
            string ans = DAL.addForum(add_Forum_Parameters);
            //string ans = "reply$1";
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_Forum_Exist_Parameters = { "Get_Forum_By_ID_f", split_ans[1] };
            string[] remove_Forum_Parameters = { "removeForum", split_ans[1] };
            DAL.removeForumByID(remove_Forum_Parameters);
            Assert.AreEqual(DAL.isForumExistByForumID(is_Forum_Exist_Parameters), "Get_Forum_By_ID_Response$false");

        }

        [TestMethod]
        public void removeSubForumByIDTest()
        {
            string[] add_SubForum_Parameters = { "addSubForum", "YAKAZUMBA", "1" };
            string ans = DAL.addSubForum(add_SubForum_Parameters);
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_SubForum_Exist_Parameters = { "Get_SubForum_By_ID_f", split_ans[1] };
            string[] remove_SubForum_Parameters = { "removeSubForum", split_ans[1] };
            DAL.removeSubForumBySubForumID(remove_SubForum_Parameters);
            Assert.AreEqual(DAL.isSubForumExistBySubForumID(is_SubForum_Exist_Parameters), "Get_Sub_Forum_By_ID_Response$false");
        }


        [TestMethod]
        public void removeThreadTest()
        {
            string[] add_Thread_Parameters = { "addTread", "Tutit", "HAMUDA-METUKA", "1", "1990-09-15 22:30:30", "Carmel@bgu.co.il" };
            string ans = DAL.addThread(add_Thread_Parameters);
            //string ans = "reply$1";
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_Thread_Exist_Parameters = { "Get_Post_By_ID_f", split_ans[1] };
            string[] remove_Thread_Parameters = { "removeTread", split_ans[1] };
            DAL.removePostByID(remove_Thread_Parameters);
            Assert.AreEqual(DAL.isThreadExistByThreadID(is_Thread_Exist_Parameters), "Get_Thread_By_ID_Response$false");
        }

        [TestMethod]
        public void removeReplyTest()
        {
            string[] add_Reply_Parameters = { "addReply", "Ice-Kafaj", "BLABLABLA", "1", "1", "1990-09-15 22:30:30", "Carmel@bgu.co.il" };
            string ans = DAL.addThread(add_Reply_Parameters);
            //string ans = "reply$1";
            char[] separatingChars = { '$' };
            string[] split_ans = ans.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] is_Thread_Exist_Parameters = { "Get_Post_By_ID_f", split_ans[1] };
            string[] remove_Thread_Parameters = { "removeTread", split_ans[1] };
            DAL.removePostByID(remove_Thread_Parameters);
            Assert.AreEqual(DAL.isThreadExistByThreadID(is_Thread_Exist_Parameters), "Get_Thread_By_ID_Response$false");
        }







    }
}
