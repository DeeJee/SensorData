﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SensorDataApi.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SensorDataSqlEntities : DbContext
    {
        public SensorDataSqlEntities()
            : base("name=SensorDataSqlEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SensorData> SensorData { get; set; }
        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<DataSource> DataSource { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<DataType> DataType { get; set; }
    }
}
