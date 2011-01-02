﻿using System;
using System.Diagnostics;

namespace BTDB
{
    public sealed class Sector
    {
        public SectorType Type { get; internal set; }

        internal long Position { get; set; }

        public bool InTransaction { get; internal set; }

        public bool Dirty { get; internal set; }

        internal bool Deleted { get; set; }

        public ulong LastAccessTime { get; internal set; }

        internal Sector Parent { get; set; }

        internal Sector NextLink { get; set; }

        internal Sector PrevLink { get; set; }

        public int Deepness
        {
            get
            {
                int result = 0;
                var t = this;
                while (t != null)
                {
                    result++;
                    t = t.Parent;
                }
                return result;
            }
        }

        public bool Allocated
        {
            get { return Position > 0; }
        }

        internal byte[] Data
        {
            get { return _data; }
        }

        public int Length
        {
            get
            {
                if (_data == null) return 0;
                return _data.Length;
            }

            internal set
            {
                Debug.Assert(value >= 0 && value <= LowLevelDB.MaxSectorSize);
                Debug.Assert(value % LowLevelDB.AllocationGranularity == 0);
                if (value == 0)
                {
                    _data = null;
                    return;
                }
                if (_data == null)
                {
                    _data = new byte[value];
                    return;
                }
                byte[] oldData = _data;
                _data = new byte[value];
                Array.Copy(oldData, _data, Math.Min(oldData.Length, value));
            }
        }

        internal void SetLengthWithRound(int length)
        {
            Length = LowLevelDB.RoundToAllocationGranularity(length);
        }

        internal SectorPtr ToSectorPtr()
        {
            SectorPtr result;
            result.Ptr = Position;
            if (Position > 0)
            {
                result.Ptr |= (uint) (Length / LowLevelDB.AllocationGranularity - 1);
            }
            result.Checksum = Dirty ? 0 : Checksum.CalcFletcher(Data, 0, (uint)Length);
            return result;
        }

        byte[] _data;
    }
}