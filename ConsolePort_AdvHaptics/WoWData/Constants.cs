using System;

namespace ConsolePort.WoWData
{
    public class Constants
    {
        public enum ShapeshiftForm
        {
            Normal = 0,
            Cat = 1,
            TreeOfLife = 2,
            Travel = 3,
            Aqua = 4,
            Bear = 5,
            Ambient = 6,
            Ghoul = 7,
            DireBear = 8,
            CreatureBear = 14,
            CreatureCat = 15,
            GhostWolf = 16,
            BattleStance = 17,
            DefensiveStance = 18,
            BerserkerStance = 19,
            EpicFlightForm = 27,
            Shadow = 28,
            Stealth = 30,
            Moonkin = 31,
            SpiritOfRedemption = 32
        }

        public enum StandState : byte
        {
            Stand = 0,
            Sit = 1,
            SittingInChair = 2,
            Sleeping = 3,
            SittingInLowChair = 4,
            SittingInMediumChair = 5,
            SittingInHighChair = 6,
            Dead = 7,
            Kneeling = 8,
            Type9 = 9,
        }

        [Flags]
        public enum PvPState
        {
            None = 0,
            PVP = 0x1,
            FFAPVP = 0x4,
            InPvPSanctuary = 0x8,
        }

        public enum WoWItemType
        {
            Consumable,
            Container,
            Weapon,
            Gem,
            Armor,
            Reagent,
            Projectile,
            TradeGoods,
            Generic,
            Recipe,
            Money,
            Quiver,
            QUest,
            Key,
            Permanent,
            Misc
        }

        public enum WoWPowerType
        {
            Mana,
            Rage,
            Focus,
            Energy,
            Happiness,
            RunicPower,
            Runes,
            Health,
            UNKNOWN
        }

        [Flags]
        public enum WoWObjectTypeFlag
        {
            Object = 0x1,
            Item = 0x2,
            Container = 0x4,
            Unit = 0x8,
            Player = 0x10,
            GameObject = 0x20,
            DynamicObject = 0x40,
            Corpse = 0x80,
            AiGroup = 0x100,
            AreaTrigger = 0x200,
        }

        public enum WoWObjectType : uint
        {
            Object = 0,
            Item = 1,
            Container = 2,
            Unit = 3,
            Player = 4,
            GameObject = 5,
            DynamicObject = 6,
            Corpse = 7,
            AiGroup = 8,
            AreaTrigger = 9
        }

        public enum WoWGameObjectType : uint
        {
            Door = 0,
            Button = 1,
            QuestGiver = 2,
            Chest = 3,
            Binder = 4,
            Generic = 5,
            Trap = 6,
            Chair = 7,
            SpellFocus = 8,
            Text = 9,
            Goober = 0xa,
            Transport = 0xb,
            AreaDamage = 0xc,
            Camera = 0xd,
            WorldObj = 0xe,
            MapObjTransport = 0xf,
            DuelArbiter = 0x10,
            FishingNode = 0x11,
            Ritual = 0x12,
            Mailbox = 0x13,
            AuctionHouse = 0x14,
            SpellCaster = 0x16,
            MeetingStone = 0x17,
            Unkown18 = 0x18,
            FishingPool = 0x19,
            FORCEDWORD = 0xFFFFFFFF,
        }

        public enum WoWEquipSlot
        {
            Head = 0,
            Neck,
            Shoulders,
            Body,
            Chest,
            Waist,
            Legs,
            Feet,
            Wrists,
            Hands,
            Finger1,
            Finger2,
            Trinket1,
            Trinket2,
            Back,
            MainHand,
            OffHand,
            Ranged,
            Tabard
        }

        public enum WoWClass : uint
        {
            None = 0,
            Warrior = 1,
            Paladin = 2,
            Hunter = 3,
            Rogue = 4,
            Priest = 5,
            DeathKnight = 6,
            Shaman = 7,
            Mage = 8,
            Warlock = 9,
            Druid = 11,
        }

        public enum WoWClassification
        {
            Normal = 0,
            Elite = 1,
            RareElite = 2,
            WorldBoss = 3,
            Rare = 4
        }

