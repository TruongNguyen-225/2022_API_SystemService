﻿using System;

namespace SystemServiceAPI.Dto.Report
{
    public class ReportRequestDto
    {
        public int ServiceID { get; set; }
        public int RetailID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
