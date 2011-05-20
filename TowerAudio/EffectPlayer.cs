﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace SurfaceTower.TowerAudio
{
    class EffectPlayer
    {

        private AudioEngine audioEngine;
        private SoundBank soundBank;
        private TowerAudioEngine towerAudioEngine;
        private bool exclusiveMode = false;
        private Cue heartbeatCue;

        public EffectPlayer(AudioEngine audioEngine, TowerAudioEngine tae)
        {
            this.towerAudioEngine = tae;
            this.audioEngine = audioEngine;

            soundBank = new SoundBank(audioEngine, "Content/Effect Sound.xsb");

            App.Instance.Model.AddPlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnAddPlayer);
            App.Instance.Model.RemovePlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnRemovePlayer);
            App.Instance.Model.NewEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(OnNewEnemy);

            heartbeatCue = soundBank.GetCue("Hearbeat");
        }

        public void Restart()
        {
            App.Instance.Model.AddPlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnAddPlayer);
            App.Instance.Model.RemovePlayer += new EventHandler<SurfaceTower.Model.EventArguments.PlayerArgs>(OnRemovePlayer);
            App.Instance.Model.NewEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(OnNewEnemy);
        }
        
        #region Things that happen on events

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
            soundBank.PlayCue("Aww");
        }

        void OnAddPlayer(object sender, SurfaceTower.Model.EventArguments.PlayerArgs e)
        {
            soundBank.PlayCue("Crowdcheer");
        }

        internal void OnClick()
        {
            if (App.Instance.Model.Tower.Health < Constants.HP_LIMIT_CRITICAL_TOWER)
            {
                audioEngine.GetCategory("Music").SetVolume(0.07f);
                audioEngine.GetCategory("Drums").SetVolume(0.1f);
                if(!heartbeatCue.IsPlaying)
                    heartbeatCue.Play();
                exclusiveMode = true;
            }
            else
                if (exclusiveMode)
                {
                    audioEngine.GetCategory("Music").SetVolume(MelodyPlayer.musicVolume);
                    audioEngine.GetCategory("Drums").SetVolume(DrumPlayer.drumVolume);
                    heartbeatCue.Stop(AudioStopOptions.Immediate);

                    exclusiveMode = false;
                }
        }

        #endregion
    }
}
