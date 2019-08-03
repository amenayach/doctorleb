
namespace Health.Doctors
{

    /// <summary>
    /// Represents the information of region.
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Gets or sets the id for region.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for region in Arabic language.
        /// </summary>
        public string NameAr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for region in foreign language.
        /// </summary>
        public string NameFo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the kaza for region.
        /// </summary>
        public string KazaId
        {
            get;
            set;
        }
    }
}
