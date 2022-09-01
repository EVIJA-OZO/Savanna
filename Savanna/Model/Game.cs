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
        /// <param name="newRowCoordinate">New rox coordinate.</param>
        /// <param name="newColumnCoordinate">New column coordinate.</param>
        /// <returns>Is cell on board.</returns>
        public static bool IsCellOnBoard(int newColumnCoordinate, int newRowCoordinate)
        {
            return (newColumnCoordinate < 0) ||
                   (newColumnCoordinate > GameParameters.boardColumns - 1) ||
                   (newRowCoordinate < 0) ||
                   (newRowCoordinate > GameParameters.boardRows - 1);
        }

        /// <summary>
        /// Fills the game field with the first animals' letter. 
        /// </summary>
        /// <param name="animals">List of animals.</param>
        /// <param name="gameField">Array containing the game field.</param>
        public static void FillGameFieldWithAnimals(List<Animal> animals, string[,] gameField)
        {
            foreach (var animal in animals)
            {
                gameField[animal.ColumnCoordinate, animal.RowCoordinate] = animal.Letter;
            }
        }

        /// <summary>
        /// Removes animal from game field.
        /// </summary>
        /// <param name="animals">List of animals.</param>
        /// <param name="gameField">Array containing the game field.</param>
        public static void RemoveAnimalFromBoard(List<Animal> animals, string[,] gameField)
        {
            foreach (Animal animal in animals)
            {
                gameField[animal.ColumnCoordinate, animal.RowCoordinate] = GameMessages.emptyCell;
            }
        }

        /// <summary>
        /// Removes dead animals from list.
        /// </summary>
        /// <param name="animals">List of animals.</param>
        public static void RemoveDeadAnimals(List<Animal> animals)
        {
            animals.RemoveAll(animal => animal.IsAlive = false);
        }
    }
}