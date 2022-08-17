namespace Savanna
{
    public class Antelope : Animal
    {
        /// <summary>
        /// Antilope entity.
        /// </summary>
        public Antelope()
        {
            Vision = GameParameters.vision;
            IsAlive = true;
            Letter = GameMessages.antilopeLetter;
        }

        /// <summary>
        /// Moves current animal on the game field.
        /// </summary>
        /// <param name="game">Current game field.</param>
        /// <param name="animal">Current animal.</param>
        /// <param name="animals">List of animals.</param>
        public override void MoveAnimal(Game game, Animal animal, List<Animal> animals)
        {
            Antelope antelope = (Antelope)animal;
            List<Lion> lionsAround = AnimalsInRange<Lion>(antelope, animals);
            Animal? nearestLion = FindNearestLion(lionsAround, antelope);
            List<AnimalNewCoordinates> newAnimalsCoordinates = FreeCellsToMove(animals, antelope);

            if (nearestLion == null)
            {
                MakeNextRandomMove(antelope, newAnimalsCoordinates);
            }
            else
            {
                RunAway(nearestLion, antelope, newAnimalsCoordinates);
            }
        }

        /// <summary>
        /// Gets closest lion with all parameters to current antelope.
        /// </summary>
        /// <param name="lionsAround">List of lions in current animal vision range.</param>
        /// <param name="antilope">Current animal.</param>
        /// <returns>Returns closest lion with all parameters to current antelope.</returns>
        private Animal? FindNearestLion(List<Lion> lionsAround, Animal antelope)
        {
            Animal? nearestLion = null;
            int nearestLionIndex = 0;
            int count = 0;
            double lionDistance;
            double minLionDistance = double.MaxValue;

            if (lionsAround.Count != 0)
            {
                foreach (var lion in lionsAround)
                {
                    lionDistance = Math.Pow(antelope.RowCoordinate - lion.RowCoordinate, 2) + Math.Pow(antelope.ColumnCoordinate - lion.ColumnCoordinate, 2);

                    if (minLionDistance > lionDistance)
                    {
                        minLionDistance = lionDistance;
                        nearestLionIndex = count;
                    }

                    count++;
                }
                nearestLion = lionsAround[nearestLionIndex];
            }

            return nearestLion;
        }

        /// <summary>
        /// Calculates next antelope move while it tries to run away from nearest lion.
        /// </summary>
        /// <param name="nearestLion">Closest lion to current antelope.</param>
        /// <param name="antelope">Current animal.</param>
        /// <param name="newAnimalsCoordinates">List of free cells for possible move for current animal.</param>
        private void RunAway(Animal nearestLion, Antelope antelope, List<AnimalNewCoordinates> newAnimalsCoordinates)
        {
            antelope.RowCoordinate = newAnimalsCoordinates[CalculateMaxDistanceFromLion(newAnimalsCoordinates, nearestLion)].NewRowCoordinate;
            antelope.ColumnCoordinate = newAnimalsCoordinates[CalculateMaxDistanceFromLion(newAnimalsCoordinates, nearestLion)].NewColumnCoordinate;
        }

        /// <summary>
        /// Calculates maximal possible distance from closest lion.
        /// </summary>
        /// <param name="newAnimalsCoordinates">List of free cells for possible move for current animal.</param>
        /// <param name="lion">Closest lion to current antelope.</param>
        /// <returns>Index of farest possible cell from lion.</returns>
        private int CalculateMaxDistanceFromLion(List<AnimalNewCoordinates> newAnimalsCoordinates, Animal lion)
        {
            int farestFromLionIndex = 0;
            int count = 0;
            double lionDistance;
            double maxLionDistance = 0;

            foreach (var cell in newAnimalsCoordinates)
            {
                lionDistance = Math.Pow(cell.NewRowCoordinate - lion.RowCoordinate, 2) + Math.Pow(cell.NewColumnCoordinate - lion.ColumnCoordinate, 2);

                if (maxLionDistance < lionDistance)
                {
                    maxLionDistance = lionDistance;
                    farestFromLionIndex = count;
                }

                count++;
            }

            return farestFromLionIndex;
        }
    }
}