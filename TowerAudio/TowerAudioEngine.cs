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
        private EffectPlayer effectPlayer;
        public DrumPlayer drumPlayer;
        public MelodyPlayer melodyPlayer;
        private ICollection<WaveBank> waveBanks;

        public TowerAudioEngine(BaseModel baseModel)
        {
          try
          {
            this.baseModel = baseModel;

            audioEngine = new AudioEngine("Content/8bit.xgs");
            audioEngine.Update();

            //setting the values in App.Instance.Music
            App.Instance.Model.Music.TimeSignature = new TimeSignature(4, 4);
            App.Instance.Model.Music.Tempo = 60;
            App.Instance.Model.Music.ClicksPerBeat = 4;

            //this is needed as the constructor does something magical which allows the sounds to play
            waveBanks = new LinkedList<WaveBank>();
            waveBanks.Add(new WaveBank(audioEngine, "Content/Drum Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Effect Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Eva Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Simpleb Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Spaceb Bank.xwb"));
            waveBanks.Add(new WaveBank(audioEngine, "Content/Weeping Bank.xwb"));

            drumPlayer = new DrumPlayer(audioEngine);
            melodyPlayer = new MelodyPlayer(audioEngine);
            effectPlayer = new EffectPlayer(audioEngine, this);

            App.Instance.Model.Music.Click += new EventHandler(OnClick);
            App.Instance.Model.Music.Beat += new EventHandler(OnBeat);
            App.Instance.Model.Music.Bar += new EventHandler(OnBar);
            App.Instance.Model.Update += new EventHandler<SurfaceTower.Model.EventArguments.UpdateArgs>(OnUpdate);
          }
          catch (InvalidOperationException e)
          {
            Console.WriteLine("There is no audio device plugged in. AudioEngine will not be used.");
          }
        }

        public void Restart()
        {
          App.Instance.Model.Music.Click += new EventHandler(OnClick);
          App.Instance.Model.Music.Beat += new EventHandler(OnBeat);
          App.Instance.Model.Music.Bar += new EventHandler(OnBar);
          App.Instance.Model.Update += new EventHandler<SurfaceTower.Model.EventArguments.UpdateArgs>(OnUpdate);
          effectPlayer.Restart();
        }
        
        #region Called on Event

        void OnClick(object sender, EventArgs e)
        {
            audioEngine.Update();
            drumPlayer.OnClick();
            melodyPlayer.OnClick();
            effectPlayer.OnClick();
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
