﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.Request
{
    public class AssignUserRoleRequest
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
