using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SharpDevelopMVC4.Models
{
	[Table("OV_Appoinment")]
	public class Appointment
	{
		
		public int Id { get; set; }
		
		public string Dayofappointment { get; set; }
		
		public string Customername {get; set;}
		
		public string Number { get; set; }
		
		public string Address { get; set; }
		
		public string Concern { get; set; }
		
		public int VetId {get; set;}
		
		
		
	}
}
