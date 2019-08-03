namespace Health.Doctors
{
    using System;

    /// <summary>
    /// Represents the information of UserActivation.
    /// </summary>
    public class UserActivation
    {
        /// <summary>
        /// Gets or sets the user activation id.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user activation ShortCode.
        /// </summary>
        public string ShortCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user activation IdentityCode.
        /// </summary>
        public string IdentityCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user activation Created date.
        /// </summary>
        public DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user activation expiry date.
        /// </summary>
        public DateTime Expires
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user activation user id.
        /// </summary>
        public string UserId
        {
            get;
            set;
        }
    }
}
