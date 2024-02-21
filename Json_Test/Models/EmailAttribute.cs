using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Json_Test.Models
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")
        {
        }
    }

    public class UserNameAttribute : RegularExpressionAttribute
    {
        public UserNameAttribute()
            : base("^[a-z0-9_-]+(\\.[a-z0-9_-]+)*$")
        {
        }
    }

    public class NumberAttribute : RegularExpressionAttribute
    {
        public NumberAttribute()
            : base("^[0-9]*$")
        {
        }
    }
}