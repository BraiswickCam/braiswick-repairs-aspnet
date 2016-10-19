using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace WebApplication2
{
    public class RepairsSQL
    {
        //The object used for the main database connection string
        SQLiteConnection m_dbConnection;

        /// <summary>
        /// Constructor used to create the main database connection string
        /// </summary>
        /// <param name="databaseLocation">Location of the SQLite database</param>
        public RepairsSQL(string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite")
        {
            m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation));
            //"datetimeformat=CurrentCulture" makes sure that DateTimes work when brought back from the database
        }

        /// <summary>
        /// Used to close any exisiting connections to the database in case of an SQL exception
        /// </summary>
        public void CloseConnection()
        {
            m_dbConnection.Close();
        }

        /// <summary>
        /// Adds a new photographer to the database
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="intitial">Initials</param>
        /// <param name="active">Employment status</param>
        /// <param name="office">Closest office</param>
        public void NewPhotog(string name, string intitial, bool active, string office)
        {
            m_dbConnection.Open();
            string activeString = active ? "1" : "0";
            string sql = String.Format("INSERT INTO Photographers (Name, Initials, Active, Office) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", 
                name, intitial, activeString, office);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
            sql = String.Format("INSERT INTO Repairs (PhotogID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", 1, \"{1}\", \"{2}\")",
                PhotogIDSearch(intitial), DateTime.Now, "New photographer added to database");
            m_dbConnection.Open();
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Updates an exisiting photographers record
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="initial">Initials</param>
        /// <param name="active">Employment status</param>
        /// <param name="office">Closest office</param>
        /// <param name="id">Photographers database ID, can be found using PhotogIDSearch()</param>
        public void UpdatePhotog(string name, string initial, bool active, string office, int id)
        {
            m_dbConnection.Open();
            string activeString = active ? "1" : "0";
            string sql = String.Format("UPDATE Photographers SET Name = \"{0}\", Initials = \"{1}\", Active = \"{2}\", Office = \"{3}\" WHERE ID = {4}",
                name, initial, activeString, office, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = String.Format("INSERT INTO Repairs (PhotogID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", 1, \"{1}\", \"{2}\")",
                id, DateTime.Now, "Photographer details updated");
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Finds a photographers database ID from their initials
        /// </summary>
        /// <param name="initial">Initials</param>
        /// <returns>The database ID for the photographer as an integer</returns>
        public int PhotogIDSearch(string initial)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT ID FROM Photographers WHERE Initials = \"{0}\"", initial);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int lookID = -1;
            try { lookID = Convert.ToInt32(reader["ID"]); }
            catch (System.InvalidCastException) { lookID = -1; }
            m_dbConnection.Close();
            return lookID;
        }

        /// <summary>
        /// Retrieve a photographers record from the database using their ID
        /// </summary>
        /// <param name="id">The photographers database ID</param>
        /// <returns>[0] name, [1] initials, [2] active, [3] office</returns>
        public object[] LookUpPhotog(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Photographers WHERE ID = \"{0}\"", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string name = reader["Name"].ToString();
            string initials = reader["Initials"].ToString();
            bool active;
            try { active = Convert.ToBoolean(reader["Active"]); }
            catch (System.InvalidCastException) { active = false; }
            string office = reader["Office"].ToString();
            object[] objectArray = new object[] { name, initials, active, office };
            m_dbConnection.Close();
            return objectArray;
        }

        /// <summary>
        /// Adds a new laptop to the database
        /// </summary>
        /// <param name="serial">Serial number</param>
        /// <param name="make">Make</param>
        /// <param name="model">Model</param>
        /// <param name="os">Operating System</param>
        /// <param name="active">If false, laptop has been salvaged for parts/sold etc.</param>
        public void NewLaptop(string serial, string make, string model, string os, bool active)
        {
            m_dbConnection.Open();
            string activeString = active ? "1" : "0";
            string sql = String.Format("INSERT INTO Laptops (SerialNumber, Make, Model, OS, Active) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\")",
                serial, make, model, os, activeString);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
            sql = String.Format("INSERT INTO Repairs (LaptopID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", 1, \"{1}\", \"Laptop added to database\")",
                GetEquipmentID(serial, "Laptop"), DateTime.Now);
            m_dbConnection.Open();
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Retrieves the unique ID for a piece of equipment from its serial number
        /// </summary>
        /// <param name="serial">The items serial number</param>
        /// <param name="type">"Camera" or "Laptop"</param>
        /// <returns>The unique ID number as an int</returns>
        public int GetEquipmentID(string serial, string type)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT {1}ID FROM {1}s WHERE SerialNumber = \"{0}\"", serial, type);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int equipID = 0;
            while (reader.Read()) equipID = Convert.ToInt32(reader[type + "ID"]);
            m_dbConnection.Close();
            return equipID;
        }

        /// <summary>
        /// Update a laptops record in the database
        /// </summary>
        /// <param name="id">LaptopID</param>
        /// <param name="serial">Serial Number</param>
        /// <param name="make">Make</param>
        /// <param name="model">Model</param>
        /// <param name="os">Operating System</param>
        /// <param name="active">If false, laptop has been decomissioned</param>
        public void UpdateLaptop(int id, string serial, string make, string model, string os, bool active)
        {
            m_dbConnection.Open();
            string activeString = active ? "1" : "0";
            string sql = String.Format("UPDATE Laptops SET SerialNumber = \"{0}\", Make = \"{1}\", Model = \"{2}\", OS = \"{3}\", Active = \"{4}\" WHERE LaptopID = {5}",
                serial, make, model, os, activeString, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = String.Format("INSERT INTO Repairs (LaptopID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", 1, \"{1}\", \"Laptop details updated\")",
                id, DateTime.Now);
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Adds a new camera to the database
        /// </summary>
        /// <param name="serial">Serial number</param>
        /// <param name="make">Make</param>
        /// <param name="model">Model</param>
        /// <param name="active">If false, camera has been salvaged for parts/sold etc.</param>
        public void NewCamera(string serial, string make, string model, bool active)
        {
            m_dbConnection.Open();
            string activeString = active ? "1" : "0";
            string sql = String.Format("INSERT INTO Cameras (SerialNumber, Make, Model, Active) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")",
                serial, make, model, activeString);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
            sql = String.Format("INSERT INTO Repairs (CameraID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", 1, \"{1}\", \"Camera added to database\")",
                GetEquipmentID(serial, "Camera"), DateTime.Now);
            m_dbConnection.Open();
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Update a cameras record in the database
        /// </summary>
        /// <param name="id">CameraID</param>
        /// <param name="serial">Serial Number</param>
        /// <param name="make">Make</param>
        /// <param name="model">Model</param>
        /// <param name="active">If false, camera has been decomissioned</param>
        public void UpdateCamera(int id, string serial, string make, string model, bool active)
        {
            m_dbConnection.Open();
            string activeString = active ? "1" : "0";
            string sql = String.Format("UPDATE Cameras SET SerialNumber = \"{0}\", Make = \"{1}\", Model = \"{2}\", Active = \"{3}\" WHERE CameraID = {4}",
                serial, make, model, activeString, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = String.Format("INSERT INTO Repairs (CameraID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", 1, \"{1}\", \"Camera details updated\")",
                id, DateTime.Now);
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Retrieves all the kit ph numbers from the databse
        /// </summary>
        /// <returns>All exisiting kit ph numbers as a string array</returns>
        public string[] GetAllKits()
        {
            m_dbConnection.Open();
            List<string> kits = new List<string>();
            string sql = "SELECT KitPH FROM Kits";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                kits.Add(reader["KitPH"].ToString());
            }
            m_dbConnection.Close();
            return kits.ToArray();
        }

        /// <summary>
        /// Retrieves all IDs associated with a kits ph number including the kits ID, the photographers ID, camera ID and laptop ID
        /// </summary>
        /// <param name="kitPH">The kit ph number</param>
        /// <returns>[0] = PhotogID, [1] = LaptopID, [2] = CameraID, [3] = KitID</returns>
        public int[] LookUpKit(string kitPH)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Kits WHERE KitPH = \"{0}\"", kitPH);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int photogID, laptopID, cameraID, kitID, spareLaptopID, spareCameraID;
            try { photogID = Convert.ToInt32(reader["PhotogID"]); }
            catch (System.InvalidCastException) { photogID = 0; }
            try { laptopID = Convert.ToInt32(reader["LaptopID"]); }
            catch (System.InvalidCastException) { laptopID = 0; }
            try { cameraID = Convert.ToInt32(reader["CameraID"]); }
            catch (System.InvalidCastException) { cameraID = 0; }
            try { spareLaptopID = Convert.ToInt32(reader["SpareLaptopID"]); }
            catch (System.InvalidCastException) { spareLaptopID = 0; }
            try { spareCameraID = Convert.ToInt32(reader["SpareCameraID"]); }
            catch (System.InvalidCastException) { spareCameraID = 0; }
            kitID = Convert.ToInt32(reader["KitID"]);
            m_dbConnection.Close();
            return new int[] { photogID, laptopID, cameraID, kitID, photogID, spareLaptopID, spareCameraID };
        }

        /// <summary>
        /// Retrieves the record for a kit from its KitID number
        /// </summary>
        /// <param name="id">KitID</param>
        /// <returns>[0] KitPH, [1] PhotogID, [2] LaptopID, [3] CameraID, [4] SpareLaptopID, [5] SpareCameraID</returns>
        public object[] LookUpKitAlt(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Kits WHERE KitID = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            object kitph = reader["KitPh"];
            object photogID, laptopID, cameraID, sparelaptopID, sparecameraID;
            try { photogID = Convert.ToInt32(reader["PhotogID"]); }
            catch (System.InvalidCastException) { photogID = 0; }
            try { laptopID = Convert.ToInt32(reader["LaptopID"]); }
            catch (System.InvalidCastException) { laptopID = 0; }
            try { cameraID = Convert.ToInt32(reader["CameraID"]); }
            catch (System.InvalidCastException) { cameraID = 0; }
            try { sparelaptopID = Convert.ToInt32(reader["SpareLaptopID"]); }
            catch (System.InvalidCastException) { sparelaptopID = 0; }
            try { sparecameraID = Convert.ToInt32(reader["SpareCameraID"]); }
            catch (System.InvalidCastException) { sparecameraID = 0; }
            m_dbConnection.Close();
            return new object[] { id, kitph, photogID, laptopID, cameraID, sparelaptopID, sparecameraID };
        }

        /// <summary>
        /// Retrieves a list of all camera IDs stored in the databse. Boolean controls whether to return all, or just those not associated with a kit
        /// </summary>
        /// <param name="availableOnly">If true, will only return camera IDs not already associated with a kit</param>
        /// <returns>An integer array of camera IDs</returns>
        public int[] GetAllCameraIDs(bool availableOnly)
        {
            List<int> cameraIDs = new List<int>();
            if (availableOnly)
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT CameraID FROM Cameras WHERE Active = 1 AND NOT EXISTS (SELECT 1 FROM Kits WHERE Kits.CameraID = Cameras.CameraID) AND NOT EXISTS (SELECT 1 FROM Kits WHERE Kits.SpareCameraID = Cameras.CameraID)");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { cameraIDs.Add(Convert.ToInt32(reader["CameraID"])); }
            }
            else
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT CameraID FROM Cameras");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { cameraIDs.Add(Convert.ToInt32(reader["CameraID"])); }
            }
            m_dbConnection.Close();
            return cameraIDs.ToArray();
        }

        /// <summary>
        /// Retrieves an array of all photographers unique IDs from the database. Bool can be set to filter only photographers not assigned to a kit
        /// </summary>
        /// <param name="availableOnly">If true, only returns IDs for photographers who are not assigned to a kit</param>
        /// <returns>An integer array of photgrapher IDs</returns>
        public int[] GetAllPhotogIDs(bool availableOnly)
        {
            List<int> photogIDs = new List<int>();
            if (availableOnly)
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT ID FROM Photographers WHERE Active = 1 AND NOT EXISTS (SELECT 1 FROM Kits WHERE Kits.PhotogID = Photographers.ID)");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { photogIDs.Add(Convert.ToInt32(reader["ID"])); }
            }
            else
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT ID FROM Photographers");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { photogIDs.Add(Convert.ToInt32(reader["ID"])); }
            }
            m_dbConnection.Close();
            return photogIDs.ToArray();
        }

        /// <summary>
        /// Retrieves an array of all photographers IDs. Bool can be set to filter out those whose active field is set to false
        /// </summary>
        /// <param name="active">If true, only returns photographer IDs whose active field is set to true</param>
        /// <returns>An integer array of photographer IDs</returns>
        public int[] GetAllActivePhotogIDs(bool active)
        {
            List<int> photogIDs = new List<int>();
            if (active)
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT ID FROM Photographers WHERE Active = 1");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { photogIDs.Add(Convert.ToInt32(reader["ID"])); }
            }
            else
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT ID FROM Photographers");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { photogIDs.Add(Convert.ToInt32(reader["ID"])); }
            }
            m_dbConnection.Close();
            return photogIDs.ToArray();
        }

        /// <summary>
        /// Retrieves a list of all laptop IDs stored in the database. Boolean controls whether to return all, or just those not associated with a kit
        /// </summary>
        /// <param name="availableOnly">If true, will only return laptop IDs not already associated with a kit</param>
        /// <returns>An integer array of laptop IDs</returns>
        public int[] GetAllLaptopIDs(bool availableOnly)
        {
            List<int> laptopIDs = new List<int>();
            if (availableOnly)
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT LaptopID FROM Laptops WHERE Active = 1 AND NOT EXISTS (SELECT 1 FROM Kits WHERE Kits.LaptopID = Laptops.LaptopID) AND NOT EXISTS (SELECT 1 FROM Kits WHERE Kits.SpareLaptopID = Laptops.LaptopID)");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { laptopIDs.Add(Convert.ToInt32(reader["LaptopID"])); }
            }
            else
            {
                m_dbConnection.Open();
                string sql = String.Format("SELECT LaptopID FROM Laptops");
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { laptopIDs.Add(Convert.ToInt32(reader["LaptopID"])); }
            }
            m_dbConnection.Close();
            return laptopIDs.ToArray();
        }

        /// <summary>
        /// Returns a list of all kit IDs
        /// </summary>
        /// <returns>Integer array of kit IDs</returns>
        public int[] GetAllKitIDs()
        {
            List<int> kitIDs = new List<int>();
            m_dbConnection.Open();
            string sql = "SELECT KitID From Kits";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) { kitIDs.Add(Convert.ToInt32(reader["KitID"])); }
            m_dbConnection.Close();
            return kitIDs.ToArray();
        }

        /// <summary>
        /// Returns the full record for a given camera ID
        /// </summary>
        /// <param name="id">The camera ID</param>
        /// <returns>[0] SerialNumber, [1] Make, [2] Model, [3] Active</returns>
        public object[] LookUpCamera(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Cameras WHERE CameraID = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            object sn = reader["SerialNumber"];
            object make = reader["Make"];
            object model = reader["Model"];
            object active = reader["Active"];
            m_dbConnection.Close();
            return new object[] { sn, make, model, active };
        }

        /// <summary>
        /// Returns the full record for a given laptop ID
        /// </summary>
        /// <param name="id">The laptop ID</param>
        /// <returns>[0] SerialNumber, [1] Make, [2] Model, [3] OS, [4] Active</returns>
        public object[] LookUpLaptop(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Laptops WHERE LaptopID = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            object sn = reader["SerialNumber"];
            object make = reader["Make"];
            object model = reader["Model"];
            object os = reader["OS"];
            object active = reader["Active"];
            m_dbConnection.Close();
            return new object[] { sn, make, model, os, active };
        }

        /// <summary>
        /// Retrieves all the entries for a given cameras repair history
        /// </summary>
        /// <param name="id">The cameras ID</param>
        /// <returns>An array of arrays, for each: [0] RepairID, [1] CameraID, [2] KitID, [3] PhotogID, [4] Date, [5] Fixed, [6] FixedDate, [7] TechInitials, [8] Notes, [9] RepairCost</returns>
        public object[][] GetCameraHistory(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Repairs WHERE CameraID = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<object[]> repairEntries = new List<object[]>();
            while (reader.Read())
            {
                object repairID = reader["RepairID"];
                object cameraID = reader["CameraID"];
                object kitID = reader["KitID"];
                object photogID = reader["PhotogID"];
                object date = reader["Date"];
                object fixedBool = reader["Fixed"];
                object fixedDate = reader["FixedDate"].ToString();
                object techInitials = reader["TechInitials"];
                object notes = reader["Notes"];
                object repairCost = reader["RepairCost"];
                object[] entry = new object[] { repairID, cameraID, kitID, photogID, date, fixedBool, fixedDate, techInitials, notes, repairCost };
                repairEntries.Add(entry);
            }
            m_dbConnection.Close();
            return repairEntries.ToArray();
        }

        /// <summary>
        /// Retrieves all the entries for a given laptops repair history
        /// </summary>
        /// <param name="id">The laptops ID</param>
        /// <returns>An array of arrays, for each: [0] RepairID, [1] LaptopID, [2] KitID, [3] PhotogID, [4] Date, [5] Fixed, [6] FixedDate, [7] TechInitials, [8] Notes, [9] RepairCost</returns>
        public object[][] GetLaptopHistory(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Repairs WHERE LaptopID = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<object[]> repairEntries = new List<object[]>();
            while (reader.Read())
            {
                object repairID = reader["RepairID"];
                object laptopID = reader["LaptopID"];
                object kitID = reader["KitID"];
                object photogID = reader["PhotogID"];
                object date = reader["Date"];
                object fixedBool = reader["Fixed"];
                object fixedDate = reader["FixedDate"].ToString();
                object techInitials = reader["TechInitials"];
                object notes = reader["Notes"];
                object repairCost = reader["RepairCost"];
                object[] entry = new object[] { repairID, laptopID, kitID, photogID, date, fixedBool, fixedDate, techInitials, notes, repairCost };
                repairEntries.Add(entry);
            }
            m_dbConnection.Close();
            return repairEntries.ToArray();
        }

        /// <summary>
        /// Retrieves all the entries for a given kits repair history
        /// </summary>
        /// <param name="id">The kit ID</param>
        /// <returns>An array of arrays, for each: [0] RepairID, [1] CameraID, [2] LaptopID, [3] PhotogID, [4] Date, [5] Fixed, [6] FixedDate, [7] TechInitials, [8] Notes, [9] RepairCost, [10] KitID</returns>
        public object[][] GetKitHistory(int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Repairs WHERE KitID = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<object[]> repairEntries = new List<object[]>();
            while (reader.Read())
            {
                object repairID = reader["RepairID"];
                object cameraID = reader["CameraID"];
                object laptopID = reader["LaptopID"];
                object photogID = reader["PhotogID"];
                object date = reader["Date"];
                object fixedBool = reader["Fixed"];
                object fixedDate = reader["FixedDate"].ToString();
                object techInitials = reader["TechInitials"];
                object notes = reader["Notes"];
                object repairCost = reader["RepairCost"];
                object kitID = reader["KitID"];
                object[] entry = new object[] { repairID, cameraID, laptopID, photogID, date, fixedBool, fixedDate, techInitials, notes, repairCost, kitID };
                repairEntries.Add(entry);
            }
            m_dbConnection.Close();
            return repairEntries.ToArray();
        }

        /// <summary>
        /// Retrieves all repair/history records for a given item as an array of object arrays
        /// </summary>
        /// <param name="type">"CameraID", "LaptopID", "KitID", "PhotogID"</param>
        /// <param name="id">The ID number as an int</param>
        /// <returns></returns>
        public object[][] GetAllHistory(string type, int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT * FROM Repairs WHERE {1} = {0}", id, type);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<object[]> repairEntries = new List<object[]>();
            while (reader.Read())
            {
                object repairID = reader["RepairID"];
                object cameraID = reader["CameraID"];
                object laptopID = reader["LaptopID"];
                object kitID = reader["KitID"];
                object photogID = reader["PhotogID"];
                object date = reader["Date"];
                object fixedBool = reader["Fixed"];
                object fixedDate = reader["FixedDate"].ToString();
                object techInitials = reader["TechInitials"];
                object notes = reader["Notes"];
                object repairCost = reader["RepairCost"];
                object[] entry = new object[] { repairID, cameraID, laptopID, kitID, photogID, date, fixedBool, fixedDate, techInitials, notes, repairCost };
                repairEntries.Add(entry);
            }
            m_dbConnection.Close();
            return repairEntries.ToArray();
        }

        /// <summary>
        /// Remove an item from an exisiting kit. Will also add an entry to the repair/history log
        /// </summary>
        /// <param name="remove">"PhotogID", "CameraID" or "LaptopID"</param>
        /// <param name="id">The kit ID</param>
        /// <param name="removeid">The item of the item being removed</param>
        public void RemoveFromKit(string remove, int id, int removeid)
        {
            m_dbConnection.Open();
            string sql = String.Format("UPDATE Kits SET {0} = null WHERE KitID = {1}", remove, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            string remove2 = remove.Substring(0, 5) == "Spare" ? remove.Substring(5) : remove;
            sql = String.Format("INSERT INTO Repairs ({0}, Date, Fixed, FixedDate, Notes, KitID) VALUES ({1}, \"{2}\", \"{3}\", \"{4}\", \"{5}\", {6})",
                remove2, removeid, DateTime.Now, "1", DateTime.Now, remove + " removed from kit", id);
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Add an item to an exisiting kit. Will also add an entry to the repair/history log
        /// </summary>
        /// <param name="add">"PhotogID", "CameraID" or "LaptopID"</param>
        /// <param name="newid">New items ID</param>
        /// <param name="kitid">The kits ID</param>
        /// <param name="photogid">Photographers ID</param>
        public void AddToKit(string add, int newid, int kitid, int photogid)
        {
            m_dbConnection.Open();
            string sql = String.Format("UPDATE Kits SET {0} = {1} WHERE KitID = {2}", add, newid, kitid);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            string add2 = add.Substring(0, 5) == "Spare" ? add.Substring(5) : add;
            sql = String.Format("INSERT INTO Repairs ({0}, KitID, PhotogID, Date, Fixed, FixedDate, Notes) VALUES ({1}, {2}, \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\")",
                add2, newid, kitid, photogid, DateTime.Now, "1", DateTime.Now, add +" added to kit");
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Creates an entry for a new kit in the database
        /// </summary>
        /// <param name="ph">KitPH name</param>
        public void NewKit(string ph)
        {
            m_dbConnection.Open();
            string sql = String.Format("INSERT INTO Kits (KitPH) VALUES (\"{0}\")", ph);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
            int[] kitID = LookUpKit(ph);
            m_dbConnection.Open();
            sql = String.Format("INSERT INTO Repairs (KitID, Date, Fixed, FixedDate, Notes) VALUES ({0}, \"{1}\", \"{2}\", \"{3}\", \"{4}\")",
                kitID[3], DateTime.Now, "1", DateTime.Now, "Kit " + ph + " created.");
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Updates an exisiting repair entry in the repairs database
        /// </summary>
        /// <param name="repairID">The exisiting repairID</param>
        /// <param name="cameraID">CameraID</param>
        /// <param name="laptopID">LaptopID</param>
        /// <param name="kitID">KitID</param>
        /// <param name="photogID">PhotogID</param>
        /// <param name="date">The date the entry was created</param>
        /// <param name="fix">If the repair has been completed, or true if n/a</param>
        /// <param name="fixedDate">The date the repair was completed, or same as date if n/a</param>
        /// <param name="techInitials">TechInitials</param>
        /// <param name="notes">Any notes on the repair/history</param>
        /// <param name="repairCost">Cost of repair</param>
        public void UpdateRepairEntry(int repairID, int cameraID, int laptopID, int kitID, int photogID, DateTime date, bool fix, DateTime fixedDate, string techInitials, string notes, double repairCost)
        {
            m_dbConnection.Open();
            int fixedInt = fix ? 1 : 0;
            string sql = String.Format("UPDATE Repairs SET CameraID = {0}, LaptopID = {1}, KitID = {2}, PhotogID = {3}, Date = \"{4}\", Fixed = {5}, FixedDate = \"{6}\", TechInitials = \"{7}\", Notes = \"{8}\", RepairCost = {9} WHERE RepairID = {10}",
                cameraID, laptopID, kitID, photogID, date, fixedInt, fixedDate, techInitials, notes, repairCost, repairID);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Creates a new repair entry in the Repairs database table
        /// </summary>
        /// <param name="cameraID">CameraID</param>
        /// <param name="laptopID">LaptopID</param>
        /// <param name="kitID">KitID</param>
        /// <param name="photogID">PhotogID</param>
        /// <param name="date">Current date and time</param>
        /// <param name="fix">True if item repaired of if n/a</param>
        /// <param name="fixedDate">If fixed true, current time/date</param>
        /// <param name="techInitials">TechInitials</param>
        /// <param name="notes">Any notes about the repair/history</param>
        /// <param name="repairCost">RepairCost</param>
        public void NewRepairEntry(int cameraID, int laptopID, int kitID, int photogID, DateTime date, bool fix, DateTime fixedDate, string techInitials, string notes, double repairCost)
        {
            m_dbConnection.Open();
            int fixedInt = fix ? 1 : 0;
            string sql = String.Format("INSERT INTO Repairs (CameraID, LaptopID, KitID, PhotogID, Date, Fixed, FixedDate, TechInitials, Notes, RepairCost) VALUES ({0}, {1}, {2}, {3}, \"{4}\", {5}, \"{6}\", \"{7}\", \"{8}\", {9})",
                cameraID, laptopID, kitID, photogID, date, fixedInt, fixedDate, techInitials, notes, repairCost);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Checks if an item has any outstanding repairs (repair entry with 'Fixed' marked false)
        /// </summary>
        /// <param name="item">"Camera" or "Laptop"</param>
        /// <param name="id">The items ID number</param>
        /// <returns>False = outstanding repair</returns>
        public bool CheckFixedStatus(string item, int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT Fixed FROM Repairs WHERE {0}ID = {1}", item, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<bool> fixedStatus = new List<bool>();
            while (reader.Read()) { fixedStatus.Add(Convert.ToBoolean(reader["Fixed"])); }
            m_dbConnection.Close();
            foreach (bool b in fixedStatus)
            {
                if (!b) return false;
            }
            return true;
        }

        /// <summary>
        /// Checks to see whether the given initials are already assigned to a photographer in the database
        /// </summary>
        /// <param name="initials">The given initials</param>
        /// <param name="id">If users initails are trying to be changed, this is the PhotogID of the user. Otherwise = 0</param>
        /// <returns></returns>
        public bool CheckInitials(string initials, int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT ID FROM Photographers WHERE Initials = \"{0}\" AND ID != {1}", initials, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<string> init = new List<string>();
            while (reader.Read()) { init.Add(reader["ID"].ToString()); }
            m_dbConnection.Close();
            return init.Count > 0 ? true : false;
        }

        /// <summary>
        /// Used primarily to check whether a piece of equipment is already assigned to a kit and retrieve the KitID
        /// </summary>
        /// <param name="idType">"CameraID", "LaptopID", "PhotogID", "KitID"</param>
        /// <param name="repairType">"CameraID", "LaptopID", "PhotogID", "KitID"</param>
        /// <param name="id">The repairTypes ID</param>
        /// <returns></returns>
        public int CheckRelatedIDs(string idType, string repairType, int id)
        {
            m_dbConnection.Open();
            string sql = String.Format("SELECT {0} FROM Kits WHERE {1} = {2}", idType, repairType, id);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int outputID = 0;
            while (reader.Read()) outputID = Convert.ToInt32(reader[idType]);
            m_dbConnection.Close();
            return outputID;
        }

        //public object[] KitHistory(int id)
        //{
        //    m_dbConnection.Open();
        //    string sql = String.Format("SELECT * FROM Repairs WHERE KitID = {0}", id);
        //    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        //    SQLiteDataReader reader = command.ExecuteReader();

        //}
    }
}
