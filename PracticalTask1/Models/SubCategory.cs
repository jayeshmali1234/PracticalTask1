﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalTask1.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}