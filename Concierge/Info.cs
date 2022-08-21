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
            private HashSet<String> tags; // Consolidation of all above into single unordered list of distinct values


            public static char GroupSeparator = (char)29;     //character that separates sections within a recipie
            public static char RecordSeparator = (char)30;    //character that separates two recipies
            public static char UnitSeparator = (char)31;      //character that separates parts of each section in a recipie


            public Recipie() {
                this.name = "";
                this.ingredients = "";
                this.steps = "";
                this.tags = new HashSet<string>();
            }

            public string GetName() {return this.name;}
            public string GetIngredients() {return this.ingredients;}
            public string GetSteps() {return this.steps;}

            public HashSet<String> GetTags() {return this.tags;}

            public void SetName(string name) {this.name = name;}
            public void SetIngredients(string ing) {this.ingredients = ing;}
            public void SetSteps(string steps) {this.steps = steps;}

            public void setTags(string tags) {
                this.tags = new HashSet<string>();
                string[] parts = tags.Split(',');
                for(int i = 0; i < parts.Length; i++) {
                    this.tags.Add(parts[i]);
                }
            }

            public static Recipie AddRecipie() {
                Recipie r = new Recipie();
                var inpt = "";
                System.Console.WriteLine("----------\nPlease enter the recipe's name\n----------");
                
                while((inpt = System.Console.ReadLine()) == null || inpt.Length < 2) {
                    System.Console.WriteLine("----------\nInvalid name, please enter at least two characters\n----------");
                }
                r.name = inpt;

                System.Console.WriteLine("----------\nEnter Recipe Ingredients one at a time, to move on to the method enter 0\n----------");
                string ing = "";
                int ingNum = 1;
                while ( (inpt = System.Console.ReadLine()) != "0") {
                    if(inpt == null || inpt.Length < 2) {
                        System.Console.WriteLine("Invalid Ingredient Name, Minimum length of two characters\n");
                        continue;
                    }
                    else {
                        if(ingNum != 1) ing += ("\n" + ingNum + ". " + inpt);
                        else ing += (ingNum + ". " + inpt);
                        ingNum++;
                    }
                }
                r.ingredients = ing;


                System.Console.WriteLine("----------\nEnter Recipe Step by Step, to complete the recipie enter 0\n----------");
                string steps = "";
                int stepNum = 1;
                while ( (inpt = System.Console.ReadLine()) != "0" ) {
                    if(inpt == null || inpt.Length < 4) {
                        Console.WriteLine("Invalid step: Minimum length of four characters\n");
                        continue;
                    }
                    if(stepNum != 1) steps += ("\n" + stepNum + ". " + inpt);
                    else steps += (stepNum + ". " + inpt);
                    stepNum++;
                }
                r.steps = steps;


                System.Console.WriteLine("----------\nEnter tags (such as lunch, spicy, italian etc.) one after the other, to finalise the recipie enter 0\n----------");
                inpt = System.Console.ReadLine();
                while (inpt != "0") {
                    if(inpt == null || inpt.Length < 3) {
                        Console.WriteLine("Invalid tag: Minimum length of a tag is three characters\n");
                        inpt = Console.ReadLine();
                        continue;
                    }
                    r.tags.Add(inpt.ToLower());
                    inpt = System.Console.ReadLine();
                }

                return r;
            }

            public string ToDatabaseEntry() {
                string retVal = "";
                retVal += (this.name + GroupSeparator);
                retVal += (this.ingredients.Replace('\n', UnitSeparator) + GroupSeparator);
                retVal += (this.steps.Replace('\n', UnitSeparator) + GroupSeparator);
                
                int tagNum = 0;
                foreach(String str in this.tags) {
                    if(tagNum > 0) retVal += UnitSeparator;
                    retVal += str;
                    tagNum++;
                }

                System.Console.WriteLine("Converting Recipie to Database Entry gives\n" + retVal + "\n");
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