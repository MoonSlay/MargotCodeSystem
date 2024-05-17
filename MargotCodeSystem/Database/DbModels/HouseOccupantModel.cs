﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Database.DbModels
{
    public class HouseOccupantModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string LName { get; set; }

        [Required]
        public string FName { get; set; }

        [Required]
        public string MName { get; set; }

        [Required]
        public string Name
        {
            get { return LName + ", " + FName + " " + MName + "."; }
        }

        [Required]
        public string Position { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string CivilStatus { get; set; }

        [Required]
        public string SourceIncome { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public int ResidentId { get; set; }

        [ForeignKey("ResidentId")]
        public ResidentModel ResidentModel { get; set; }
    }
}