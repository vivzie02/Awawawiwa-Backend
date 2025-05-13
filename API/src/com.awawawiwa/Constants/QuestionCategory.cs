using System.Collections.Generic;
using System.ComponentModel;

namespace com.awawawiwa.Constants
{
    /// <summary>
    /// QuestionCategory
    /// </summary>
    public static class QuestionCategory
    {
        private static readonly HashSet<string> _allCategories = new HashSet<string>
        {
            General,
            Science,
            History,
            Geography,
            Sports,
            Entertainment,
            Art,
            Technology
        };

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool IsValid(string category)
        {
            return _allCategories.Contains(category);
        }

        /// <summary>
        /// ListAll
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ListAll()
        {
            return _allCategories;
        }

        /// <summary>
        /// General
        /// </summary>
        public const string General = "General";
        /// <summary>
        /// Science
        /// </summary>
        public const string Science = "Science";
        /// <summary>
        /// History
        /// </summary>
        public const string History = "History";
        /// <summary>
        /// Geography
        /// </summary>
        public const string Geography = "Geography";
        /// <summary>
        /// Sports
        /// </summary>
        public const string Sports = "Sports";
        /// <summary>
        /// Entertainment
        /// </summary>
        public const string Entertainment = "Entertainment";
        /// <summary>
        /// Art
        /// </summary>
        public const string Art = "Art";
        /// <summary>
        /// Technology
        /// </summary>
        public const string Technology = "Technology";
    }
}
