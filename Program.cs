using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Ex02.ConsoleUtils;

public class Game
{
    public static void PrintGrid(Card[,] io_CardsInGame)
    {
        string alotOfEquals = new string('=', io_CardsInGame.GetLength(1) * 4);

        // prints the A B C D E
        Console.Write("    ");
        for (int i = 0; i < io_CardsInGame.GetLength(1); i++)
        {
            string col = String.Format("{0}   ", Convert.ToChar(i + 65));
            Console.Write(col);
        }
        for (int i = 0; i < io_CardsInGame.GetLength(0); i++)
        {
            Console.WriteLine('\n' + "  " + alotOfEquals);
            Console.Write(i + 1 + " ");

            for (int j = 0; j < io_CardsInGame.GetLength(1); j++)
            {
                if (io_CardsInGame[i, j].GetIsVisible())
                {
                    string row = String.Format("| {0} ", io_CardsInGame[i, j].GetValue());
                    Console.Write(row);
                }
                else if (!io_CardsInGame[i, j].GetIsVisible())
                {
                    string row = String.Format("| {0} ", " ");
                    Console.Write(row);
                }
            }
        }

        Console.WriteLine('\n' + "  " + alotOfEquals);
    }

    public static void PrintScoreBoard(Player io_Player1, Player io_Player2)
    {
        bool isPlayerOneTurn = io_Player1.IsMyTurn();

        Console.WriteLine("You can exit the program any time. Press Q for exit and press Enter");
        Console.WriteLine(String.Format("{0} Score: {1}", io_Player1.GetName(), io_Player1.GetScore()));
        Console.WriteLine(String.Format("{0} Score: {1}", io_Player2.GetName(), io_Player2.GetScore()));
       
        if(isPlayerOneTurn)
        {
            Console.WriteLine(String.Format("Its {0} turn", io_Player1.GetName()));
        }
        else
        {
            Console.WriteLine(String.Format("Its {0} turn", io_Player2.GetName()));
        }
    }

    
    public static Card MakeMove(Board io_Board, int io_HightOfBoard, int io_WidthOfBoard, bool io_PlayerVsCpu, Player io_Player1, Player io_Player2)
    {
        TimeSpan twoSecondsInterval = new TimeSpan(0, 0, 2);
        Card[,] gameBoard = io_Board.GetCardsInGame();
        string firstCardLocationInString = String.Empty;

        if (io_PlayerVsCpu && io_Player2.IsMyTurn())
        {
            firstCardLocationInString = ProbingValidCoordinate(CpuMove(io_Board, io_HightOfBoard, io_WidthOfBoard), io_HightOfBoard, io_WidthOfBoard, io_Board);
            Thread.Sleep(twoSecondsInterval);
        }
        else if (io_PlayerVsCpu && io_Player1.IsMyTurn())
        {
            firstCardLocationInString = ProbingValidCoordinate(Console.ReadLine(), io_HightOfBoard, io_WidthOfBoard, io_Board);
        }
        else if (!io_PlayerVsCpu)
        {
            firstCardLocationInString = ProbingValidCoordinate(Console.ReadLine(), io_HightOfBoard, io_WidthOfBoard, io_Board);
        }
        
        int[] firstCardLocationOnArray = StringCoordianteToArrayLocation(firstCardLocationInString);
        Card cardPicked = gameBoard[firstCardLocationOnArray[0], firstCardLocationOnArray[1]];
        cardPicked.TurnCardVisible();

        return cardPicked;
    }

    public static string CpuMove(Board io_Board, int io_HightOfBoard, int io_WidthOfBoard) 
    {
        Card[,] gameBoard = io_Board.GetCardsInGame();
        StringBuilder move = new StringBuilder();
        Random rand = new Random();
        Char colCoordinate = Convert.ToChar(rand.Next(65, 65 + io_WidthOfBoard));
        char rawCoordinate = Convert.ToChar(rand.Next(49, 49 + io_HightOfBoard));
        move.Append(Char.ToString(colCoordinate));
        move.Append(Char.ToString(rawCoordinate));
        int[] locationOnArray = StringCoordianteToArrayLocation(move.ToString());
        Card cardPicked = gameBoard[locationOnArray[0], locationOnArray[1]];

        while(gameBoard[locationOnArray[0], locationOnArray[1]].GetIsVisible())
        {
            move.Clear();
            colCoordinate = Convert.ToChar(rand.Next(65, 65 + io_WidthOfBoard));
            rawCoordinate = Convert.ToChar(rand.Next(49, 49 + io_HightOfBoard));
            move.Append(Char.ToString(colCoordinate));
            move.Append(Char.ToString(rawCoordinate));
            locationOnArray = StringCoordianteToArrayLocation(move.ToString());
            cardPicked = gameBoard[locationOnArray[0], locationOnArray[1]];
        }

        return move.ToString();
    }

