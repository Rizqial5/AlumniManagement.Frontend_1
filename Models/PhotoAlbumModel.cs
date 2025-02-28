using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class PhotoAlbumModel
    {
        [Key]
        [DisplayName("Album")]
        public int AlbumID { get; set; }

        [Required(ErrorMessage ="Album name is Required")]
        [StringLength(50)]
        [DisplayName("Name")]
        public string AlbumName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }


        public string ThumbnailImage { get; set; }
    }
}