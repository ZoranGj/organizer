//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Organizer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Goal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goal()
        {
            this.Activities = new HashSet<Activity>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }
        public short MinHoursPerWeek { get; set; }
        public short MaxHoursPerWeek { get; set; }
        public string Color { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual User User { get; set; }
    }
}
