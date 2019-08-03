
namespace Health.Doctors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Handles doctors operations.
    /// </summary>
    public class DoctorManager
    {
        private readonly IDoctorStore doctorStore;

        /// <summary>
        /// Initializes a new insteance of <see cref="DoctorManager"/> class.
        /// </summary>
        /// <param name="doctorStore">Data contract for doctor.</param>
        public DoctorManager(IDoctorStore doctorStore)
        {
            this.doctorStore = doctorStore ?? throw new ArgumentNullException(nameof(doctorStore));
        }

        /// <summary>
        /// Creates a new contact us ticket.
        /// </summary>
        /// <param name="info">Ticket information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponse> CreateContactUsTicket(CreateContactUsTicketInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info), "Value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(info.Description))
            {
                throw new ArgumentNullException(nameof(info.Description), "Value cannot be null");
            }

            return await doctorStore.CreateContactUsTicket(info);
        }

        /// <summary>
        /// Returns a list of doctors according to the entered keyword
        /// </summary>
        /// <param name="keyword">the user input to search on</param>
        /// <returns>A list of Doctors</returns>
        public async Task<IEnumerable<Doctor>> GetSearchAutoComplete(string keyword)
        {
            return await doctorStore.GetSearchAutoComplete(keyword);
        }

        /// <summary>
        /// Search doctors database via variaty of search criterias.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctors.</returns>
        public async Task<IEnumerable<Doctor>> SearchDoctor(SearchOptionsInfo searchOptions)
        {
            return await doctorStore.SearchDoctor(searchOptions);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="info">Review information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponse> CreateReview(CreateReviewInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info), "Value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(info.DoctorId))
            {
                throw new ArgumentNullException(nameof(info.DoctorId), "Value cannot be null");
            }

            if (info.Rank < 1 || info.Rank > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(info.Rank), "Rank should be between 1 and 5");
            }

            return await doctorStore.CreateReview(info);
        }

        /// <summary>
        /// Approves a review.
        /// </summary>
        /// <param name="reviewId">Review id.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponse> ApproveReview(string reviewId)
        {
            if (string.IsNullOrEmpty(reviewId))
            {
                throw new ArgumentNullException(nameof(reviewId), "Value cannot be null");
            }

            return await doctorStore.ApproveReview(reviewId);
        }

        /// <summary>
        /// Retrieves the information for doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<Doctor> GetDoctor(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                throw new ArgumentNullException(nameof(doctorId), "Value cannot be null");
            }

            return await doctorStore.GetDoctor(doctorId);
        }

        /// <summary>
        /// Retrieves the information for addresses of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="DoctorAddress"/>
        /// class that represents the informatino for addresses of doctor.</returns>
        public async Task<IEnumerable<DoctorAddress>> GetDoctorAddresses(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                throw new ArgumentNullException(nameof(doctorId), "Value cannot be null");
            }

            var addresses = await doctorStore.GetDoctorAddresses(doctorId);

            if (addresses != null)
            {
                var phones = await doctorStore.GetDoctorPhones(doctorId);

                List<Phonebook> addressPhone = null;

                foreach (var address in addresses)
                {
                    addressPhone = new List<Phonebook>();

                    foreach (var phone in phones)
                    {
                        if (phone.AddressId.Equals(address.Id))
                        {
                            addressPhone.Add(phone);
                        }
                    }

                    address.Phonebooks = addressPhone;
                }

                return addresses;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the information for phones of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Phonebook"/>
        /// class that represents the information for phones of doctor.</returns>
        public async Task<IEnumerable<Phonebook>> GetDoctorPhones(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                throw new ArgumentNullException(nameof(doctorId), "Value cannot be null");
            }

            return await doctorStore.GetDoctorPhones(doctorId);
        }

        /// <summary>
        /// Retrieves the information for specialties of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Phonebook"/>
        /// class that represents the information for phones of doctor.</returns>
        public async Task<IEnumerable<Speciality>> GetDoctorSpecialities(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                throw new ArgumentNullException(nameof(doctorId), "Value cannot be null");
            }

            return await doctorStore.GetDoctorSpecialities(doctorId);
        }

        /// <summary>
        /// Retrieves the information for doctors that related to a specific speciality.
        /// </summary>
        /// <param name="specialityId">Speciality id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(string specialityId)
        {
            if (string.IsNullOrEmpty(specialityId))
            {
                throw new ArgumentNullException(nameof(specialityId), "Value cannot be null");
            }

            return await doctorStore.GetDoctorsBySpeciality(specialityId);
        }

        /// <summary>
        /// Retrieves the information for kazas.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Kaza"/>
        /// class that represents the information for kazas.</returns>
        public async Task<IEnumerable<Kaza>> GetKazas()
        {
            return await doctorStore.GetKazas();
        }

        /// <summary>
        /// Retrieves the information for kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Kaza"/>
        /// class that represents the information for kaza.</returns>
        public async Task<Kaza> GetKaza(string kazaId)
        {
            return await doctorStore.GetKaza(kazaId);
        }

        /// <summary>
        /// Retrieves the informatino for kazas that related to a specific province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Kaza"/>
        /// class that represents the information for kazas.</returns>
        public async Task<IEnumerable<Kaza>> GetKazasByProvince(string provinceId)
        {
            if (string.IsNullOrEmpty(provinceId))
            {
                throw new ArgumentNullException(nameof(provinceId), "Value cannot be null");
            }

            return await doctorStore.GetKazasByProvince(provinceId);
        }

        /// <summary>
        /// Retrieves the information for provinces.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Province"/>
        /// class that represents the information for provinces.</returns>
        public async Task<IEnumerable<Province>> GetProvinces()
        {
            return await doctorStore.GetProvinces();
        }

        /// <summary>
        /// Retrieves the information for province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Province"/>
        /// class that represents the information for province.</returns>
        public async Task<Province> GetProvince(string provinceId)
        {
            if (string.IsNullOrEmpty(provinceId))
            {
                throw new ArgumentNullException(nameof(provinceId), "Value cannot be null");
            }

            return await doctorStore.GetProvince(provinceId);
        }

        /// <summary>
        /// Retrieves the information for regions.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<IEnumerable<Region>> GetRegions()
        {
            return await doctorStore.GetRegions();
        }

        /// <summary>
        /// Retrieves the information for region.
        /// </summary>
        /// <param name="provinceId">Region id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<Region> GetRegion(string regionId)
        {
            if (string.IsNullOrEmpty(regionId))
            {
                throw new ArgumentNullException(nameof(regionId), "Value cannot be null");
            }

            return await doctorStore.GetRegion(regionId);
        }

        /// <summary>
        /// Retrieves the information for regions that related to a specific kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<IEnumerable<Region>> GetRegionsByKaza(string kazaId)
        {
            if (string.IsNullOrEmpty(kazaId))
            {
                throw new ArgumentNullException(nameof(kazaId), "Value cannot be null");
            }

            return await doctorStore.GetRegionsByKaza(kazaId);
        }

        /// <summary>
        /// Retrieves the information for reviews that related to a specifc doctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Review"/>
        /// class that represents the information for reviews.</returns>
        public async Task<IEnumerable<Review>> GetReviewsByDoctor(string userId, string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                throw new ArgumentNullException(nameof(doctorId), "Value cannot be null");
            }

            return await doctorStore.GetReviewsByDoctor(userId, doctorId);
        }

        /// <summary>
        /// Retrieves doctor reviews.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<IEnumerable<Review>> GetDoctorPagedReviews(string userId, string doctorId, int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                throw new ArgumentNullException(nameof(doctorId), "Value cannot be null");
            }

            return await doctorStore.GetDoctorPagedReviews(userId, doctorId, pageIndex, pageSize);
        }

        /// <summary>
        /// Retrieves the information for specialities.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Speciality"/>
        /// class that represents the information for specialities.</returns>
        public async Task<IEnumerable<Speciality>> GetSpecialities()
        {
            return await doctorStore.GetSpecialities();
        }

        /// <summary>
        /// Retrieves UserActivation by user id and short code
        /// </summary>
        /// class that represents the information of <see cref="UserActivation">.</returns>
        public async Task<UserActivation> GetUserActivation(string userId, string shortCode)
        {
            return await doctorStore.GetUserActivation(userId, shortCode);
        }

        /// <summary>
        /// Creates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        public async Task<SubmitResponse> AddUserActivation(UserActivation userActivation)
        {
            if (userActivation == null)
            {
                throw new ArgumentNullException(nameof(userActivation), "Value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(userActivation.UserId))
            {
                throw new ArgumentNullException(nameof(userActivation.UserId), "Value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(userActivation.Id))
            {
                throw new ArgumentNullException(nameof(userActivation.Id), "Value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(userActivation.ShortCode))
            {
                throw new ArgumentNullException(nameof(userActivation.ShortCode), "Value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(userActivation.IdentityCode))
            {
                throw new ArgumentNullException(nameof(userActivation.IdentityCode), "Value cannot be null");
            }
            
            return await doctorStore.AddUserActivation(userActivation);
        }
        
        /// <summary>
        /// Updates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        public async Task<SubmitResponse> UpdateUserActivation(UserActivation userActivation)
        {
            if (userActivation == null)
            {
                throw new ArgumentNullException(nameof(userActivation), "Value cannot be null");
            }
            
            if (string.IsNullOrWhiteSpace(userActivation.Id))
            {
                throw new ArgumentNullException(nameof(userActivation.Id), "Value cannot be null");
            }
            
            return await doctorStore.UpdateUserActivation(userActivation);
        }
    }
}
