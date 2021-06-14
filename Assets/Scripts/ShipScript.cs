using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    //Turning Variables
    public float tilt;
    public float maxTilt;
    public float tiltBack;
    public float centeringTol;
   
    //Particle Variables
    public ParticleSystem ps;
    private ParticleSystem.VelocityOverLifetimeModule velModule;
    private ParticleSystem.EmissionModule emModule;
    private ParticleSystem.TrailModule trailModule;
    public Material newMaterial;
 
    public float particleTurn;
    public float particleTurnMax;
    // Start is called before the first frame update
    void Start()
    {
        //Assign Particule Modules
        velModule = ps.velocityOverLifetime;
        emModule = ps.emission;
        trailModule = ps.trails;
        
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Center();
        SpeedCheck();
    }

    void Inputs()
    { //Ship Controls
        if (Input.GetKey("a") && transform.rotation.z < maxTilt)
        {
            transform.Rotate(0f, 0f, tilt, Space.Self);
            //controls particle X Velocity
            if (velModule.xMultiplier < particleTurnMax)
                velModule.xMultiplier += particleTurn;
        }
        if (Input.GetKey("d") && transform.rotation.z > -maxTilt)
        {
            transform.Rotate(0f, 0f, -tilt, Space.Self);

            //controls particle -X velocity
            if (velModule.xMultiplier > -particleTurnMax)
                velModule.xMultiplier -= particleTurn;
        }
        if (Input.GetKey("w") && velModule.zMultiplier < 500f)
        {
            velModule.zMultiplier += 2;
        }
        if (Input.GetKey("s") && velModule.zMultiplier >30f)
        {
            velModule.zMultiplier -= 4;
            trailModule.ratio = 0;
        }
        //Debug. Center Key
        if (Input.GetKeyDown("c")) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("My rotation is now Zero");
        }
        if (Input.GetKey("h"))
        {
            trailModule.enabled = true;
            trailModule.ratio += 400f;
        }
    }

    void SpeedCheck()
        //prevents stars from having negative velocity
    {
        if (velModule.zMultiplier < 0)
            velModule.zMultiplier = 0;

        if (velModule.zMultiplier == 0)
            emModule.enabled = false;
        else
        {
            emModule.enabled = true;
            emModule.rateOverTime = velModule.zMultiplier;
        }

    }

    void Center()
    {
        //tests current rotation.z and centers to zero
        if (transform.rotation.z > 0)
        {
            transform.Rotate(0f, 0f, -tiltBack , Space.Self);
        }
        if (transform.rotation.z < 0)
        {
            transform.Rotate(0f, 0f, tiltBack, Space.Self);
        }
        //checks for smallest/largest number to center from
        if (transform.rotation.z > -centeringTol && transform.rotation.z < centeringTol)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // Recdenter our particle X velocity
        if (velModule.xMultiplier > 0 && transform.rotation.z < maxTilt )
            velModule.xMultiplier -= 1;
        else if (velModule.xMultiplier < 0 && transform.rotation.z > -maxTilt)
            velModule.xMultiplier += 1;
    }
    
}
