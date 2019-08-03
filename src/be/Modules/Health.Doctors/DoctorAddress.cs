
namespace Health.Doctors
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents the information of doctor address.
    /// </summary>
    public class DoctorAddress
    {
        /// <summary>
        /// Gets or sets the id for address.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the region of address.
        /// </summary>
        public string RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Address for doctor.
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the doctor for address.
        /// </summary>
        public string DoctorId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of address.
        /// </summary>
        public AddressType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phonebooks for address.
        /// </summary>
        public IEnumerable<Phonebook> Phonebooks
        {
            get;
            set;
        }
    }
}
