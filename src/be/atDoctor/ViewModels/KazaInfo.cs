
namespace atDoctor.ViewModels
{
    
    /// <summary>
    /// Represents the informatino for kaza.
    /// </summary>
    public class KazaInfo
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
        public ProvinceInfo Province
        {
            get;
            set;
        }
    }
}
