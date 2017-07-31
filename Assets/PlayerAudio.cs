using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private AudioSource source;
	[SerializeField] private AudioClip DeathSound;
	[SerializeField] private Vector2 DeathVolume;
	[SerializeField] private List<AudioClip> JumpSounds;
	[SerializeField] private Vector2 JumpVolume;

	[SerializeField] private AudioSource runSource;

	[SerializeField] private AudioClip RunSound;
	[SerializeField] private Vector2 RunVolume;

	void Start () {
	}

	public void PlayDeathSound(){
		source.PlayOneShot (DeathSound, Random.Range(DeathVolume.x, DeathVolume.y));
	}

	public void PlayJumpSound(){
		source.PlayOneShot (JumpSounds [Random.Range (0, JumpSounds.Count - 1)], Random.Range(JumpVolume.x, JumpVolume.y));
	}

	public void PlayRunSound(){
		runSource.PlayOneShot (RunSound, Random.Range(RunVolume.x, RunVolume.y));
	}
}
