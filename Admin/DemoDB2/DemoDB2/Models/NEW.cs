//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class NEW
    {
        public NEW()
        {
            new_image = "~/Content/images/upload-i.jpg";
        }
        public int new_id { get; set; }
        public int tag_id { get; set; }
        public int new_catalog_id { get; set; }
        public int new_created_by { get; set; }
        public string tittle { get; set; }
        public string description { get; set; }
        public string new_image { get; set; }
        public string detail { get; set; }
        public Nullable<System.DateTime> new_created { get; set; }
    
        public virtual ADMIN ADMIN { get; set; }
        public virtual CATALOG CATALOG { get; set; }
        public virtual TAG TAG { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}
