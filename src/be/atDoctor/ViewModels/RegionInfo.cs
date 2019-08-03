
namespace atDoctor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the information for region.
    /// </summary>
    public class RegionInfo
    {
        /// <summary>
        /// Gets or sets the id for region.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for region in Arabic language.
        /// </summary>
        public string NameAr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for region in foreign language.
        /// </summary>
        public string NameFo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the kaza for region.
        /// </summary>
        public KazaInfo Kaza
        {
            get;
            set;
        }
    }
}
