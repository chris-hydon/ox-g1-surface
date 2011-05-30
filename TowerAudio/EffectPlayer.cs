using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace SurfaceTower.TowerAudio
{

    /*EffectPlayer class is responsible with the logic deciding when non-musical sound effects should be played
     * i.e. player joins the game, player leaves etc*/

    class EffectPlayer
    {

        private AudioEngine audioEngine;
        private SoundBank soundBank;
        private TowerAudioEngine towerAudioEngine;
        private bool heartbeatOnlyMode = false;
        private Cue heartbeatCue;
        private Cue introCue;
        private bool firstPlayerHasJoined;

        public EffectPlayer(AudioEngine audioEngine, TowerAudioEngine tae)
        {
            this.towerAudioEngine = tae;
            this.audioEngine = audioEngine;

            soundBank = new SoundBank(audioEngine, "Content/Effect Sound.xsb");

            App.Instance.Model.AddPlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnAddPlayer);
            App.Instance.Model.RemovePlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnRemovePlayer);
            App.Instance.Model.NewEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(OnNewEnemy);
            App.Instance.Model.Tower.ZeroHealth += new EventHandler(Tower_ZeroHealth);

            heartbeatCue = soundBank.GetCue("Hearbeat");
            introCue = soundBank.GetCue("Intro");
            introCue.Play();
            firstPlayerHasJoined = false;
        }

        public void Restart()
        {
            firstPlayerHasJoined = false;
            App.Instance.Model.AddPlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnAddPlayer);
            App.Instance.Model.RemovePlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnRemovePlayer);
            App.Instance.Model.NewEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(OnNewEnemy);
            App.Instance.Model.Tower.ZeroHealth += new EventHandler(Tower_ZeroHealth);

            heartbeatCue = soundBank.GetCue("Hearbeat");
            introCue = soundBank.GetCue("Intro");
            introCue.Play();

            audioEngine.GetCategory("Music").SetVolume(MelodyPlayer.musicVolume);
            audioEngine.GetCategory("Drums").SetVolume(DrumPlayer.drumVolume);
            
            if(heartbeatCue.IsPlaying)
                heartbeatCue.Stop(AudioStopOptions.Immediate);

            if(heartbeatOnlyMode)
                heartbeatOnlyMode = false;
        }
        
        #region Things that happen on events

        void Tower_ZeroHealth(object sender, EventArgs e)
        {
            if(heartbeatCue.IsPlaying)
                heartbeatCue.Stop(AudioStopOptions.Immediate);
            audioEngine.GetCategory("Drums").Stop(AudioStopOptions.Immediate);
            soundBank.PlayCue("Outro");
        }

        void OnNewEnemy(object sender, SurfaceTower.Model.EventArguments.EnemyArgs e)
        {
            e.Enemy.EnemyReached += new EventHandler(Enemy_EnemyReached);
        }

        void Enemy_EnemyReached(object sender, EventArgs e)
        {
            soundBank.PlayCue("Anvil");
        }

        void OnRemovePlayer(object sender, SurfaceTower.Model.EventArguments.PlayerArgs e)
        {
            //soundBank.PlayCue("Aww");
        }

        void OnAddPlayer(object sender, SurfaceTower.Model.EventArguments.PlayerArgs e)
        {
            if (!firstPlayerHasJoined)
            {
                if(introCue.IsPlaying)
                    introCue.Stop(AudioStopOptions.AsAuthored);
                soundBank.PlayCue("Crowdcheer");
                firstPlayerHasJoined = true;
            }
        }

        internal void OnClick()
        {
            if (App.Instance.Model.Tower.Health < Constants.HP_LIMIT_CRITICAL_TOWER)
            {
                audioEngine.GetCategory("Music").SetVolume(0.07f);
                audioEngine.GetCategory("Drums").SetVolume(0.3f);
                if (!heartbeatCue.IsPlaying)
                {
                  Console.WriteLine("HB");
                  heartbeatCue.Play();
                }
                heartbeatOnlyMode = true;
            }
            else
                if (heartbeatOnlyMode)
                {
                    audioEngine.GetCategory("Music").SetVolume(MelodyPlayer.musicVolume);
                    audioEngine.GetCategory("Drums").SetVolume(DrumPlayer.drumVolume);
                    heartbeatCue.Stop(AudioStopOptions.Immediate);
                    heartbeatCue = soundBank.GetCue("Hearbeat");

                    heartbeatOnlyMode = false;
                }
        }

        #endregion
    }
}
