﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class RegLinePicture
    {
        [PrimaryKey]
        public Guid RegLineGuid { get; set; }
        public string Picture { get; set; }
    }
}
