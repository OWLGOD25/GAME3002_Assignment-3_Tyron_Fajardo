using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorControl : MonoBehaviour
{
    [SerializeField]
    private float m_dSpringConst = 0f;

    [SerializeField]
    private float m_ClosedPos = 0f;

    [SerializeField]
    private float m_OpenPos = 0f;

    [SerializeField]
    private float m_DoorSpringDamp = 0f;

    [SerializeField]
    private int keysNeeded = 1;

    [SerializeField]
    private GameObject popUpUI;

    // Local-space offset for where the player should respawn relative to the door.
    // Edit this in the Inspector (default moves the spawn 1 unit backward on the door's local Z).
    [SerializeField]
    private Vector3 spawnOffset = new Vector3(0f, -2f, 0f);

    private HingeJoint m_hingeJoint = null;

    private JointSpring m_jointSpring;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            OnDoorOpenedInternal();
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            OnDoorClosedInternal();
        }
    }

    private void Start()
    {
        m_hingeJoint = GetComponent<HingeJoint>();
        m_hingeJoint.useSpring = true;

        m_jointSpring = new JointSpring();
        m_jointSpring.spring = m_dSpringConst;
        m_jointSpring.damper = m_DoorSpringDamp;

        m_hingeJoint.spring = m_jointSpring;
    }
    private void OnDoorOpenedInternal()
    {
        m_jointSpring.targetPosition = m_OpenPos;
        m_hingeJoint.spring = m_jointSpring;
    }

    private void OnDoorClosedInternal()
    {
        m_jointSpring.targetPosition = m_ClosedPos;
        m_hingeJoint.spring = m_jointSpring;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<PlayerMovement>().keys != keysNeeded)
            {
                Debug.Log("Need KEY to open");
                popUpUI.SetActive(true);
            }
            else
            {
                OnDoorOpenedInternal();
                popUpUI.SetActive(false);

                // Move the player's spawn to this door's position (plus local offset) so the player will respawn here next time
                PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    // Use TransformPoint so spawnOffset is interpreted in the door's local space
                    Vector3 spawnWorldPos = transform.TransformPoint(spawnOffset);
                    playerMovement.SetSpawnPosition(spawnWorldPos);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        popUpUI.SetActive(false);
    }
}
