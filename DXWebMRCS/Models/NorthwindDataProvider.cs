
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using DXWebMRCS.Models;
using System.Data.SqlClient;
using System.Web;

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
                        Image = reader["Image"] == DBNull.Value ? string.Empty : (string)reader["Image"],
                        ParentId = (reader["ParentId"] == DBNull.Value ? null : (int?)reader["ParentId"]),
                        BranchID = (reader["BranchID"] == DBNull.Value ? null : (int?)reader["BranchID"])
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
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Menu (NameMon, NameEng, NavigateUrl, MenuType, Image, ParentId, BranchID) VALUES (@NameMon, @NameEng, @NavigateUrl, @MenuType, @Image, @ParentId, @BranchID)", connection);

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

                if (Menu.Image == null)
                    insertCommand.Parameters.AddWithValue("@Image", DBNull.Value);
                else
                    insertCommand.Parameters.AddWithValue("@Image", Menu.Image);

                if (Menu.ParentId.HasValue)
                    insertCommand.Parameters.AddWithValue("@ParentId", Menu.ParentId);
                else
                    insertCommand.Parameters.AddWithValue("@ParentId", DBNull.Value);

                if (Menu.BranchID.HasValue)
                    insertCommand.Parameters.AddWithValue("@BranchID", Menu.BranchID);
                else
                    insertCommand.Parameters.AddWithValue("@BranchID", DBNull.Value);

                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateMenu(Menu Menu)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand updateCommand = new SqlCommand("UPDATE [Menu] SET [NameMon] = @NameMon, [NameEng] = @NameEng, [NavigateUrl] = @NavigateUrl, [MenuType] = @MenuType, Image = @Image, [ParentId] = @ParentId, BranchID = @BranchID WHERE [MenuID] = @MenuID", connection);

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

                if (Menu.Image == null)
                    updateCommand.Parameters.AddWithValue("@Image", DBNull.Value);
                else
                    updateCommand.Parameters.AddWithValue("@Image", Menu.Image);

                if (Menu.ParentId.HasValue)
                    updateCommand.Parameters.AddWithValue("@ParentId", Menu.ParentId);
                else
                    updateCommand.Parameters.AddWithValue("@ParentId", DBNull.Value);

                if (Menu.BranchID.HasValue)
                    updateCommand.Parameters.AddWithValue("@BranchID", Menu.BranchID);
                else
                    updateCommand.Parameters.AddWithValue("@BranchID", DBNull.Value);
                updateCommand.Parameters.AddWithValue("@MenuID", Menu.MenuID);

                connection.Open();
                updateCommand.ExecuteNonQuery();
            }
        }

        public static void MoveMenu(int MenuID, int? ParentId)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand updateCommand = new SqlCommand("UPDATE Menu SET ParentId = @ParentId WHERE MenuID = @MenuID", connection);

                if (ParentId == null)
                    updateCommand.Parameters.AddWithValue("@ParentId", DBNull.Value);
                else
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
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Menu WHERE MenuID = " + MenuID + " OR ParentId = " + MenuID, connection);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
            }
        }

        private static UsersContext DB = new UsersContext();
        public static IEnumerable GetBranchs()
        {
            List<Branch> Branch = new List<Branch>();

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM Branches", connection);

                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    Branch.Add(new Branch()
                    {
                        BranchID = (int)reader["BranchID"],
                        NameMon = reader["NameMon"] == DBNull.Value ? string.Empty : (string)reader["NameMon"],
                        NameEng = reader["NameEng"] == DBNull.Value ? string.Empty : (string)reader["NameEng"],
                    });
                }

                reader.Close();
            }

            return Branch;
        }

        public static void DeleteBranch(int BranchID)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Branches WHERE BranchID = " + BranchID, connection);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
            }
        }

        public static void InsertBranch(Branch Branch)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Branches (NameMon, NameEng) VALUES (@NameMon, @NameEng)", connection);

                insertCommand.Parameters.AddWithValue("@NameMon", Branch.NameMon);
                insertCommand.Parameters.AddWithValue("@NameEng", Branch.NameEng);

                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateBranch(Branch Branch)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand updateCommand = new SqlCommand("UPDATE [Branches] SET [NameMon] = @NameMon, [NameEng] = @NameEng WHERE [BranchID] = @BranchID", connection);

                updateCommand.Parameters.AddWithValue("@NameMon", Branch.NameMon);
                updateCommand.Parameters.AddWithValue("@NameEng", Branch.NameEng);

                updateCommand.Parameters.AddWithValue("@BranchID", Branch.BranchID);

                connection.Open();
                updateCommand.ExecuteNonQuery();
            }
        }

        public static IEnumerable GetNews(int UserID)
        {
            List<News> News = new List<News>();

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand selectCommand;
                if (UserID > 0)
                    selectCommand = new SqlCommand(@"select * from news where 
                exists (select null from userprofile join webpages_UsersInRoles on webpages_UsersInRoles.userid = userprofile.userid join webpages_Roles on webpages_Roles.RoleId = webpages_UsersInRoles.RoleId where userprofile.userid = '" + UserID + @"' and webpages_Roles.RoleName = 'Admin') 
                or exists (select null from userprofile where userid = '" + UserID + "' and branchid = news.branchid)", connection);
                else
                    selectCommand = new SqlCommand("SELECT * FROM News ", connection);
                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    News.Add(new News()
                    {
                        CID = (int)reader["CID"],
                        TitleMon = reader["TitleMon"] == DBNull.Value ? string.Empty : (string)reader["TitleMon"],
                        TitleEng = reader["TitleEng"] == DBNull.Value ? string.Empty : (string)reader["TitleEng"],
                        BodyMon = reader["BodyMon"] == DBNull.Value ? string.Empty : (string)reader["BodyMon"],
                        BodyEng = reader["BodyEng"] == DBNull.Value ? string.Empty : (string)reader["BodyEng"],
                        Image = reader["Image"] == DBNull.Value ? string.Empty : (string)reader["Image"],
                        ImageMedium = reader["ImageMedium"] == DBNull.Value ? string.Empty : (string)reader["ImageMedium"],
                        MenuID = reader["MenuID"] == DBNull.Value ? null : (int?)reader["MenuID"],
                        BranchID = reader["BranchID"] == DBNull.Value ? null : (int?)reader["BranchID"],
                        ContentType = reader["ContentType"] == DBNull.Value ? string.Empty : (string)reader["ContentType"],
                        Date = reader["Date"] == DBNull.Value ? new DateTime() : (DateTime)reader["Date"],

                    });
                }

                reader.Close();
            }

            return News;
        }

        public static IEnumerable GetTraining()
        {
            List<Training> Training = new List<Training>();

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM Trainings ", connection);

                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    Training.Add(new Training()
                    {
                        TrainingID = (int)reader["TrainingID"],
                        NameMon = reader["NameMon"] == DBNull.Value ? string.Empty : (string)reader["NameMon"],
                        NameEng = reader["NameEng"] == DBNull.Value ? string.Empty : (string)reader["NameEng"],
                        ContentMon = reader["ContentMon"] == DBNull.Value ? string.Empty : (string)reader["ContentMon"],
                        ContentEng = reader["ContentEng"] == DBNull.Value ? string.Empty : (string)reader["ContentEng"],
                        Where = reader["Where"] == DBNull.Value ? string.Empty : (string)reader["Where"],
                        When = reader["When"] == DBNull.Value ? new DateTime() : (DateTime)reader["When"],
                        Duration = reader["Duration"] == DBNull.Value ? 0 : (decimal)reader["Duration"],
                        Status = reader["Status"] == DBNull.Value ? 0 : (decimal)reader["Status"],
                    });
                }

                reader.Close();
            }

            return Training;
        }

        public static IEnumerable GetTrainingRequests()
        {
            List<TrainingRequest> TrainingRequest = new List<TrainingRequest>();

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM TrainingRequests ", connection);

                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    TrainingRequest.Add(new TrainingRequest()
                    {
                        ID = (int)reader["ID"],
                        UserID = (int)reader["UserID"],
                        TrainingID = (int)reader["TrainingID"],
                        Status = (int)reader["Status"]
                    });
                }

                reader.Close();
            }

            return TrainingRequest;
        }
        public static IEnumerable GetTrainingRequests(int TrainingID)
        {
            List<TrainingRequest> TrainingRequest = new List<TrainingRequest>();

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM TrainingRequests where TrainingID = " + TrainingID, connection);

                connection.Open();

                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    TrainingRequest.Add(new TrainingRequest()
                    {
                        ID = (int)reader["ID"],
                        UserID = (int)reader["UserID"],
                        TrainingID = (int)reader["TrainingID"],
                        Status = (int)reader["Status"]
                    });
                }

                reader.Close();
            }

            return TrainingRequest;
        }

        public static void RemoveUserByID(int UserID)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM TrainingRequests WHERE UserID = " + UserID, connection);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
            }
        }

        public static void DeleteTrainingRequest(int ID)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM TrainingRequests WHERE ID = " + ID, connection);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
            }
        }

        public static void InsertTrainingRequest(TrainingRequest TrainingRequest)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO TrainingRequests (TrainingID, UserID, Status) VALUES (@TrainingID, @UserID, 0)", connection);

                insertCommand.Parameters.AddWithValue("@UserID", TrainingRequest.UserID);
                insertCommand.Parameters.AddWithValue("@TrainingID", TrainingRequest.TrainingID);

                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateTrainingRequest(TrainingRequest TrainingRequest)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand updateCommand = new SqlCommand("UPDATE [TrainingRequests] SET [TrainingID] = @TrainingID, [UserID] = @UserID, Status = @Status WHERE [ID] = @ID", connection);

                updateCommand.Parameters.AddWithValue("@UserID", TrainingRequest.UserID);
                updateCommand.Parameters.AddWithValue("@TrainingID", TrainingRequest.TrainingID);
                updateCommand.Parameters.AddWithValue("@Status", TrainingRequest.Status);

                updateCommand.Parameters.AddWithValue("@ID", TrainingRequest.ID);

                connection.Open();
                updateCommand.ExecuteNonQuery();
            }
        }

    }
}