
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Configuration;
using DXWebMRCS.Models;

public class NorthwindDataProvider
{
    public static IEnumerable GetMenus()
    {
        List<Menu> Menu = new List<Menu>();

        using (OleDbConnection connection = new OleDbConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
            OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM Menu", connection);

            connection.Open();

            OleDbDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                Menu.Add(new Menu()
                {
                    MenuID = (int)reader["MenuID"],
                    NameMon = (string)reader["NameMon"],
                    NameEng = (string)reader["NameEng"],
                    NavigateUrl = (string)reader["NavigateUrl"],
                    ParentId = (reader["ParentId"] == DBNull.Value ? null : (int?)reader["ParentId"])
                });
            }

            reader.Close();
        }

        return Menu;
    }

    public static void InsertMenu(Menu Menu)
    {
        using (OleDbConnection connection = new OleDbConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
            OleDbCommand insertCommand = new OleDbCommand("INSERT INTO [Menu] ([NameMon], [NameEng], [NavigateUrl], [ParentId]) VALUES (?, ?, ?, ?)", connection);

            insertCommand.Parameters.AddWithValue("NameMon", Menu.NameMon);
            insertCommand.Parameters.AddWithValue("NameEng", Menu.NameEng);
            insertCommand.Parameters.AddWithValue("NavigateUrl", Menu.NavigateUrl);

            if (Menu.ParentId.HasValue)
                insertCommand.Parameters.AddWithValue("ParentId", Menu.ParentId);
            else
                insertCommand.Parameters.AddWithValue("ParentId", DBNull.Value);

            connection.Open();
            insertCommand.ExecuteNonQuery();
        }
    }

    public static void UpdateMenu(Menu Menu)
    {
        using (OleDbConnection connection = new OleDbConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
            OleDbCommand updateCommand = new OleDbCommand("UPDATE [Menu] SET [NameMon] = ?, [NameEng] = ?, [NavigateUrl] = ?, [ParentId] = ? WHERE [MenuID] = ?", connection);

            updateCommand.Parameters.AddWithValue("NameMon", Menu.NameMon);
            updateCommand.Parameters.AddWithValue("NameEng", Menu.NameEng);
            updateCommand.Parameters.AddWithValue("NavigateUrl", Menu.NavigateUrl);
            updateCommand.Parameters.AddWithValue("ParentId", Menu.ParentId);
            updateCommand.Parameters.AddWithValue("MenuID", Menu.MenuID);

            connection.Open();
            updateCommand.ExecuteNonQuery();
        }
    }

    public static void MoveMenu(int MenuID, int ParentId)
    {
        using (OleDbConnection connection = new OleDbConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
            OleDbCommand updateCommand = new OleDbCommand("UPDATE [Menu] SET [ParentId] = ? WHERE [MenuID] = ?", connection);

            updateCommand.Parameters.AddWithValue("ParentId", ParentId);
            updateCommand.Parameters.AddWithValue("MenuID", MenuID);

            connection.Open();
            updateCommand.ExecuteNonQuery();
        }
    }

    public static void DeleteMenu(int MenuID)
    {
        using (OleDbConnection connection = new OleDbConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
            OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM Menu WHERE MenuID = " + MenuID.ToString(), connection);

            connection.Open();
            deleteCommand.ExecuteNonQuery();
        }
    }
}
