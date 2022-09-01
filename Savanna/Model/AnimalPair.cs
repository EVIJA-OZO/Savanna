namespace Savanna
{
    /// <summary>
    /// Animal pairs entity.
    /// </summary>
    public class AnimalPair
    {
        /// <summary>
        /// First animal in current pair.
        /// </summary>
        public Animal? FirstAnimal { get; set; }

        /// <summary>
        /// Second animal in current pair.
        /// </summary>
        public Animal? SecondAnimal { get; set; }

        /// <summary>
        /// Shows for how long time animals in pair has been near each other.
        /// </summary>
        public int RelationshipDuration { get; set; } = 0;

        /// <summary>
        /// Indicates if animal pair exists.
        /// </summary>
        public bool PairExist { get; set; } = false;

        /// <summary>
        /// Creates pair of two same type animals if they are near each other.
        /// </summary>
        /// <param name="animals">List of animals.</param>
        public static void CreatePair(List<Animal> animals, List<AnimalPair> pairs)
        {
            foreach (var currentAnimal in animals)
            {
                if (currentAnimal.HasAPair == false)
                {
                    List<Animal> animalsAround = Animal.LookAround(currentAnimal, animals, 1).ToList();
                    List<Animal> freeAnimalsAround = animalsAround.Where(animal => animal.HasAPair == false && animal.GetType() == currentAnimal.GetType()).Select(animal => animal).ToList();

                    Animal? animalToPair = freeAnimalsAround.FirstOrDefault();

                    if (animalToPair == null)
                    {
                        return;
                    }

                    AnimalPair newPair = new()
                    {
                        FirstAnimal = currentAnimal,
                        SecondAnimal = animalToPair,
                    };

                    newPair.FirstAnimal.HasAPair = true;
                    newPair.SecondAnimal.HasAPair = true;
                    pairs.Add(newPair);
                }
            }
        }

        /// <summary>
        /// Checks pair existance and relationship duration. If duration is 3 - gives birth to new animal.
        /// </summary>
        /// <param name="pairs">List of animal pairs.</param>
        /// <param name="animals">List of animals.</param>
        public static void CheckPair(List<Animal> animals, List<AnimalPair> pairs)
        {
            foreach (var pair in pairs)
            {
                if (pair != null)
                {
                    if (!IsAnimalStillNear(pair.FirstAnimal, pair.SecondAnimal))
                    {
                        pair.FirstAnimal.HasAPair = false;
                        pair.SecondAnimal.HasAPair = false;
                        pairs.Remove(pair);
                    }
                    else
                    {
                        pair.RelationshipDuration++;

                        if (pair.RelationshipDuration == GameParameters.relationshipDuration)
                        {
                            Animal child = pair.FirstAnimal.GiveBirth(pair.FirstAnimal, animals);
                            animals.Add(child);
                            pair.RelationshipDuration = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if paired animal is still near current animal.
        /// </summary>
        /// <param name="currentAnimal"></param>
        /// <param name="pairAnimal"></param>
        /// <returns></returns>
        private static bool IsAnimalStillNear(Animal currentAnimal, Animal pairAnimal)
        {
            return Math.Abs(currentAnimal.RowCoordinate - pairAnimal.RowCoordinate) <= 1 &&
                   Math.Abs(currentAnimal.ColumnCoordinate - pairAnimal.ColumnCoordinate) <= 1;
        }
    }
}