using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Filters;

namespace WaterLogged
{
    public static class Extensions
    {
        /// <summary>
        /// Adds a whitelist filter based on message tags to the <see cref="IOutput"/> item.
        /// </summary>
        /// <param name="outputItem">The <see cref="IOutput"/> item to add the whitelist to.</param>
        /// <param name="items">The items to whitelist.</param>
        public static T FilterWhitelist<T>(this T outputItem, params string[] items) where T : IOutput
        {
            TagWhitelistFilter filter = (TagWhitelistFilter)outputItem.FilterManager.Filters.FirstOrDefault(f => f.GetType() == typeof(TagWhitelistFilter));
            if(filter == null)
            {
                filter = new TagWhitelistFilter();
            }
            filter.Whitelist.AddRange(items);
            return outputItem;
        }
        
        /// <summary>
        /// Adds a blacklist filter based on message tags to the <see cref="IOutput"/> item.
        /// </summary>
        /// <param name="outputItem">The <see cref="IOutput"/> item to add the blacklist to.</param>
        /// <param name="items">The items to blacklist.</param>
        public static T FilterBlacklist<T>(this T outputItem, params string[] items) where T : IOutput
        {
            TagBlacklistFilter filter = (TagBlacklistFilter)outputItem.FilterManager.Filters.FirstOrDefault(f => f.GetType() == typeof(TagBlacklistFilter));
            if(filter == null)
            {
                filter = new TagBlacklistFilter();
            }
            filter.Blacklist.AddRange(items);
            return outputItem;
        }
    }
}
