using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SurfaceTower.Model;
using Microsoft.Xna.Framework.Audio;

namespace SurfaceTower.TowerAudioEngine
{
    class TowerAudioEngine
    {
        private BaseModel baseModel;
        private AudioEngine audioEngine;
        private DrumPlayer drumPlayer;
        private SoundBank drumSoundBank;
        private MelodyPlayer melodyPlayer;
        private SoundBank melodySoundBank;

        public TowerAudioEngine(BaseModel baseModel)
        {
            this.baseModel = baseModel;

            audioEngine = new AudioEngine("Content/AudioResources.xgs");
            drumSoundBank = new SoundBank(audioEngine, "Content/Drum Bank.xwb");
            melodySoundBank = new SoundBank(audioEngine, "Content/Melody Bank.xwb");

            drumPlayer = new DrumPlayer(drumSoundBank);
            melodyPlayer = new MelodyPlayer(melodySoundBank);

            App.Instance.Model.Music.Click += new EventHandler(OnClick);
            App.Instance.Model.Music.Beat += new EventHandler(OnBeat);
            App.Instance.Model.Music.Bar += new EventHandler(OnBar);
        }

        #region Called on Event

        void OnClick(object sender, EventArgs e)
        {
            drumPlayer.OnClick();
            melodyPlayer.OnClick();
        }

        void OnBeat(object sender, EventArgs e)
        {
            drumPlayer.OnBeat();
            melodyPlayer.OnBeat();
        }

        void OnBar(object sender, EventArgs e)
        {
            drumPlayer.OnBar();
            melodyPlayer.OnBar();
        }

        #endregion

    }
}
