using UnityEngine;

public class PlayerStats : MonoBehaviour
{
[Header("Stats")]
public float Oxygen;
public float OxygenUsageSpeed;
public float OxygenRegenSpeed;
public float Paranoia;
public float ParanoiaUsageSpeed;
public float ParanoiaRegenSpeed;
[Header("Limits (Oxygen Limit is 100f)")]
public float ParanoiaHardLimit;
public float OxygenUsageSpeedHardLimit;
[Header("Bools")]
public bool IsThereOxygenAround;
public bool IsDead;
public bool IsParanoid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsDead){
            
            if(Paranoia > 100f && OxygenUsageSpeed < OxygenUsageSpeedHardLimit)
            {
                OxygenUsageSpeed += ParanoiaUsageSpeed*0.1f*Time.deltaTime; // Oxygen Usage rises if paranoia is more than 100f
                if(OxygenUsageSpeed > OxygenUsageSpeedHardLimit){OxygenUsageSpeed = OxygenUsageSpeedHardLimit;}
            }
            else if (Paranoia < 101f)
            {
                OxygenUsageSpeed = 1f;
            }
            if (IsThereOxygenAround){
                if(Oxygen < 100f){Oxygen += OxygenRegenSpeed * Time.deltaTime;} //Replenishes Oxygen if there's oxygen around
                else if(Oxygen > 100f){Oxygen = 100f;} // Limits max oxygen to 100f
                if (!IsParanoid){ //If the environment is safe
                    if(Paranoia < 0f){
                        Paranoia = 0f; // If paranoia is at its lowest, hard limit it to 0
                    }
                    else if (Paranoia > 0f){
                        Paranoia -= ParanoiaRegenSpeed * Time.deltaTime; // Increases Sanity if the environment is safe
                    }
                }
            }
            else
            {
                Oxygen -= OxygenUsageSpeed * Time.deltaTime; //Oxygen Drain
            }
            if(Oxygen < 0f)
            {
            IsDead = true; //IsDead Check
            }
            if(Paranoia < ParanoiaHardLimit && IsParanoid){Paranoia += ParanoiaUsageSpeed * Time.deltaTime;}
            else if(Paranoia > ParanoiaHardLimit){Paranoia = ParanoiaHardLimit;}
        }
    }
}