        public enum WoWRace
        {
            Human = 1,
            Orc,
            Dwarf,
            NightElf,
            Undead,
            Tauren,
            Gnome,
            Troll,
            Goblin,
            BloodElf,
            Draenei,
            FelOrc,
            Naga,
            Broken,
            Skeleton = 15,
        }

        public enum WoWCreatureType
        {
            Unknown = 0,
            Beast,
            Dragon,
            Demon,
            Elemental,
            Giant,
            Undead,
            Humanoid,
            Critter,
            Mechanical,
            NotSpecified,
            Totem,
            NonCombatPet,
            GasCloud
        }

        public enum WoWGender
        {
            Male,
            Female,
            Unknown
        }

        public enum WoWUnitRelation : uint
        {
            Hated = 0,
            Hostile = 1,
            Unfriendly = 2,
            Neutral = 3,
            Friendly = 4,
        }

        public enum WoWDispelType : uint
        {
            None = 0,
            Magic = 1,
            Curse = 2,
            Disease = 3,
            Poison = 4,
            Stealth = 5,
            Invisibility = 6,
            All = 7,

            //Special_NpcOnly=8,
            Enrage = 9,

            //ZgTrinkets=10
        }

        public enum WoWQuestType : uint
        {
            Group = 1,
            Life = 21,
            PvP = 41,
            Raid = 62,
            Dungeon = 81,
            WorldEvent = 82,
            Legendary = 83,
            Escort = 84,
            Heroic = 85,
            Raid_10 = 88,
            Raid_25 = 89,
        }

        public enum SheathType : sbyte
        {
            Undetermined = -1,
            None = 0,
            Melee = 1,
            Ranged = 2,

            Shield = 4,
            Rod = 5,
            Light = 7
        }

        /// <summary>
        /// The direction of movement in WoW as per the CGInputControl_ToggleControlBit function.
        /// These are actually the flags that are set/unset!
        /// </summary>
        [Flags]
        public enum MovementDirection : uint
        {
            None = 0,
            RMouse = (1 << 0), // 0x1,
            LMouse = (1 << 1), // 0x2,

                               // 2 and 3 not used apparently. Possibly for flag masking?
            Forward = (1 << 4), // 0x10,

            Backward = (1 << 5), // 0x20,
            StrafeLeft = (1 << 6), // 0x40,
            StrafeRight = (1 << 7), // 0x80,
            TurnLeft = (1 << 8), // 0x100,
            TurnRight = (1 << 9), // 0x200,
            PitchUp = (1 << 10), // 0x400, For flying/swimming
            PitchDown = (1 << 11), // 0x800, For flying/swimming
            AutoRun = (1 << 12), // 0x1000,
            JumpAscend = (1 << 13), // 0x2000, For flying/swimming
            Descend = (1 << 14), // 0x4000, For flying/swimming

            ClickToMove = (1 << 22), // 0x400000, Note: Only turns the CTM flag on or off. Has no effect on movement!

            // 25 used somewhere. Can't figure out what for. Checked in Lua_IsMouseTurning. Possible camera turn?
            // Or mouse input flag? (Flag used: 0x2000001)
        }

        [Flags]
        internal enum MovementFlags
        {
            Forward = 0x1,
            Backward = 0x2,
            StrafeLeft = 0x4,
            StrafeRight = 0x8,

            StrafeMask = StrafeLeft | StrafeRight,

            Left = 0x10,
            Right = 0x20,

            TurnMask = Left | Right,

            MoveMask = Forward | Backward | StrafeMask | TurnMask,

            PitchUp = 0x40,
            PitchDown = 0x80,
            Walk = 0x100,
            TimeValid = 0x200,
            Immobilized = 0x400,
            DontCollide = 0x800,

            // JUMPING
            Redirected = 0x1000,

            Rooted = 0x2000,
            Falling = 0x4000,
            FallenFar = 0x8000,
            PendingStop = 0x10000,
            Pendingunstrafe = 0x20000,
            Pendingfall = 0x40000,
            Pendingforward = 0x80000,
            PendingBackward = 0x100000,
            PendingStrafeLeft = 0x200000,
            PendingStrafeRght = 0x400000,
            PendMoveMask = 0x180000,
            PendStrafeMask = 0x600000,
            PendingMask = 0x7f0000,
            Moved = 0x800000,
            Sliding = 0x1000000,
            Swimming = 0x2000000,
            SplineMover = 0x4000000,
            SpeedDirty = 0x8000000,
            Halted = 0x10000000,
            Nudge = 0x20000000,

