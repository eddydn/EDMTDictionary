using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDMTDictionary.Helpers
{
    public class Database
    {
        public static List<Words> getAllWords(string tableName)
        {
            List<Words> result = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM " + tableName);
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            Words word = new Words();
                            word.Id = Convert.ToInt32(statement["Id"]);
                            word.Word = statement["Word"].ToString();
                            word.Type = statement["Type"].ToString();
                            word.Description = statement["Description"].ToString();
                            result.Add(word);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<string> getGroupChar(string tableName)
        {
            List<string> result = new List<string>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT DISTINCT UPPER(SUBSTR(Word,1,1)) AS GroupChar FROM " + tableName);
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            result.Add(statement["GroupChar"].ToString());
                        }
                    }
                    return result;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static int deleteFromTable(string TableName, Words word)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = string.Format("DELETE FROM {0} WHERE ID='{1}'", TableName, word.Id);
                    var statement = connection.Prepare(query);
                    statement.Step();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Because we have Words,Favorites,Recents have same struct
        /// So we just need write one function
        /// </summary>
        /// <param name="tableName">name of Table</param>
        /// <param name="word">value</param>
        /// <returns></returns>
        public static int insertIntoTable(string tableName, Words word)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = String.Format("INSERT INTO {0} VALUES({1},'{2}','{3}','{4}')", tableName,word.Id, word.Word, word.Type, word.Description);
                    var statement = connection.Prepare(query);
                    statement.Step();
                    return 1;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public static int searchFavorites(Words word)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM Favorites WHERE Id=" + word.Id);
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                            return 1; // found
                    }
                    return 0; // not found
                }
            }
            catch
            {
                return -1; // error
            }
        }

        public static List<Words> searchWord(string character)
        {
            List<Words> lstWords = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM Words WHERE Word LIKE '" + character + "%'");
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            Words word = new Words();
                            word.Id = Convert.ToInt32(statement["Id"]);
                            word.Word = statement["Word"].ToString();
                            word.Type = statement["Type"].ToString();
                            word.Description = statement["Description"].ToString();
                            lstWords.Add(word);
                        }
                    }
                    return lstWords;
                }
            }
            catch
            {
                return null;
            }
        }

        public static object getUserData(string column)
        {
            object result = null;
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare(String.Format("SELECT {0} FROM UserData", column));
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            result = statement[column];
                        }

                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static void setUserData(string column,object value)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = String.Format("UPDATE UserData Set {0}='{1}'", column, value);
                    var statement = connection.Prepare(query);
                    statement.Step();
                }
            }
            catch
            {
                return;
            }
                }

        public static  List<Words> getAllWordsByChar(string groupChar)
        {
            List<Words> result = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM Words where Word Like  '" + groupChar+"%'");
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            Words word = new Words();
                            word.Id = Convert.ToInt32(statement["Id"]);
                            word.Word = statement["Word"].ToString();
                            word.Type = statement["Type"].ToString();
                            word.Description = statement["Description"].ToString();
                            result.Add(word);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
