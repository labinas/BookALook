using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookALookProject.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Salon> Salons { get; set; }

        public Category()
        {
            Salons = new List<Salon>();
        }
    }

    public class Salon
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalonId { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<Category> Categories { get; set; }
        public DateTime WorkingFrom { get; set; }
        public DateTime WorkingTo { get; set; }
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public Salon()
        {
            Categories = new List<Category>();
        }

        public string CategoriesNames()
        {
            int i = 1;
            string names = "";
            foreach(var Category in Categories)
            {
                names += Category.CategoryName;
                if(i < this.Categories.Count)
                {
                    names += ", ";
                }
                i++;
            }
            return names;
        }
        
    }

    public class SalonService
    {
        public int SalonServiceId { get; set; }
        public Salon Salon { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public ApplicationUser Client { get; set; }
        public Salon Salon { get; set; }
        public string AppointmentService { get; set; }
        public string AppointmentCategory { get; set; }
        public DateTime Time { get; set; }
        public DateTime Date { get; set; }
    }

    public class SalonWithAppointments
    {
        public Salon Salon { get; set; }
        public List<Appointment> Appointments { get; set; }
    }

    public class UserWithAppointments
    {
        public string UserRole { get; set; }
        public ApplicationUser User { get; set; }
        public List<Appointment> Appointments { get; set; }
    }

    public class SalonWithServices
    {
        public Salon Salon { get; set; }
        public List<SalonService> SalonServices { get; set; }
    }

    public class AppointmentViewModel
    {
        public string AppointmentService { get; set; }
        public string AppointmentCategory { get; set; }
        public DateTime Time { get; set; }
        public DateTime Date { get; set; }
        public string SalonName {get; set;}
    }

    public class SalonServiceViewModel
    {
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
