using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using SurfaceTower.Model;


namespace SurfaceTower.TowerAudio
{
    /*DrumPlayer is responsible with creating backing drum loops in sync with the game. On each Bar it also updates the nextBarRythm
     * value (a one dimensional boolean array) in
     * each of the player voices, notifying the model about what the beat feels like.*/
    public class DrumPlayer
    {
        private struct DrumLoop
        {
            //a loopTrace is a 2 dimensional int array. On the y axis, it lists the instruments available and on the
            //x axis it marks each click in the bar. A non-zero value in the array tells the specific instrument to make a noise
            //on the specified click.

            public int positionPointer;
            public int[][] loopTrace;
            public bool[] playing;
            public int[][] futureLoopTrace;
            public bool[] futurePlaying;

            public DrumLoop(int[][] loopTrace, bool[] playing, int[][] futureLoopTrace, bool[] futurePlaying)
            {
                this.loopTrace = loopTrace;
                this.playing = playing;
                this.futureLoopTrace = futureLoopTrace;
                this.futurePlaying = futurePlaying;
                positionPointer = 0;
            }
        }
        private struct Instrument
        {
            private int range;
            private String name;

            public Instrument(int range, String name)
            {
                this.range = range;
                this.name = name;
            }

            public int Range
            {
                get { return range; }
            }

            public String Name
            {
                get { return name; }
            }
        }

        #region Constants

        private const int NO_OF_INSTRUMENTS = 11;

        private enum InstrumentName { bassdrum, tambourine, kickdrum, snaredrum, distortedsnare, distortedkick, kettledrum,
                                      triangle, rattle, hihat, cowbell};
        private ICollection<Instrument> instruments;

        #endregion

        #region Variables
        public static float drumVolume = 0.7f; 
        private bool drumsPlaying = true;

        private AudioEngine audioEngine;
        private ICollection<SoundBank> soundBanks;
        private DrumLoop drumLoop;

        private Random random;
        public readonly int barSizeInClicks;
        private Music music = App.Instance.Model.Music;
        #endregion

        public DrumPlayer(AudioEngine audioEngine)
        {
            barSizeInClicks = music.TimeSignature.number * music.ClicksPerBeat;
            random = new Random();

            #region Loading Drums
            instruments = new LinkedList<Instrument>();
            instruments.Add(new Instrument(200, "bassdrum"));
            instruments.Add(new Instrument(10, "tambourine"));
            instruments.Add(new Instrument(200, "kickdrum"));
            instruments.Add(new Instrument(200, "snaredrum"));
            instruments.Add(new Instrument(100, "distortedsnare"));
            instruments.Add(new Instrument(100, "distortedkick"));
            instruments.Add(new Instrument(10, "kettledrum"));
            instruments.Add(new Instrument(10, "triangle"));
            instruments.Add(new Instrument(10, "rattle"));
            instruments.Add(new Instrument(100, "hihat"));
            instruments.Add(new Instrument(10, "cowbell"));
            #endregion
            #region DrumLoop initialization
            int[][] loopTrace = new int[NO_OF_INSTRUMENTS][];
            bool[] playing = new bool[NO_OF_INSTRUMENTS];
            for (int i = 0; i < NO_OF_INSTRUMENTS; i++)
            {
                loopTrace[i] = InitializeEmptyLoop(barSizeInClicks);
            }

            loopTrace[(int)InstrumentName.bassdrum] = InitializeLoop(barSizeInClicks, 1, (int)InstrumentName.bassdrum);
            playing[(int)InstrumentName.bassdrum] = true;

            int[][] futureLoopTrace = new int[loopTrace.Length][];
            bool[] futurePlaying = new bool[playing.Length];
            for (int i = 0; i < loopTrace.Length; i++)
            {
                futureLoopTrace[i] = new int[loopTrace[i].Length];
                Array.Copy(loopTrace[i], futureLoopTrace[i], loopTrace[i].Length);
            }
            Array.Copy(playing, futurePlaying, playing.Length);
            
            drumLoop = new DrumLoop(loopTrace, playing, futureLoopTrace, futurePlaying);
            #endregion



            this.audioEngine = audioEngine;
            soundBanks = new LinkedList<SoundBank>();
            soundBanks.Add(new SoundBank(audioEngine, "Content/Drum Sound.xsb"));

            //Calling onBar twice because of the needs to initialize the voices in the model and the drumLoop
            //both of these need to know what is going to happen next bar.
            OnBar();
            OnBar();

            audioEngine.GetCategory("Drums").SetVolume(drumVolume);
        }

