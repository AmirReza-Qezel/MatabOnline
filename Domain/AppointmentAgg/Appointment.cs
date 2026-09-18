using Domain.Common;
using Domain.DoctorAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AppointmentAgg
{
    public class Appointment : BaseEntity
    {
        public Appointment(int doctorId, string patientName, string patientPhoneNumber, DateTime appointmentDate)
        {
            DoctorId = doctorId;
            PatientName = patientName;
            PatientPhoneNumber = patientPhoneNumber;
            AppointmentDate = appointmentDate;
        }

        protected Appointment() { }
        public int DoctorId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhoneNumber { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public void Edit(int doctorId, string patientName, string patientPhoneNumber, DateTime appointmentDate)
        {
            DoctorId = doctorId;
            PatientName = patientName;
            PatientPhoneNumber = patientPhoneNumber;
            AppointmentDate = appointmentDate;
        }
    }
}