    public static void Exit()
    {
        System.Environment.Exit(1);
    }

    public static void InitializeGame()
    {
        Console.WriteLine("Please Enter Your Name");
        string name = Console.ReadLine();
        Player player1 = new Player(name);
        Player player2 = new Player();
        Console.WriteLine("How Many Players? (Up to 2 players)");
        string numberOfPlayers = ValidNumberOfPlayers(Console.ReadLine());
        bool v_PlayerVsCpu = false;

        if (numberOfPlayers == "2")
        {
            Console.WriteLine("Please Enter Second Player Name");
            player2 = new Player(Console.ReadLine());
            v_PlayerVsCpu = false;
        }
        else if (numberOfPlayers == "1")
        {
            player2 = new Player("CPU");
            v_PlayerVsCpu = true;
        }

        Console.WriteLine("Choose the height of the board");
        string heightInput = Console.ReadLine();
        Console.WriteLine("Choose the width of the board");
        string widthInput = Console.ReadLine();
        int[] coordinates = ProbingValidDiamantionsOfBoard(heightInput, widthInput);
        int hight = coordinates[0];
        int width = coordinates[1];
        Board board = new Board(hight, width);
        board.InitBoard();
        PlayGame(player1, player2, board, v_PlayerVsCpu);
    }

    public static void PlayGame(Player io_Player1, Player io_Player2, Board io_Board, bool io_PlayerVsCpu)
    {
        Ex02.ConsoleUtils.Screen.Clear();
        TimeSpan twoSecondsInterval = new TimeSpan(0, 0, 2);
        io_Player1.SetTurn();
        Card[,] gameBoard = io_Board.GetCardsInGame();
        int hightOfBoard = io_Board.GetBoardHight();
        int widthOfBoard = io_Board.GetBoardWidth();
        
        while (!io_Board.IsAllCardsVisible())
        {         
            gameBoard = io_Board.GetCardsInGame();
            PrintGrid(io_Board.GetCardsInGame());
            PrintScoreBoard(io_Player1, io_Player2);
            Console.WriteLine("Please enter a valid Coordinate");
            Card firstPicked = MakeMove(io_Board, hightOfBoard, widthOfBoard, io_PlayerVsCpu, io_Player1, io_Player2);
            Ex02.ConsoleUtils.Screen.Clear();
            PrintGrid(io_Board.GetCardsInGame());
            Console.WriteLine("Please Enter another Valid Coordinate");
            Card secondPicked = MakeMove(io_Board, hightOfBoard, widthOfBoard, io_PlayerVsCpu, io_Player1, io_Player2);         
            Ex02.ConsoleUtils.Screen.Clear();
            PrintGrid(io_Board.GetCardsInGame());
            CalculateMove(io_Player1, io_Player2, firstPicked, secondPicked, io_Board);
        }
    }

