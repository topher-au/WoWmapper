using System;
using System.Collections.Generic;
using System.Diagnostics;
using Binarysharp.MemoryManagement.Core.Patterns;
using Binarysharp.MemoryManagement.Core.Patterns.Objects;
using Binarysharp.MemoryManagement.Models.Memory;

namespace Binarysharp.MemoryManagement.Models.Modules
{
    public class PatternScanner
    {
        #region Fields, Private Properties
        /// <summary>
        ///     The field for storing the modules data once dumped.
        /// </summary>
        private byte[] _moduleData;
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="PatternScanner" /> class.
        /// </summary>
        /// <param name="processMemory">The process the <see cref="ProcessMemory" /> reference for this instance.</param>
        /// <param name="processModule">The the process module that contains the data to match patterns with.</param>
        public PatternScanner(ProcessMemory processMemory, ProcessModule processModule)
        {
            ProcessMemory = processMemory;
            ProcessModule = processModule;
        }
        #endregion

        #region Public Properties, Indexers
        public ProcessMemory ProcessMemory { get; }

        /// <summary>
        ///     A dump of the modules data as a byte array.
        /// </summary>
        public byte[] ModuleData
            =>
                _moduleData ??
                (_moduleData = ProcessMemory.ReadBytes(ProcessModule.BaseAddress, ProcessModule.ModuleMemorySize));

        /// <summary>
        ///     Gets the process module reference for this instance.
        /// </summary>
        /// <value>The process module.</value>
        public ProcessModule ProcessModule { get; set; }
        #endregion

        /// <summary>
        ///     Adds all pointers found from scanning a xml file to a given dictonary using the <code>IDictonary</code> interface.
        /// </summary>
        /// <param name="xmlFileNameOrPath">The name or path to the xml ProcessModulePattern file to use.</param>
        /// <param name="thePointerDictionary">The dictonary to fill.</param>
        public void CollectXmlScanResults(string xmlFileNameOrPath, IDictionary<string, IntPtr> thePointerDictionary)
        {
            var patterns = PatternCore.LoadXmlPatternFile(xmlFileNameOrPath);
            foreach (var pattern in patterns)
            {
                thePointerDictionary.Add(pattern.Description, Find(pattern).Address);
            }
        }

        /// <summary>
        ///     Performs a pattern scan from the data inside the <see cref="SerializablePattern" /> instance supplied in the
        ///     parameter.
        /// </summary>
        /// m>
        /// <param name="pattern">The <see cref="SerializablePattern" /> Instance containing the data to use.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(SerializablePattern pattern)
        {
            var bytes =
                PatternCore.GetBytesFromDwordPattern(pattern.TextPattern);
            var mask = PatternCore.GetMaskFromDwordPattern(pattern.TextPattern);
            return Find(bytes, mask, pattern.OffsetToAdd, pattern.IsOffsetMode, pattern.RebaseAddress);
        }

        /// <summary>
        ///     Performs a pattern scan.
        /// </summary>
        /// m>
        /// <param name="pattern">The <see cref="Pattern" /> Instance containing the data to use.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(Pattern pattern)
        {
            var bytes =
                PatternCore.GetBytesFromDwordPattern(pattern.TextPattern);
            var mask = PatternCore.GetMaskFromDwordPattern(pattern.TextPattern);
            return Find(bytes, mask, pattern.OffsetToAdd, pattern.IsOffsetMode, pattern.RebaseAddress);
        }

        /// <summary>
        ///     Performs a pattern scan.
        /// </summary>
        /// m>
        /// <param name="patternText">
        ///     The dword formatted text of the pattern.
        ///     <example>A2 5B ?? ?? ?? A2</example>
        /// </param>
        /// <param name="offsetToAdd">The offset to add to the offset result found from the pattern.</param>
        /// <param name="isOffsetMode">If the address is found from the base address + offset or not.</param>
        /// <param name="reBase">If the address should be rebased to this <see cref="RemoteModule" /> Instance's base address.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(string patternText, int offsetToAdd, bool isOffsetMode, bool reBase)
        {
            var bytes = PatternCore.GetBytesFromDwordPattern(patternText);
            var mask = PatternCore.GetMaskFromDwordPattern(patternText);
            return Find(bytes, mask, offsetToAdd, isOffsetMode, reBase);
        }

        /// <summary>
        ///     Preformpattern scan from byte[]
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="offsetToAdd"></param>
        /// <param name="isOffsetMode"></param>
        /// <param name="reBase"></param>
        /// <returns></returns>
        public ScanResult Find(byte[] pattern, int offsetToAdd, bool isOffsetMode, bool reBase)
        {
            var bytes = pattern;
            var mask = MaskFromPattern(pattern);
            return Find(bytes, mask, offsetToAdd, isOffsetMode, reBase);
        }


        /// <summary>
        ///     Performs a pattern scan.
        /// </summary>
        /// <param name="myPattern">The patterns bytes.</param>
        /// <param name="mask">The mask of the pattern. ? Is for wild card, x otherwise.</param>
        /// <param name="offsetToAdd">The offset to add to the offset result found from the pattern.</param>
        /// <param name="isOffsetMode">If the address is found from the base address + offset or not.</param>
        /// <param name="reBase">If the address should be rebased to this <see cref="RemoteModule" /> Instance's base address.</param>
        /// <returns>A new <see cref="ScanResult" /></returns>
        public ScanResult Find(byte[] myPattern, string mask, int offsetToAdd, bool isOffsetMode, bool reBase)
        {
            var patternBytes = myPattern;
            var patternMask = mask;
            var scanResult = PatternCore.Find(ProcessMemory.Handle, ProcessModule, patternBytes, patternMask,
                offsetToAdd,
                isOffsetMode, reBase);
            return scanResult;
        }


        /// <summary>
        ///     Creates a mask from a given pattern, using the given chars
        /// </summary>
        /// <param name="pattern">The pattern this functions designs a mask for</param>
        /// <param name="wildcardByte">Byte that is interpreted as a wildcard</param>
        /// <param name="wildcardChar">Char that is used as wildcard</param>
        /// <param name="matchChar">Char that is no wildcard</param>
        /// <returns></returns>
        public static string MaskFromPattern(byte[] pattern, byte wildcardByte = 0, char wildcardChar = '?',
                                             char matchChar = 'x')
        {
            var chr = new char[pattern.Length];
            for (var i = 0; i < chr.Length; i++)
            {
                chr[i] = pattern[i] == wildcardByte ? wildcardChar : matchChar;
            }
            return new string(chr);
        }
    }
}