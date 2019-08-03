
namespace Health.Doctors
{
    /// <summary>
    /// Represents the information for kaza.
    /// </summary>
    public class Kaza
    {
        /// <summary>
        /// Gets or sets the id for kaza.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for kaza in Arabic language.
        /// </summary>
        public string NameAr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for kaza in foreign language.
        /// </summary>
        public string NameFo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the province for kaza.
        /// </summary>
        public string ProvinceId
        {
            get;
            set;
        }
    }
}
