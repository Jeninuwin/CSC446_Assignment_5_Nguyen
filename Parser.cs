/// <summary>
/// Name: Jenny Nguyen 
/// Assignment: 5
/// Description: 
/// </summary>
using System;


namespace CSC446_Assignment_5_Nguyen
{
    /// <summary>
    /// Defines the <see cref="Parser" />.
    /// </summary>
    public class Parser
    {
        public static int currentOffset = 0;
        public static int charOffset = 1;
        public static int floatOffset = 4;
        public static int intOffset = 2;
        public static int totalOffset;
        public static int depth = 0;
        public static int tempies;

        public static SymbolTable.entryTable val = new SymbolTable.entryTable();

        public SymbolTable sym = new SymbolTable();
        /// <summary>
        /// Defines the increments  while setting it to 0.
        /// </summary>
        public static int increments = 0;
        public static string temp = "";
        /// <summary>
        /// The Parse will grab the MatchToken list and will call Prog to start the parsing
        /// </summary>
        public static void Parse()
        {
            //Console.WriteLine(temp.PadRight(22, ' ') + "Tokens");
            //Console.WriteLine(temp.PadRight(14, ' ') + "________________________");
            //for (int i = 0; i < Lexie.counting; i++)
            //{
            //    Console.WriteLine(Lexie.MatchTokens[i].PadLeft(28, ' '));
            //}

            Prog();
        }

        /// <summary>
        /// The Prog sees if the the current MatchToken equals either int, float, or char. If it doesn't equal any of those go to eofft
        /// </summary>
        public static void Prog()
        {
            bool done = false;

            while (done == false) //if done is true it will exit the loop and prog will be completed
            {
                switch (Lexie.MatchTokens[increments])
                {
                    case "intt":
                    case "floatt":
                    case "chart":
                        {
                            //offset increase base on type 
                            if(Lexie.MatchTokens[increments] == "intt")
                            {
                                currentOffset = intOffset;
                            }
                            else if (Lexie.MatchTokens[increments] == "floatt")
                            {
                                currentOffset = floatOffset;
                            }
                            else
                            {
                                currentOffset = charOffset;
                            }

                            totalOffset += currentOffset;

                            increments++;

                            switch (Lexie.MatchTokens[increments]) //this will see if the MatchTokens will match "idt" if it does call Rest, else error
                            {
                                case "idt":
                                    {
                                        //store after the value into a temp                                       
                                        //lookup list for dups 
                                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                        if (val.lexeme != Lexie.LexemeString[increments])
                                        {
                                            //insert after storing the value
                                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments], depth, SymbolTable.RecordEnum.Constant);
                                        }

                                        increments++;
                                        Rest();
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                        Environment.Exit(1);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "constt":
                        {
                            switch (Lexie.MatchTokens[increments])
                            {
                                case "idt":
                                case "assignopt":
                                case "numt":
                                case "semit":
                                    {
                                        //check if float or int then increase offset 
                                        if (Lexie.MatchTokens[increments] == "intt")
                                        {
                                            totalOffset += intOffset;
                                        }
                                        else if (Lexie.MatchTokens[increments] == "floatt")
                                        {
                                            totalOffset += floatOffset;
                                        }

                                        Decl();
                                        break;
                                    }
                                case "eoftt":
                                    {
                                        done = true;
                                        break;
                                    }
                            }
                            break;
                        }
                    case "eoftt":
                        done = true;
                        break;
                    default:
                        {
                            done = true;
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'intt', 'floatt', or 'chart'. Not correct Grammar.");
                            Environment.Exit(1);
                            break;
                        }
                }

            }
        }

        /// <summary>
        /// The Rest will see if the MatchTokens fit either lparent, commat, or semit.
        /// </summary>
        public static void Rest()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "lparent":
                    {
                        //increase depth
                        depth++;
                        increments++;
                        Paramlist();

                        if (Lexie.MatchTokens[increments] == "rparent")
                        {
                            increments++;
                            Compound();
                        }
                        break;
                    }
                case "semit":
                case "commat":
                    {
                        IDTail();
                        if (Lexie.MatchTokens[increments] == "semit")
                        {
                            increments++;
                        }

                        else
                        {
                            Console.WriteLine("Error:  " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                            Environment.Exit(1);
                        }
                        break;
                    }
                //case "rparent":
                //    break;
                default:
                    {
                        Console.WriteLine("Error:  " + Lexie.MatchTokens[increments] + " was found when searching for 'semit', 'commat', or 'lparent'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// The Paramlist will see if the MatchTokens fit either int, float, char, or rparent
        /// </summary>
        public static void Paramlist()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "intt":
                case "floatt":
                case "chart":
                    {
                        //increase offset 
                        if (Lexie.MatchTokens[increments] == "intt")
                        {
                            currentOffset = intOffset;
                        }
                        else if (Lexie.MatchTokens[increments] == "floatt")
                        {
                            currentOffset = floatOffset;
                        }
                        else
                        {
                            currentOffset = charOffset;
                        }

                        totalOffset += currentOffset;

                        increments++;
                        if (Lexie.MatchTokens[increments] == "idt")
                        {
                            //check for dups and insert 
                            val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                            if (val.lexeme != Lexie.LexemeString[increments])
                            {
                                //insert after storing the value
                                SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments], depth, SymbolTable.RecordEnum.Variable);
                            }
                            increments++;
                            ParamTail();
                        }

                        else
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar."); Environment.Exit(1);
                        }
                        break;
                    }
                case "rparent":
                    break;
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'rparent'. Not correct Grammar."); Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// The ParamTail will see if the MatchTokens fit either int, float, char, or rparent
        /// </summary>
        public static void ParamTail()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "commat":
                    {

                        increments++;

                        if (Lexie.MatchTokens[increments] == "intt" | Lexie.MatchTokens[increments] == "floatt" | Lexie.MatchTokens[increments] == "chart")
                        {
                            //offset increase 
                            if (Lexie.MatchTokens[increments] == "intt")
                            {
                                currentOffset = intOffset;
                            }
                            else if (Lexie.MatchTokens[increments] == "floatt")
                            {
                                currentOffset = floatOffset;
                            }
                            else
                            {
                                currentOffset = charOffset;
                            }

                            totalOffset += currentOffset;

                            increments++;

                            switch (Lexie.MatchTokens[increments]) //this will see if the MatchTokens will match "idt" if it does call Rest, else error
                            {
                                case "idt":
                                    {
                                        //lookup for dups and insert
                                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                        if (val.lexeme != Lexie.LexemeString[increments])
                                        {
                                            //insert after storing the value
                                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments], depth, SymbolTable.RecordEnum.Variable);
                                        }
                                        increments++;
                                        ParamTail();
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                        Environment.Exit(1);
                                        break;
                                    }
                            }
                        }

