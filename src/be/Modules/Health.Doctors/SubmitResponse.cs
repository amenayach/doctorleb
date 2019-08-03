
namespace Health.Doctors
{
    using System;

    /// <summary>
    /// Represents the information for submit response.
    /// </summary>
    public class SubmitResponse
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

        /// <summary>
        /// Submission is succeed.
        /// </summary>
        /// <returns>Returns new insteance of <see cref="SubmitResponse"/> class.</returns>
        public static SubmitResponse Ok()
        {
            return new SubmitResponse
            {
                Success = true
            };
        }

        /// <summary>
        /// Submission is failed.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="note"></param>
        /// <returns>Returns new insteance of <see cref="SubmitResponse"/> class.</returns>
        public static SubmitResponse Error(Exception exception = null, string note = "")
        {
            return new SubmitResponse
            {
                Success = false,
                Exception = exception,
                Note = note
            };
        }
    }
}