            FallMask = 0x100c000,
            Local = 0x500f400,
            PitchMask = 0xc0,
            MotionMask = 0xff,
            StoppedMask = 0x3100f,
        }

        [Flags]
        private enum StateFlag
        {
            None = 0,
            AlwaysStand = 0x1,
            Sneaking = 0x2,
            UnTrackable = 0x4,
        }

        [Flags]
        private enum UnitDynamicFlags
        {
            None = 0,
            Lootable = 0x1,
            TrackUnit = 0x2,
            TaggedByOther = 0x4,
            TaggedByMe = 0x8,
            SpecialInfo = 0x10,
            Dead = 0x20,
            ReferAFriendLinked = 0x40,
            IsTappedByAllThreatList = 0x80,
        }

        [Flags]
        private enum UnitFlags : uint
        {
            None = 0,
            Sitting = 0x1,

            //SelectableNotAttackable_1 = 0x2,
            Influenced = 0x4, // Stops movement packets

            PlayerControlled = 0x8, // 2.4.2
            Totem = 0x10,
            Preparation = 0x20, // 3.0.3
            PlusMob = 0x40, // 3.0.2

            //SelectableNotAttackable_2 = 0x80,
            NotAttackable = 0x100,

            //Flag_0x200 = 0x200,
            Looting = 0x400,

            PetInCombat = 0x800, // 3.0.2
            PvPFlagged = 0x1000,
            Silenced = 0x2000, //3.0.3

            //Flag_14_0x4000 = 0x4000,
            //Flag_15_0x8000 = 0x8000,
            //SelectableNotAttackable_3 = 0x10000,
            Pacified = 0x20000, //3.0.3

            Stunned = 0x40000,
            CanPerformAction_Mask1 = 0x60000,
            Combat = 0x80000, // 3.1.1
            TaxiFlight = 0x100000, // 3.1.1
            Disarmed = 0x200000, // 3.1.1
            Confused = 0x400000, //  3.0.3
            Fleeing = 0x800000,
            Possessed = 0x1000000, // 3.1.1
            NotSelectable = 0x2000000,
            Skinnable = 0x4000000,
            Mounted = 0x8000000,

            //Flag_28_0x10000000 = 0x10000000,
            Dazed = 0x20000000,

            Sheathe = 0x40000000,
            //Flag_31_0x80000000 = 0x80000000,
        }

        [Flags]
        private enum UnitFlags2
        {
            FeignDeath = 0x1,
            NoModel = 0x2,
            Flag_0x4 = 0x4,
            Flag_0x8 = 0x8,
            Flag_0x10 = 0x10,
            Flag_0x20 = 0x20,
            ForceAutoRunForward = 0x40,

            /// <summary>
            /// Treat as disarmed?
            /// Treat main and off hand weapons as not being equipped?
            /// </summary>
            Flag_0x80 = 0x80,

            /// <summary>
            /// Skip checks on ranged weapon?
            /// Treat it as not being equipped?
            /// </summary>
            Flag_0x400 = 0x400,

            Flag_0x800 = 0x800,
            Flag_0x1000 = 0x1000,
        }

