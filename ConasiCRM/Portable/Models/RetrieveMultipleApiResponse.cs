using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class RetrieveMultipleApiResponse<T> where T : class
    {
        public List<T> value { get; set; }
    }
}
