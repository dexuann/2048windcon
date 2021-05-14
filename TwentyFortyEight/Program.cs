using System;
using System.Linq;

namespace TwentyFortyEight {
    /// <summary>
    ///   Runs the game 2048
    /// </summary>
    /// <author>Ashley De Xuan Poon</author>
    /// <student_id>n9629238</student_id>
    public static class Program {
        /// <summary>
        /// Specifies possible moves in the game
        /// </summary>
        public enum Move { Up, Left, Down, Right, Restart, Quit };

        /// <summary>
        /// Generates random numbers
        /// </summary>
        static Random numberGenerator = new Random();

        /// <summary>
        /// Number of initial digits on a new 2048 board
        /// </summary>
        const int NUM_STARTING_DIGITS = 2;

        /// <summary>
        /// The chance of a two spawning
        /// </summary>
        const float CHANCE_OF_TWO = 0.9f; // 90% chance of a two; 10% chance of a four

        /// <summary>
        /// The size of the 2048 board
        /// </summary>
        const int BOARD_SIZE = 4; // 4x4

        /// <summary>
        /// Runs the game of 2048
        /// </summary>
        static void Main() {
            
            //initiate the program by clearing console, display title, board and instructions
            OpeningProcedure();
            int[,] board = MakeBoard();
            DisplayBoard(board);
            Instructions();

            //make a loop that keep the game recurring until user quits
            do {

                //if the game is over clear console, display title, board and 
                //the option to restart or quit
                if (GameOver(board) == true) {
                    OpeningProcedure();
                    DisplayBoard(board);
                    Instructions();
                    Console.Write("\nRestart or quit game (r/q): \n");
                    MakeMove(ChooseMove(), board);


                } else {
                    //else execute the specific move and populate board once

                    Move userMove = ChooseMove();
                    bool userMoveValid = MakeMove(userMove, board);

                    if (userMoveValid == true) {
                        PopulateAnEmptyCell(board);

                    }
                    //update the screen with selected choice
                    OpeningProcedure();
                    DisplayBoard(board);
                    Instructions();
                }
                //update the screen with selected choice
                OpeningProcedure();
                DisplayBoard(board);
                Instructions();
            } while (true);
        }

        /// <summary>
        /// Generates a new 2048 board
        /// </summary>
        /// <returns>A new 2048 board</returns>
        public static int[,] MakeBoard() {
            // Make a BOARD_SIZExBOARD_SIZE array of integers (filled with zeros)
            int[,] board = new int[BOARD_SIZE, BOARD_SIZE];

            // Populate some random empty cells
            for (int i = 0; i < NUM_STARTING_DIGITS; i++) {
                PopulateAnEmptyCell(board);
            }

            return board;
        }

        /// <summary>
        /// Display the given 2048 board
        /// </summary>
        /// <param name="board">The 2048 board to display</param>
        public static void DisplayBoard(int[,] board) {
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int column = 0; column < board.GetLength(1); column++) {
                    Console.Write("{0,4}", board[row, column] == 0 ? "-" : board[row, column].ToString());
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// If the board is not full, choose a random empty cell and add a two or a four.
        /// There should be a 90% chance of adding a two, and a 10% chance of adding a four.
        /// </summary>
        /// <param name="board">The board to add a new number to</param>
        /// <returns>False if the board is already full; true otherwise</returns>
        public static bool PopulateAnEmptyCell(int[,] board) {

            //create bool var. to return board status and if board is full
            bool checkBoardFilled = IsFull(board);
            bool emptySpaceOnBoard = false;

            //create int var. for random row, column and probability value
            int rowRandom, columnRandom, probabilityValue;

            //check if board is alr full
            if (IsFull(board) == false) {

                //repeat sequence if its false
                do {

                    //choose random row, column and number
                    rowRandom = numberGenerator.Next(BOARD_SIZE);
                    columnRandom = numberGenerator.Next(BOARD_SIZE);

                    //check location to see if its a zero
                    if (board[rowRandom, columnRandom] == 0) {
                        emptySpaceOnBoard = true;
                    }

                } while (emptySpaceOnBoard == false);

                //choose a random number between 1-100
                probabilityValue = numberGenerator.Next(minValue: 1, maxValue: 100);

                //generate new number for the random location according to the probability value
                //if probability value is between 1-90, generate 2
                //else generate 4
                if (probabilityValue <= CHANCE_OF_TWO * 100) {

                    board[rowRandom, columnRandom] = 2;

                } else {

                    board[rowRandom, columnRandom] = 4;
                }
            }
            //return board full status
            return emptySpaceOnBoard;
        }

        /// <summary>
        /// Returns true if the given 2048 board is full (contains no zeros)
        /// </summary>
        /// <param name="board">A 2048 board to check</param>
        /// <returns>True if the board is full; false otherwise</returns>
        public static bool IsFull(int[,] board) {

            //boolean variable to show if board is full
            bool boardFull = true;

            //for loop to go check every tile on the board
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int column = 0; column < board.GetLength(1); column++) {
                    //if there is a 0, board is not full
                    if (board[row, column] == 0) {
                        boardFull = false;
                    }
                }
            }
            //return board full status
            return boardFull;
        }

