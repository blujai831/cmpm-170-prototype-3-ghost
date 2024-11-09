using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This convoluted mess surely can't be the simplest way
 * to do framewise 2D animations in Unity.
 * Surely I must be missing something here.
 * But, oh well, if it works it works. */

public class FramewiseAnimator : MonoBehaviour
{
    [System.Serializable]
    public class AnimationFrame {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public bool FlipX;
        [SerializeField] public bool FlipY;
        [SerializeField] public int DurationInGameFrames;
    }

    [System.Serializable]
    public class FramewiseAnimation {
        [SerializeField] public string Name;
        [SerializeField] public Direction2D Direction;
        [SerializeField] public List<AnimationFrame> Frames =
            new List<AnimationFrame>();
        [SerializeField] public int LoopBackTo = 0;
    }

    [SerializeField]
    public List<FramewiseAnimation> Animations =
        new List<FramewiseAnimation>();

    [SerializeField] private string _initialAnimation;
    [SerializeField] private Direction2D _initialDirection;

    private SpriteRenderer _spriteRenderer;
    private FramewiseAnimation _currentAnimation;
    private int _currentFrame;
    private int _frameCountdown;
    private bool _needUpdateSprite;

    public string CurrentAnimation {
        get {return _currentAnimation.Name;}
        set {PlayAnimation(value);}
    }

    public Direction2D FacingDirection {
        get {return _currentAnimation.Direction;}
        set {FaceDirection(value);}
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        PlayAnimation(_initialAnimation, _initialDirection);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_currentAnimation != null) {
            UpdateSprite();
            TickAnimationFrame();
            //DebugReport();
        }
    }

    void PlayAnimation(string animation, Direction2D direction) {
        if (
            _currentAnimation == null ||
            _currentAnimation.Name != animation ||
            _currentAnimation.Direction != direction
        ) {
            _currentAnimation = null;
            foreach (var candidate in Animations) {
                if (
                    candidate.Name == animation &&
                    candidate.Direction == direction
                ) {
                    _currentAnimation = candidate;
                    _currentFrame = 0;
                    _frameCountdown = candidate.Frames[0].DurationInGameFrames;
                    _needUpdateSprite = true;
                    break;
                }
            }
            if (_currentAnimation == null) {
                throw new ArgumentException(
                    "Object has no animation " +
                    $"with name ${name} " +
                    $"and direction ${direction}"
                );
            }
        }
    }

    void PlayAnimation(string animation) {
        PlayAnimation(animation, _currentAnimation.Direction);
    }

    void FaceDirection(Direction2D direction) {
        PlayAnimation(_currentAnimation.Name, direction);
    }

    void TickAnimationFrame() {
        _frameCountdown--;
        if (_frameCountdown <= 0) {
            AdvanceAnimationFrame();
        }
    }

    void AdvanceAnimationFrame() {
        _currentFrame++;
        if (_currentFrame >= _currentAnimation.Frames.Count) {
            if (_currentAnimation.LoopBackTo >= 0) {
                _currentFrame = _currentAnimation.LoopBackTo;
            } else {
                _currentFrame--;
            }
        }
        _frameCountdown =
            _currentAnimation.Frames[_currentFrame].DurationInGameFrames;
        _needUpdateSprite = true;
    }

    void UpdateSprite() {
        if (_needUpdateSprite) {
            ForceUpdateSprite();
        }
    }

    void ForceUpdateSprite() {
        var frame = _currentAnimation.Frames[_currentFrame];
        _spriteRenderer.sprite = frame.Sprite;
        _spriteRenderer.flipX = frame.FlipX;
        _spriteRenderer.flipY = frame.FlipY;
        _needUpdateSprite = false;
    }

    void DebugReport() {
        Debug.Log(
            $"{_currentAnimation.Name} {_currentAnimation.Direction} " +
            $"{_currentFrame} {_frameCountdown}"
        );
    }
}
