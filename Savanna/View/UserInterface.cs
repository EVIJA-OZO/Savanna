namespace Savanna
{
    public class UserInterface
    {
        /// <summary>
        /// Makes view of the game field.
        /// </summary>
        /// <param name="args">Array containing the game field.</param>
        /// <param name="game">Current game generation.</param>
        public static void Display(string[,] args, Game game)
        {
            Console.Clear();
            Console.WriteLine(GameMessages.welcomeMessage);
            Console.WriteLine();

            for (int row = 0; row < GameParameters.boardRows; row++)
            {
                for (int column = 0; column < GameParameters.boardColumns; column++)
                {
                    Console.Write(args[column, row]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(GameMessages.addAntilope);
            Console.WriteLine(GameMessages.addLion);
            Console.WriteLine(GameMessages.quitGame);
        }

        /// <summary>
        /// Gets input key. 
        /// </summary>
        /// <returns>Pressed button value.</returns>
        public static ConsoleKey? GetInputKey() => Console.KeyAvailable ? Console.ReadKey(true).Key : null;
    }
}