﻿using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
	public bool DebugMode;
	public float PlayerMoveSpeed;
	public float PlayerMoveReachDistance;
	public float ScrollSpeed;
	public float ColumnWidth;
	public int ColumnCount;
	public float FirstWaveOffset;
	public int InitPlayerHeartCount;
    public int MaxPlayerHeartCount;
	public float JumpDuration;
	//public Color[] PlayerColors;
	//public Color NeutralColor;
	public Sprite[] HazardSprites;
    public AudioClip[] HazardAudioSources;
    public RuntimeAnimatorController[] HazardAnimators;
	public WaveGroupPattern[] WaveGroupPatterns;
	public Sequence[] Sequences;

	public GameObject HazardPrefab;
	public GameObject WavePrefab;
	public GameObject SequenceTitlePrefab;

	public Sprite GetHazardSprite(Hazard.HazardType hazardType)
	{
		string hazardTypeName = hazardType.ToString();

		for (int i = 0; i < this.HazardSprites.Length; i++)
		{
			Sprite hazardSprite = this.HazardSprites[i];

			if (hazardSprite.name == hazardTypeName)
			{
				return hazardSprite;
			}
		}

		Debug.LogErrorFormat("Unable to find {0} hazard type sprite!", hazardTypeName);
		return null;
	}

    public AudioClip GetHazardAudio(string hazardTypeName)
    {
        for (int i = 0; i < this.HazardAudioSources.Length; i++)
        {
            AudioClip hazardAudio = this.HazardAudioSources[i];

            if (hazardAudio.name == hazardTypeName)
            {
                return hazardAudio;
            }
        }
        return null;
    }

    public RuntimeAnimatorController GetHazardAnimator(Hazard.HazardType hazardType)
    {
        string hazardTypeName = hazardType.ToString();

        for (int i = 0; i < this.HazardAnimators.Length; i++)
        {
            RuntimeAnimatorController hazardAnimator = this.HazardAnimators[i];

            if (hazardAnimator != null && hazardAnimator.name == hazardTypeName)
            {
                return hazardAnimator;
            }
        }

        //Debug.LogErrorFormat("Unable to find {0} hazard type animator!", hazardTypeName);
        return null;
    }
}
