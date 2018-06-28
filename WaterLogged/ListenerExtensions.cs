using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Filters;

namespace WaterLogged
{
    public static class ListenerExtensions
    {
        /// <summary>
        /// Adds a whitelist filter based on message tags to the listener.
        /// </summary>
        /// <param name="listener">The listener to add the whitelist to.</param>
        /// <param name="items">The items to whitelist.</param>
        public static Listener FilterWhitelist(this Listener listener, params string[] items)
        {
            TagWhitelistFilter filter = (TagWhitelistFilter)listener.FilterManager.Filters.FirstOrDefault(f => f.GetType() == typeof(TagWhitelistFilter));
            if(filter == null)
            {
                filter = new TagWhitelistFilter();
            }
            filter.Whitelist.AddRange(items);
            return listener;
        }
        
        /// <summary>
        /// Adds a blacklist filter based on message tags to the listener.
        /// </summary>
        /// <param name="listener">The listener to add the blacklist to.</param>
        /// <param name="items">The items to blacklist.</param>
        public static Listener FilterBlacklist(this Listener listener, params string[] items)
        {
            TagBlacklistFilter filter = (TagBlacklistFilter)listener.FilterManager.Filters.FirstOrDefault(f => f.GetType() == typeof(TagBlacklistFilter));
            if(filter == null)
            {
                filter = new TagBlacklistFilter();
            }
            filter.Blacklist.AddRange(items);
            return listener;
        }
    }
}
