
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using DXWebMRCS.Models;
using System.Data.SqlClient;

namespace DXWebMRCS.Models
{
    public class NorthwindDataProvider
    {
        public static IEnumerable GetMenus()
        {
            List<Menu> Menu = new List<Menu>();

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM Menu", connection);

                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    Menu.Add(new Menu()
                    {
                        MenuID = (int)reader["MenuID"],
                        NameMon = reader["NameMon"] == DBNull.Value ? string.Empty : (string)reader["NameMon"],
                        NameEng = reader["NameEng"] == DBNull.Value ? string.Empty : (string)reader["NameEng"],
                        NavigateUrl = reader["NavigateUrl"] == DBNull.Value ? string.Empty : (string)reader["NavigateUrl"],
                        MenuType = reader["MenuType"] == DBNull.Value ? string.Empty : (string)reader["MenuType"],
                        ParentId = (reader["ParentId"] == DBNull.Value ? null : (int?)reader["ParentId"])
                    });
                }

                reader.Close();
            }

            return Menu;
        }

        public static void InsertMenu(Menu Menu)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Menu (NameMon, NameEng, NavigateUrl, MenuType, ParentId) VALUES (@NameMon, @NameEng, @NavigateUrl, @MenuType, @ParentId)", connection);

                insertCommand.Parameters.AddWithValue("@NameMon", Menu.NameMon);
                insertCommand.Parameters.AddWithValue("@NameEng", Menu.NameEng);
                if (Menu.NavigateUrl == null)
                    insertCommand.Parameters.AddWithValue("@NavigateUrl", DBNull.Value);
                else
                    insertCommand.Parameters.AddWithValue("@NavigateUrl", Menu.NavigateUrl);

                if (Menu.MenuType == null)
                    insertCommand.Parameters.AddWithValue("@MenuType", DBNull.Value);
                else
                    insertCommand.Parameters.AddWithValue("@MenuType", Menu.MenuType);

                if (Menu.ParentId.HasValue)
                    insertCommand.Parameters.AddWithValue("@ParentId", Menu.ParentId);
                else
                    insertCommand.Parameters.AddWithValue("@ParentId", DBNull.Value);

                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateMenu(Menu Menu)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand updateCommand = new SqlCommand("UPDATE [Menu] SET [NameMon] = @NameMon, [NameEng] = @NameEng, [NavigateUrl] = @NavigateUrl, [MenuType] = @MenuType, [ParentId] = @ParentId WHERE [MenuID] = @MenuID", connection);

                updateCommand.Parameters.AddWithValue("@NameMon", Menu.NameMon);
                updateCommand.Parameters.AddWithValue("@NameEng", Menu.NameEng);
                if (Menu.NavigateUrl == null)
                    updateCommand.Parameters.AddWithValue("@NavigateUrl", DBNull.Value);
                else
                    updateCommand.Parameters.AddWithValue("@NavigateUrl", Menu.NavigateUrl);
                if (Menu.MenuType == null)
                    updateCommand.Parameters.AddWithValue("@MenuType", DBNull.Value);
                else
                    updateCommand.Parameters.AddWithValue("@MenuType", Menu.MenuType);
                if (Menu.ParentId.HasValue)
                    updateCommand.Parameters.AddWithValue("@ParentId", Menu.ParentId);
                else
                    updateCommand.Parameters.AddWithValue("@ParentId", DBNull.Value);
                updateCommand.Parameters.AddWithValue("@MenuID", Menu.MenuID);

                connection.Open();
                updateCommand.ExecuteNonQuery();
            }
        }

        public static void MoveMenu(int MenuID, int ParentId)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand updateCommand = new SqlCommand("UPDATE Menu SET ParentId = @ParentId WHERE MenuID = @MenuID", connection);

                updateCommand.Parameters.AddWithValue("@ParentId", ParentId);
                updateCommand.Parameters.AddWithValue("@MenuID", MenuID);

                connection.Open();
                updateCommand.ExecuteNonQuery();
            }
        }

        public static void DeleteMenu(int MenuID)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Menu WHERE MenuID = " + MenuID.ToString(), connection);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}