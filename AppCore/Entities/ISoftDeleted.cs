﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Entities
{
    public interface ISoftDeleted
    {
        public bool IsDeleted { get; set; }
    }
}
