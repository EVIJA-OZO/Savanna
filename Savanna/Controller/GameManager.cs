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
        /// List that holds all pairs of animals in the game.
        /// </summary>
        public List<AnimalPair> pairs = new();

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
                AnimalPair.CreatePair(animals, pairs);
                AnimalPair.CheckPair(animals, pairs);

                foreach (Animal animal in animals)
                {
                    animal.MoveAnimal(game, animal, animals);

                    if (animal.Health <= 0)
                    {
                        animal.IsAlive = false;
                    }
                }

                Game.RemoveDeadAnimals(animals);
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