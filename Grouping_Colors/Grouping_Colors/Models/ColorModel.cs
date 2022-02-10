using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Grouping_Colors.Models
{
    public class ColorModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the color.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        [JsonProperty("year")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the pant one value.
        /// </summary>
        /// <value>The pant one value.</value>
        [JsonProperty("pantone_value")]
        public string PantOneValue { get; set; }
    }

    /// <summary>
    /// Color group view model.
    /// </summary>
    public class ColorGroupViewModel
    {
        public List<ColorModel> GroupOne { get; set; }
        public List<ColorModel> GroupTwo { get; set; }
        public List<ColorModel> GroupThree { get; set; }
    }
}