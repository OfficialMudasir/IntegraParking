using System;

namespace integraMobile.WS.WebAPI.Exceptions
{
    [Serializable]
    public class InvalidAuthenticationHashException : Exception
    {
        #region Constructor

        public InvalidAuthenticationHashException(string message) 
            : base(message) { }

        #endregion
    }
}