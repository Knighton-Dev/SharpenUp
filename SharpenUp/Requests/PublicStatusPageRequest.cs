﻿using System.Collections.Generic;

namespace SharpenUp.Requests
{
    public class PublicStatusPageRequest
    {
        /// <summary>
        /// Optional (if not used, will return all public status pages in an account. Else, it is possible to define any number of public status pages with their IDs like: psps=236-1782-4790).
        /// </summary>
        public List<int> PublicStatusPages { get; set; } = new List<int>();

        /// <summary>
        /// Optional (used for pagination. Defines the record to start paginating. Default is 0).
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Optional (used for pagination. Defines the max number of records to return for the response. Default and max. is 50).
        /// </summary>
        public int Limit { get; set; } = 50;
    }
}
