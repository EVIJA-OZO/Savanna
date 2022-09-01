namespace Savanna
{
    /// <summary>
    /// Contains all common properties and behaviour of all types of animals.
    /// </summary>
    public abstract class Animal
    {
        /// <summary>
        /// Coordinate X - row.
        /// </summary>
        public int RowCoordinate { get; set; }

        /// <summary>
        /// Coordinate Y - column.
        /// </summary>
        public int ColumnCoordinate { get; set; }

        /// <summary>
        /// Animal vision.
        /// </summary>
        public int Vision { get; set; }

        /// <summary>
        /// Animal health.
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        /// Indicates if animal is alive.
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Indicates if animal has a pair.
        /// </summary>
        public bool HasAPair { get; set; } = false;

        /// <summary>
        /// Symbol for animal on a game field.
        /// </summary>
        public string? Letter { get; set; }

        /// <summary>
        /// Makes new animal, generates the new animal's coordinates and adds it to list.
        /// </summary>
        /// <param name="newAnimal">New animal.</param>
        /// <param name="animals">List of animals.</param>
        public static void AddAnimal(Animal newAnimal, List<Animal> animals)
        {
            Random random = new();
            newAnimal.RowCoordinate = random.Next(0, GameParameters.boardRows - 1);
            newAnimal.ColumnCoordinate = random.Next(0, GameParameters.boardColumns - 1);

            if (!IsCellReserved(newAnimal.RowCoordinate, newAnimal.ColumnCoordinate, animals) && !Game.IsCellOnBoard(newAnimal.RowCoordinate, newAnimal.ColumnCoordinate))
            {
                animals.Add(newAnimal);
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
            animal.RowCoordinate = nextCoordinatesToMove.NewRowCoordinate;
            animal.ColumnCoordinate = nextCoordinatesToMove.NewColumnCoordinate;
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

            /// Contains new animal coordinates for possible next move.
            AnimalNewCoordinates newCoordinates;

            for (int newColumnCoordinate = animal.ColumnCoordinate - 1; newColumnCoordinate <= animal.ColumnCoordinate + 1; newColumnCoordinate++)
            {
                for (int newRowCoordinate = animal.RowCoordinate - 1; newRowCoordinate <= animal.RowCoordinate + 1; newRowCoordinate++)
                {
                    if (!IsCellReserved(newColumnCoordinate, newRowCoordinate, animals) && !Game.IsCellOnBoard(newColumnCoordinate, newRowCoordinate))
                    {
                        newCoordinates = new AnimalNewCoordinates
                        {
                            NewColumnCoordinate = newColumnCoordinate,
                            NewRowCoordinate = newRowCoordinate
                        };

                        newAnimalsCoordinates.Add(newCoordinates);
                    }
                }
            }

            newCoordinates = new AnimalNewCoordinates
            {
                NewColumnCoordinate = animal.ColumnCoordinate,
                NewRowCoordinate = animal.RowCoordinate
            };

            newAnimalsCoordinates.Add(newCoordinates);
            return newAnimalsCoordinates;
        }

        /// <summary>
        /// Checks if cell is reserved.
        /// </summary>
        /// <param name="newRowCoordinate">New row position.</param>
        /// <param name="newColumnCoordinate">New column position.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>Is cell reserved.</returns>
        private static bool IsCellReserved(int newRowCoordinate, int newColumnCoordinate, List<Animal> animals)
        {
            return animals.Any(animal => animal.RowCoordinate.Equals(newRowCoordinate) && animal.ColumnCoordinate.Equals(newColumnCoordinate));
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
        /// Returns if there are searched animals in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="animalForSearch">Animal for search.</param>
        /// <param name="vision">How many cells animal can sense other animals.</param>
        /// <returns>True if there are animals in the range of vision, excluding itself. Otherwise - false.</returns>
        private static bool AnimalsAreInRange(Animal currentAnimal, Animal animalForSearch, int vision)
        {
            bool isOwnPosition = currentAnimal.ColumnCoordinate == animalForSearch.ColumnCoordinate &&
                                 currentAnimal.RowCoordinate == animalForSearch.RowCoordinate;
            return !isOwnPosition &&
                    animalForSearch.ColumnCoordinate >= currentAnimal.ColumnCoordinate - vision &&
                    animalForSearch.ColumnCoordinate <= currentAnimal.ColumnCoordinate + vision &&
                    animalForSearch.RowCoordinate >= currentAnimal.RowCoordinate - vision &&
                    animalForSearch.RowCoordinate <= currentAnimal.RowCoordinate + vision;
        }

        /// <summary>
        /// Finds animals around by type and adds them to list.
        /// </summary>
        /// <typeparam name="T">Type to look for in animal vision range.</typeparam>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>List of animals around by type.</returns>
        public List<T> AnimalsInRange<T>(Animal animal, List<Animal> animals) where T : Animal
        {
            IEnumerable<Animal> animalsAround = LookAround(animal, animals, animal.Vision);
            return animalsAround.Where(animal => typeof(T).IsAssignableTo(animal.GetType())).Select(animal => (T)animal).ToList();
        }

        /// <summary>
        /// Gives birth to a new animal.
        /// </summary>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>New animal (child).</returns>
        public abstract Animal? GiveBirth(Animal animal, List<Animal> animals);

        /// <summary>
        /// Calculates first free cells for birth for new animal.
        /// </summary>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>New animal coordinates for child to birth.</returns>
        protected AnimalNewCoordinates? FreeCellsToBirth(Animal animal, List<Animal> animals)
        {
            AnimalNewCoordinates? newCoordinates = null;

            for (int newRowCoordinate = RowCoordinate - 1; newRowCoordinate <= RowCoordinate + 1; newRowCoordinate++)
            {
                for (int newColumnCoordinate = ColumnCoordinate - 1; newColumnCoordinate <= ColumnCoordinate + 1; newColumnCoordinate++)
                {
                    if (!IsCellReserved(newColumnCoordinate, newRowCoordinate, animals) && !Game.IsCellOnBoard(newColumnCoordinate, newRowCoordinate))
                    {
                        newCoordinates = new AnimalNewCoordinates
                        {
                            NewRowCoordinate = newRowCoordinate,
                            NewColumnCoordinate = newColumnCoordinate
                        };

                        break;
                    }
                }
            }

            return newCoordinates;
        }
    }
}