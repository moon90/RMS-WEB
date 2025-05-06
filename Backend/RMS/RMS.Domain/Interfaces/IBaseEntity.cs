﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Interfaces
{
    public interface IBaseEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
