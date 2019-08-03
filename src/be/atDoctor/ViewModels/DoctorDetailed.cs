
namespace atDoctor.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the detailed information for doctor.
    /// </summary>
    public class DoctorDetailed
    {
        /// <summary>
        /// Gets or sets the doctor information.
        /// </summary>
        public DoctorInfo DoctorInfo
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the addresses for doctor.
        /// </summary>
        public IEnumerable<DoctorAddressInfo> DoctorAddressesInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reviews for doctor.
        /// </summary>
        public IEnumerable<ReviewInfo> Reviews
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Specialities for doctor.
        /// </summary>
        public IEnumerable<SpecialityInfo> Specialities
        {
            get;
            set;
        }
    }
}
