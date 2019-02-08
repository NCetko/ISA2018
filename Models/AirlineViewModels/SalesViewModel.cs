﻿using ISA.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.AirlineViewModels
{
    public class SalesViewModel
    {
        public string Date { get; set; }
        public int Count { get; set; }

    }

}