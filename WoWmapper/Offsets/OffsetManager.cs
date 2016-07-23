using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Farplane.Memory;
using WoWmapper.Classes;

namespace WoWmapper.Offsets
{
    public class OffsetManager
    {
        public static OffsetData Offset;
        public static bool OffsetsAvailable = false;

        private static IntPtr GetOffset(OffsetType type)
        {
            switch (type)
            {
                case OffsetType.PlayerClass:
                {
                    var offset = OffsetScanner.FindClassOffset();
                    return offset;
                }
                case OffsetType.GameState:
                {
                    var offset = OffsetScanner.FindGameState();
                    return offset;
                }
                case OffsetType.PlayerName:
                {
                    var offset = OffsetScanner.FindPlayerName();
                    return offset;
                }
                case OffsetType.MouseLook:
                {
                    var offset = OffsetScanner.FindMouseLook();
                    return offset;
                }
                case OffsetType.PlayerWalk:
                {
                    var offset = OffsetScanner.FindPlayerWalk();
                    return offset;
                }
                case OffsetType.GameBuild:
                {
                    var offset = OffsetScanner.FindGameBuild();
                    return offset;
                }
                case OffsetType.PlayerBase:
                {
                    var offset = OffsetScanner.FindPlayerBase();
                    return offset;
                }
                case OffsetType.PlayerAOE:
                    {
                        var offset = OffsetScanner.FindPlayerAOE();
                        return offset;
                    }
                case OffsetType.PlayerTarget:
                    {
                        var offset = OffsetScanner.FindPlayerTarget();
                        return offset;
                    }
                default:
                    return IntPtr.Zero;
            }
        }

        public static void Clear()
        {
            OffsetsAvailable = false;
            OffsetScanner.Reset();
            Offset = null;
        }

        public static void InitializeOffsets()
        {
            OffsetsAvailable = false;
            OffsetScanner.Reset();
            Offset = new OffsetData()
            {
                GameState = GetOffset(OffsetType.GameState),
                PlayerName = GetOffset(OffsetType.PlayerName),
                PlayerClass = GetOffset(OffsetType.PlayerClass),
                PlayerBase = GetOffset(OffsetType.PlayerBase),
                MouseLook = GetOffset(OffsetType.MouseLook),
                PlayerWalk = GetOffset(OffsetType.PlayerWalk),
                PlayerAOE = GetOffset(OffsetType.PlayerAOE),
            };
            OffsetsAvailable = true;
        }

        public class OffsetData
        {
            public IntPtr PlayerClass;
            public IntPtr GameState;
            public IntPtr PlayerName;
            public IntPtr PlayerBase;
            public IntPtr MouseLook;
            public IntPtr PlayerWalk;
            public IntPtr PlayerAOE;
            public IntPtr PlayerTarget;
        }
    }
}