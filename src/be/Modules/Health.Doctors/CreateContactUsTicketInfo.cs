
namespace Health.Doctors
{
    /// <summary>
    /// Represents the needed information for create a new contact ticket.
    /// </summary>
    public class CreateContactUsTicketInfo
    {
        /// <summary>
        /// Gets or sets the name for ticket initializer.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phonenumber for ticket initializer.
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the e-mail for ticket initializer.
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description for ticket.
        /// </summary>
        public string Description
        {
            get;
            set;
        }
    }
}
