﻿namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CheckSubsidyStateResponse
    {
        public int state { get; set; }

        public Subsidy subsidy { get; set; }
    }
}