                        else
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'intt', 'floatt', or 'chart'. Not correct Grammar.");
                            Environment.Exit(1);

                        }
                        break;
                    }
                case "rparent":
                    break;
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'rparent'. Not correct Grammar."); Environment.Exit(1);
                        Environment.Exit(1);
                        break;
                    }

            }
        }

        /// <summary>
        /// The Compound will see if the MatchTokens start with a '{' if not then it's an error 
        /// </summary>
        public static void Compound()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "openCurlyParent":
                    {
                        increments++;
                        Decl();
                        if (Lexie.MatchTokens[increments] == "closeCurlyParent")
                        {
                            increments++;
                            Prog();
                            if (Lexie.MatchTokens[increments] == "eoftt")
                            {
                                break;
                            }
                        }

                        else
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'closeCurlyParent'. Not correct Grammar.");
                            Environment.Exit(1);

                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'openCurlyParent'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        
        /// <summary>
        /// The Decl will see if the MatchToken matches either int, float, char, or '}'. If it doesn't then it will throw an error
        /// </summary>
        public static void Decl()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "intt":
                case "floatt":
                case "chart":
                    {
                        //offsets increase 
                        if (Lexie.MatchTokens[increments] == "intt")
                        {
                            currentOffset = intOffset;
                        }
                        else if (Lexie.MatchTokens[increments] == "floatt")
                        {
                            currentOffset = floatOffset;
                        }
                        else
                        {
                            currentOffset = charOffset;
                        }

                        totalOffset += currentOffset;

                        increments++;
                        IDList();
                        break;
                    }
                case "closeCurlyParent":
                    {
                        //decrease depth 
                        depth--;
                        //print lexeme, class of lexeme (function, constant, or variable) 
                        SymbolTable.writeTable(depth);
                        break;
                    }
                case "constt":
                    {
                        switch (Lexie.MatchTokens[increments])
                        {
                            case "idt":
                            case "assignopt":
                            case "numt":
                                {
                                    //check if float or int then increase offset 
                                    if (Lexie.MatchTokens[increments] == "intt")
                                    {
                                        totalOffset += intOffset;
                                    }
                                    else if (Lexie.MatchTokens[increments] == "floatt")
                                    {
                                        totalOffset += floatOffset;
                                    }
                                    Decl();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'intt', 'floatt', or 'chart'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }

        }

        /// <summary>
        /// The IDList will see if the MatchToken Matches with idt and semit after. if not then an error has occurred
        /// </summary>
        public static void IDList()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "idt":
                    {
                        //lookup for dups and insert
                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                        if (val.lexeme != Lexie.LexemeString[increments])
                        {
                            //insert after storing the value
                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments], depth, SymbolTable.RecordEnum.Variable);
                        }
                        increments++;
                        IDTail();

                        switch (Lexie.MatchTokens[increments])
                        {
                            case "semit":
                                {
                                    increments++;
                                    Decl();
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                                    Environment.Exit(1);
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// The IDTail will see if the MatchToken matches with Commat or Semmit. If it doesn't then it's an error
        /// </summary>
        public static void IDTail()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "commat":
                    {
                        //increase sym tab offset += local current offset
                        totalOffset += currentOffset;

                        increments++;
                        switch (Lexie.MatchTokens[increments])
                        {
                            case "idt":
                                {
                                    //lookup for dups and insert
                                    val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                    if (val.lexeme != Lexie.LexemeString[increments])
                                    {
                                        //insert after storing the value
                                        SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments], depth, SymbolTable.RecordEnum.Variable);
                                    }
                                    increments++;
                                    IDTail();
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                    Environment.Exit(1);
                                    break;
                                }
                        }
                        break;
                    }
                case "semit":
                    {
                        break;

                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

    }
}



