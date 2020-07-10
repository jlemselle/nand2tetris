using System.Linq;

namespace Assembler.Core.Instructions
{
    internal class CInstruction : IInstruction
    {
        private readonly Destination[] destinations;
        private readonly Computation computation;
        private readonly JumpCondition condition;

        public CInstruction
            (Destination[] destinations, Computation computation, JumpCondition condition)
        {
            this.destinations = destinations;
            this.computation = computation;
            this.condition = condition;
        }

        public string AsString()
        {
            bool[] values = new bool[]
            {
                true,
                true,
                true,
                ReadMemory,
                
                ZeroD,
                NegateD,
                ZeroAM,
                NegateAM,
                
                IsAdd,
                NegateOutput,
                D1,
                D2,
                
                D3,
                J1,
                J2,
                J3
            };

            return new string(values.Select(x => x ? '1' : '0').ToArray());
        }

        public bool ReadMemory
            => computation == Computation.M
            || computation == Computation.InverseOfM
            || computation == Computation.NegativeM
            || computation == Computation.MPlusOne
            || computation == Computation.MMinusOne
            || computation == Computation.DPlusM
            || computation == Computation.DMinusM
            || computation == Computation.MMinusD
            || computation == Computation.DAndM
            || computation == Computation.DOrM;

        public bool ZeroD
            => computation == Computation.Zero
            || computation == Computation.One
            || computation == Computation.NegativeOne
            || computation == Computation.A
            || computation == Computation.M
            || computation == Computation.InverseOfA
            || computation == Computation.InverseOfM
            || computation == Computation.NegativeA
            || computation == Computation.NegativeM
            || computation == Computation.APlusOne
            || computation == Computation.MPlusOne
            || computation == Computation.AMinusOne
            || computation == Computation.MMinusOne;

        public bool NegateD
            => computation == Computation.One
            || computation == Computation.NegativeOne
            || computation == Computation.A
            || computation == Computation.M
            || computation == Computation.InverseOfA
            || computation == Computation.InverseOfM
            || computation == Computation.DPlusOne
            || computation == Computation.NegativeA
            || computation == Computation.NegativeM
            || computation == Computation.APlusOne
            || computation == Computation.MPlusOne
            || computation == Computation.AMinusOne
            || computation == Computation.MMinusOne
            || computation == Computation.DMinusA
            || computation == Computation.DMinusM
            || computation == Computation.DOrA
            || computation == Computation.DOrM;

        public bool ZeroAM
            => computation == Computation.Zero
            || computation == Computation.One
            || computation == Computation.NegativeOne
            || computation == Computation.D
            || computation == Computation.InverseOfD
            || computation == Computation.NegativeD
            || computation == Computation.DPlusOne
            || computation == Computation.DMinusOne;

        public bool NegateAM
            => computation == Computation.One
            || computation == Computation.D
            || computation == Computation.InverseOfD
            || computation == Computation.NegativeD
            || computation == Computation.DPlusOne
            || computation == Computation.APlusOne
            || computation == Computation.MPlusOne
            || computation == Computation.DMinusOne
            || computation == Computation.AMinusD
            || computation == Computation.MMinusD
            || computation == Computation.DOrA
            || computation == Computation.DOrM;

        public bool IsAdd
            => computation == Computation.Zero
            || computation == Computation.One
            || computation == Computation.NegativeOne
            || computation == Computation.NegativeD
            || computation == Computation.NegativeA
            || computation == Computation.NegativeM
            || computation == Computation.DPlusOne
            || computation == Computation.APlusOne
            || computation == Computation.MPlusOne
            || computation == Computation.DMinusOne
            || computation == Computation.AMinusOne
            || computation == Computation.MMinusOne
            || computation == Computation.DPlusA
            || computation == Computation.DPlusM
            || computation == Computation.DMinusA
            || computation == Computation.DMinusM
            || computation == Computation.AMinusD
            || computation == Computation.MMinusD;

        public bool NegateOutput
            => computation == Computation.One
            || computation == Computation.InverseOfD
            || computation == Computation.InverseOfA
            || computation == Computation.InverseOfM
            || computation == Computation.NegativeD
            || computation == Computation.NegativeA
            || computation == Computation.NegativeM
            || computation == Computation.DPlusOne
            || computation == Computation.APlusOne
            || computation == Computation.MPlusOne
            || computation == Computation.DMinusA
            || computation == Computation.DMinusM
            || computation == Computation.AMinusD
            || computation == Computation.MMinusD
            || computation == Computation.DOrA
            || computation == Computation.DOrM;

        public bool D1
            => destinations.Contains(Destination.A);

        public bool D2
            => destinations.Contains(Destination.D);

        public bool D3
            => destinations.Contains(Destination.M);

        public bool J1
            => condition == JumpCondition.IsLessThanZero
            || condition == JumpCondition.IsNotZero
            || condition == JumpCondition.IsLessThanOrEqualToZero
            || condition == JumpCondition.Always;

        public bool J2
            => condition == JumpCondition.IsZero
            || condition == JumpCondition.IsGreaterThanOrEqualToZero
            || condition == JumpCondition.IsLessThanOrEqualToZero
            || condition == JumpCondition.Always;

        public bool J3
            => condition == JumpCondition.IsGreaterThanZero
            || condition == JumpCondition.IsGreaterThanOrEqualToZero
            || condition == JumpCondition.IsNotZero
            || condition == JumpCondition.Always;
    }
}