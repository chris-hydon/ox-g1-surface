using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.TowerAudio
{
    class ScaleDecider
    {
        #region Struct and Enum
        public struct Chord
        {
            private int[] notes;

            public Chord(int[] notes)
            {
                this.notes = notes;
            }

            public int[] Notes
            {
                get { return notes; }
            }
        }
        public enum ChordType {I = 1, II, III, IV, V, VI, VII};

        public struct Scale
        {
            private int[] notes;
            private ScaleType scaleType;

            public Scale(int[] note, ScaleType scaleType)
            {
                this.notes = note;
                this.scaleType = scaleType;
            }

            public ScaleType ScaleType
            {
                get { return scaleType; }
            }

            public int Length
            {
                get { return notes.Length; }
            }

            public int NoteAt(int position)
            {
                return notes[position];
            }
        }
        public enum ScaleType { Major, NatMinor, HarMinor, Pentatonic, Transition };
        #endregion

        #region Constants
        private int[][] scaleSignature = new int[][]
        {
        new int[]{2,2,1,2,2,2,1},
        new int[]{2,1,2,2,1,2,2},
        new int[]{2,1,2,2,1,3,1},
        new int[]{2,2,3,2,3},
        };
        private int SCALE_CHANGE_HP_TRESHOLD;
        #endregion

        #region Variables
        private Scale currentScale;
        private ScaleType currentScaleType;
        private bool towerUnderHP;
        private bool inTransition;
        private int clickCounter;
        private Random random;
        #endregion


        public ScaleDecider(MelodyPlayer.Note note, ScaleType scaleType)
        {
            currentScale = CreateScale(note, scaleType);
            currentScaleType = scaleType;
            towerUnderHP = false;
            inTransition = false;
            clickCounter = 0;
            random = new Random();
            SCALE_CHANGE_HP_TRESHOLD = (App.Instance.Model.Tower.MaxHealth - 100) * 0;
        }

        #region Properties

        public Scale CurrentScale
        {
            get { return currentScale; }
        }

        public int Length
        {
            get { return currentScale.Length; }
        }

        #endregion

        #region Methods
        public Chord GenerateChord(ChordType chordType, int octave)
        {
            if (currentScaleType == ScaleType.Pentatonic)
                return new Chord(new int[] { 0, 1, 2, 3 });

            if (currentScaleType == ScaleType.Major ||
                currentScaleType == ScaleType.NatMinor ||
                currentScaleType == ScaleType.HarMinor)
            {
                int correction = octave * 12;
                int root = (int)chordType + correction;
                int range = 28;
                return new Chord(new int[] { root % range , (root + 2) % range, (root + 4) % range});
            }

            return new Chord(new int[] { });
        }

        public int NoteAt(int position)
        {
            return currentScale.NoteAt(position);
        }

        private Scale CreateScale(MelodyPlayer.Note note, ScaleType scaleType)
        {
            int scaleLength = scaleSignature[(int)scaleType].Length;
            int len = scaleLength * 4;

            int[] result = new int[len];
            result[0] = (int)note;
            for (int i = 0; i < len - 1; i++)
            {
                result[i + 1] = (result[i] + scaleSignature[(int)scaleType][i % scaleLength]);
                if (result[i + 1] > MelodyPlayer.SOUNDPACKSIZE)
                    result[i + 1] = (result[i + 1] % MelodyPlayer.SOUNDPACKSIZE);
            }

            return new Scale(result, scaleType);
        }

        private Scale CreateTransitionScale(Scale prev, Scale next)
        {
            int[] aux = new int[MelodyPlayer.SOUNDPACKSIZE];
            int auxPointer = 0;
            for (int i = 1; i <= prev.Length; i++)
            {
                for (int j = 1; j <= next.Length; j++)
                {
                    if (prev.NoteAt(i) == next.NoteAt(j))
                    {
                        aux[auxPointer] = prev.NoteAt(i);
                        auxPointer++;
                    }
                }
            }

            auxPointer = 0;
            while (auxPointer < MelodyPlayer.SOUNDPACKSIZE && aux[auxPointer] != 0)
            {
                auxPointer++;
            }

            int[] result = new int[auxPointer];
            for (int i = 0; i < auxPointer; i++)
            {
                result[i] = aux[i];
            }

            return new Scale(result, ScaleType.Transition);
        }

        internal void ClickUpdate()
        {
            if ((App.Instance.Model.Tower.Health < SCALE_CHANGE_HP_TRESHOLD) && !towerUnderHP)
            {
                towerUnderHP = true;
                currentScale = CreateScale((MelodyPlayer.Note)random.Next(12),ScaleType.HarMinor);
                System.Console.WriteLine("Tower under half HP");
            }
            if ((App.Instance.Model.Tower.Health > SCALE_CHANGE_HP_TRESHOLD) && !towerUnderHP)
            {
                towerUnderHP = false;
                currentScale = CreateScale((MelodyPlayer.Note)random.Next(12), ScaleType.Major);
            }
        }
        #endregion

    }
}
