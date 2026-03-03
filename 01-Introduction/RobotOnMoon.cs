using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;


public class RobotOnMoon
{
    public string isSafeCommand(string[] board, string S)
    {
		// validatate input
        if (board == null) throw new ArgumentNullException(nameof(board));
        if (board.Length < 1 || board.Length > 50) throw new ArgumentOutOfRangeException(nameof(board), "Length of board must be between 1 and 50.");

        int rows = board.Length;
        int cols = board[0].Length;
        if (cols < 1 || cols > 50) throw new ArgumentOutOfRangeException(nameof(board), "Length of each row in board must be between 1 and 50.");

        int sCount = 0;
        int startX = -1, startY = -1;
        for (int i = 0; i < rows; i++)
        {
            if (board[i].Length != cols) throw new ArgumentException("All rows in board must have the same length.");
            for (int j = 0; j < cols; j++)
            {
                if (board[i][j] == 'S')
                {
                    sCount++;
                    startX = i;
                    startY = j;
                }
                else if (board[i][j] != '.' && board[i][j] != '#')
                {
                    throw new ArgumentException("Board can only contain characters '.', '#', and 'S'.");
                }
            }
		}

        if (sCount != 1) throw new ArgumentException("Board must contain exactly one 'S' character.");

        if (S == null) throw new ArgumentNullException(nameof(S));
        if (S.Length < 1 || S.Length > 50) throw new ArgumentOutOfRangeException(nameof(S), "Length of S must be between 1 and 50.");
        foreach (char c in S)
        {
            if (c != 'U' && c != 'D' && c != 'L' && c != 'R')
            {
                throw new ArgumentException("S can only contain characters 'U', 'D', 'L', and 'R'.");
            }
		}

		// simulate robot movement
        foreach (char cmd in S)
        {
            int newX = startX, newY = startY;
            switch (cmd)
            {
                case 'U': newX--; break;
                case 'D': newX++; break;
                case 'L': newY--; break;
                case 'R': newY++; break;
            }
            // if new position is out of bound of the map -> Dead
            if (newX < 0 || newX >= rows || newY < 0 || newY >= cols)
            {
                return "Dead";
            }
            // if new position is an obstacle -> ignore move
            if (board[newX][newY] == '#')
            {
                continue;
			}

			// make a move to new position
			startX = newX;
            startY = newY;
		}
        return "Alive";
    }

    #region Testing code

    [STAThread]
    private static Boolean KawigiEdit_RunTest(int testNum, string[] p0, string p1, Boolean hasAnswer, string p2)
    {
        Console.Write("Test " + testNum + ": [" + "{");
        for (int i = 0; p0.Length > i; ++i)
        {
            if (i > 0)
            {
                Console.Write(",");
            }
            Console.Write("\"" + p0[i] + "\"");
        }
        Console.Write("}" + "," + "\"" + p1 + "\"");
        Console.WriteLine("]");
        RobotOnMoon obj;
        string answer;
        obj = new RobotOnMoon();
        DateTime startTime = DateTime.Now;
        answer = obj.isSafeCommand(p0, p1);
        DateTime endTime = DateTime.Now;
        Boolean res;
        res = true;
        Console.WriteLine("Time: " + (endTime - startTime).TotalSeconds + " seconds");
        if (hasAnswer)
        {
            Console.WriteLine("Desired answer:");
            Console.WriteLine("\t" + "\"" + p2 + "\"");
        }
        Console.WriteLine("Your answer:");
        Console.WriteLine("\t" + "\"" + answer + "\"");
        if (hasAnswer)
        {
            res = answer == p2;
        }
        if (!res)
        {
            Console.WriteLine("DOESN'T MATCH!!!!");
        }
        else if ((endTime - startTime).TotalSeconds >= 2)
        {
            Console.WriteLine("FAIL the timeout");
            res = false;
        }
        else if (hasAnswer)
        {
            Console.WriteLine("Match :-)");
        }
        else
        {
            Console.WriteLine("OK, but is it right?");
        }
        Console.WriteLine("");
        return res;
    }

    public static void Run()
    {
        Boolean all_right;
        all_right = true;

        string[] p0;
        string p1;
        string p2;

        // ----- test 0 -----
        p0 = new string[] {".....", ".###.", "..S#.", "...#."};
        p1 = "URURURURUR";
        p2 = "Alive";
        all_right = KawigiEdit_RunTest(0, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 1 -----
        p0 = new string[] {".....", ".###.", "..S..", "...#."};
        p1 = "URURURURUR";
        p2 = "Dead";
        all_right = KawigiEdit_RunTest(1, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 2 -----
        p0 = new string[] {".....", ".###.", "..S..", "...#."};
        p1 = "URURU";
        p2 = "Alive";
        all_right = KawigiEdit_RunTest(2, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 3 -----
        p0 = new string[] {"#####", "#...#", "#.S.#", "#...#", "#####"};
        p1 = "DRULURLDRULRUDLRULDLRULDRLURLUUUURRRRDDLLDD";
        p2 = "Alive";
        all_right = KawigiEdit_RunTest(3, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 4 -----
        p0 = new string[] {"#####", "#...#", "#.S.#", "#...#", "#.###"};
        p1 = "DRULURLDRULRUDLRULDLRULDRLURLUUUURRRRDDLLDD";
        p2 = "Dead";
        all_right = KawigiEdit_RunTest(4, p0, p1, true, p2) && all_right;
        // ------------------

        // ----- test 5 -----
        p0 = new string[] {"S"};
        p1 = "R";
        p2 = "Dead";
        all_right = KawigiEdit_RunTest(5, p0, p1, true, p2) && all_right;
        // ------------------

        if (all_right)
        {
            Console.WriteLine("You're a stud (at least on the example cases)!");
        }
        else
        {
            Console.WriteLine("Some of the test cases had errors.");
        }
    }

    #endregion
}