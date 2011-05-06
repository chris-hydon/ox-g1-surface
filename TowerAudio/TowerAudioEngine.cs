using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SurfaceTower.Model;
using Microsoft.Xna.Framework.Audio;

namespace SurfaceTower.TowerAudio
{
    public class TowerAudioEngine
    {
        private BaseModel baseModel;
        private AudioEngine audioEngine;
        private DrumPlayer drumPlayer;
        private MelodyPlayer melodyPlayer;
        private ICollection<WaveBank> waveBanks;

        public TowerAudioEngine(BaseModel baseModel)
        {
            this.baseModel = baseModel;

            audioEngine = new AudioEngine("Content/8bit.xgs");
            audioEngine.Update();
            //this is needed as the constructor does something magical which allows the sounds to play
            waveBanks = new LinkedList<WaveBank>();
            waveBanks.Add(new WaveBank(audioEngine, "Content/Arabian Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Heavy Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Drum Bank.xwb"));

            drumPlayer = new DrumPlayer(audioEngine);
            melodyPlayer = new MelodyPlayer(audioEngine);

            App.Instance.Model.Music.Click += new EventHandler(OnClick);
            App.Instance.Model.Music.Beat += new EventHandler(OnBeat);
            App.Instance.Model.Music.Bar += new EventHandler(OnBar);
            App.Instance.Model.Update += new EventHandler<SurfaceTower.Model.EventArguments.UpdateArgs>(OnUpdate);
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

        void OnUpdate(object sender, SurfaceTower.Model.EventArguments.UpdateArgs e)
        {
            audioEngine.Update();
        }

        #endregion

    }
}
