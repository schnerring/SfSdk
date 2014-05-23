namespace SfSdk.Contracts
{
    /// <summary>
    ///     An item containing the base properties of any S&amp;F item.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        ///     The item's ID.
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     The items Content ID.
        /// </summary>
        int ContentId { get; }
    }
}