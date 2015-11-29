/**************************************************************************************
 *	File:		 ExceptionExtensions.cs
 *	Description: Extension methods for the System.Exception data type.
 *
 *
 *	Author:		 infloper@gmail.com
 *	Created:	 6/14/2014 5:52:30 PM
 *	CLR ver:	 4.0.30319.18444
 *
 **************************************************************************************
 * Changes history.
 **************************************************************************************
 * Date:		Author:				  Description:
 * --------		--------------------
 **************************************************************************************/

using System;
using System.Collections;
using System.Text;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     Extension methods for the <see cref="System.Exception" /> data type.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Fields, Private Properties

        /// <summary>
        /// </summary>
        private static int _exceptionLevel;

        #endregion Fields, Private Properties

        /// <summary>
        ///     Returns full information about the exception
        /// </summary>
        /// <param name="ex">Thrown an exception to be reported.</param>
        /// <param name="htmlFormatted">Replace newline to HTML tag. Default <c>false</c>. </param>
        /// <returns></returns>
        public static string ExtractDetailedInformation(this Exception ex, bool htmlFormatted = false)
        {
            var sb = new StringBuilder();
            var boldFontTagOpen = htmlFormatted ? "<b>" : "";
            var boldFontTagClose = htmlFormatted ? "</b>" : "";

            _exceptionLevel++;
            var indent = new string('\t', _exceptionLevel - 1);
            sb.AppendFormat("{3}{0}*** Exception level {1} *************************************************{4}{2}",
                indent, _exceptionLevel, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}ExceptionType:{4} {1}{2}", indent, ex.GetType().Name, Environment.NewLine,
                boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}HelpLink:{4} {1}{2}", indent, ex.HelpLink, Environment.NewLine, boldFontTagOpen,
                boldFontTagClose);
            sb.AppendFormat("{3}{0}Message:{4} {1}{2}", indent, ex.Message, Environment.NewLine, boldFontTagOpen,
                boldFontTagClose);
            sb.AppendFormat("{3}{0}Source:{4} {1}{2}", indent, ex.Source, Environment.NewLine, boldFontTagOpen,
                boldFontTagClose);
            sb.AppendFormat("{3}{0}StackTrace:{4} {1}{2}", indent, ex.StackTrace, Environment.NewLine, boldFontTagOpen,
                boldFontTagClose);
            sb.AppendFormat("{3}{0}TargetSite:{4} {1}{2}", indent, ex.TargetSite, Environment.NewLine, boldFontTagOpen,
                boldFontTagClose);

            if (ex.Data.Count > 0)
            {
                sb.AppendFormat("{2}{0}Data:{3}{1}", indent, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
                foreach (DictionaryEntry de in ex.Data)
                    sb.AppendFormat("{0}\t{1} : {2}", indent, de.Key, de.Value);
            }

            var innerException = ex.InnerException;

            while (innerException != null)
            {
                sb.Append(innerException.ExtractDetailedInformation());
                innerException = _exceptionLevel > 1 ? innerException?.InnerException : null;
            }

            _exceptionLevel--;

            var result = htmlFormatted ? sb.ToString().Replace(Environment.NewLine, "<br />") : sb.ToString();

            return result;
        }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The Exception Message text</returns>
        public static string GetErrorMessage(this Exception exception)
        {
            if (exception == null) return string.Empty;
            return exception.InnerException?.Message ?? exception.Message;
        }

        /// <summary>
        ///     Gets the error source.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>the exception source text</returns>
        public static string GetErrorSource(this Exception exception)
        {
            if (exception == null) return string.Empty;
            return exception.InnerException != null ? exception.InnerException.Source : exception.Source;
        }

        /// <summary>
        ///     Gets the name of the error type.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>the exception type name text</returns>
        public static string GetErrorTypeName(this Exception exception)
        {
            if (exception == null) return string.Empty;
            return exception.InnerException != null
                ? exception.InnerException.GetType().FullName
                : exception.GetType().FullName;
        }

        /// <summary>
        ///     Gets the error error details.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>the exception detail text</returns>
        public static string GetErrorErrorDetails(this Exception exception)
        {
            if (exception == null) return string.Empty;
            return exception.InnerException?.ToString() ?? exception.ToString();
        }
    }
}