    public static void CalculateMove(Player io_Player1, Player io_Player2, Card i_FirstPicked, Card i_SecondPicked, Board io_Board)
    {
        TimeSpan twoSecondsInterval = new TimeSpan(0, 0, 2);

        if (i_FirstPicked.Equals(i_SecondPicked))
        {
            Console.WriteLine("Good job, you got the right pair, you have another turn");
            Thread.Sleep(twoSecondsInterval);
            Ex02.ConsoleUtils.Screen.Clear();
            if (io_Player1.IsMyTurn())
            {
                io_Player1.IncrementScore();
            }
            else if (io_Player2.IsMyTurn())
            {
                io_Player2.IncrementScore();
            }
        }
        if (!i_FirstPicked.Equals(i_SecondPicked) && io_Player1.IsMyTurn())
        {
            io_Player1.EndTurn();
            io_Player2.SetTurn();
        }
        else if (!i_FirstPicked.Equals(i_SecondPicked) && io_Player2.IsMyTurn())
        {
            io_Player2.EndTurn();
            io_Player1.SetTurn();
        }
        if (!i_FirstPicked.Equals(i_SecondPicked))
        {
            Console.WriteLine("You Chose A Wrong pair, Sorry..");
            Thread.Sleep(twoSecondsInterval);
            io_Board.Close2Card(i_FirstPicked, i_SecondPicked);
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }

    public static bool SecondCheck(string io_Coordinate, int io_HightOfBoard, int io_WidthOfBoard)
    {
        bool v_ProperLength = io_Coordinate.Length == 2;
        bool v_StartsWithDesiredLetter = false;
        bool v_EndsWithDesiredLetter = false;
        StringBuilder numbersOnRaws = new StringBuilder();
        StringBuilder lettersOnCols = new StringBuilder();

        if (io_Coordinate.Equals("Q"))
        {
            Exit();
        }
        for (int i = 1; i <= io_HightOfBoard; i++)
        {
            numbersOnRaws.Append(i.ToString());
        }

        for (int i = 0; i < io_WidthOfBoard; i++)
        {
            lettersOnCols.Append(Convert.ToChar(i + 65));
        }
        foreach (char c in lettersOnCols.ToString())
        {
            if (io_Coordinate.StartsWith(c.ToString()))
            {
                v_StartsWithDesiredLetter = true;
            }
        }
        foreach (char c in numbersOnRaws.ToString())
        {
            if (io_Coordinate.EndsWith(c.ToString()))
            {
                v_EndsWithDesiredLetter = true;
            }
        }

        return v_EndsWithDesiredLetter && v_StartsWithDesiredLetter && v_ProperLength;
    }

    public static string ProbingValidCoordinate(string io_Coordinate, int io_HightOfBoard, int io_WidthOfBoard, Board io_Board)
    {
        Card[,] cardsInGame = io_Board.GetCardsInGame();
        bool v_IsCardVisible = false;

        while (!SecondCheck(io_Coordinate, io_HightOfBoard, io_WidthOfBoard) || !v_IsCardVisible)
        {
            if (SecondCheck(io_Coordinate, io_HightOfBoard, io_WidthOfBoard))
            {
                int[] coordinatesOnArray = StringCoordianteToArrayLocation(io_Coordinate);

                if (cardsInGame[coordinatesOnArray[0], coordinatesOnArray[1]].GetIsVisible())
                {
                    Console.WriteLine("The card is already open. Please choose diffrent card");
                    io_Coordinate = Console.ReadLine();
                }
                else if(!cardsInGame[coordinatesOnArray[0], coordinatesOnArray[1]].GetIsVisible())
                {
                    v_IsCardVisible = true;
                }
            }
            else if (!SecondCheck(io_Coordinate, io_HightOfBoard, io_WidthOfBoard))
            {
                Console.WriteLine("The choosen card is not in board boundries");
                io_Coordinate = Console.ReadLine();
            }      
        }

        return io_Coordinate;
    }

    public static int[] StringCoordianteToArrayLocation(string i_Coordiante)
    {
        char indexOnCols = char.Parse(i_Coordiante.Substring(0, 1));
        char indexOnRaws = char.Parse(i_Coordiante.Substring(1, 1));
        int[] coordinatesOnArray = new int[2];
        coordinatesOnArray[1] = Convert.ToInt32(indexOnCols) - 65;
        coordinatesOnArray[0] = Convert.ToInt32(indexOnRaws) - 49;

        return coordinatesOnArray;
    }

    public static bool ValidDaimantions(string i_HightOfBoard, string i_WidthOfBoard)
    {       
        List<string> validDaimantions = new List<string> { "4", "5", "6" };
        int result1;
        int result2;
        bool v_IsProductEven = false;
        bool v_IsValidDaimantions = validDaimantions.Contains(i_HightOfBoard) && validDaimantions.Contains(i_WidthOfBoard);
        bool v_IsInputInt = int.TryParse((i_HightOfBoard), out result1) && int.TryParse(i_WidthOfBoard, out result2);
        
        if (v_IsInputInt)
        {
            v_IsProductEven = Convert.ToInt32(i_HightOfBoard) * Convert.ToInt32(i_WidthOfBoard) % 2 == 0;
        }    

        return v_IsInputInt && v_IsValidDaimantions && v_IsProductEven;
    }

    public static int[] ProbingValidDiamantionsOfBoard(string i_HightOfBoard, string i_WidthOfBoard)
    {
        int[] coordinates = new int[2];
        
        while (!ValidDaimantions(i_HightOfBoard, i_WidthOfBoard))
        {
            Console.WriteLine("A Valid Board size is 4x4 to 6x6");
            Console.WriteLine("Choose the height of the board");
            i_HightOfBoard = Console.ReadLine();
            Console.WriteLine("Choose the width of the board");
            i_WidthOfBoard = Console.ReadLine();
        }

        coordinates[0] = Convert.ToInt32(i_HightOfBoard);
        coordinates[1] = Convert.ToInt32(i_WidthOfBoard);

        return coordinates;
    }

    public static string ValidNumberOfPlayers(string io_NumberOfPlayers)
    {
        while (!io_NumberOfPlayers.Equals("2") && !io_NumberOfPlayers.Equals("1"))
        {
            Console.WriteLine("Please enter a valid number of players");
            io_NumberOfPlayers = Console.ReadLine();
        }

        return io_NumberOfPlayers;
    }

    public static void Main()
    {
        InitializeGame();
    }
}