﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Role { get; set; }
    }
}
