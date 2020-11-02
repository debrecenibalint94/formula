using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.DataAccess.Exceptions
{
    public class DatabaseConstraintException : Exception
    {
        public string Table { get; set; }
        public string Column { get; set; }
    }
}
