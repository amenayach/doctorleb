
namespace Health.Doctors.Sql
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Health.Configuration;

    /// <summary>
    /// Handles doctors operations
    /// </summary>
    public class DoctorStore : IDoctorStore
    {
        /// <summary>
        /// The SQL data provider
        /// </summary>
        private readonly SqlDataProvider sqlDataProvider;

        /// <summary>
        /// Initialize the SQL store
        /// </summary>
        public DoctorStore(SqlConfig sqlConfig)
        {
            if (string.IsNullOrWhiteSpace(sqlConfig?.ConnectionString))
            {
                throw new ArgumentNullException(nameof(sqlConfig));
            }

            sqlDataProvider = new SqlDataProvider(sqlConfig.ConnectionString);
        }

        /// <summary>
        /// Creates a new contact us ticket.
        /// </summary>
        /// <param name="info">Ticket information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponse> CreateContactUsTicket(CreateContactUsTicketInfo info)
        {
            var result = await sqlDataProvider.ExecuteScalarAsync<int>("dbo.[AddContactUsTicket]", true,
                new System.Data.SqlClient.SqlParameter("@name", info.Name),
                new System.Data.SqlClient.SqlParameter("@phoneNumber", info.PhoneNumber),
                new System.Data.SqlClient.SqlParameter("@email", info.Email),
                new System.Data.SqlClient.SqlParameter("@description", info.Description));

            return result == 0 ? SubmitResponse.Ok() : SubmitResponse.Error();
        }

        /// <summary>
        /// Returns a list of doctors according to the entered keyword
        /// </summary>
        /// <param name="keyword">the user input to search on</param>
        /// <returns>A list of Doctors</returns>
        public async Task<IEnumerable<Doctor>> GetSearchAutoComplete(string keyword)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Doctor>("GetSearchAutoComplete", true, new System.Data.SqlClient.SqlParameter("@keyword", keyword));
        }

        /// <summary>
        /// Search doctors database via variaty of search criterias.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctors.</returns>
        public async Task<IEnumerable<Doctor>> SearchDoctor(SearchOptionsInfo searchOptions)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Doctor>("[dbo].[SearchDoctor]", true,
                new System.Data.SqlClient.SqlParameter("@keyword", searchOptions.Keyword),
                new System.Data.SqlClient.SqlParameter("@provinceId", searchOptions.ProvinceId),
                new System.Data.SqlClient.SqlParameter("@kazaId", searchOptions.KazaId),
                new System.Data.SqlClient.SqlParameter("@regionId", searchOptions.RegionId),
                new System.Data.SqlClient.SqlParameter("@specialityId", searchOptions.SpecialityId),
                new System.Data.SqlClient.SqlParameter("@pageIndex", searchOptions.PageIndex < 0 ? 0 : searchOptions.PageIndex),
                new System.Data.SqlClient.SqlParameter("@itemPerPage", searchOptions.ItemPerPage <= 0 ? 10 : searchOptions.ItemPerPage));
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="info">Review information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponse> CreateReview(CreateReviewInfo info)
        {
            var result = await sqlDataProvider.ExecuteScalarAsync<int>("dbo.[AddOrUpdateReview]", true,
                new System.Data.SqlClient.SqlParameter("@doctorId", info.DoctorId),
                new System.Data.SqlClient.SqlParameter("@reviewerName", info.ReviewerName),
                new System.Data.SqlClient.SqlParameter("@description", info.Description),
                new System.Data.SqlClient.SqlParameter("@rank", info.Rank),
                new System.Data.SqlClient.SqlParameter("@userId", info.UserId));

            return result == 0 ? SubmitResponse.Ok() : SubmitResponse.Error();
        }

        /// <summary>
        /// Approves a review.
        /// </summary>
        /// <param name="reviewId">Review id.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponse"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponse> ApproveReview(string reviewId)
        {
            var result = await sqlDataProvider.ExecuteScalarAsync<int>("dbo.[ApproveReview]", true,
                new System.Data.SqlClient.SqlParameter("@reviewId", reviewId));

            return result == 0 ? SubmitResponse.Ok() : SubmitResponse.Error();
        }

        /// <summary>
        /// Retrieves the information for doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<Doctor> GetDoctor(string doctorId)
        {
            return (await sqlDataProvider.ExecuteQueryAsync<Doctor>("dbo.[GetDoctor]", true, new System.Data.SqlClient.SqlParameter("@doctorId", doctorId))).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the information for addresses of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="DoctorAddress"/>
        /// class that represents the informatino for addresses of doctor.</returns>
        public async Task<IEnumerable<DoctorAddress>> GetDoctorAddresses(string doctorId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<DoctorAddress>("dbo.[GetDoctorAddresses]", true, new System.Data.SqlClient.SqlParameter("@doctorId", doctorId));
        }

        /// <summary>
        /// Retrieves the information for phones of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Phonebook"/>
        /// class that represents the information for phones of doctor.</returns>
        public async Task<IEnumerable<Phonebook>> GetDoctorPhones(string doctorId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Phonebook>("dbo.[GetDoctorPhones]", true, new System.Data.SqlClient.SqlParameter("@doctorId", doctorId));
        }

        /// <summary>
        /// Retrieves the information for specialties of doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Phonebook"/>
        /// class that represents the information for phones of doctor.</returns>
        public async Task<IEnumerable<Speciality>> GetDoctorSpecialities(string doctorId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Speciality>("dbo.[GetDoctorSpecialities]", true, new System.Data.SqlClient.SqlParameter("@doctorId", doctorId));
        }

        /// <summary>
        /// Retrieves the information for doctors that related to a specific speciality.
        /// </summary>
        /// <param name="specialityId">Speciality id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(string specialityId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Doctor>("dbo.[GetDoctorsBySpeciality]", true, new System.Data.SqlClient.SqlParameter("@specialityId", specialityId));
        }

        /// <summary>
        /// Retrieves the information for kazas.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Kaza"/>
        /// class that represents the information for kazas.</returns>
        public async Task<IEnumerable<Kaza>> GetKazas()
        {
            return await sqlDataProvider.ExecuteQueryAsync<Kaza>("dbo.[GetKazas]", true);
        }

        /// <summary>
        /// Retrieves the information for kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Kaza"/>
        /// class that represents the information for kaza.</returns>
        public async Task<Kaza> GetKaza(string kazaId)
        {
            return (await sqlDataProvider.ExecuteQueryAsync<Kaza>("dbo.[GetKaza]", true, new System.Data.SqlClient.SqlParameter("@id", kazaId))).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the informatino for kazas that related to a specific province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Kaza"/>
        /// class that represents the information for kazas.</returns>
        public async Task<IEnumerable<Kaza>> GetKazasByProvince(string provinceId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Kaza>("dbo.[GetKazasByProvince]", true, new System.Data.SqlClient.SqlParameter("@provinceId", provinceId));
        }

        /// <summary>
        /// Retrieves the information for provinces.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Province"/>
        /// class that represents the information for provinces.</returns>
        public async Task<IEnumerable<Province>> GetProvinces()
        {
            return await sqlDataProvider.ExecuteQueryAsync<Province>("dbo.[GetProvinces]", true);
        }

        /// <summary>
        /// Retrieves the information for province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Province"/>
        /// class that represents the information for province.</returns>
        public async Task<Province> GetProvince(string provinceId)
        {
            return (await sqlDataProvider.ExecuteQueryAsync<Province>("dbo.[GetProvince]", true, new System.Data.SqlClient.SqlParameter("@id", provinceId))).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the information for regions.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<IEnumerable<Region>> GetRegions()
        {
            return await sqlDataProvider.ExecuteQueryAsync<Region>("dbo.[GetRegions]", true);
        }

        /// <summary>
        /// Retrieves the information for region.
        /// </summary>
        /// <param name="provinceId">Region id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<Region> GetRegion(string regionId)
        {
            return (await sqlDataProvider.ExecuteQueryAsync<Region>("dbo.[GetRegion]", true, new System.Data.SqlClient.SqlParameter("@id", regionId))).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the information for regions that related to a specific kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<IEnumerable<Region>> GetRegionsByKaza(string kazaId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Region>("dbo.[GetRegionsByKaza]", true, new System.Data.SqlClient.SqlParameter("@kazaId", kazaId));
        }

        /// <summary>
        /// Retrieves the information for reviews that related to a specifc doctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="doctorId"></param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Review"/>
        /// class that represents the information for reviews.</returns>
        public async Task<IEnumerable<Review>> GetReviewsByDoctor(string userId, string doctorId)
        {
            return await sqlDataProvider.ExecuteQueryAsync<Review>("dbo.[GetReviewsByDoctor]", true, new System.Data.SqlClient.SqlParameter("@userId", userId), new System.Data.SqlClient.SqlParameter("@doctorId", doctorId));
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
            return await sqlDataProvider.ExecuteQueryAsync<Review>("dbo.[GetPagedReviewsByDoctor]", true,
                new System.Data.SqlClient.SqlParameter("@userId", userId),
                new System.Data.SqlClient.SqlParameter("@doctorId", doctorId),
                new System.Data.SqlClient.SqlParameter("@pageIndex", pageIndex),
                new System.Data.SqlClient.SqlParameter("@pageSize", pageSize));
        }

        /// <summary>
        /// Retrieves the information for specialities.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Speciality"/>
        /// class that represents the information for specialities.</returns>
        public async Task<IEnumerable<Speciality>> GetSpecialities()
        {
            return await sqlDataProvider.ExecuteQueryAsync<Speciality>("dbo.[GetSpecialities]", true);
        }

        /// <summary>
        /// Retrieves UserActivation by user id and short code
        /// </summary>
        /// class that represents the information of <see cref="UserActivation">.</returns>
        public async Task<UserActivation> GetUserActivation(string userId, string shortCode)
        {
            var data = await sqlDataProvider.ExecuteQueryAsync<UserActivation>("dbo.[GetUserActivation]", true,
                new System.Data.SqlClient.SqlParameter("@ShortCode", shortCode),
                new System.Data.SqlClient.SqlParameter("@UserId", userId));

            if (data?.Any() ?? false)
            {
                return data.First();
            }

            return null;
        }

        /// <summary>
        /// Creates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        public async Task<SubmitResponse> AddUserActivation(UserActivation userActivation)
        {
            var result = await sqlDataProvider.ExecuteScalarAsync<int>("dbo.[AddUserActivation]", true,
                new System.Data.SqlClient.SqlParameter("@Id", userActivation.Id),
                new System.Data.SqlClient.SqlParameter("@ShortCode", userActivation.ShortCode),
                new System.Data.SqlClient.SqlParameter("@IdentityCode", userActivation.IdentityCode),
                new System.Data.SqlClient.SqlParameter("@Created", userActivation.Created),
                new System.Data.SqlClient.SqlParameter("@Expires", userActivation.Expires),
                new System.Data.SqlClient.SqlParameter("@UserId", userActivation.UserId));

            return result == 0 ? SubmitResponse.Ok() : SubmitResponse.Error();
        }

        /// <summary>
        /// Updates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        public async Task<SubmitResponse> UpdateUserActivation(UserActivation userActivation)
        {
            var result = await sqlDataProvider.ExecuteScalarAsync<int>("dbo.[UpdateUserActivation]", true,
                new System.Data.SqlClient.SqlParameter("@Id", userActivation.Id),
                new System.Data.SqlClient.SqlParameter("@Expires", userActivation.Expires));

            return result == 0 ? SubmitResponse.Ok() : SubmitResponse.Error();
        }
    }
}