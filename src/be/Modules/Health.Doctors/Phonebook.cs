namespace Health.Doctors
{

    /// <summary>
    /// Represents the information of phonebook.
    /// </summary>
    public class Phonebook
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
        /// Gets or sets the doctor for phonebook.
        /// </summary>
        public string DoctorId
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

        /// <summary>
        /// Gets or sets the address id for mobile.
        /// </summary>
        public string AddressId
        {
            get;
            set;
        }
    }
}
