using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Clone_Server
{
    internal class Car
    {
        public int Id { get; set; } = id++;
        public string? Manufacter {get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        private static int id = 1;


        public override string ToString()
        {
            return $"{this.Id} - {this.Manufacter} - {this.Model} - {this.Year}";
        }

    }
}
