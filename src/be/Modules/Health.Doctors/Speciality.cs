
namespace Health.Doctors
{

    /// <summary>
    /// Represents the information of specialty.
    /// </summary>
    public class Speciality
    {
        /// <summary>
        /// Gets or sets the id for specialty.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for specialty in Arabic language.
        /// </summary>
        public string NameAr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for specialty in French language.
        /// </summary>
        public string NameFr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for specialty in English language.
        /// </summary>
        public string NameEn
        {
            get;
            set;
        }
    }
}
