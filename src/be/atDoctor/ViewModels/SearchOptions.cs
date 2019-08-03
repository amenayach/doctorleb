namespace atDoctor.ViewModels
{
    using Health.Doctors;

    /// <summary>
    /// Represents the search criterias.
    /// </summary>
    public class SearchOptions
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

        /// <summary>
        /// Converts an insteance of <see cref="SearchOptions"/> class to a new insteance of <see cref="SearchOptionsInfo"/> class.
        /// </summary>
        /// <returns>Return a new insteance of <see cref="SearchOptionsInfo"/>
        /// class that represents the search options.</returns>
        internal SearchOptionsInfo ToObjectModel()
        {
            return new SearchOptionsInfo()
            {
                Keyword = this.Keyword,
                ProvinceId = this.ProvinceId,
                KazaId = this.KazaId,
                RegionId = this.RegionId,
                SpecialityId = this.SpecialityId,
                PageIndex = this.PageIndex,
                ItemPerPage = this.ItemPerPage
            };
        }

        public override string ToString()
        {
            return
                $"k_{Keyword}_P_{ProvinceId}_Z_{KazaId}_R_{RegionId}_Spc_{SpecialityId}_Pi_{PageIndex}_Ip_{ItemPerPage}";
        }
    }
}
