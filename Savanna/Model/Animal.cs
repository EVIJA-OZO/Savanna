namespace Savanna
{
    /// <summary>
    /// Contains all common properties and behaviour of all types of animals.
    /// </summary>
    public abstract class Animal
    {
        /// <summary>
        /// Animal ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Coordinate X - row.
        /// </summary>
        public int XCoordinate { get; set; }

        /// <summary>
        /// Coordinate Y - column.
        /// </summary>
        public int YCoordinate { get; set; }

        /// <summary>
        /// Animal vision.
        /// </summary>
        public int Vision { get; set; }

        /// <summary>
        /// Indicates if animal is alive.
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Symbol for animal on a game field.
        /// </summary>
        public string? Letter { get; set; }

        private static int id = 0;

        /// <summary>
        /// Generates new ID.
        /// </summary>
        /// <returns>Incremented ID.</returns>
        protected static int GenerateId() => id++;

        /// <summary>
        /// Makes new animal, generates the new animal's coordinates and adds it to list.
        /// </summary>
        /// <param name="newAnimal">New animal.</param>
        /// <param name="animals">List of animals.</param>
        public static void AddAnimal(Animal newAnimal, List<Animal> animals)
        {
            Random random = new();

            {
                newAnimal.XCoordinate = random.Next(0, GameParameters.boardRows - 1);
                newAnimal.YCoordinate = random.Next(0, GameParameters.boardColumns - 1);

                if (!IsCellReserved(newAnimal.XCoordinate, newAnimal.YCoordinate, animals) && !Game.IsCellOnBoard(newAnimal.XCoordinate, newAnimal.YCoordinate))
                {
                    animals.Add(newAnimal);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Moves current animal on the game field.
        /// </summary>
        /// <param name="game">Current game field.</param>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        public abstract void MoveAnimal(Game game, Animal animal, List<Animal> animals);

        /// <summary>
        /// Makes next move of animal.
        /// </summary>
        /// <param name="animal">Current animal.</param>
        /// <param name="newAnimalsCoordinates">Free cells to possible move.</param>
        public static void MakeNextRandomMove(Animal animal, List<AnimalNewCoordinates> newAnimalsCoordinates)
        {
            Random random = new();
            AnimalNewCoordinates nextCoordinatesToMove = newAnimalsCoordinates[random.Next(0, newAnimalsCoordinates.Count)];
            animal.XCoordinate = nextCoordinatesToMove.NewXCoordinate;
            animal.YCoordinate = nextCoordinatesToMove.NewYCoordinate;
        }

        /// <summary>
        /// Calculates possible free cells on board to move animal. 
        /// </summary>
        /// <param name="animals">List of animals.</param>
        /// <param name="animal">Current animal.</param>
        /// <returns>List of free cells for possible move for current animal.</returns>
        protected static List<AnimalNewCoordinates> FreeCellsToMove(List<Animal> animals, Animal animal)
        {
            List<AnimalNewCoordinates> newAnimalsCoordinates = new();
            AnimalNewCoordinates newCoordinates;

            for (int newYCoordinate = animal.YCoordinate - 1; newYCoordinate <= animal.YCoordinate + 1; newYCoordinate++)
            {
                for (int newXCoordinate = animal.XCoordinate - 1; newXCoordinate <= animal.XCoordinate + 1; newXCoordinate++)
                {
                    if (!IsCellReserved(newYCoordinate, newXCoordinate, animals) && !Game.IsCellOnBoard(newYCoordinate, newXCoordinate))
                    {
                        newCoordinates = new AnimalNewCoordinates
                        {
                            NewYCoordinate = newYCoordinate,
                            NewXCoordinate = newXCoordinate
                        };

                        newAnimalsCoordinates.Add(newCoordinates);
                    }
                }
            }

            newCoordinates = new AnimalNewCoordinates
            {
                NewYCoordinate = animal.YCoordinate,
                NewXCoordinate = animal.XCoordinate
            };

            newAnimalsCoordinates.Add(newCoordinates);
            return newAnimalsCoordinates;
        }

        /// <summary>
        /// Checks if cell is reserved.
        /// </summary>
        /// <param name="newXCoordinate">New X position.</param>
        /// <param name="newYCoordinate">New Y position.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>Is cell reserved.</returns>
        private static bool IsCellReserved(int newXCoordinate, int newYCoordinate, List<Animal> animals)
        {
            for (int element = 0; element < animals.Count; element++)
            {
                Animal? animal = animals[element];
                if (animal.XCoordinate.Equals(newXCoordinate) && animal.YCoordinate.Equals(newYCoordinate))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Looks around game field from animals based on it vision.
        /// </summary>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        /// <param name="vision">How many cells near animal can sense other animals.</param>
        /// <returns>Enumerable list with nearby animals.</returns>
        public static IEnumerable<Animal> LookAround(Animal animal, IEnumerable<Animal> animals, int vision)
        {
            return from animalForSearch in animals
                   where AnimalsAreInRange(animal, animalForSearch, vision)
                   select animalForSearch;
        }

        /// <summary>
        /// Returns all animals in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="animalForSearch">Animal for search.</param>
        /// <param name="vision">How many cells animal can sense other animals.</param>
        /// <returns>Are the animals in the range of vision, excluding itself.</returns>
        private static bool AnimalsAreInRange(Animal currentAnimal, Animal animalForSearch, int vision)
        {
            bool isOwnPosition = currentAnimal.YCoordinate == animalForSearch.YCoordinate &&
                                 currentAnimal.XCoordinate == animalForSearch.XCoordinate;
            return !isOwnPosition &&
                    animalForSearch.YCoordinate >= currentAnimal.YCoordinate - vision &&
                    animalForSearch.YCoordinate <= currentAnimal.YCoordinate + vision &&
                    animalForSearch.XCoordinate >= currentAnimal.XCoordinate - vision &&
                    animalForSearch.XCoordinate <= currentAnimal.XCoordinate + vision;
        }

        /// <summary>
        /// Finds animals around by type and adds them to list.
        /// </summary>
        /// <typeparam name="T">Type to look for in animal vision range.</typeparam>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>List of animals around by type.</returns>
        public List<T> AnimalsInRange<T>(Animal animal, List <Animal> animals) where T : Animal
        {
                IEnumerable<Animal> animalsAround = LookAround(animal, animals, animal.Vision);

                return animalsAround.Where(animal => typeof(T).IsAssignableTo(animal.GetType())).Select(animal => (T)animal).ToList();
        }
    }
}