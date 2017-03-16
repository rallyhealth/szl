using System;
using System.Collections.Generic;
using Szl.MvcDemo.Szl;

namespace Szl.MvcDemo
{
    public static class VeryImpressiveInMemoryDatabase
    {
        static VeryImpressiveInMemoryDatabase()
        {
            Guids = new List<Guid>();
            EnrollmentState = new EnrollmentState {Dirty = true};
        }

        public static List<Guid> Guids { get; set; }

        public static EnrollmentState EnrollmentState { get; set; }
    }
}