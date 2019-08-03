
namespace atDoctor.ViewModels
{
    using Health.Doctors;
    using System;

    /// <summary>
    /// Represents the needed information to create a new review for doctor.
    /// </summary>
    public class CreateReview
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
        /// Converts an insteance of <see cref="CreateReview"/> class to a new insteance of <see cref="CreateReviewInfo"/> class.
        /// </summary>
        /// <returns>Return a new insteance of <see cref="CreateReviewInfo"/>
        /// class that represents the review to be created.</returns>
        internal CreateReviewInfo ToObjectModel(Guid userId)
        {
            return new CreateReviewInfo()
            {
                Description = this.Description,
                DoctorId = this.DoctorId,
                Rank = this.Rank,
                ReviewerName = this.ReviewerName,
                UserId = userId
            };
        }
    }
}
