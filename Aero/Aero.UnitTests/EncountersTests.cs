using System.Linq;
using Aero.Gen;
using Aero.Gen.Attributes;
using NUnit.Framework;

namespace Aero.UnitTests
{
    [Aero(AeroGenTypes.View)]
    [AeroEncounter("race")]
    public partial class Race
    {
        [AeroNullable] private ushort race_type;

        private EntityId track_name;
        // private uint track_name;

        [AeroArray(3)] private int[] test;

        [AeroArray(2)]
        private ulong[] test2;

        private uint challenge_name;

        private ushort waypoint_counter;

        [AeroNullable] private ushort waypoint_goal;

        private ushort lap_counter;

        private ushort lap_goal;

        private Timer objective_timer;

        private ushort race_started;

        [AeroArray(12)]
        private bool[] abc;
    }

    [Aero(AeroGenTypes.View)]
    [AeroEncounter("arc")]
    public partial class Arc
    {
        private uint arc_name;

        private uint activity_string;

        private ushort activity_visible;

        private EntityId healthbar_1;

        private ushort healthbar_1_visible;
    }

    [Aero(AeroGenTypes.View)]
    [AeroEncounter("AllTypes")]
    public partial class AllTypes
    {
        private uint arc_name;

        [AeroArray(18)]
        private uint[] activity_strings;

        private ushort activity_visible;

        private EntityId healthbar_1;

        [AeroArray(10)]
        private bool[] booleans;
    }

    [Aero(AeroGenTypes.View)]
    [AeroEncounter("default")]
    public partial class HudTimer
    {
        private Timer hudtimer_timer;

        private uint hudtimer_label;
    }

    [AeroBlock]
    public struct EntityId
    {
        public ushort abc;
    }

    [AeroBlock]
    public struct Timer
    {
        // real struct would be in AeroMessages
        public byte State;

        public ulong Micro;

        public float Seconds;
    }

    // todo: tests
}