//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapreduceService.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Solution
    {
        public int ID { get; set; }
        public Nullable<int> ReposID { get; set; }
        public string Path { get; set; }
        public Nullable<int> CountFiles { get; set; }
        public Nullable<int> CountLOC { get; set; }
        public Nullable<int> CountBLOC { get; set; }
        public Nullable<int> CountCLOC { get; set; }
        public Nullable<int> CountCCLOC { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
