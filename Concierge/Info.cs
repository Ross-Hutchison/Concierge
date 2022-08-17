using System;
using System.Collections.Generic;
using Control;

namespace Information{

    class Recipie {
            private String name;
            private String ingredients;
            private String steps;
            // private String time;    // breakfast, lunch, dinner, desert, snack
            // private String flavours; // sweet, savoury, cheesy, sour, refreshing, spicy etc.
            // private String diet; // vegitarian, vegan, omnivore
            // private String origin; //chinese, italian, french, mexican, etc.  
            private String tags; // Consolidation of all above into single unordered list taken from DB.


            public static char GroupSeparator = (char)29;     //character that separates sections within a recipie
            public static char recordSeparator = (char)30;    //character that separates two recipies
            public static char UnitSeparator = (char)31;      //character that separates parts of each section in a recipie


            public Recipie() {
                this.name = "";
                this.ingredients = "";
                this.steps = "";
                this.tags = "";
            }

            public string GetName() {return this.name;}
            public string GetIngredients() {return this.ingredients;}
            public string GetSteps() {return this.steps;}

            public string GetTags() {return this.tags;}

            public void SetName(string name) {this.name = name;}
            public void SetIngredients(string ing) {this.ingredients = ing;}
            public void SetSteps(string steps) {this.steps = steps;}

            public void SetTags(string tags) {this.tags = tags;}

            public static Recipie AddRecipie() {
                Recipie r = new Recipie();
                var inpt = "";
                System.Console.WriteLine("----------\nPlease enter the recipe's name\n----------");
                inpt = System.Console.ReadLine();
                r.name = inpt;

                System.Console.WriteLine("----------\nEnter Recipe Ingredients one at a time, to move on to the method enter 0\n----------");
                string ing = "";
                int ingNum = 1;
                while ( (inpt = System.Console.ReadLine()) != "0") {
                    if(ingNum != 1) ing += ("\n" + ingNum + ". " + inpt);
                    else ing += (ingNum + ". " + inpt);
                    ingNum++;
                }
                r.ingredients = ing;

                System.Console.WriteLine("----------\nEnter Recipe Step by Step, to complete the recipie enter 0\n----------");
                string steps = "";
                int stepNum = 1;
                while ( (inpt = System.Console.ReadLine()) != "0") {
                    if(stepNum != 1) steps += ("\n" + stepNum + ". " + inpt);
                    else steps += (stepNum + ". " + inpt);
                    stepNum++;
                }
                r.steps = steps;

                System.Console.WriteLine("----------\nEnter a comma separated list of single word recipie tags such as lunch,spicy,italian\n----------");
                inpt = null;
                do {inpt = System.Console.ReadLine();}
                while( inpt == null || inpt.Length < 3);
                inpt = inpt.Remove(0, ' ').ToLower();   //remove any spaces from the data and make lower case
                
                // r.tags = "testVal,testVal2,testVal3";

                return r;
            }

            public string ToDatabaseEntry() {
                string retVal = "";
                retVal += (this.name + GroupSeparator);
                retVal += (this.ingredients.Replace('\n', UnitSeparator) + GroupSeparator);
                retVal += (this.steps.Replace('\n', UnitSeparator) + GroupSeparator);
                retVal += (this.tags);
                return retVal;
            }

            //Function to convert a recipie to a String version
            public override string ToString() {
                int nameLen = this.name.Length;
                int outputLen = (12 + nameLen); //The number 12 comes from | and 5 spaces on either side of the name
                string straightLine = new String('-', outputLen);
                string retVal = "";
                retVal += straightLine;
                retVal += "\n|     " + this.name + "     |\n";
                retVal += (straightLine + "\n");
                retVal += this.ingredients;
                retVal += (straightLine + "\n");
                retVal += this.steps;
                retVal += (straightLine + "\n");
                retVal += this.tags;

                return retVal;
            }
    }
}