/// <summary>
/// Name: Jenny Nguyen 
/// Assignment: 5
/// Description: 
/// </summary>
using System;
using System.Collections.Generic;
using System.IO;

namespace CSC446_Assignment_5_Nguyen
{
    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        public void Main(string[] args)
        {
        start:
            Lexie.LexicalAnalyzer(args);
            Console.WriteLine("Lexical Analyzer completed. Commencing Parser.\n");
            Parser.Parse();
            Console.WriteLine("\nParser completed completed.");


            string continueProgram;

        cp:
            Console.WriteLine("\nDo you want to enter another file? Enter Y for yes and N for to exit the program");
            continueProgram = Console.ReadLine();

            if (continueProgram.ToLower() == "n")
            {
                System.Environment.Exit(0);
            }
            else if (continueProgram.ToLower() == "y")
            {
                Console.Clear();
                Lexie.MatchTokens.Clear();
                Lexie.Token.Equals(null);
                Lexie.counting = 0;
                Parser.increments = 0;
                goto start;
            }
            else
            {
                Console.WriteLine("Invalid Response.");
                goto cp;
            }


        }
    }
}
