using Domain.AppointmentAgg;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DoctorAgg
{
    public class Doctor : BaseEntity
    {
        public Doctor(string fullName, string specialty, string info, string clinicPhoneNumber)
        {
            FullName = fullName;
            Specialty = specialty;
            Info = info;
            ClinicPhoneNumber = clinicPhoneNumber;
        }

        protected Doctor() { }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public string ClinicPhoneNumber { get; set; } = string.Empty ;
        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();


        public void Edit(string fullName, string specialty, string info, string clinicPhoneNumber)
        {
            FullName = fullName;
            Specialty = specialty;
            Info = info;
            ClinicPhoneNumber = clinicPhoneNumber;
        }
    }
}
