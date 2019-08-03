
namespace Health.Doctors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Handles doctors operations
    /// </summary>
    public interface IDoctorStore
    {
        /// <summary>
        /// Creates a new contact us ticket.
        /// </summary>
        /// <param name="info">Ticket information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        Task<SubmitResponse> CreateContactUsTicket(CreateContactUsTicketInfo info);
        
        /// <summary>
        /// Returns a list of doctors according to the entered keyword
        /// </summary>
        /// <param name="keyword">the user input to search on</param>
        /// <returns>A list of Doctors</returns>
        Task<IEnumerable<Doctor>> GetSearchAutoComplete(string keyword);

        /// <summary>
        /// Search doctors database via variaty of search criterias.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctors.</returns>
        Task<IEnumerable<Doctor>> SearchDoctor(SearchOptionsInfo searchOptions);

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="info">Review information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        Task<SubmitResponse> CreateReview(CreateReviewInfo info);

        /// <summary>
        /// Approves a review.
        /// </summary>
        /// <param name="reviewId">Review id.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        Task<SubmitResponse> ApproveReview(string reviewId);

        /// <summary>
        /// Retrieves the information for doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        Task<Doctor> GetDoctor(string doctorId);

        /// <summary>
        /// Retrieves the information for addresses of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="DoctorAddress"/>
        /// class that represents the informatino for addresses of doctor.</returns>
        Task<IEnumerable<DoctorAddress>> GetDoctorAddresses(string doctorId);

        /// <summary>
        /// Retrieves the information for phones of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Phonebook"/>
        /// class that represents the information for phones of doctor.</returns>
        Task<IEnumerable<Phonebook>> GetDoctorPhones(string doctorId);

        /// <summary>
        /// Retrieves the information for specialties of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Phonebook"/>
        /// class that represents the information for phones of doctor.</returns>
        Task<IEnumerable<Speciality>> GetDoctorSpecialities(string doctorId);

        /// <summary>
        /// Retrieves the information for doctors that related to a specific speciality.
        /// </summary>
        /// <param name="specialityId">Speciality id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(string specialityId);

        /// <summary>
        /// Retrieves the information for kazas.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Kaza"/>
        /// class that represents the information for kazas.</returns>
        Task<IEnumerable<Kaza>> GetKazas();

        /// <summary>
        /// Retrieves the information for kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Kaza"/>
        /// class that represents the information for kaza.</returns>
        Task<Kaza> GetKaza(string kazaId);

        /// <summary>
        /// Retrieves the informatino for kazas that related to a specific province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Kaza"/>
        /// class that represents the information for kazas.</returns>
        Task<IEnumerable<Kaza>> GetKazasByProvince(string provinceId);

        /// <summary>
        /// Retrieves the information for provinces.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Province"/>
        /// class that represents the information for provinces.</returns>
        Task<IEnumerable<Province>> GetProvinces();

        /// <summary>
        /// Retrieves the information for province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Province"/>
        /// class that represents the information for province.</returns>
        Task<Province> GetProvince(string provinceId);

        /// <summary>
        /// Retrieves the information for regions.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        Task<IEnumerable<Region>> GetRegions();

        /// <summary>
        /// Retrieves the information for region.
        /// </summary>
        /// <param name="provinceId">Region id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        Task<Region> GetRegion(string regionId);

        /// <summary>
        /// Retrieves the information for regions that related to a specific kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        Task<IEnumerable<Region>> GetRegionsByKaza(string kazaId);

        /// <summary>
        /// Retrieves the information for reviews that related to a specifc doctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Review"/>
        /// class that represents the information for reviews.</returns>
        Task<IEnumerable<Review>> GetReviewsByDoctor(string userId, string doctorId);

        /// <summary>
        /// Retrieves doctor reviews.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        Task<IEnumerable<Review>> GetDoctorPagedReviews(string userId, string doctorId, int pageIndex, int pageSize);

        /// <summary>
        /// Retrieves the information for specialities.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Speciality"/>
        /// class that represents the information for specialities.</returns>
        Task<IEnumerable<Speciality>> GetSpecialities();

        /// <summary>
        /// Retrieves UserActivation by user id and short code
        /// </summary>
        /// class that represents the information of <see cref="UserActivation">.</returns>
        Task<UserActivation> GetUserActivation(string userId, string shortCode);

        /// <summary>
        /// Creates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        Task<SubmitResponse> AddUserActivation(UserActivation userActivation);

        /// <summary>
        /// Updates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        Task<SubmitResponse> UpdateUserActivation(UserActivation userActivation);
    }
}
