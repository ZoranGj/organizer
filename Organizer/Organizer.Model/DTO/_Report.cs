﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public abstract class Report
    {
        public abstract string DisplayLabel { get; }
    }
}