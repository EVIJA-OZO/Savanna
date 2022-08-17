namespace Savanna
{
    /// <summary>
    /// Contains new animal coordinates for possible next move.
    /// </summary>
    public class AnimalNewCoordinates
    {
        /// <summary>
        /// New coordinate X - row for new animal location.
        /// </summary>
        public int NewRowCoordinate { get; set; }

        /// <summary>
        /// New coordinate Y - column for new animal location.
        /// </summary>
        public int NewColumnCoordinate { get; set; }
    }
}