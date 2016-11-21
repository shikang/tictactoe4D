using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour
{
    public float hoverSpeed;
    public float hoverHeight;
    float angle = 0;
    float angleX = 0;
    // Use this for initialization
    Vector3 tmp;
    float startY;
    float halfSpeed;
    float halfHeight;
    float startX;	
    bool shakeEnd = false;

    public float duration;
    float reduc;
    void Start()
    {
    	halfSpeed = hoverSpeed/2;
    	halfHeight = hoverHeight;
        UpdateYpos(transform.localPosition.y);
		UpdateXpos(transform.localPosition.x);
		angle = Random.Range(0,360);
		angleX = Random.Range(0,360);
		reduc = hoverSpeed /duration;
    }

    // Update is called once per frame
    void Update()
    {
    	/*if(Input.GetKeyUp(KeyCode.P))
    	{
    		StartShaking();
    	}*/
    	if (hoverSpeed >0 /*duration > 0 */)
    	{
        	tmp = transform.localPosition;
        	tmp.y = startY + Mathf.Sin(angle) * hoverHeight;
			tmp.x = startX + Mathf.Cos(angleX) * hoverHeight;
			angle += Random.Range(halfSpeed,hoverSpeed) * Time.deltaTime;
			angleX += Random.Range(halfSpeed,hoverSpeed) * Time.deltaTime;
        	transform.localPosition = tmp;
        	hoverSpeed -= Time.deltaTime * reduc;
        	if(hoverSpeed <= 0)
        		hoverSpeed = 0;
        }
        else if ( hoverSpeed == 0 )
        {
        	hoverSpeed = -1;
        	Reset();
        	shakeEnd = true;
        }
    }
    public void UpdateYpos(float y)
    {
        startY = y;
    }
	public void UpdateXpos(float x)
    {
        startX = x;
    }
    public float getYPos()
    {
        return startY;
    }
    public void Reset()
    {
    	tmp = transform.localPosition;
    	tmp.x = startX;
    	tmp.y = startY;
    	transform.localPosition = tmp;
    }
    public bool IsShakeComplete()
    {	
    	if(shakeEnd == true)
    	{
    		shakeEnd = false;
    		return true;
    	}
    	return shakeEnd;
    }
    public void StartShaking()
    {
    	shakeEnd = false;
    	//duration = 2;
		hoverSpeed = 160;
    	halfSpeed = hoverSpeed/2;
		reduc = hoverSpeed /duration;
		angle = Random.Range(0,360);
		angleX = Random.Range(0,360);
    }
	public void StartShaking(float dur)
    {
    	shakeEnd = false;
    	duration = dur;
		hoverSpeed = Random.Range(60,101);
    	halfSpeed = hoverSpeed/2;
		reduc = hoverSpeed /duration;
		angle = Random.Range(0,360);
		angleX = Random.Range(0,360);
    }
}

