
namespace atDoctor.ViewModels
{
    /// <summary>
    /// Represents the information for phonebook.
    /// </summary>
    public class PhonebookInfo
    {
        /// <summary>
        /// Gets or sets the id for phonebook.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phonenumber for doctor.
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type for mobile.
        /// </summary>
        public PhoneType Type
        {
            get;
            set;
        }
    }
}
