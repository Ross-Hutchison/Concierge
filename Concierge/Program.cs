using Control;

namespace AppMain {
    class program {
        public static void Main() {
            
            DatabaseControl.OpenConnection();

            // string recipie = DatabaseControl.queryRecipie("");
            // System.Console.WriteLine(recipie);

            userCommandHandler.addNewRecipie();

            // Recipie r = Recipie.AddRecipie();
            // System.Console.WriteLine("Have Stored:\n " + r);

            DatabaseControl.CloseConnection();

        }
    }
}