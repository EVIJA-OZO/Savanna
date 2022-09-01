namespace Savanna
{
    public class Lion : Animal
    {
        /// <summary>
        /// Lion entity.
        /// </summary>
        public Lion()
        {
            Vision = GameParameters.vision;
            Health = GameParameters.health;
            IsAlive = true;
            Letter = GameMessages.lionLetter;
        }

        /// <summary>
        /// Moves current animal on the game field.
        /// </summary>
        /// <param name="game">Current game field.</param>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        public override void MoveAnimal(Game game, Animal animal, List<Animal> animals)
        {
            Lion lion = (Lion)animal;
            List<Antelope> antelopesAround = AnimalsInRange<Antelope>(lion, animals);
            Animal? nearestAntelope = FindNearestAntelope(antelopesAround, lion);
            List<AnimalNewCoordinates> newAnimalsCoordinates = FreeCellsToMove(animals, lion);

            if (nearestAntelope == null)
            {
                MakeNextRandomMove(lion, newAnimalsCoordinates);
            }
            else
            {
                Hunt(nearestAntelope, lion, newAnimalsCoordinates);
            }

            lion.Health -= GameParameters.healthDecreaser;
        }

        /// <summary>
        /// Gets closest antelope with all parameters to current lion.
        /// </summary>
        /// <param name="antelopesAround">List of lions in current animal vision range.</param>
        /// <param name="lion">Current animal.</param>
        /// <returns></returns>
        private Animal? FindNearestAntelope(List<Antelope> antelopesAround, Animal lion)
        {
            Animal? nearestLion = null;
            int nearestAntelopeIndex = 0;
            int count = 0;
            double antelopeDistance;
            double minAntelopeDistance = double.MaxValue;

            if (antelopesAround.Count != 0)
            {
                foreach (var antelope in antelopesAround)
                {
                    antelopeDistance = Math.Pow(lion.RowCoordinate - antelope.RowCoordinate, 2) + Math.Pow(lion.ColumnCoordinate - antelope.ColumnCoordinate, 2);

                    if (minAntelopeDistance > antelopeDistance)
                    {
                        minAntelopeDistance = antelopeDistance;
                        nearestAntelopeIndex = count;
                    }

                    count++;
                }
                nearestLion = antelopesAround[nearestAntelopeIndex];
            }

            return nearestLion;
        }

        /// <summary>
        /// Calculates next lion move while it tries to hunt down nearest antelope.
        /// </summary>
        /// <param name="nearestAntelope">Closest antelope to current lion.</param>
        /// <param name="lion">Current animal.</param>
        /// <param name="newAnimalsCoordinates">List of free cells for possible move for current animal.</param>
        private void Hunt(Animal nearestAntelope, Lion lion, List<AnimalNewCoordinates> newAnimalsCoordinates)
        {
            lion.ColumnCoordinate = newAnimalsCoordinates[CalculateMinDistanceToAntelope(newAnimalsCoordinates, nearestAntelope)].NewColumnCoordinate;
            lion.RowCoordinate = newAnimalsCoordinates[CalculateMinDistanceToAntelope(newAnimalsCoordinates, nearestAntelope)].NewRowCoordinate;

            if (nearestAntelope.RowCoordinate - lion.RowCoordinate <= 1 && nearestAntelope.ColumnCoordinate - lion.ColumnCoordinate <= 1)
            {
                EatAntelope(lion, nearestAntelope);
            }
        }

        /// <summary>
        /// Calculates minimal possible distance from closest antelope.
        /// </summary>
        /// <param name="newAnimalsCoordinates">List of free cells for possible move for current animal.</param>
        /// <param name="antelope">Closest antelope to current lion.</param>
        /// <returns>Index of closest possible cell from antelope.</returns>
        private int CalculateMinDistanceToAntelope(List<AnimalNewCoordinates> newAnimalsCoordinates, Animal antelope)
        {
            int closestToAntelopeIndex = 0;
            int count = 0;
            double antelopeDistance;
            double minAntelopeDistance = double.MaxValue;

            foreach (var cell in newAnimalsCoordinates)
            {
                antelopeDistance = Math.Pow(cell.NewRowCoordinate - antelope.RowCoordinate, 2) + Math.Pow(cell.NewColumnCoordinate - antelope.ColumnCoordinate, 2);

                if (minAntelopeDistance > antelopeDistance)
                {
                    minAntelopeDistance = antelopeDistance;
                    closestToAntelopeIndex = count;
                }

                count++;
            }

            return closestToAntelopeIndex;
        }

        /// <summary>
        /// Decrease nearest hunted antelope health and increases lion health. 
        /// If antelopes health is zero - kills animal.
        /// </summary>
        /// <param name="lion">Current lion.</param>
        /// <param name="antelope">Currently closest hunted antelope.</param>
        private void EatAntelope(Lion lion, Animal antelope)
        {
            antelope.Health = antelope.Health <= 0 ? 0 : antelope.Health - 20;
            lion.Health = lion.Health >= GameParameters.health ? GameParameters.health : lion.Health + 20;

            if (antelope.Health == 0)
            {
                lion.RowCoordinate = antelope.RowCoordinate;
                lion.ColumnCoordinate = antelope.ColumnCoordinate;
                antelope.IsAlive = false;
                lion.Health = GameParameters.health;
            }
        }

        /// <summary>
        /// Gives birth of a new animal.
        /// </summary>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        /// <returns>New animal.</returns>
        public override Animal? GiveBirth(Animal animal, List<Animal> animals)
        {
            Lion lion = (Lion)animal;
            AnimalNewCoordinates birthCoordinates = FreeCellsToBirth(lion, animals);
            Animal? child = null;

            if (birthCoordinates != null)
            {
                child = new Lion
                {
                    RowCoordinate = birthCoordinates.NewRowCoordinate,
                    ColumnCoordinate = birthCoordinates.NewColumnCoordinate
                };
            }

            return child;
        }
    }
}