using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class PhotoModel
    {
        [Key]
        public int PhotoID { get; set; }

        [DisplayName("Album")]
        [Required(ErrorMessage = "Album is Required")]
        public int AlbumID { get; set; }

        [DisplayName("Photo")]
        public string PhotoPath { get; set; }

        public string PhotoFilleName { get; set; }

        public System.Nullable<bool> IsPhotoAlbumThumbnail { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }

        [Ignore]
        public string ShowPhoto { get; set; }

    }
}