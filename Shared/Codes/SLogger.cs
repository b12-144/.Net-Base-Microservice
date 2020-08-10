#region Imports
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text; 
#endregion

namespace Shared.Codes {
    public class SLogger {

        #region LogError
        static public void LogError(string msg) {
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }
        #endregion

        #region LogDebug
        static public void LogDebug(string msg) {
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }
        #endregion

        #region LogError
        static public void LogError(Exception ex) {
            Console.WriteLine(ex);
            Debug.WriteLine(ex);
        } 
        #endregion
    }
}
