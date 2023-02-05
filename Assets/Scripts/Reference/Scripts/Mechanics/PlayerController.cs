﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        private bool isAbleDoublejump;
        private bool doubleJump;
        //private bool isJumpedFirst;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
<<<<<<< Updated upstream
                    jumpState = JumpState.PrepareToJump;
                if (jumpState == JumpState.InFlight && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
=======
                { jumpState = JumpState.PrepareToJump;
     
                    isAbleDoublejump = true;
                    doubleJump = false;
                    
                }

                if (isAbleDoublejump) {
                /*if (Input.GetButtonDown("Jump") && jumpState == JumpState.Jumping)
                    {
                        doubleJump = true;
                        stopJump = false;
                        jumpState = JumpState.Jumping;
                        isAbleDoublejump = false;



                        //Schedule<PlayerStopJump>().player = this;

                    }*/

                    if (Input.GetButtonDown("Jump") && jumpState == JumpState.InFlight)
                    {
                        doubleJump = true;
                        isAbleDoublejump = false;
                        stopJump = false;
                        jumpState = JumpState.Jumping;
                        //Schedule<PlayerStopJump>().player = this;

                    }
                    /*else if (Input.GetButtonUp("Jump"))
                    {
                        stopJump = true;
                        Schedule<PlayerStopJump>().player = this;

                    }*/
                }


>>>>>>> Stashed changes
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;

                }
                
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:

                    /*if (isAbleDoublejump)
                    {
                        if (!IsGrounded && doubleJump)
                        {
                            doubleJump = true;
                            Schedule<PlayerJumped>().player = this;
                            jumpState = JumpState.Jumping;
                            //doubleJump = !doubleJump;
                            isAbleDoublejump = false;
                            print(isAbleDoublejump);

                        }
                    }*/
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }

                    break;
                case JumpState.InFlight:

                    if (isAbleDoublejump)
                    {
                        if (!IsGrounded && doubleJump)
                        {
                            doubleJump = true;
                            Schedule<PlayerJumped>().player = this;
                            jumpState = JumpState.Jumping;
                            //doubleJump = !doubleJump;
                            isAbleDoublejump = false;
                            print(isAbleDoublejump);
                        }
                    }
                    else if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                        isAbleDoublejump = true;
                        doubleJump = false;

                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    isAbleDoublejump = true;
                    doubleJump = false;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {

            if (jump)
            {
<<<<<<< Updated upstream
                if (IsGrounded)
                {
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                }
                else if (jumpState == JumpState.InFlight)
                {
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                    jump = false;
                }
            }

=======
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
                isAbleDoublejump = true;
            }

           if (isAbleDoublejump)
            {
                if (doubleJump && !IsGrounded)
                {
                    doubleJump = true;
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                    jump = false;
                    isAbleDoublejump = false;
                }
            }


>>>>>>> Stashed changes
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }


            /*
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            */



            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}