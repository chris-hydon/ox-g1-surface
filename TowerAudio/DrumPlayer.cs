using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;


namespace SurfaceTower.TowerAudio
{
    class DrumPlayer
    {
        private struct DrumLoop
        {
            //a loopTrace is a 2 dimensional int array. On the y axis, it lists the instruments available and on the
            //x axis it marks each click in the bar. A non-zero value in the array tells the specific instrument to make a noise
            //on the specified click.

            public int[][] loopTrace;

            /*
             * 0 - HiHat
             * 1 - Cymbal
             * 2 - CynbalCrash
             * 3 - Cowbell
             * 4 - Rattle
             * 5 - Tambourine
             * 6 - Congo
             * 7 - Bongo
             * 8 - Triangle
             * 9 - BassDrum
             * 10 - KickDrum
             * 11 - SnareDrum
             * 12 - KettleDrum
             * 13 - DistortedKick
             * 14 - DistortedSnare
             */

            public DrumLoop(int[][] loopTrace)
            {
                this.loopTrace = loopTrace;
            }
        }

        #region Constants
        private const int SIZE = 16; //length of a bar in clicks
        private const int NO_OF_INSTRUMENTS = 15;

        private int[] range = { 100, 10, 10, 10, 10, 10, 10, 10, 10, 200, 200, 200, 10, 100, 100 };
        public string[] instrumentName = { "hihat", "cymbal", "cymbalcrash", "cowbell", "tambourine" , "rattle",
                                                       "conga", "bongo", "triangle", "bassdrum", "kickdrum",
                                                   "snaredrum", "kettledrum", "distortedkick", "distortedsnare"};
        #endregion

        #region Variables
        private AudioEngine audioEngine;
        private ICollection<SoundBank> soundBanks;
        private int positionPointer;
        private DrumLoop drumLoop;

        private Random random;
        private bool switchFlag;
        private bool[] playing;
        #endregion

        public DrumPlayer(AudioEngine audioEngine)
        {
            playing = new bool[NO_OF_INSTRUMENTS];
            switchFlag = true;
            random = new Random();
            int[][] loopTrace = new int[NO_OF_INSTRUMENTS][];
            for (int i = 0; i < NO_OF_INSTRUMENTS; i++)
            {
                loopTrace[i] = InitializeLoop(SIZE, 0, 1, 1, i);
            }


            drumLoop = new DrumLoop(loopTrace);
            positionPointer = 0;

            this.audioEngine = audioEngine;
            soundBanks = new LinkedList<SoundBank>();
            soundBanks.Add(new SoundBank(audioEngine, "Content/Drum Sound.xsb"));
        }

        //Probability of generating a sound on a click is b/a
        //instrumentSound determines which specific recording of the given instrument will play
        public int[] InitializeLoop(int size, int b, int a, int instrumentSound, int instrument) 
        {
            int[] result = new int[size];

            for (int i = 0; i < size; i++)
                if (random.Next(a) >= b) result[i] = 0;
                else result[i] = instrumentSound;

            return result;
        }

        #region Things happening on Events

        internal void OnClick()
        {
            if (positionPointer >= SIZE) positionPointer = 0;
            for (int i = 0; i < NO_OF_INSTRUMENTS; i++)
            {
                if (drumLoop.loopTrace[i][positionPointer] != 0 && playing[i])
                {
                    //drumSoundBank.PlayCue(instrumentName[i] + drumLoop.loopTrace[i][positionPointer].ToString());
                }
            }

            positionPointer++;
        }

        internal void OnBeat()
        {
            
        }

        //Bar should happen before Click whenever it does happen.
        internal void OnBar()
        {
            if (switchFlag)
            {
                positionPointer = 0;
                switchFlag = false;
                int choice = random.Next(NO_OF_INSTRUMENTS);
                int density = random.Next(2, 8);

                if (playing[choice] == false)
                {
                    drumLoop.loopTrace[choice] = InitializeLoop(SIZE, 1, density, range[choice], choice);
                }

                playing[choice] = !playing[choice];
            }
            else
            {
                switchFlag = true;
            }
        }

        #endregion
    }
}
