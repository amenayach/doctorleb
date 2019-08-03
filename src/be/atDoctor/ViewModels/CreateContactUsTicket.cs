
namespace atDoctor.ViewModels
{
    using Health.Doctors;

    public class CreateContactUsTicket
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

        /// <summary>
        /// Converts an insteance of <see cref="CreateContactUsTicket"/> class to a new insteance of <see cref="CreateContactUsTicketInfo"/> class.
        /// </summary>
        /// <returns>Return a new insteance of <see cref="CreateContactUsTicketInfo"/>
        /// class that represents the contact ticket to be created.</returns>
        internal CreateContactUsTicketInfo ToObjectModel()
        {
            return new CreateContactUsTicketInfo()
            {
                Description = this.Description,
                Email = this.Email,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber
            };
        }
    }
}
