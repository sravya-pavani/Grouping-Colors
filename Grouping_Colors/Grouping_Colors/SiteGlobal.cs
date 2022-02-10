using System;
using System.Web;
using System.Web.Configuration;

namespace Grouping_Colors
{
    public static class SiteGlobal
    {
        /// <summary>
        /// Gets or sets the API URL.
        /// </summary>
        /// <value>The API URL.</value>
        static public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the content type header.
        /// </summary>
        /// <value>The content type header.</value>
        static public string ContentTypeHeader { get; set; }

        /// <summary>
        /// Gets or sets the accept header.
        /// </summary>
        /// <value>The accept header.</value>
        static public string AcceptHeader { get; set; }

        /// <summary>
        /// Gets or sets the data json.
        /// </summary>
        /// <value>The data json.</value>
        static public string DataJson { get; set; }

        /// <summary>
        /// Gets or sets the page json.
        /// </summary>
        /// <value>The page json.</value>
        static public string PageJson { get; set; }

        /// <summary>
        /// Gets or sets the total pages json.
        /// </summary>
        /// <value>The total pages json.</value>
        static public string TotalPagesJson { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        /// <value>The current page number.</value>
        static public int CurrentPageNumber { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>The total pages.</value>
        static public int TotalPages { get; set; }

        static SiteGlobal()
        {
            // Cache all these values in static properties.
            ApiUrl = WebConfigurationManager.AppSettings["APIUrl"];
            ContentTypeHeader = WebConfigurationManager.AppSettings["ContentTypeHeader"];
            AcceptHeader = WebConfigurationManager.AppSettings["AcceptHeader"];
            DataJson = WebConfigurationManager.AppSettings["DataJson"];
            PageJson= WebConfigurationManager.AppSettings["PageJson"];
            TotalPagesJson = WebConfigurationManager.AppSettings["TotalPagesJson"];
            CurrentPageNumber = Convert.ToInt32(WebConfigurationManager.AppSettings["CurrentPageNumber"]);
            TotalPages = Convert.ToInt32(WebConfigurationManager.AppSettings["TotalPages"]);
        }
    }
}