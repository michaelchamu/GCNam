//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GiraffeSpotter.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gallery
    {
        public int Id { get; set; }
        public byte[] ImageBytes { get; set; }
        public string Caption { get; set; }
        public Nullable<int> ObservationId { get; set; }
    
        public virtual Observation Observation { get; set; }
    }
}
