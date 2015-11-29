using System;

namespace ConsolePort.WoWData
{
    public static class Offsets
    {
        public static IntPtr GameState = (IntPtr)0x16A179E;
        public static IntPtr GameBuild = (IntPtr)0xFE20B4;

        public static class Player
        {
            public static IntPtr LocalPlayer = (IntPtr)0x01606320;
            public static IntPtr LocalCont = (IntPtr)0x13CC73C;
            public static IntPtr LocalZone = (IntPtr)0x13E1674;
            public static IntPtr IsLooting = (IntPtr)0x1715264;
            public static IntPtr IsTexting = (IntPtr)0x1454030;
            public static IntPtr Name = (IntPtr)0x17EE040;
            public static IntPtr Class = (IntPtr)0x17EE1D5;
            public static IntPtr RealmName = (IntPtr)0x17EE1F6;
            public static IntPtr AccountName = (IntPtr)0x17ED6C0;
        }

        //General
        //=======
        //GameHash	= 0D3C6997
        //IconHash = A118EC28
        //GameBuild	= 0FE20B4
        //GameState = 16A179E

        //Camera
        //======
        //CameraStruct	= 16A2250
        //CameraOffset = 7768
        //CameraOrigin	= 10
        //CameraMatrix	= 1C
        //CameraFov = 40

        //Player
        //======
        //LocalPlayer	= 1606320
        //LocalCont	= 13CC73C
        //LocalZone = 13E1674
        //IsLooting	= 1715264
        //IsTexting	= 1454030
        // MouseGuid	= 16A1DD8
        //TargetGuid = 16A1E08

        //Entity List
        //===========
        //EntityList	= 14E4DC0
        //FirstEntity = 18
        // NextEntity	= 68

        //EntityType	= 18
        //Descriptors	= 08
        //GlobalID	= 00
        //EntityID	= 24
        //DynFlags	= 28

        //Unit
        //====
        //UnitTransport	= 1538
        //UnitOrigin	= 1548
        //UnitAngle	= 1558
        //UnitCasting	= 1B98
        //UnitChannel = 1BB8

        //UnitCreator = 080
        //UnitHealth	= 0F0
        //UnitPower	= 0F4
        //UnitHealthMax	= 10C
        //UnitPowerMax = 110
        //UnitLevel	= 158
        //UnitFlags	= 17C

        //PlayerMoney1 = 2790
        //PlayerMoney2	= 1890
        //PlayerArch	= 2798
        //PlayerArchCount	= 08
        //PlayerArchSites	= 18

        //NpcCache	= 16F0
        //NpcName		= 00A0

        //Object
        //======
        //ObjectBobbing	= 1E0
        //ObjectTransport	= 238
        //ObjectOrigin	= 248
        //ObjectRotation	= 258
        //ObjectTransform	= 4A0
        //ObjectCache = 498
        //ObjectName	= 0D8

        //ObjectCreator	= 030
        //ObjectDisplay	= 040

        //Name Cache
        //==========
        //NameCacheBase	= 14C2818
        //NameCacheNext = 00
        //NameCacheGuid	= 20
        //NameCacheName	= 31
        //NameCacheRace	= 88
        //NameCacheClass	= 90

        //Chat System
        //===========
        //ChatPosition	= 16FE08C
        //ChatBuffer = 16A3B30
        //ChatMsgSize = 17F0

        //Message
        //=======
        //MsgSenderGuid	= 0000
        //MsgSenderName	= 0034
        //MsgFullMessage	= 0065
        //MsgOnlyMessage	= 0C1D
        //MsgChannelNum = 17D8
        //MsgTimeStamp	= 17E8
    }
}