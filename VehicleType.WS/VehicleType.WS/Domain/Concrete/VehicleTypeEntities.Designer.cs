﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using NHibernate template.
// Code is generated on: 21/12/2018 17:32:11
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace VehicleType.WS.Domain.Entities
{

    /// <summary>
    /// There are no comments for VehicleType.WS.Domain.Entities.Vehicle, VehicleType.WS in the schema.
    /// </summary>
    public partial class Vehicle {
    
        #region Extensibility Method Definitions
        
        /// <summary>
        /// There are no comments for OnCreated in the schema.
        /// </summary>
        partial void OnCreated();
        
        #endregion
        /// <summary>
        /// There are no comments for Vehicle constructor in the schema.
        /// </summary>
        public Vehicle()
        {
            OnCreated();
        }

    
        /// <summary>
        /// There are no comments for VehPlate in the schema.
        /// </summary>
        public virtual string VehPlate
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for VehType in the schema.
        /// </summary>
        public virtual string VehType
        {
            get;
            set;
        }
    }

}