        private enum UnitNPCFlags
        {
            UNIT_NPC_FLAG_NONE = 0x00000000,
            UNIT_NPC_FLAG_GOSSIP = 0x00000001, // 100%
            UNIT_NPC_FLAG_QUESTGIVER = 0x00000002, // guessed, probably ok
            UNIT_NPC_FLAG_UNK1 = 0x00000004,
            UNIT_NPC_FLAG_UNK2 = 0x00000008,
            UNIT_NPC_FLAG_TRAINER = 0x00000010, // 100%
            UNIT_NPC_FLAG_TRAINER_CLASS = 0x00000020, // 100%
            UNIT_NPC_FLAG_TRAINER_PROFESSION = 0x00000040, // 100%
            UNIT_NPC_FLAG_VENDOR = 0x00000080, // 100%
            UNIT_NPC_FLAG_VENDOR_AMMO = 0x00000100, // 100%, general goods vendor
            UNIT_NPC_FLAG_VENDOR_FOOD = 0x00000200, // 100%
            UNIT_NPC_FLAG_VENDOR_POISON = 0x00000400, // guessed
            UNIT_NPC_FLAG_VENDOR_REAGENT = 0x00000800, // 100%
            UNIT_NPC_FLAG_REPAIR = 0x00001000, // 100%
            UNIT_NPC_FLAG_FLIGHTMASTER = 0x00002000, // 100%
            UNIT_NPC_FLAG_SPIRITHEALER = 0x00004000, // guessed
            UNIT_NPC_FLAG_SPIRITGUIDE = 0x00008000, // guessed
            UNIT_NPC_FLAG_INNKEEPER = 0x00010000, // 100%
            UNIT_NPC_FLAG_BANKER = 0x00020000, // 100%
            UNIT_NPC_FLAG_PETITIONER = 0x00040000, // 100% 0xC0000 = guild petitions, 0x40000 = arena team petitions
            UNIT_NPC_FLAG_TABARDDESIGNER = 0x00080000, // 100%
            UNIT_NPC_FLAG_BATTLEMASTER = 0x00100000, // 100%
            UNIT_NPC_FLAG_AUCTIONEER = 0x00200000, // 100%
            UNIT_NPC_FLAG_STABLEMASTER = 0x00400000, // 100%
            UNIT_NPC_FLAG_GUILD_BANKER = 0x00800000, // cause client to send 997 opcode
            UNIT_NPC_FLAG_SPELLCLICK = 0x01000000, // cause client to send 1015 opcode (spell click)
            UNIT_NPC_FLAG_GUARD = 0x10000000, // custom flag for guards
        }

        [Flags]
        public enum GameObjectFlags // :ushort
        {
            /// <summary>
            /// 0x1
            /// Disables interaction while animated
            /// </summary>
            InUse = 0x01,

            /// <summary>
            /// 0x2
            /// Requires a key, spell, event, etc to be opened.
            /// Makes "Locked" appear in tooltip
            /// </summary>
            Locked = 0x02,

            /// <summary>
            /// 0x4
            /// Objects that require a condition to be met before they are usable
            /// </summary>
            ConditionalInteraction = 0x04,

            /// <summary>
            /// 0x8
            /// any kind of transport? Object can transport (elevator, boat, car)
            /// </summary>
            Transport = 0x08,

            GOFlag_0x10 = 0x10,

            /// <summary>
            /// 0x20
            /// These objects never de-spawn, but typically just change state in response to an event
            /// Ex: doors
            /// </summary>
            DoesNotDespawn = 0x20,

            /// <summary>
            /// 0x40
            /// Typically, summoned objects. Triggered by spell or other events
            /// </summary>
            Triggered = 0x40,

            GOFlag_0x80 = 0x80,
            GOFlag_0x100 = 0x100,
            GOFlag_0x200 = 0x200,
            GOFlag_0x400 = 0x400,
            GOFlag_0x800 = 0x800,
            GOFlag_0x1000 = 0x1000,
            GOFlag_0x2000 = 0x2000,
            GOFlag_0x4000 = 0x4000,
            GOFlag_0x8000 = 0x8000,

            Flag_0x10000 = 0x10000,
            Flag_0x20000 = 0x20000,
            Flag_0x40000 = 0x40000,
        }

        private enum CorpseFlags
        {
            CORPSE_FLAG_NONE = 0x00,
            CORPSE_FLAG_BONES = 0x01,
            CORPSE_FLAG_UNK1 = 0x02,
            CORPSE_FLAG_UNK2 = 0x04,
            CORPSE_FLAG_HIDE_HELM = 0x08,
            CORPSE_FLAG_HIDE_CLOAK = 0x10,
            CORPSE_FLAG_LOOTABLE = 0x20
        }

