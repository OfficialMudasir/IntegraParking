using integraMobile.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.WS.Tools;

namespace integraMobile.WS.Entity
{
    public class ServicesPhotosEntity: SERVICES_PHOTO
    {
        #region Properties
        public string Image { get; set; }
        #endregion
        #region constructor
        public ServicesPhotosEntity()
        {
        }

        public ServicesPhotosEntity(PhotoEntity photo)
        {
            Image = photo.Image;
            decimal? dNumber = Helpers.ValidateStringToDecimal(photo.Number);
            if(dNumber.HasValue)
            {
                SERPHO_NUMBER = dNumber.Value;
            }
        }
        #endregion

    }
}