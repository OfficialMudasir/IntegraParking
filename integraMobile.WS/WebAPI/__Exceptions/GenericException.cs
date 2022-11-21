using System;

namespace integraMobile.WS.WebAPI.Exceptions
{
    [Serializable]
    public class GenericException : Exception
    {
        #region Constructor

        public GenericException(string message) : base(message) { }

        #endregion
    }
}