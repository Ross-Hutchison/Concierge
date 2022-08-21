// File used for general control of system functionality and interaction with the database

using System;
using System.Data.SqlClient;
using System.Linq;
using Staff;
using Information;



namespace Control
{


    //Class for initialisation and usage of the database.
    class DatabaseControl {

    const string username = "";
    // const string connectionString = @"Data Source=localhost;Initial Catalog=concierge;";
    const string connectionString = "Server=localhost;Database=concierge;Trusted_Connection=True;";
    const string selectQueryBase = "select name, ingredients, steps from test";
    public static SqlConnection connection = new SqlConnection(connectionString);

    public static void OpenConnection() {
        connection.Open();
        System.Console.WriteLine("hi\n");
    }

    public static void CloseConnection() {
        connection.Close();
        System.Console.WriteLine("bye\n");
    }

    public static string queryRecipie(String criteria) {
        string retval = "";
        string query = "" + selectQueryBase;
        if(criteria != "") query += ("WHERE " + criteria); 
        
        SqlCommand com = new SqlCommand(query, connection);
        SqlDataReader reader = com.ExecuteReader();
        while(reader.Read()) {
            string name = (String)reader.GetValue(0);
            string ingredients = ((String)reader.GetValue(1)).Replace("\n", (""+'\n'));
            String steps = ((String)reader.GetValue(2)).Replace("\n", (""+'\n'));
            retval += "\\/\\/\\/\\/\\/\\\n" + name + "\n----------\n" + ingredients + "\n----------\n" + steps + "\n\\/\\/\\/\\/\\/\\\n"; 
        }
        if(retval.Equals("")) retval = "No match found\n";
        return retval;
    }

    }

    //Class for holding the event handlers for the main user interactions 
    class userCommandHandler {
        public static void addNewRecipie() {
            Recipie r = Recipie.AddRecipie();
            string[] r_db = r.ToDatabaseEntry().Split(Recipie.GroupSeparator); 

            if(r_db.Length < 4) {
                System.Console.WriteLine("ERROR: Malformed Database Entry in addNewRecipie handler\n");
                return;
            }

            SqlTransaction transaction; //transaction top ensure both inserts occur or neither occur
            transaction = DatabaseControl.connection.BeginTransaction();

            //strings that detail the begining of the insert queriers
            String insertStr = "INSERT INTO test (name,ingredients,steps) VALUES('" + r_db[0] + "','" + r_db[1] + "','" + r_db[2] + "')";
            String tagsInsertString = "INSERT INTO test_tags (name, tag) VALUES";

            System.Console.WriteLine(insertStr); //DEBUG

            try {

                //create a new SQL command for the first insert, and make it part of the transaction
                SqlCommand command = new SqlCommand(insertStr, DatabaseControl.connection); 
                command.Connection = DatabaseControl.connection;
                command.Transaction = transaction;

                //create a new adapter to execute the command and update the Database
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = new SqlCommand(insertStr, DatabaseControl.connection);
                // adapter.InsertCommand.Connection = DatabaseControl.connection;
                // adapter.InsertCommand.Transaction = transaction;
                adapter.InsertCommand = command;
                
                //execute the first insert query and close objects
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();

                string[] tags = r_db[3].Split(Recipie.UnitSeparator);
                for(int i = 0; i < tags.Length; i++) {
                    if(i > 0) tagsInsertString += ",";
                    tagsInsertString += "('" + r_db[0] + "','" + tags[i] + "')";
                }

                System.Console.WriteLine(tagsInsertString); //DEBUG

                //create new command for insertion to tags table and make part of transaction
                SqlCommand tagsCommand = new SqlCommand(tagsInsertString, DatabaseControl.connection); 
                tagsCommand.Connection = DatabaseControl.connection;
                tagsCommand.Transaction = transaction;

                //create new adapter for updating the data
                SqlDataAdapter tagsAdapter = new SqlDataAdapter();
                tagsAdapter.InsertCommand = new SqlCommand(tagsInsertString, DatabaseControl.connection);
                // tagsAdapter.InsertCommand.Connection = DatabaseControl.connection;
                // tagsAdapter.InsertCommand.Transaction = transaction;
                tagsAdapter.InsertCommand = tagsCommand;

                //execute second insert query and close objects 
                tagsAdapter.InsertCommand.ExecuteNonQuery();
                tagsCommand.Dispose();

                //commit the completed transaction
                transaction.Commit();
            }
            catch (Exception e) {
                System.Console.WriteLine("Exception occurred when adding new recipie: " + e.ToString());
                try{
                    transaction.Rollback();
                }
                catch(Exception e2) {
                    System.Console.WriteLine("Exception occurred during Rollback of transaction: " + e2.ToString());
                }
                return;
            }
        }
    }

}