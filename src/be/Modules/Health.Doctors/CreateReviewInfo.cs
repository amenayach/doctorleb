
using System;

namespace Health.Doctors
{

    /// <summary>
    /// Represents the needed information to create a new review for doctor.
    /// </summary>
    public class CreateReviewInfo
    {
        /// <summary>
        /// Gets or sets the doctor id.
        /// </summary>
        public string DoctorId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reviewer name.
        /// </summary>
        public string ReviewerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description for review.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rank for review.
        /// </summary>
        public int Rank
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user id for review.
        /// </summary>
        public Guid UserId
        {
            get;
            set;
        }
    }
}
