using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float fMoveSpeed;
    [SerializeField]
    private SimpleJoyStick m_joyStick;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (m_joyStick.isMoved)
        {
            transform.forward = new Vector3(m_joyStick.xAxis, transform.forward.y, m_joyStick.yAxis);
            transform.position += new Vector3(m_joyStick.xAxis, 0, m_joyStick.yAxis) * fMoveSpeed *Time.deltaTime;
        }
	}
}
