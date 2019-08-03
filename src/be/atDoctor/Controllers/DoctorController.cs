
namespace atDoctor.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using atDoctor.ViewModels;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Net;
    using System.Collections.Generic;
    using NETCore.MailKit.Core;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// Rest api for doctor.
    /// </summary>
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        /// <summary>
        /// Manages doctor functionalities.
        /// </summary>
        private readonly DoctorManager doctorManager;

        /// <summary>
        /// Initializes a new insteance of <see cref="DoctorController"/> class.
        /// </summary>
        /// <param name="doctorManager">Manages doctor functionalities.</param>
        public DoctorController(atDoctor.DoctorManager doctorManager, IEmailService emailService)
        {
            this.doctorManager = doctorManager;
        }

        /// <summary>
        /// Creates a new contact us ticket.
        /// </summary>
        /// <param name="info">Ticket information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponseInfo"/>
        /// class that represents the submit reponse.</returns>
        [HttpPost]
        [Route("tickets/create")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Ticket created successfully.", Type = typeof(SubmitResponseInfo))]
        public async Task<IActionResult> CreateContactUsTicket([FromBody]CreateContactUsTicket info)
        {
            var result = await doctorManager.CreateContactUsTicket(info);

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of doctors according to the entered keyword
        /// </summary>
        /// <param name="keyword">the user input to search on</param>
        /// <returns>A list of Doctors</returns>
        [HttpGet]
        [Route("search/autocomplete/{keyword}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns a list of doctors according to the entered keyword.", Type = typeof(IEnumerable<DoctorDetailed>))]
        public async Task<IActionResult> GetSearchAutoComplete(string keyword)
        {
            var result = await doctorManager.GetSearchAutoComplete(keyword);

            return Ok(result);
        }

        /// <summary>
        /// Search doctors database via variaty of search criterias.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctors.</returns>
        [HttpPost]
        [Route("search")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns a list of doctors according to the entered keyword.", Type = typeof(IEnumerable<DoctorDetailed>))]
        public async Task<IActionResult> SearchDoctor([FromBody]SearchOptions searchOptions)
        {
            var result = await doctorManager.SearchDoctor(searchOptions);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="info">Review information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponseInfo"/>
        /// class that represents the submit reponse.</returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Route("reviews/create")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Review created successfully.", Type = typeof(SubmitResponseInfo))]
        public async Task<IActionResult> CreateReview([FromBody]CreateReview info)
        {
            var userId = User.GetUserId();

            var result = await doctorManager.CreateReview(info, userId);

            return Ok(result);
        }

        /// <summary>
        /// Approves a review.
        /// </summary>
        /// <param name="reviewId">Review id.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponseInfo"/>
        /// class that represents the submit reponse.</returns>
        [HttpPost]
        [Route("reviews/{reviewId}/approve/{secret}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Review is approved successfully.", Type = typeof(SubmitResponseInfo))]
        public async Task<IActionResult> ApproveReview(string reviewId, string secret)
        {
            if (secret == $"{System.DateTime.Now.Year}aB0Majd{System.DateTime.Now.Day * 3}")
            {
                var result = await doctorManager.ApproveReview(reviewId);

                return Ok(result);
            }

            return Ok(new { Success = true });
        }

        /// <summary>
        /// Retrieves the information for doctor.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        [HttpGet]
        [Route("{doctorId}/info")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Doctor retrieved successfully.", Type = typeof(ViewModels.DoctorDetailed))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Doctor not found.", Type = typeof(void))]
        public async Task<IActionResult> GetDoctor([FromRoute]string doctorId)
        {
            var userId = User.Claims.Where(m => m.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            var result = await doctorManager.GetDoctor(userId, doctorId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves doctor reviews.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        [HttpGet]
        [Route("{doctorId}/reviews/{pageIndex}/{pageSize}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Doctor retrieved successfully.", Type = typeof(ViewModels.DoctorDetailed))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Doctor not found.", Type = typeof(void))]
        public async Task<IActionResult> GetDoctorPagedReviews([FromRoute]string doctorId, [FromRoute]int pageIndex, [FromRoute]int pageSize)
        {
            var userId = User.Claims.Where(m => m.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            var result = await doctorManager.GetDoctorPagedReviews(userId, doctorId, pageIndex, pageSize);

            return Ok(result ?? new List<ReviewInfo>());
        }

        /// <summary>
        /// Retrieves the information for kazas.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="KazaInfo"/>
        /// class that represents the information for kazas.</returns>
        [HttpGet]
        [Route("kazas")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Kazas retrieved successfully.", Type = typeof(IEnumerable<KazaInfo>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Kazas are not found.", Type = typeof(void))]
        public async Task<IActionResult> GetKazas()
        {
            var result = await doctorManager.GetKazas();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the informatino for kazas that related to a specific province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="KazaInfo"/>
        /// class that represents the information for kazas.</returns>
        [HttpGet]
        [Route("provinces/{provinceId}/kazas")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Kazas retrieved successfully.", Type = typeof(IEnumerable<KazaInfo>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Kazas are not found.", Type = typeof(void))]
        public async Task<IActionResult> GetKazasByProvince(string provinceId)
        {
            var result = await doctorManager.GetKazasByProvince(provinceId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the information for provinces.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="ProvinceInfo"/>
        /// class that represents the information for provinces.</returns>
        [HttpGet]
        [Route("provinces")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Provinces retrieved successfully.", Type = typeof(IEnumerable<ProvinceInfo>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Provinces are not found.", Type = typeof(void))]
        public async Task<IActionResult> GetProvinces()
        {
            var result = await doctorManager.GetProvinces();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the information for regions.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="RegionInfo"/>
        /// class that represents the information for regions.</returns>
        [HttpGet]
        [Route("regions")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Regions retrieved successfully.", Type = typeof(IEnumerable<RegionInfo>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Regions are not found.", Type = typeof(void))]
        public async Task<IActionResult> GetRegions()
        {
            var result = await doctorManager.GetRegions();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the information for regions that related to a specific kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="RegionInfo"/>
        /// class that represents the information for regions.</returns>
        [HttpGet]
        [Route("kazas/{kazaId}/regions")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Regions retrieved successfully.", Type = typeof(IEnumerable<RegionInfo>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Regions are not found.", Type = typeof(void))]
        public async Task<IActionResult> GetRegionsByKaza(string kazaId)
        {
            var result = await doctorManager.GetRegionsByKaza(kazaId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the information for specialities.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="SpecialityInfo"/>
        /// class that represents the information for specialities.</returns>
        [HttpGet]
        [Route("specialities")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Specialities retrieved successfully.", Type = typeof(IEnumerable<SpecialityInfo>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Specialities are not found.", Type = typeof(void))]
        public async Task<IActionResult> GetSpecialities()
        {
            var result = await doctorManager.GetSpecialities();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
