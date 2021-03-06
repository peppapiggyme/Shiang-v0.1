
using System;
using UnityEngine;

namespace Shiang
{
    public abstract class Ability : ShiangObject, IVivid
    {
        public event Action OnUse;
        
        string[] _clipNames;
        AnimationClip[] _animationClips;
        Cooldown _cd;

        public virtual AnimationClip[] Clips
            => _animationClips == null
            ? _animationClips = Utils.BuildClips(
                Info.PLAYER_ANIM_CLIPS,
                Info.ABILITY_DATA[ClassID].AnimPattern)
            : _animationClips;

        public override string Name => Info.ABILITY_DATA[ClassID].Name;

        public override string Description => Info.ABILITY_DATA[ClassID].Description;

        public override uint Hash => Info.ABILITY_DATA[ClassID].Hash;

        public override Sprite Image => Info.SPRITES_ICON1[Info.ABILITY_DATA[ClassID].SpriteIndex];

        public virtual float CdTime => Info.ABILITY_DATA[ClassID].CdTime;

        public string[] ClipNames
            => _clipNames == null ? _clipNames = new string[2] { Clips[0].name, Clips[1].name } : _clipNames;

        public float ClipLength => Clips[0].length;

        public Cooldown Cd
            => _cd == null ? _cd = new Cooldown(CdTime) : _cd;

        public abstract void Affect(IGameEntity entity);
    }
}