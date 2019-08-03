namespace atDoctor
{
    using ViewModels;
    using Health.Doctors;
    using Health.Doctors.Caching;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DoctorManager
    {
        private readonly Health.Doctors.DoctorManager doctorManager;

        private readonly ICacheManager cacheManager;

        private const string SearchAutoCompleteKey = "SearchAutoComplete";
        private const string DoctorInfoKey = "DoctorInfo";
        private const string KazasKey = "Kazas";
        private const string SearchDoctorKey = "SearchDoctor";
        private const string KazaKey = "Kaza";
        private const string ProvincesKey = "Provinces";
        private const string RegionsKey = "Regions";
        private const string SpecialitiesKey = "Specialities";
        private const string DoctorReviewsKey = "DoctorReviews";

        /// <summary>
        /// Initializes a new insteance of <see cref="DoctorManager"/> class.
        /// </summary>
        /// <param name="doctorManager">Doctor manager.</param>
        /// <param name="cacheManager">InMemoryCache manager.</param>
        public DoctorManager(Health.Doctors.DoctorManager doctorManager, ICacheManager cacheManager)
        {
            this.doctorManager = doctorManager;
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Creates a new contact us ticket.
        /// </summary>
        /// <param name="info">Ticket information.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponseInfo"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponseInfo> CreateContactUsTicket(CreateContactUsTicket info)
        {
            var result = await doctorManager.CreateContactUsTicket(info.ToObjectModel());

            if (result == null)
            {
                return null;
            }

            return ToSubmitResponseViewModel(result);
        }

        /// <summary>
        /// Returns a list of doctors according to the entered keyword
        /// </summary>
        /// <param name="keyword">the user input to search on</param>
        /// <returns>A list of Doctors</returns>
        public async Task<IEnumerable<DoctorInfo>> GetSearchAutoComplete(string keyword)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(SearchAutoCompleteKey + keyword,
                async () =>
                {
                    var result = await doctorManager.GetSearchAutoComplete(keyword);

                    return result?.Select(ToDoctorViewModel);
                });

            return cacheResult;
        }

        /// <summary>
        /// Search doctors database via variaty of search criterias.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Doctor"/>
        /// class that represents the information for doctors.</returns>
        public async Task<IEnumerable<DoctorInfo>> SearchDoctor(SearchOptions searchOptions)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(SearchDoctorKey + searchOptions,
                async () =>
                {
                    var result = await doctorManager.SearchDoctor(searchOptions.ToObjectModel());

                    return result?.Select(ToDoctorViewModel);
                });

            return cacheResult;
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="info">Review information.</param>
        /// <param name="userId"></param>
        /// <returns>Returns an insteance of <see cref="SubmitResponseInfo"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponseInfo> CreateReview(CreateReview info, Guid userId)
        {
            var result = await doctorManager.CreateReview(info.ToObjectModel(userId));

            if (result == null)
            {
                return null;
            }

            await cacheManager.RemoveItemsFromCacheAsync(DoctorInfoKey + info.DoctorId + "_" + userId);

            return ToSubmitResponseViewModel(result);
        }

        /// <summary>
        /// Approves a review.
        /// </summary>
        /// <param name="reviewId">Review id.</param>
        /// <returns>Returns an insteance of <see cref="SubmitResponseInfo"/>
        /// class that represents the submit reponse.</returns>
        public async Task<SubmitResponseInfo> ApproveReview(string reviewId)
        {
            var result = await doctorManager.ApproveReview(reviewId);

            if (result == null)
            {
                return null;
            }

            return ToSubmitResponseViewModel(result);
        }

        /// <summary>
        /// Retrieves the information for doctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="doctorId">Doctor id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<DoctorDetailed> GetDoctor(string userId, string doctorId)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(DoctorInfoKey + doctorId + "_" + userId,
                async () =>
                {
                    var doctor = new DoctorDetailed();

                    await Task.WhenAll(
                        doctorManager.GetDoctor(doctorId).ContinueWith(result =>
                        {
                            var doctorInfo = result.Result;

                            if (doctorInfo != null)
                            {
                                doctor.DoctorInfo = ToDoctorViewModel(doctorInfo);
                            }
                        }),
                        doctorManager.GetDoctorAddresses(doctorId).ContinueWith(result =>
                        {
                            var doctorAddresses = result.Result;

                            if (doctorAddresses != null && doctorAddresses.Any())
                            {
                                doctor.DoctorAddressesInfo = ToDoctorAddressViewModel(doctorAddresses).Result;
                            }
                        }),
                        doctorManager.GetDoctorSpecialities(doctorId).ContinueWith(result =>
                        {
                            var doctorSpecialties = result.Result;

                            if (doctorSpecialties != null && doctorSpecialties.Any())
                            {
                                doctor.Specialities = doctorSpecialties.Select(ToSpecialityViewModel);
                            }
                        }),
                        doctorManager.GetReviewsByDoctor(userId, doctorId).ContinueWith(result =>
                        {
                            var doctorReviews = result.Result;

                            if (doctorReviews != null && doctorReviews.Any())
                            {
                                doctor.Reviews = doctorReviews.Select(ToReviewViewModel);
                            }
                        })
                    );

                    return doctor;
                });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for kazas.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="KazaInfo"/>
        /// class that represents the information for kazas.</returns>
        public async Task<IEnumerable<KazaInfo>> GetKazas()
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(KazasKey, async () =>
            {
                var result = await doctorManager.GetKazas();

                if (result == null || !result.Any())
                {
                    return null;
                }

                var provinces = await GetProvinces();

                return result.Select(o => ToKazaViewModel(o, provinces));
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Kaza"/>
        /// class that represents the information for kaza.</returns>
        public async Task<KazaInfo> GetKaza(string kazaId)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(KazaKey + kazaId, async () =>
            {
                var result = await doctorManager.GetKaza(kazaId);

                if (result == null)
                {
                    return null;
                }

                return ToKazaViewModel(result, await GetProvinces());
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the informatino for kazas that related to a specific province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="KazaInfo"/>
        /// class that represents the information for kazas.</returns>
        public async Task<IEnumerable<KazaInfo>> GetKazasByProvince(string provinceId)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(KazasKey + "_P_" + provinceId,
                async () =>
                {
                    var result = await doctorManager.GetKazasByProvince(provinceId);

                    if (result == null || !result.Any())
                    {
                        return null;
                    }

                    var provinces = await GetProvinces();

                    return result.Select(o => ToKazaViewModel(o, provinces));
                });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for provinces.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="ProvinceInfo"/>
        /// class that represents the information for provinces.</returns>
        public async Task<IEnumerable<ProvinceInfo>> GetProvinces()
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(ProvincesKey, async () =>
            {
                var result = await doctorManager.GetProvinces();

                if (result == null || !result.Any())
                {
                    return null;
                }

                return result.Select(ToProvinceViewModel);
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for province.
        /// </summary>
        /// <param name="provinceId">Province id.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Province"/>
        /// class that represents the information for province.</returns>
        public async Task<ProvinceInfo> GetProvince(string provinceId)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(ProvincesKey + provinceId, async () =>
            {
                var result = await doctorManager.GetProvince(provinceId);

                if (result == null)
                {
                    return null;
                }

                return ToProvinceViewModel(result);
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for regions.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="RegionInfo"/>
        /// class that represents the information for regions.</returns>
        public async Task<IEnumerable<RegionInfo>> GetRegions()
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(RegionsKey, async () =>
            {
                var result = await doctorManager.GetRegions();

                if (result == null || !result.Any())
                {
                    return null;
                }

                //Todo: To be replaced by GetKazasByRegionIds if still needed
                var kazas = await GetKazas();

                return result.Select(o => ToRegionViewModel(o, kazas));
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for region.
        /// </summary>
        /// <param name="provinceId">Region id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="Region"/>
        /// class that represents the information for regions.</returns>
        public async Task<RegionInfo> GetRegion(string regionId)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(RegionsKey + regionId, async () =>
            {
                var result = await doctorManager.GetRegion(regionId);

                if (result == null)
                {
                    return null;
                }

                //Todo: To be replaced by GetKazasByRegionIds if still needed
                var kazas = await GetKazas();

                return ToRegionViewModel(result, kazas);
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for regions that related to a specific kaza.
        /// </summary>
        /// <param name="kazaId">Kaza id.</param>
        /// <returns>Returns asynchronous job that contains a list of <see cref="RegionInfo"/>
        /// class that represents the information for regions.</returns>
        public async Task<IEnumerable<RegionInfo>> GetRegionsByKaza(string kazaId)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(RegionsKey + "_K_" + kazaId, async () =>
            {
                var result = await doctorManager.GetRegionsByKaza(kazaId);

                if (result == null || !result.Any())
                {
                    return null;
                }

                //Todo: To be replaced by GetKazasByRegionIds if still needed
                var kazas = await GetKazas();

                return result.Select(o => ToRegionViewModel(o, kazas));
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves the information for specialities.
        /// </summary>
        /// <returns>Returns asynchronous job that contains a list of <see cref="SpecialityInfo"/>
        /// class that represents the information for specialities.</returns>
        public async Task<IEnumerable<SpecialityInfo>> GetSpecialities()
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(SpecialitiesKey, async () =>
            {
                var result = await doctorManager.GetSpecialities();

                if (result == null || !result.Any())
                {
                    return null;
                }

                return result.Select(ToSpecialityViewModel);
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves doctor reviews.
        /// </summary>
        /// <param name="doctorId">Doctor id.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Returns asynchronous job that contains an insteance of <see cref="Doctor"/>
        /// class that represents the information for doctor.</returns>
        public async Task<IEnumerable<ReviewInfo>> GetDoctorPagedReviews(string userId, string doctorId, int pageIndex,
            int pageSize)
        {
            var cacheResult = await cacheManager.GetOrAddCachedObjectAsync(DoctorReviewsKey + $"u_{userId}_d_{doctorId}_Pi_{pageIndex}_Ps_{pageSize}", async () =>
            {
                var result = await doctorManager.GetDoctorPagedReviews(userId, doctorId, pageIndex, pageSize);

                if (result == null || !result.Any())
                {
                    return null;
                }

                return result.Select(ToReviewViewModel);
            });

            return cacheResult;
        }

        /// <summary>
        /// Retrieves UserActivation by user id and short code
        /// </summary>
        /// class that represents the information of <see cref="UserActivation">.</returns>
        public async Task<UserActivation> GetUserActivation(string userId, string shortCode)
        {
            return await doctorManager.GetUserActivation(userId, shortCode);
        }

        /// <summary>
        /// Creates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        public async Task<SubmitResponse> AddUserActivation(UserActivation userActivation)
        {
            return await doctorManager.AddUserActivation(userActivation);
        }
        
        /// <summary>
        /// Updates a new UserActivation
        /// </summary>
        /// class that represents the information of <see cref="SubmitResponse">.</returns>
        public async Task<SubmitResponse> UpdateUserActivation(UserActivation userActivation)
        {
            return await doctorManager.UpdateUserActivation(userActivation);
        }

        /// <summary>
        /// Converts an insteance of <see cref="Province"/> Class to a new insteance of <see cref="ProvinceInfo"/> class.
        /// </summary>
        /// <param name="info">Province information.</param>
        /// <returns>Returns new insteance of <see cref="ProvinceInfo"/> class.</returns>
        private ProvinceInfo ToProvinceViewModel(Province info)
        {
            return new ProvinceInfo()
            {
                Id = info.Id,
                NameAr = info.NameAr,
                NameFo = info.NameFo
            };
        }

        /// <summary>
        /// Converts an insteance of <see cref="Kaza"/> Class to a new insteance of <see cref="KazaInfo"/> class.
        /// </summary>
        /// <param name="info">Kaza information.</param>
        /// <returns>Returns new insteance of <see cref="KazaInfo"/> class.</returns>
        private KazaInfo ToKazaViewModel(Kaza info, IEnumerable<ProvinceInfo> provinces)
        {
            return new KazaInfo()
            {
                Id = info.Id,
                NameAr = info.NameAr,
                NameFo = info.NameFo,
                Province = provinces?.FirstOrDefault(m => m.Id == info.ProvinceId)
            };
        }

        /// <summary>
        /// Converts an insteance of <see cref="Region"/> Class to a new insteance of <see cref="RegionInfo"/> class.
        /// </summary>
        /// <param name="info">Region information.</param>
        /// <returns>Returns new insteance of <see cref="RegionInfo"/> class.</returns>
        private RegionInfo ToRegionViewModel(Region info, IEnumerable<KazaInfo> kazas)
        {
            return new RegionInfo()
            {
                Id = info.Id,
                NameAr = info.NameAr,
                NameFo = info.NameFo,
                Kaza = kazas?.FirstOrDefault(m => m.Id == info.KazaId)
            };
        }

        /// <summary>
        /// Converts an insteance of <see cref="Doctor"/> Class to a new insteance of <see cref="DoctorInfo"/> class.
        /// </summary>
        /// <param name="info">Doctor information.</param>
        /// <returns>Returns new insteance of <see cref="DoctorInfo"/> class.</returns>
        private DoctorInfo ToDoctorViewModel(Doctor info)
        {
            return new DoctorInfo()
            {
                Id = info.Id,
                NameAr = info.NameAr,
                NameFo = info.NameFo,
                NumberOfReviewer = info.NumberOfReviewer,
                Rank = info.Rank
            };
        }

        /// <summary>
        /// Converts a list of <see cref="DoctorAddress"/> Class to a new list of <see cref="DoctorAddressInfo"/> class.
        /// </summary>
        /// <param name="info">Doctor address information.</param>
        /// <returns>Returns new list of <see cref="DoctorAddress"/> class.</returns>
        private async Task<IEnumerable<DoctorAddressInfo>> ToDoctorAddressViewModel(IEnumerable<DoctorAddress> info)
        {
            if (info == null)
            {
                return null;
            }

            var result = new List<DoctorAddressInfo>();

            foreach (var ad in info)
            {
                result.Add(await ToDoctorAddressViewModel(ad));
            }

            return result;
        }

        /// <summary>
        /// Converts an insteance of <see cref="DoctorAddress"/> Class to a new insteance of <see cref="DoctorAddressInfo"/> class.
        /// </summary>
        /// <param name="info">Doctor address information.</param>
        /// <returns>Returns new insteance of <see cref="DoctorAddress"/> class.</returns>
        private async Task<DoctorAddressInfo> ToDoctorAddressViewModel(DoctorAddress info)
        {
            return new DoctorAddressInfo()
            {
                Id = info.Id,
                Type = (ViewModels.AddressType) (int) info.Type,
                Address = info.Address,
                Region = await GetRegion(info.RegionId),
                Phonebooks = ToPhonebookViewModel(info.Phonebooks)
            };
        }

        /// <summary>
        /// Converts a list of <see cref="Phonebook"/> Class to a new list of <see cref="PhonebookInfo"/> class.
        /// </summary>
        /// <param name="info">Phonebook information.</param>
        /// <returns>Returns new list of <see cref="PhonebookInfo"/> class.</returns>
        private IEnumerable<PhonebookInfo> ToPhonebookViewModel(IEnumerable<Phonebook> info)
        {
            if (info == null)
            {
                return null;
            }

            var result = new List<PhonebookInfo>();

            foreach (var ph in info)
            {
                result.Add(ToPhonebookViewModel(ph));
            }

            return result;
        }

        /// <summary>
        /// Converts an insteance of <see cref="Phonebook"/> Class to a new insteance of <see cref="PhonebookInfo"/> class.
        /// </summary>
        /// <param name="info">Phonebook information.</param>
        /// <returns>Returns new insteance of <see cref="PhonebookInfo"/> class.</returns>
        private PhonebookInfo ToPhonebookViewModel(Phonebook info)
        {
            return new PhonebookInfo()
            {
                Id = info.Id,
                PhoneNumber = info.PhoneNumber,
                Type = (ViewModels.PhoneType) (int) info.Type
            };
        }

        /// <summary>
        /// Converts an insteance of <see cref="Speciality"/> class to a new insteance of <see cref="SpecialityInfo"/> class.
        /// </summary>
        /// <param name="info">Speciality information.</param>
        /// <returns>Returns new insteance of <see cref="SpecialityInfo"/> class.</returns>
        private SpecialityInfo ToSpecialityViewModel(Speciality info)
        {
            return new SpecialityInfo()
            {
                Id = info.Id,
                NameAr = info.NameAr,
                NameEn = info.NameEn,
                NameFr = info.NameFr
            };
        }

        /// <summary>
        /// Converts an insteance of <see cref="Review"/> class to a new insteance of <see cref="ReviewInfo"/> class.
        /// </summary>
        /// <param name="info">Review information.</param>
        /// <returns>Returns new insteance of <see cref="ReviewInfo"/> class.</returns>
        private ReviewInfo ToReviewViewModel(Review info)
        {
            return new ReviewInfo()
            {
                Id = info.Id,
                Description = info.Description,
                Rank = info.Rank,
                ReviewerName = info.ReviewerName,
                UserId = info.UserId,
                CreatedOn = info.CreatedOn
            };
        }

        /// <summary>
        /// Converts an insteance of <see cref="SubmitResponse"/> class to a new insteance of <see cref="SubmitResponseInfo"/> class.
        /// </summary>
        /// <param name="info">Submit response information.</param>
        /// <returns>Returns new insteance of <see cref="SubmitResponseInfo"/> class.</returns>
        private SubmitResponseInfo ToSubmitResponseViewModel(SubmitResponse info)
        {
            return new SubmitResponseInfo()
            {
                Exception = info.Exception,
                Note = info.Note,
                Success = info.Success
            };
        }
    }
}