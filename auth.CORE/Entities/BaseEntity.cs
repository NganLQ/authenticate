using System;
using System.Collections.Generic;
using System.Text;

namespace auth.CORE.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ModifiedBy { get; set; }
    }
}
