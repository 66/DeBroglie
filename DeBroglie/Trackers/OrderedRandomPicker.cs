﻿using DeBroglie.Wfc;
using System;

namespace DeBroglie.Trackers
{
    internal class OrderedRandomPicker : IRandomPicker
    {

        private readonly int patternCount;

        private readonly double[] frequencies;

        private readonly bool[] mask;

        private readonly int indices;

        private readonly Wave wave;

        public OrderedRandomPicker(
            Wave wave,
            double[] frequencies,
            bool[] mask)
        {
            this.frequencies = frequencies;
            this.patternCount = frequencies.Length;
            this.mask = mask;

            this.wave = wave;
            this.indices = wave.Indicies;
        }

        public int GetRandomIndex(Func<double> randomDouble, int[] externalPriority = null)
        {
            if(externalPriority != null)
            {
                throw new NotSupportedException();
            }
            for (int i = 0; i < indices; i++)
            {
                if (mask != null && !mask[i])
                    continue;
                var c = wave.GetPatternCount(i);
                if (c <= 1)
                {
                    continue;
                }
                return i;
            }
            return -1;
        }

        public int GetRandomPossiblePatternAt(int index, Func<double> randomDouble)
        {
            var s = 0.0;
            for (var pattern = 0; pattern < patternCount; pattern++)
            {
                if (wave.Get(index, pattern))
                {
                    s += frequencies[pattern];
                }
            }
            var r = randomDouble() * s;
            for (var pattern = 0; pattern < patternCount; pattern++)
            {
                if (wave.Get(index, pattern))
                {
                    r -= frequencies[pattern];
                }
                if (r <= 0)
                {
                    return pattern;
                }
            }
            return patternCount - 1;
        }

    }
}
