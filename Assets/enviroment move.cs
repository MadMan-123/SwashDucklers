
using UnityEngine;

public class Enviromentmove : MonoBehaviour
{
    public bool isboat;
    public bool isisland;
    public bool issmall;

    public int boatspd;
    public int islandspd;
    public int smallspd;
    enviroment getspeed;
    
    private CharacterController envi;
    private float x;
    //private float y;
    
    // Start is called before the first frame update
    void Start()
    { 
        transform.Rotate(new Vector3(0,-90,0));
        getspeed = GameObject.FindWithTag("environment").GetComponent<enviroment>();
        envi = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
   public void Update()
    {
       
        
        
        if (issmall)
        {
            smallspd = getspeed.SpeedSmall;
            var move =  Time.deltaTime * smallspd;
            transform.Translate(new Vector3(0,0,move));
        }
        
        else
        if (isisland)
        {
            islandspd = getspeed.SpeedIsland;
            var move =  Time.deltaTime * islandspd;
            transform.Translate(new Vector3(0,0,move));
        }
        else
        if (isboat) 
        {
            
            boatspd = getspeed.SpeedBoat;
            var move =  Time.deltaTime * boatspd;
            transform.Translate(new Vector3(0,0,move));
            
                    
        }
    }
}
