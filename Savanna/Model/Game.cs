namespace Savanna
{
    /// <summary>
    /// Mechanics of the game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Array that will contain the game field.
        /// </summary>
        public string[,] GameField { get; set; }

        /// <summary>
        /// Starts the game with empty board.
        /// </summary>
        public Game()
        {
            GameField = new string[GameParameters.boardColumns, GameParameters.boardRows];
            ClearBoard();
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        private void ClearBoard()
        {
            for (int column = 0; column < GameParameters.boardColumns; column++)
            {
                for (int row = 0; row < GameParameters.boardRows; row++)
                {
                    GameField[column, row] = GameMessages.emptyCell;
                }
            }
        }

        /// <summary>
        /// Checks if cell is on the board.
        /// </summary>
        /// <param name="newXCoordinate">New X coordinate.</param>
        /// <param name="newYCoordinate">New Y coordinate.</param>
        /// <returns>Is cell on board.</returns>
        public static bool IsCellOnBoard(int newYCoordinate, int newXCoordinate)
        {
            return (newYCoordinate < 0) ||
                   (newYCoordinate > GameParameters.boardColumns - 1) ||
                   (newXCoordinate < 0) ||
                   (newXCoordinate > GameParameters.boardRows - 1);
        }

        /// <summary>
        /// Fills the game field with the first animals' letter. 
        /// </summary>
        /// <param name="animals">List of animals.</param>
        /// <param name="args">Array containing the game field.</param>
        public static void FillGameFieldWithAnimals(List<Animal> animals, string[,] args)
        {
            foreach (var animal in animals)
            {
                args[animal.YCoordinate, animal.XCoordinate] = animal.Letter;
            }
        }

        /// <summary>
        /// Removes animal from game field.
        /// </summary>
        /// <param name="animals">List of animals.</param>
        /// <param name="args">Array containing the game field.</param>
        public static void RemoveAnimalFromBoard(List<Animal> animals, string[,] args)
        {
            foreach (Animal animal in animals)
            {
                args[animal.YCoordinate, animal.XCoordinate] = GameMessages.emptyCell;
            }
        }
    }
}