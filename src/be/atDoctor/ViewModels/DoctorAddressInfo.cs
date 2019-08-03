
namespace atDoctor.ViewModels
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents the information for doctor address.
    /// </summary>
    public class DoctorAddressInfo
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
        public RegionInfo Region
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
        /// Gets or sets the type of address.
        /// </summary>
        public AddressType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phones for doctor.
        /// </summary>
        public IEnumerable<PhonebookInfo> Phonebooks
        {
            get;
            set;
        }
    }
}
