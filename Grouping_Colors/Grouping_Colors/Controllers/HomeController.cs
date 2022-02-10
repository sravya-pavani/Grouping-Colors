using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Grouping_Colors.Models;
using Newtonsoft.Json.Linq;

namespace Grouping_Colors.Controllers
{
    public class HomeController : Controller
    {
        //declare all global variables       
        List<ColorModel> ColorInfo;
        ColorGroupViewModel viewModel;
        JObject results;

        //initialise current page number and total pages to 1 from web.config
        //as the API retreives the data for page 1 by default.
        int currentPageNumber = SiteGlobal.CurrentPageNumber;
        int totalPages = SiteGlobal.TotalPages;

        /// <summary>
        /// returns the viewmodel having the lists of 3 groups to the view
        /// </summary>
        /// <returns>The index.</returns>
        public ActionResult Index()
        {
            ColorInfo = new List<ColorModel>();
            viewModel = new ColorGroupViewModel();

            //retrieves the colors from the API initially with page 1
            //and stores the colors in results object
            ReteiveDataFromURL(currentPageNumber);

            //if the data is returned from API, calling 
            //groupcolors function to group the colors 
            //based on the ruleset which will return the viewmodel.
            if(results != null)
            {
                viewModel = GroupColors(ColorInfo);
            }
            else 
            {
                viewModel = null;
            }
            return View(viewModel);
        }

        /// <summary>
        /// Reteives the colors from API.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
       public void ReteiveDataFromURL(int pageNumber)
        {
            //retreiving all the global values
            string baseurl = SiteGlobal.ApiUrl;
            string contentTypeHeader = SiteGlobal.ContentTypeHeader;
            string acceptHeader = SiteGlobal.AcceptHeader;
            string dataJson = SiteGlobal.DataJson;
            string pageJson = SiteGlobal.PageJson;
            string totalPagesJson = SiteGlobal.TotalPagesJson;

            //setting url path with pagenumber, initially with 1, i.e., page 1
            string uriPath = baseurl.Replace("{pageNumber}", pageNumber.ToString());

            try
            {
                //retrieving the API data using webclient
                using (var client = new WebClient())
                {
                    client.Headers.Add(contentTypeHeader);
                    client.Headers.Add(acceptHeader);
                    var res = client.DownloadString(uriPath);
                    if (res != null)
                    {
                        //parsing the json results retrieved from API
                        results = JObject.Parse(res);                       
                        if (results[dataJson].HasValues)
                        {
                            // calling merge the retrieved colors into list
                            ColorInfo = MergeDataIntoList(results);

                            //retreiving current page number and total pages count
                            //in order to retrieve all the colors from all the pages.
                            currentPageNumber = (int)results[pageJson];
                            totalPages = (int)results[totalPagesJson];

                            //if the current page iterating is not equal to total 
                            //pages, call the same method to get colors from next page.
                            if (currentPageNumber != totalPages)
                                ReteiveDataFromURL(currentPageNumber + 1);
                        }
                        else
                            results = null;
                    }
                    else
                    {
                        results = null;
                    }
                }               
            }
            catch (Exception ex)
            {
                //catching the web exception
                if (ex is WebException we && we.Response is HttpWebResponse)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    // it can be 404, 500 etc...
                    Console.WriteLine(response.StatusCode);
                }             
            }
        }

        /// <summary>
        /// Merges all the colors into list.
        /// </summary>
        /// <returns>The data into list.</returns>
        /// <param name="results">Json object containing all the colors</param>
        public List<ColorModel> MergeDataIntoList(JObject results)
        {
            string dataJson = SiteGlobal.DataJson;           
            //insert the data retrieved from API into a list
            foreach (var result in results[dataJson])
            {
                ColorInfo.Add(new ColorModel()
                {
                    ID = (int)result["id"],
                    Name = (string)result["name"],
                    Year = (int)result["year"],
                    Color = (string)result["color"],
                    PantOneValue = (string)result["pantone_value"]
                });
            }
            return ColorInfo;
        }

        /// <summary>
        /// Groups the colors based on ruleset.
        /// Group One: if the first part in pantone_value is divisible by 3.
        /// Group Two: if the first part in pantone_value is divisible by 2 and not exists in group 1.
        /// Group Three: any color that doesn't fit in group one or two.
        /// </summary>
        /// <returns>The colors.</returns>
        /// <param name="ColorInfo">List of Colors.</param>
        public ColorGroupViewModel GroupColors(List<ColorModel> ColorInfo)
        {
            //declare three lists to hold the colors according to the ruleset.
            List<ColorModel> groupOneColorList = new List<ColorModel>();
            List<ColorModel> groupTwoColorList = new List<ColorModel>();
            List<ColorModel> groupThreeColorList = new List<ColorModel>();
           
            //iterate through the main list of colors
            //and divide them into three lists based on the ruleset.
            foreach (var item in ColorInfo)
            {
                //calling the function to get the first part of pantone_value
                //that is the value before '-'.
                string value = GetFirstPartOrEmpty(item.PantOneValue);
                int intValue = Convert.ToInt32(value);

                if (intValue % 3 == 0)
                {
                    groupOneColorList.Add(item);
                }
                else if (intValue % 2 == 0)
                {
                    groupTwoColorList.Add(item);
                }
                else
                {
                    groupThreeColorList.Add(item);
                }
            }
            //sorting all the lists based on the year in an ascending order 
            groupOneColorList.OrderBy(s => s.Year);
            groupTwoColorList.OrderBy(s => s.Year);
            groupThreeColorList.OrderBy(s => s.Year);

            //adding all the lists to the viewmodel
            viewModel.GroupOne = groupOneColorList;
            viewModel.GroupTwo = groupTwoColorList;
            viewModel.GroupThree = groupThreeColorList;
            return viewModel;
        }

        /// <summary>
        /// Gets the first part in pantone_value or empty if it 
        /// doesn't contain any characters before '-'.
        /// </summary>
        /// <returns>The until or empty.</returns>
        /// <param name="pantOneValue">Pant one value.</param>
        /// <param name="stopAt">Stop at.</param>
        public string GetFirstPartOrEmpty(string pantOneValue, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(pantOneValue))
            {
                int charLocation = pantOneValue.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return pantOneValue.Substring(0, charLocation);
                }
            }
            return String.Empty;
        }
    }
}