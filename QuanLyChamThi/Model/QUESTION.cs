//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyChamThi.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class QUESTION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QUESTION()
        {
            this.TESTDETAIL = new HashSet<TESTDETAIL>();
        }
    
        public int IDQuestion { get; set; }
        public string Content { get; set; }
        public Nullable<int> IDDifficulty { get; set; }
        public string IDSubject { get; set; }
    
        public virtual DIFFICULTY Difficulty { get; set; }
        public virtual SUBJECT SUBJECT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESTDETAIL> TESTDETAIL { get; set; }
    }
}
