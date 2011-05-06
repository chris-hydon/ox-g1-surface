using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using SurfaceTower.Model;

namespace SurfaceTower.TowerAudio
{
    class MelodyPlayer
    {
        #region Structures and Enums
        private struct Scale
        {
            public int[] notes;

            public Scale(int[] note)
            {
                this.notes = note;
            }
        }
        enum ScaleType { Major, NatMinor, HarMinor, Pentatonic };
        #endregion


        #region Constants
        public const int SPREAD = 2;

        private int[][] scaleSignature = new int[][]
        {
        new int[]{2,2,1,2,2,2,1},
        new int[]{2,1,2,2,1,2,2},
        new int[]{2,1,2,2,1,3,1},
        new int[]{2,2,3,2,3},
        };
        #endregion

        #region Variables
        private Scale currentScale;

        private BaseModel model;
        private AudioEngine audioEngine;
        private ICollection<SoundBank> soundBanks;
        private Random random;
        #endregion

        
        public MelodyPlayer(AudioEngine audioEngine)
        {
            this.audioEngine = audioEngine;
            random = new Random();
            model = App.Instance.Model;

            soundBanks = new LinkedList<SoundBank>();
            soundBanks.Add(new SoundBank(audioEngine, "Content/Arabian Sound.xsb"));
            soundBanks.Add(new SoundBank(audioEngine, "Content/Heavy Sound.xsb"));

            currentScale = CreateScale(1, ScaleType.Major);
        }

        #region Methods
        private Scale CreateScale(int note, ScaleType scaleType)
        {
            int scaleLength = scaleSignature[(int)scaleType].Length;
            int len = scaleLength * 2;

            int[] result = new int[len];
            result[0] = note;
            for (int i = 0; i < len - 1; i++)
            {
                result[i + 1] = (result[i] + scaleSignature[(int)scaleType][i % scaleLength]);
                if (result[i + 1] > 24)
                    result[i + 1] = (result[i + 1] % 24) + 1;
            }

            return new Scale(result);
        }

        private Scale CreateTransitionScale(Scale prev, Scale next)
        {
            int[] aux = new int[24];
            int auxPointer = 0;
            for (int i = 0; i < prev.notes.Length; i++)
            {
                for (int j = 0; j < next.notes.Length; j++)
                {
                    if (prev.notes[i] == next.notes[j])
                    {
                        aux[auxPointer] = prev.notes[i];
                        auxPointer++;
                    }
                }
            }

            auxPointer = 0;
            while (auxPointer < 24 && aux[auxPointer] != 0)
            {
                auxPointer++;
            }

            int[] result = new int[auxPointer];
            for (int i = 0; i < auxPointer; i++)
            {
                result[i] = aux[i];
            }

            return new Scale(result);
        }
        #endregion

        #region Things happening on Events

        internal void OnClick()
        {
            if (model.Dying.Count > 0)
            {
                //Sound has to be played.
                int scaleLength = currentScale.notes.Length;
                int notePosition = random.Next(scaleLength);
                soundBanks.ElementAt(1).PlayCue( currentScale.notes[notePosition]+ "s");

                while (model.Dying.Count != 0)
                {
                    model.Kill(model.Dying.ElementAt(0));
                }
            }
        }

        internal void OnBeat()
        {
            
        }

        internal void OnBar()
        {
            
        }

        #endregion
    }
}