        #region Methods

        private int[] InitializeEmptyLoop(int size)
        {
            int[] result = new int[size];

            for (int i = 0; i < size; i++)
                result[i] = 0;

            return result;
        }

        //instrumentSound determines which specific recording of the given instrument will play
        //based on the instrument that the loop is being generated for, different logic applies
        public int[] InitializeLoop(int size, int instrumentSound, int instrument) 
        {
            int[] result = new int[size];

            switch (instrument)
            {
                #region bassdrum
                case (int)InstrumentName.bassdrum:
                    while (IsLoopEmpty(result))
                    {
                        for (int i = 0; i < size / 2; i++)
                        {
                            if (random.Next(2) < 1)
                                result[2 * i] = 0;
                            else
                                result[2 * i] = instrumentSound;
                        }
                    }
                    return result;
                #endregion
                #region cowbell
                case (int)InstrumentName.cowbell:
                    while (IsLoopEmpty(result))
                    {
                        for (int i = 0; i < size; i++)
                            if (random.Next(size) >= 2 * size/ 3) result[i] = 0;
                            else result[i] = instrumentSound;
                    }
                    return result;
                #endregion
                #region distortedkick
                case (int)InstrumentName.distortedkick:
                    return result;
                #endregion
                #region distortedsnare
                case (int)InstrumentName.distortedsnare:
                    return result;
                #endregion
                #region hihat
                case (int)InstrumentName.hihat:
                    while (IsLoopEmpty(result))
                    {

                        for (int i = 0; i < size; i++)
                            if (random.Next(size) >= (2 * size) / 3) result[i] = 0;
                            else result[i] = instrumentSound;

                    }
                    return result;
                #endregion
                #region kettledrum
                case (int)InstrumentName.kettledrum:
                    while (IsLoopEmpty(result))
                    {
                        for (int i = 0; i < music.TimeSignature.number; i++)
                        {
                            if (random.Next(music.TimeSignature.number) < music.TimeSignature.number - 1)
                                result[music.ClicksPerBeat * i] = 0;
                            else
                                result[music.ClicksPerBeat * i] = instrumentSound;
                        }
                    }
                    return result;
                #endregion
                #region kickdrum
                case (int)InstrumentName.kickdrum:
                    while (IsLoopEmpty(result))
                    {
                        for (int i = 0; i < size / 2; i++)
                        {
                            if (random.Next(4) < 2)
                                result[2 * i] = 0;
                            else
                                result[2 * i] = instrumentSound;
                        }
                    }
                    return result;
                #endregion
                #region rattle
                case (int)InstrumentName.rattle:
                    while (IsLoopEmpty(result))
                    {
                        result[0] = instrumentSound;
                    }
                    return result;
                #endregion
                #region snaredrum
                case (int)InstrumentName.snaredrum:
                    while (IsLoopEmpty(result))
                    {
                        for (int i = 0; i < size / 1; i++)
                        {
                            if (random.Next(4) < 2)
                                result[1 * i] = 0;
                            else
                                result[1 * i] = instrumentSound;
                        }
                    }
                    return result;
                #endregion
                #region tambourine
                case (int)InstrumentName.tambourine:
                    while (IsLoopEmpty(result))
                    {
                        int numberOfClicks = (music.ClicksPerBeat / 2) * music.TimeSignature.number;

                        for(int i = 0; i < numberOfClicks; i++)
                        {
                            if(random.Next(numberOfClicks) < numberOfClicks - 1)
                                result[i * 2] = instrumentSound;
                        }
                    }
                    return result;
                #endregion
                #region triangle
                case (int)InstrumentName.triangle:
                    while (IsLoopEmpty(result))
                    {
                        for (int i = 0; i < size; i++)
                            if (random.Next(size) >= (2 * size) / 3) result[i] = 0;
                            else result[i] = instrumentSound;
                    }
                    return result;
                #endregion

                default: return result;
            }
        }

