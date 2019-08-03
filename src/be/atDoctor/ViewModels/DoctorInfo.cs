
namespace atDoctor.ViewModels
{
    /// <summary>
    /// Represents the information for doctor.
    /// </summary>
    public class DoctorInfo
    {
        /// <summary>
        /// Gets or sets the id for doctor.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for doctor in Arabic language .
        /// </summary>
        public string NameAr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for doctor in foreign language.
        /// </summary>
        public string NameFo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of reviewer.
        /// </summary>
        public int NumberOfReviewer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rank for doctor.
        /// </summary>
        public decimal Rank
        {
            get;
            set;
        }
    }
}