        /// <summary>
        /// Get a Move from the user (such as UP, LEFT, DOWN, RIGHT, RESTART or QUIT)
        /// </summary>
        /// <returns>The chosen Move</returns>
        public static Move ChooseMove() {

            //get and store user's input
            string userInput = Console.ReadKey().KeyChar.ToString();

            //return the chosen move/result according to the input
            switch (userInput) {
                case "w":
                    //when move == up return Move.Up
                    return Move.Up;
                case "a":
                    //when move == left return Move.Left
                    return Move.Left;
                case "s":
                    //when move == down return Move.Down
                    return Move.Down;
                case "d":
                    //when move == right return Move.Right
                    return Move.Right;
                case "r":
                    //when move == restart return Move.Restart
                    return Move.Restart;
                case "q":
                    //when move == quit return Move.Quit
                    return Move.Quit;
                default:
                    //return a 6 if input does not apply to the cases
                    return (Move)6;
            }
        }

        /// <summary>
        /// Applies the chosen Move on the given 2048 board
        /// </summary>
        /// <param name="move">A move such as UP, LEFT, RIGHT or DOWN</param>
        /// <param name="board">A 2048 board</param>
        /// <returns>True if the move had an affect on the game; false otherwise</returns>
        public static bool MakeMove(Move move, int[,] board) {

            //create a bool var. that returns if move had an affect on the game 
            bool makeMoveEffect = false;

            switch (move) {
                //when move == Up
                case Move.Up:
                    //get each column
                    for (int columnNumber = 0; columnNumber < BOARD_SIZE; columnNumber++) {
                        int[] column = MatrixExtensions.GetCol(board, columnNumber);

                        //if ShiftCombineShift is possible, do so and return true to makeMoveEffect
                        if (ShiftCombineShift(column, shiftLeft: true) == true) {
                            makeMoveEffect = true;
                        }
                        //update each column
                        MatrixExtensions.SetCol(board, columnNumber, column);

                    }
                    break;

                //when move == Down
                case Move.Down:
                    //get each column
                    for (int columnNumber = 0; columnNumber < BOARD_SIZE; columnNumber++) {
                        int[] column = MatrixExtensions.GetCol(board, columnNumber);

                        //if ShiftCombineShift is possible, do so and return true to makeMoveEffect
                        if (ShiftCombineShift(column, shiftLeft: false) == true) {
                            makeMoveEffect = true;
                        }
                        //update each column
                        MatrixExtensions.SetCol(board, columnNumber, column);

                    }
                    break;

                //when move == Left
                case Move.Left:
                    //get each row
                    for (int rowNumber = 0; rowNumber < BOARD_SIZE; rowNumber++) {
                        int[] row = MatrixExtensions.GetRow(board, rowNumber);

                        //if ShiftCombineShift is possible, do so and return true to makeMoveEffect
                        if (ShiftCombineShift(row, shiftLeft: true) == true) {
                            makeMoveEffect = true;
                        }
                        //update each row
                        MatrixExtensions.SetRow(board, rowNumber, row);

                    }
                    break;

                //when move == Right
                case Move.Right:
                    //get each row
                    for (int rowNumber = 0; rowNumber < BOARD_SIZE; rowNumber++) {
                        int[] row = MatrixExtensions.GetRow(board, rowNumber);

                        //if ShiftCombineShift is possible, do so and return true to makeMoveEffect
                        if (ShiftCombineShift(row, shiftLeft: false) == true) {
                            makeMoveEffect = true;
                        }
                        //update each row
                        MatrixExtensions.SetRow(board, rowNumber, row);

                    }
                    break;

                //when move == Restart
                case Move.Restart:

                    //make a new board
                    int[,] newboard = MakeBoard();

                    //replace current board values with new board values
                    for (int i = 0; i < BOARD_SIZE; i++) {
                        int[] row = MatrixExtensions.GetRow(newboard, i);
                        MatrixExtensions.SetRow(board, i, row);
                    }
                    break;

                //when move == Quit
                case Move.Quit:
                    Console.Write("\n");
                    //exit the application
                    Environment.Exit(0);
                    break;

            }
            //return status of move affecting on the game
            return makeMoveEffect;
        }

        /// <summary>
        /// Shifts the non-zero integers in the given 1D array to the left
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if shifting had an effect; false otherwise</returns>
        public static bool ShiftLeft(int[] nums) {

            //create a new array to store the shifted array
            int[] shiftedLeft = new int[nums.Length];

            //for loop to iterate through the array
            int counter = 0;
            for (int index = 0; index < nums.Length; index++) {

                //if array value != 0 copy it into the new array and increase index
                if (nums[index] != 0) {
                    shiftedLeft[counter] = nums[index];
                    counter++;
                }
            }

            //boolean variable to show if shifting had an effect
            bool shiftEffect = false;

            //copy content of new shifted array back into the original array
            for (int index = 0; index < shiftedLeft.Length; index++) {

                //if update array with values from the new array when they are not equal
                if (nums[index] != shiftedLeft[index]) {
                    nums[index] = shiftedLeft[index];
                    //change shiftEffect status
                    shiftEffect = true;
                }
            }
            //return status of shiftEffect
            return shiftEffect;
        }

