
namespace atDoctor.ViewModels
{
    using System;

    /// <summary>
    /// Represents the submit response information.
    /// </summary>
    public class SubmitResponseInfo
    {
        /// <summary>
        /// Gets or sets the indicates if the submission is succeed 
        /// </summary>
        public bool Success
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exception of submission if any.
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the note for submission.
        /// </summary>
        public string Note
        {
            get;
            set;
        }
    }
}
