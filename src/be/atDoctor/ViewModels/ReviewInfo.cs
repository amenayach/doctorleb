namespace atDoctor.ViewModels
{
    using System;

    /// <summary>
    /// Represent the review information.
    /// </summary>
    public class ReviewInfo
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
        /// Gets or sets the reviewer name.
        /// </summary>
        public string ReviewerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description of review.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rank of review
        /// From 1 to 5
        /// </summary>
        public string Rank
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the creatoin date of review
        /// </summary>
        public DateTime CreatedOn
        {
            get;
            set;
        }
    }
}