        /// <summary>
        /// Combines identical, non-zero integers that are adjacent to one another by summing 
        /// them in the left integer, and replacing the right-most integer with a zero
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3  }
        ///   It will be modified to:
        ///       { 0, 4, 0, 8, 0, 0, 0, 16, 0, 5, 3  }
        /// </example>
        public static bool CombineLeft(int[] nums) {

            //boolean variable to test if combining had an effect
            bool combineEffect = false;

            //iterate through the array
            for (int index = 0; index < nums.Length - 1; index++) {

                //if current and next value are equal, sum both values into current index 
                //and replace the next with 0
                if (nums[index] == nums[index + 1]) {
                    nums[index] += nums[index];
                    nums[index + 1] = 0;

                    //go to the index after
                    index++;

                    //change combine status to true
                    combineEffect = true;
                }
            }
            //return combine status
            return combineEffect;
        }

        /// <summary>
        /// Shifts the numbers in the array in the specified direction, then combines them, then 
        /// shifts them again.
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <param name="left">True if numbers should be shifted to the left; false otherwise</param>
        /// <returns>True if shifting and combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values below, and shiftLeft is true:
        ///       { 0, 2, 2,  4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 4, 8, 16, 5, 3, 0, 0, 0, 0, 0, 0  }
        ///       
        ///   If nums has the values below, and shiftLeft is false:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 0, 0, 0, 0, 0, 0, 2, 8, 16, 5, 3 }
        /// </example>
        public static bool ShiftCombineShift(int[] nums, bool shiftLeft) {

            //boolean variable to test if ShiftCombineShift had an effect
            bool shiftCombineEffect = false;

            //create a temp array to store cloned values of the original array
            //and to be used as a variable
            int[] tempNums = new int[nums.Length];
            tempNums = (int[])nums.Clone();

            //reverse array if shiftLeft is false
            if (shiftLeft == false) {
                Array.Reverse(tempNums);
            }

            //shift combine shift the temp array
            ShiftLeft(tempNums);
            CombineLeft(tempNums);
            ShiftLeft(tempNums);

            //reverse array back after shifting and combining if shiftLeft is false
            if (shiftLeft == false) {
                Array.Reverse(tempNums);
            }

            //check if the temp array is equal to the original array
            bool equalValues = nums.SequenceEqual(tempNums);

            //if temp array does not equal the original array
            if (equalValues != true) {

                //show that shift combine shift has an effect
                shiftCombineEffect = true;

                //copy the changed array back into the original array
                for (int index = 0; index < tempNums.Length; index++) {
                    nums[index] = tempNums[index];
                }
            }
            //return status
            return shiftCombineEffect;
        }
        /// <summary>
        /// clears the screen and writes the title
        /// </summary>
        public static void OpeningProcedure() {

            //clear the screen and write the title
            Console.Clear();
            Console.WriteLine("2048 - Join the numbers and get to the 2048 tile! \n");

        }
        /// <summary>
        /// writes the instructions of the game and a line for user input
        /// </summary>
        public static void Instructions() {

            //write game instructions and a line for user input
            Console.WriteLine("\nw: Up \t\ta: Left \ns: Down \td: Right \nr: Restart \tq: Quit");
            Console.Write("\nEnter a key: ");

        }
        /// <summary>
        /// checks if the game is over and return its status
        /// </summary>
        /// <param name="board">a 2048 board</param>
        /// <returns>True if game is over; false otherwise</returns>
        public static bool GameOver(int[,] board) {

            //a bool var. to show the game over status
            bool gameIsOver = false;

            //if the board is full
            if (IsFull(board) == true) {

                //create 4 boards for testing from the current board
                int[,] testBoardUp = (int[,])board.Clone();
                int[,] testBoardDown = (int[,])board.Clone();
                int[,] testBoardLeft = (int[,])board.Clone();
                int[,] testBoardRight = (int[,])board.Clone();

                //check if up, down, left and right has possible movement

                //up
                bool possibleUp = MakeMove(Move.Up, testBoardUp);

                //down
                bool possibleDown = MakeMove(Move.Down, testBoardDown);

                //left
                bool possibleLeft = MakeMove(Move.Left, testBoardLeft);

                //right
                bool possibleRight = MakeMove(Move.Right, testBoardRight);

                //if all four directions return no movement change game over status to true
                if (possibleUp == false &&
                   possibleDown == false &&
                   possibleLeft == false &&
                   possibleRight == false) {
                    gameIsOver = true;

                }
            }
            //return game over status
            return gameIsOver;
        }
    }
}