        private bool IsLoopEmpty(int[] loop)
        {
            for (int i = 0; i < loop.Length; i++)
            {
                if (loop[i] != 0)
                    return false;
            }
            return true;
        }

        private void UpdateDrumLoop()
        {
            drumLoop.positionPointer = 0;

            #region Copying loopTraces
            Array.Copy(drumLoop.futureLoopTrace, drumLoop.loopTrace, drumLoop.futureLoopTrace.Length);
            Array.Copy(drumLoop.futurePlaying, drumLoop.playing, drumLoop.futurePlaying.Length);
            #endregion

            int choice = random.Next(0, NO_OF_INSTRUMENTS);

            if (drumLoop.futurePlaying[choice] == false)
            {
                drumLoop.futureLoopTrace[choice] = InitializeLoop(barSizeInClicks, instruments.ElementAt(choice).Range, choice);
            }

            drumLoop.futurePlaying[choice] = !drumLoop.futurePlaying[choice];

            //Testing

            //System.Console.Write("  1 - - - 2 - - - 3 - - - 4 - - -\n");
            //for (int i = 0; i < drumLoop.loopTrace.Length; i++)
            //{
            //    System.Console.Out.Write(drumLoop.playing[i].ToString()[0] + " ");
            //    for (int j = 0; j < drumLoop.loopTrace[i].Length; j++)
            //    {
            //        if (drumLoop.loopTrace[i][j] == 0)
            //            System.Console.Out.Write("O ");
            //        else
            //            System.Console.Out.Write("X ");
            //    }
            //    System.Console.Out.Write((InstrumentName)i);
            //    System.Console.Out.Write('\n');
            //}
        }

        internal SurfaceTower.Model.BarRhythm GetNextBarRhythm()
        {
            bool[] notes = new bool[drumLoop.futureLoopTrace[(int)InstrumentName.bassdrum].Length];
            for (int i = 0; i < notes.Length; i++)
            {
                if (drumLoop.futureLoopTrace[(int)InstrumentName.bassdrum][i] != 0)
                    notes[i] = true;
            }

            return new SurfaceTower.Model.BarRhythm(notes);
        }

        #endregion

        #region Things happening on Events

        internal void OnClick()
        {
            //play the next section of the drumLoop.
            for (int i = 0; i < NO_OF_INSTRUMENTS; i++)
            {
                if (drumLoop.loopTrace[i][drumLoop.positionPointer] != 0 && drumLoop.playing[i] && drumsPlaying)
                {
                    soundBanks.ElementAt(0).PlayCue(instruments.ElementAt(i).Name + drumLoop.loopTrace[i][drumLoop.positionPointer].ToString());
                }
            }

            drumLoop.positionPointer++;
            drumLoop.positionPointer %= barSizeInClicks;
        }

        internal void OnBeat()
        {
            
        }

        //Bar should happen before Click whenever it does happen.
        internal void OnBar()
        {
            UpdateDrumLoop();

            foreach (SurfaceTower.Model.Music.Voice v in App.Instance.Model.Music.Voices)
            {
                v.NextBarRhythm = GetNextBarRhythm();
            }
        }

        
        #endregion
    }
}
