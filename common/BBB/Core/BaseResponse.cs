/**
 * Author: dyx1001
 * Email: dyx1001@126.com
 * License: MIT
 * Git URL: https://github.com/dyx1001/BigBlueButtonAPI.NET
 */
using BigBlueButtonAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BigBlueButtonAPI.Core
{
    /// <summary>
    /// The following response parameters are standard to every call and may be returned from any call.
    /// </summary>
    [XmlRoot("response")]
    public class BaseResponse: IBaseResponse
    {
        /// <summary>
        /// Indicates whether the intended function was successful or not.
        /// </summary>
        public Returncode Returncode { get; set; }

        /// <summary>
        /// Provides similar functionality to the Message and follows the same rules. 
        /// However, a Message key will be much shorter and will generally remain the same for the life of the API whereas a Message may change over time. 
        /// If your third party application would like to internationalize or otherwise change the standard messages returned, you can look up your own custom messages based on this MessageKey.
        /// </summary>
        public string MessageKey { get; set; }

        /// <summary>
        /// A Message that gives additional information about the status of the call. 
        /// A Message parameter will always be returned if the Returncode was Failed. 
        /// A Message may also be returned in some cases where Returncode was Success if additional information would be helpful.
        /// </summary>
        public string Message { get; set; }
    }
}