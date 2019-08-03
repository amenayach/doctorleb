namespace Health.Doctors
{

    /// <summary>
    /// Represents the search criterias.
    /// </summary>
    public class SearchOptionsInfo
    {
        /// <summary>
        /// Gets or sets the search keyword.
        /// </summary>
        public string Keyword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the province id.
        /// </summary>
        public string ProvinceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the kaza id.
        /// </summary>
        public string KazaId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        public string RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the speciality.
        /// </summary>
        public string SpecialityId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page index.
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int ItemPerPage
        {
            get;
            set;
        }
    }
}
