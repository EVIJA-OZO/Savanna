namespace Savanna
{
    /// <summary>
    /// Management of the game.
    /// </summary>
    public class GameManager : IGameManager
    {
        /// <summary>
        /// List that holds all animals in the game.
        /// </summary>
        public List<Animal> animals = new();

        /// <summary>
        /// Starts the game and processes the user's input in game.
        /// </summary>
        public void Play()
        {
            Game game = new();
            bool gameOn = true;

            while (gameOn)
            {
                Game.FillGameFieldWithAnimals(animals, game.GameField);
                UserInterface.Board(game.GameField, game);
                Game.RemoveAnimalFromBoard(animals, game.GameField);

                foreach (Animal animal in animals)
                {
                    animal.MoveAnimal(game, animal, animals);
                }

                Thread.Sleep(1000);
                ConsoleKey? consoleKey = UserInterface.GetInputKey();

                switch (consoleKey)
                {
                    case ConsoleKey.A:
                        Animal.AddAnimal(new Antelope(), animals);
                        break;
                    case ConsoleKey.L:
                        Animal.AddAnimal(new Lion(), animals);
                        break;
                }

                gameOn = (consoleKey != ConsoleKey.Escape);
            }
        }
    }
}