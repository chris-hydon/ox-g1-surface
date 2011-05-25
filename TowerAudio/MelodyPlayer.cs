using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using SurfaceTower.Model;
using ChordType = SurfaceTower.TowerAudio.ScaleDecider.ChordType;

namespace SurfaceTower.TowerAudio
{
    /* MelodyPlayer holds the logic deciding what sounds should be played as part of the melody. On Click events, it checks
     * if any sounds need to be played and then, if so, decides what sounds to play.
     * Some logic is also concerned with changing the scale the music is being played in and making sure chords don't sound
     * dissonant with the current scale*/
     
    public class MelodyPlayer
    {
        #region Structures and Enums
        public enum Note {C = 0, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B };
        public enum ScaleDegree { Tonic1 = 0, Supertonic1, Mediant1, Subdominant1, Dominant1, Submediant1, Subtonic1,
                                  Tonic2, Supertonic2, Mediant2, Subdominant2, Dominant2, Submediant2, Subtonic2};
        enum SoundPack { Eva, Simpleb, Spaceb, Weeping };
        #endregion


        #region Constants
        private ChordType[][] CHORD_PROGRESSIONS_LIST = new ChordType[][]{
        new ChordType[]{ChordType.I, ChordType.VI, ChordType.IV, ChordType.V}, //Love songs
        new ChordType[]{ChordType.I, ChordType.V, ChordType.VI, ChordType.IV}, //Extremely often used
        new ChordType[]{ChordType.I, ChordType.IV, ChordType.I, ChordType.V}, //Happy
        new ChordType[]{ChordType.I, ChordType.IV, ChordType.V, ChordType.IV}, //Happy again
        new ChordType[]{ChordType.I, ChordType.III, ChordType.IV, ChordType.V}, //Inspirational
        new ChordType[]{ChordType.VI, ChordType.V, ChordType.IV, ChordType.V}, //Hip-Hop
        new ChordType[]{ChordType.VI, ChordType.IV, ChordType.I, ChordType.V}, //Angsty?
        new ChordType[]{ChordType.VI, ChordType.V, ChordType.IV, ChordType.III}, //Cool, jazzy (Hit the Road, Jack)
        new ChordType[]{ChordType.VI, ChordType.IV, ChordType.V, ChordType.VI}, //Uplifting
        new ChordType[]{ChordType.VI, ChordType.I, ChordType.II, ChordType.VI}, //tough blues
        };

        private SoundPack[] playerSoundPack = new SoundPack[] { SoundPack.Eva, SoundPack.Simpleb, SoundPack.Spaceb, SoundPack.Weeping };

        private int progression = 1;
        
        //the number of different notes in the sound pack.
        public const int SOUNDPACKSIZE = 48;
        public const int SPREAD = 1;
        public static float musicVolume = 1.0f;
        #endregion

        #region Variables
        public bool notePlaying = true;

        private ScaleDecider scaleDecider;
        private int currentNotePosition;
        private int previousNotePosition;

        private BaseModel model;
        private AudioEngine audioEngine;
        private ICollection<SoundBank> soundBanks;
        private Cue[] previousCues;
        private Random random;

        private int position = 0;
        private int clickPosition = 0;
        private Music music = App.Instance.Model.Music;
        #endregion

        
        public MelodyPlayer(AudioEngine audioEngine)
        {
            this.audioEngine = audioEngine;
            model = App.Instance.Model;

            soundBanks = new LinkedList<SoundBank>();

            soundBanks.Add(new SoundBank(audioEngine, "Content/Eva Sound.xsb"));
            soundBanks.Add(new SoundBank(audioEngine, "Content/Simpleb Sound.xsb"));
            soundBanks.Add(new SoundBank(audioEngine, "Content/Spaceb Sound.xsb"));
            soundBanks.Add(new SoundBank(audioEngine, "Content/Weeping Sound.xsb"));

            scaleDecider = new ScaleDecider(Note.C, ScaleDecider.ScaleType.Major);

            currentNotePosition = (int)ScaleDegree.Tonic1;
            previousNotePosition = (int)ScaleDegree.Tonic1;
            previousCues = new Cue[3];
            random = new Random();
        }

        #region Things happening on Events

        internal void OnClick()
        {
            //decide if scale should change
            scaleDecider.ClickUpdate();

            UpdateCurrentNote();

            if (model.Dying.Count > 0)
            {
                //Sound has to be played.
                //check if any correction needs to be done to resolve tension.
                currentNotePosition = CorrectNote(previousNotePosition, currentNotePosition);

                int position = 0;
                ScaleDecider.Chord chord = scaleDecider.GenerateChord((ChordType)(currentNotePosition % 12), currentNotePosition / 12);

                String lengthString;
                if (random.Next(3) < 2)
                    lengthString = "s";
                else
                    lengthString = "l";
                

                while (model.Dying.Count != 0)
                {
                    if(notePlaying)
                        soundBanks.ElementAt((int)playerSoundPack[model.Dying.ElementAt(0).who]).PlayCue((scaleDecider.NoteAt(chord.Notes[position]) + 1) + lengthString);

                    position++;
                    position %= chord.Notes.Length;
                    model.Kill(model.Dying.ElementAt(0));
                }
            }

            
            clickPosition++;
            clickPosition %= music.TimeSignature.number * music.ClicksPerBeat;
        }

        internal void OnBeat()
        {
            position = (position + 1) % 4;
        }

        internal void OnBar()
        {
            progression = random.Next(10);
        }

        #endregion

        #region Methods

        private int PlayerDyingCount(int playerNumber)
        {
            int result = 0;

            foreach (EnemyTimeWho etw in model.Dying)
            {
                if (etw.who == playerNumber)
                {
                    result++;
                }
            }
            return result;
        }

        private int CorrectNote(int previousNote, int currentNote)
        {
            if (scaleDecider.CurrentScale.ScaleType == ScaleDecider.ScaleType.Major ||
                scaleDecider.CurrentScale.ScaleType == ScaleDecider.ScaleType.HarMinor ||
                scaleDecider.CurrentScale.ScaleType == ScaleDecider.ScaleType.NatMinor)
            {
                //if previous note was a subtonic, resolve the tension
                if (previousNote == (int)ScaleDegree.Subtonic1 || previousNote == (int)ScaleDegree.Subtonic2)
                    return (int)ScaleDegree.Tonic2;

                //if previous note was a supertonic go towards the V note (dominant)
                if (previousNote == (int)ScaleDegree.Supertonic1)
                    return (int)ScaleDegree.Dominant1;
                if (previousNote == (int)ScaleDegree.Supertonic2)
                    return (int)ScaleDegree.Dominant2;
            }
            return currentNote;
        }

        private void UpdateCurrentNote()
        {
            //set up the note modifier and update the current note
            int noteModifier = random.Next(2 * SPREAD) - 1;
            noteModifier -= SPREAD;
            int scaleLength = scaleDecider.Length;
            currentNotePosition += noteModifier;

            //do corrections
            if (currentNotePosition >= scaleLength)
            {
                currentNotePosition -= currentNotePosition % scaleLength;
            }
            if (currentNotePosition < 1)
            {
                currentNotePosition = 1 - currentNotePosition;
            }

            //just as a precaution if the size of the scale is smaller than SPREAD
            currentNotePosition = (currentNotePosition % scaleLength) + 1;
        }

        #endregion
    }
}