        public enum PlayerDataType
        {
            Charm = 0x30, // Size: 0x4, Flags: 0x1
            Summon = 0x40, // Size: 0x4, Flags: 0x1
            Critter = 0x50, // Size: 0x4, Flags: 0x2
            CharmedBy = 0x60, // Size: 0x4, Flags: 0x1
            SummonedBy = 0x70, // Size: 0x4, Flags: 0x1
            CreatedBy = 0x80, // Size: 0x4, Flags: 0x1
            DemonCreator = 0x90, // Size: 0x4, Flags: 0x1
            Target = 0xA0, // Size: 0x4, Flags: 0x1
            BattlePetCompanionGUID = 0xB0, // Size: 0x4, Flags: 0x1
            BattlePetDBID = 0xC0, // Size: 0x2, Flags: 0x1
            ChannelObject = 0xC8, // Size: 0x4, Flags: 0x201
            ChannelSpell = 0xD8, // Size: 0x1, Flags: 0x201
            ChannelSpellXSpellVisual = 0xDC, // Size: 0x1, Flags: 0x201
            SummonedByHomeRealm = 0xE0, // Size: 0x1, Flags: 0x1
            Sex = 0xE4, // Size: 0x1, Flags: 0x1
            DisplayPower = 0xE8, // Size: 0x1, Flags: 0x1
            OverrideDisplayPowerID = 0xEC, // Size: 0x1, Flags: 0x1
            Health = 0xF0, // Size: 0x1, Flags: 0x1
            Power = 0xF4, // Size: 0x6, Flags: 0x401
            MaxHealth = 0x10C, // Size: 0x1, Flags: 0x1
            MaxPower = 0x110, // Size: 0x6, Flags: 0x1
            PowerRegenFlatModifier = 0x128, // Size: 0x6, Flags: 0x46
            PowerRegenInterruptedFlatModifier = 0x140, // Size: 0x6, Flags: 0x46
            Level = 0x158, // Size: 0x1, Flags: 0x1
            EffectiveLevel = 0x15C, // Size: 0x1, Flags: 0x1
            FactionTemplate = 0x160, // Size: 0x1, Flags: 0x1
            VirtualItems = 0x164, // Size: 0x6, Flags: 0x1
            Flags = 0x17C, // Size: 0x1, Flags: 0x1
            Flags2 = 0x180, // Size: 0x1, Flags: 0x1
            Flags3 = 0x184, // Size: 0x1, Flags: 0x1
            AuraState = 0x188, // Size: 0x1, Flags: 0x1
            AttackRoundBaseTime = 0x18C, // Size: 0x2, Flags: 0x1
            RangedAttackRoundBaseTime = 0x194, // Size: 0x1, Flags: 0x2
            BoundingRadius = 0x198, // Size: 0x1, Flags: 0x1
            CombatReach = 0x19C, // Size: 0x1, Flags: 0x1
            DisplayID = 0x1A0, // Size: 0x1, Flags: 0x280
            NativeDisplayID = 0x1A4, // Size: 0x1, Flags: 0x201
            MountDisplayID = 0x1A8, // Size: 0x1, Flags: 0x201
            MinDamage = 0x1AC, // Size: 0x1, Flags: 0x16
            MaxDamage = 0x1B0, // Size: 0x1, Flags: 0x16
            MinOffHandDamage = 0x1B4, // Size: 0x1, Flags: 0x16
            MaxOffHandDamage = 0x1B8, // Size: 0x1, Flags: 0x16
            AnimTier = 0x1BC, // Size: 0x1, Flags: 0x1
            PetNumber = 0x1C0, // Size: 0x1, Flags: 0x1
            PetNameTimestamp = 0x1C4, // Size: 0x1, Flags: 0x1
            PetExperience = 0x1C8, // Size: 0x1, Flags: 0x4
            PetNextLevelExperience = 0x1CC, // Size: 0x1, Flags: 0x4
            ModCastingSpeed = 0x1D0, // Size: 0x1, Flags: 0x1
            ModSpellHaste = 0x1D4, // Size: 0x1, Flags: 0x1
            ModHaste = 0x1D8, // Size: 0x1, Flags: 0x1
            ModRangedHaste = 0x1DC, // Size: 0x1, Flags: 0x1
            ModHasteRegen = 0x1E0, // Size: 0x1, Flags: 0x1
            CreatedBySpell = 0x1E4, // Size: 0x1, Flags: 0x1
            NpcFlags = 0x1E8, // Size: 0x2, Flags: 0x81
            EmoteState = 0x1F0, // Size: 0x1, Flags: 0x1
            Stats = 0x1F4, // Size: 0x5, Flags: 0x6
            StatPosBuff = 0x208, // Size: 0x5, Flags: 0x6
            StatNegBuff = 0x21C, // Size: 0x5, Flags: 0x6
            Resistances = 0x230, // Size: 0x7, Flags: 0x16
            ResistanceBuffModsPositive = 0x24C, // Size: 0x7, Flags: 0x6
            ResistanceBuffModsNegative = 0x268, // Size: 0x7, Flags: 0x6
            ModBonusArmor = 0x284, // Size: 0x1, Flags: 0x6
            BaseMana = 0x288, // Size: 0x1, Flags: 0x1
            BaseHealth = 0x28C, // Size: 0x1, Flags: 0x6
            ShapeshiftForm = 0x290, // Size: 0x1, Flags: 0x1
            AttackPower = 0x294, // Size: 0x1, Flags: 0x6
            AttackPowerModPos = 0x298, // Size: 0x1, Flags: 0x6
            AttackPowerModNeg = 0x29C, // Size: 0x1, Flags: 0x6
            AttackPowerMultiplier = 0x2A0, // Size: 0x1, Flags: 0x6
            RangedAttackPower = 0x2A4, // Size: 0x1, Flags: 0x6
            RangedAttackPowerModPos = 0x2A8, // Size: 0x1, Flags: 0x6
            RangedAttackPowerModNeg = 0x2AC, // Size: 0x1, Flags: 0x6
            RangedAttackPowerMultiplier = 0x2B0, // Size: 0x1, Flags: 0x6
            MinRangedDamage = 0x2B4, // Size: 0x1, Flags: 0x6
            MaxRangedDamage = 0x2B8, // Size: 0x1, Flags: 0x6
            PowerCostModifier = 0x2BC, // Size: 0x7, Flags: 0x6
            PowerCostMultiplier = 0x2D8, // Size: 0x7, Flags: 0x6
            MaxHealthModifier = 0x2F4, // Size: 0x1, Flags: 0x6
            HoverHeight = 0x2F8, // Size: 0x1, Flags: 0x1
            MinItemLevelCutoff = 0x2FC, // Size: 0x1, Flags: 0x1
            MinItemLevel = 0x300, // Size: 0x1, Flags: 0x1
            MaxItemLevel = 0x304, // Size: 0x1, Flags: 0x1
            WildBattlePetLevel = 0x308, // Size: 0x1, Flags: 0x1
            BattlePetCompanionNameTimestamp = 0x30C, // Size: 0x1, Flags: 0x1
            InteractSpellID = 0x310, // Size: 0x1, Flags: 0x1
            StateSpellVisualID = 0x314, // Size: 0x1, Flags: 0x280
            StateAnimID = 0x318, // Size: 0x1, Flags: 0x280
            StateAnimKitID = 0x31C, // Size: 0x1, Flags: 0x280
            StateWorldEffectID = 0x320, // Size: 0x4, Flags: 0x280
            ScaleDuration = 0x330, // Size: 0x1, Flags: 0x1
            LooksLikeMountID = 0x334, // Size: 0x1, Flags: 0x1
            LooksLikeCreatureID = 0x338, // Size: 0x1, Flags: 0x1
            LookAtControllerID = 0x33C, // Size: 0x1, Flags: 0x1
            LookAtControllerTarget = 0x340, // Size: 0x4, Flags: 0x1
            DuelArbiter = 0x350, // Size: 0x4, Flags: 0x1
            WowAccount = 0x360, // Size: 0x4, Flags: 0x1
            LootTargetGUID = 0x370, // Size: 0x4, Flags: 0x1
            PlayerFlags = 0x380, // Size: 0x1, Flags: 0x1
            PlayerFlagsEx = 0x384, // Size: 0x1, Flags: 0x1
            GuildRankID = 0x388, // Size: 0x1, Flags: 0x1
            GuildDeleteDate = 0x38C, // Size: 0x1, Flags: 0x1
            GuildLevel = 0x390, // Size: 0x1, Flags: 0x1
            HairColorID = 0x394, // Size: 0x1, Flags: 0x1
            RestState = 0x398, // Size: 0x1, Flags: 0x1
            ArenaFaction = 0x39C, // Size: 0x1, Flags: 0x1
            DuelTeam = 0x3A0, // Size: 0x1, Flags: 0x1
            GuildTimeStamp = 0x3A4, // Size: 0x1, Flags: 0x1
            QuestLog = 0x3A8, // Size: 0x2EE, Flags: 0x20
            VisibleItems = 0xF60, // Size: 0x26, Flags: 0x1
            PlayerTitle = 0xFF8, // Size: 0x1, Flags: 0x1
            FakeInebriation = 0xFFC, // Size: 0x1, Flags: 0x1
            VirtualPlayerRealm = 0x1000, // Size: 0x1, Flags: 0x1
            CurrentSpecID = 0x1004, // Size: 0x1, Flags: 0x1
            TaxiMountAnimKitID = 0x1008, // Size: 0x1, Flags: 0x1
            AvgItemLevel = 0x100C, // Size: 0x4, Flags: 0x1
            CurrentBattlePetBreedQuality = 0x101C, // Size: 0x1, Flags: 0x1
            InvSlots = 0x1020, // Size: 0x2E0, Flags: 0x2
            FarsightObject = 0x1BA0, // Size: 0x4, Flags: 0x2
            KnownTitles = 0x1BB0, // Size: 0xC, Flags: 0x2
            Coinage = 0x1BE0, // Size: 0x2, Flags: 0x2
            XP = 0x1BE8, // Size: 0x1, Flags: 0x2
            NextLevelXP = 0x1BEC, // Size: 0x1, Flags: 0x2
            Skill = 0x1BF0, // Size: 0x1C0, Flags: 0x2
            CharacterPoints = 0x22F0, // Size: 0x1, Flags: 0x2
            MaxTalentTiers = 0x22F4, // Size: 0x1, Flags: 0x2
            TrackCreatureMask = 0x22F8, // Size: 0x1, Flags: 0x2
            TrackResourceMask = 0x22FC, // Size: 0x1, Flags: 0x2
            MainhandExpertise = 0x2300, // Size: 0x1, Flags: 0x2
            OffhandExpertise = 0x2304, // Size: 0x1, Flags: 0x2
            RangedExpertise = 0x2308, // Size: 0x1, Flags: 0x2
            CombatRatingExpertise = 0x230C, // Size: 0x1, Flags: 0x2
            BlockPercentage = 0x2310, // Size: 0x1, Flags: 0x2
            DodgePercentage = 0x2314, // Size: 0x1, Flags: 0x2
            ParryPercentage = 0x2318, // Size: 0x1, Flags: 0x2
            CritPercentage = 0x231C, // Size: 0x1, Flags: 0x2
            RangedCritPercentage = 0x2320, // Size: 0x1, Flags: 0x2
            OffhandCritPercentage = 0x2324, // Size: 0x1, Flags: 0x2
            SpellCritPercentage = 0x2328, // Size: 0x7, Flags: 0x2
            ShieldBlock = 0x2344, // Size: 0x1, Flags: 0x2
            ShieldBlockCritPercentage = 0x2348, // Size: 0x1, Flags: 0x2
            Mastery = 0x234C, // Size: 0x1, Flags: 0x2
            Amplify = 0x2350, // Size: 0x1, Flags: 0x2
            Multistrike = 0x2354, // Size: 0x1, Flags: 0x2
            MultistrikeEffect = 0x2358, // Size: 0x1, Flags: 0x2
            Readiness = 0x235C, // Size: 0x1, Flags: 0x2
            Speed = 0x2360, // Size: 0x1, Flags: 0x2
            Lifesteal = 0x2364, // Size: 0x1, Flags: 0x2
            Avoidance = 0x2368, // Size: 0x1, Flags: 0x2
            Sturdiness = 0x236C, // Size: 0x1, Flags: 0x2
            Cleave = 0x2370, // Size: 0x1, Flags: 0x2
            Versatility = 0x2374, // Size: 0x1, Flags: 0x2
            VersatilityBonus = 0x2378, // Size: 0x1, Flags: 0x2
            PvpPowerDamage = 0x237C, // Size: 0x1, Flags: 0x2
            PvpPowerHealing = 0x2380, // Size: 0x1, Flags: 0x2
            ExploredZones = 0x2384, // Size: 0x100, Flags: 0x2
            RestStateBonusPool = 0x2784, // Size: 0x1, Flags: 0x2
            ModDamageDonePos = 0x2788, // Size: 0x7, Flags: 0x2
            ModDamageDoneNeg = 0x27A4, // Size: 0x7, Flags: 0x2
            ModDamageDonePercent = 0x27C0, // Size: 0x7, Flags: 0x2
            ModHealingDonePos = 0x27DC, // Size: 0x1, Flags: 0x2
            ModHealingPercent = 0x27E0, // Size: 0x1, Flags: 0x2
            ModHealingDonePercent = 0x27E4, // Size: 0x1, Flags: 0x2
            ModPeriodicHealingDonePercent = 0x27E8, // Size: 0x1, Flags: 0x2
            WeaponDmgMultipliers = 0x27EC, // Size: 0x3, Flags: 0x2
            WeaponAtkSpeedMultipliers = 0x27F8, // Size: 0x3, Flags: 0x2
            ModSpellPowerPercent = 0x2804, // Size: 0x1, Flags: 0x2
            ModResiliencePercent = 0x2808, // Size: 0x1, Flags: 0x2
            OverrideSpellPowerByAPPercent = 0x280C, // Size: 0x1, Flags: 0x2
            OverrideAPBySpellPowerPercent = 0x2810, // Size: 0x1, Flags: 0x2
            ModTargetResistance = 0x2814, // Size: 0x1, Flags: 0x2
            ModTargetPhysicalResistance = 0x2818, // Size: 0x1, Flags: 0x2
            LocalFlags = 0x281C, // Size: 0x1, Flags: 0x2
            LifetimeMaxRank = 0x2820, // Size: 0x1, Flags: 0x2
            SelfResSpell = 0x2824, // Size: 0x1, Flags: 0x2
            PvpMedals = 0x2828, // Size: 0x1, Flags: 0x2
            BuybackPrice = 0x282C, // Size: 0xC, Flags: 0x2
            BuybackTimestamp = 0x285C, // Size: 0xC, Flags: 0x2
            YesterdayHonorableKills = 0x288C, // Size: 0x1, Flags: 0x2
            LifetimeHonorableKills = 0x2890, // Size: 0x1, Flags: 0x2
            WatchedFactionIndex = 0x2894, // Size: 0x1, Flags: 0x2
            CombatRatings = 0x2898, // Size: 0x20, Flags: 0x2
            PvpInfo = 0x2918, // Size: 0x24, Flags: 0x2
            MaxLevel = 0x29A8, // Size: 0x1, Flags: 0x2
            RuneRegen = 0x29AC, // Size: 0x4, Flags: 0x2
            NoReagentCostMask = 0x29BC, // Size: 0x4, Flags: 0x2
            GlyphSlots = 0x29CC, // Size: 0x6, Flags: 0x2
            Glyphs = 0x29E4, // Size: 0x6, Flags: 0x2
            GlyphSlotsEnabled = 0x29FC, // Size: 0x1, Flags: 0x2
            PetSpellPower = 0x2A00, // Size: 0x1, Flags: 0x2
            Researching = 0x2A04, // Size: 0xA, Flags: 0x2
            ProfessionSkillLine = 0x2A2C, // Size: 0x2, Flags: 0x2
            UiHitModifier = 0x2A34, // Size: 0x1, Flags: 0x2
            UiSpellHitModifier = 0x2A38, // Size: 0x1, Flags: 0x2
            HomeRealmTimeOffset = 0x2A3C, // Size: 0x1, Flags: 0x2
            ModPetHaste = 0x2A40, // Size: 0x1, Flags: 0x2
            SummonedBattlePetGUID = 0x2A44, // Size: 0x4, Flags: 0x2
            OverrideSpellsID = 0x2A54, // Size: 0x1, Flags: 0x402
            LfgBonusFactionID = 0x2A58, // Size: 0x1, Flags: 0x2
            LootSpecID = 0x2A5C, // Size: 0x1, Flags: 0x2
            OverrideZonePVPType = 0x2A60, // Size: 0x1, Flags: 0x402
            ItemLevelDelta = 0x2A64, // Size: 0x1, Flags: 0x2
            BagSlotFlags = 0x2A68, // Size: 0x4, Flags: 0x2
            BankBagSlotFlags = 0x2A78, // Size: 0x7, Flags: 0x2
            InsertItemsLeftToRight = 0x2A94, // Size: 0x1, Flags: 0x2
            QuestCompleted = 0x2A98, // Size: 0x36B, Flags: 0x2
        };
    }
}