using UnityEngine;

[CreateAssetMenu]
public class WavePattern : ScriptableObject
{
	public Hazard.HazardType GroundLeft;
	public Hazard.HazardType GroundMiddle;
	public Hazard.HazardType GroundRight;
	public Hazard.HazardType AirLeft;
	public Hazard.HazardType AirMiddle;
	public Hazard.HazardType AirRight;
	public bool NoHazardHiding;
}
