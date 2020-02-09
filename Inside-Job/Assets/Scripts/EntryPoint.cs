﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessagesEntryPosition
{
    public GameObject messageSlot;
    [HideInInspector]
    public bool slotIsAquired = false;
}

public class EntryPoint : MonoBehaviour
{
    
    public enum EntryPointType
    {
        request,
        response
    }

    public EntryPointType entryPointType;


    public entry currentEntryPoint;
    
    public List<MessagesEntryPosition> messageEntryPos;
    public List<GameObject> arrayOfMessages = new List<GameObject>(1);


    private void Awake()
    {
        switch (entryPointType)
        {
            case EntryPointType.request:
                currentEntryPoint = new Request(messageEntryPos, arrayOfMessages);
                break;

            case EntryPointType.response:
                currentEntryPoint = new Response();
                break;

            default:
                break;
        }
    }

    


}

public class Request : entry
{
    List<MessagesEntryPosition> messageEntryPos;
    List<GameObject> arrayOfMessages;

    //public bool ListIsFull { get; set; }


    public Request(List<MessagesEntryPosition> messageSlotPos, List<GameObject> messageArray)
    {
        messageEntryPos = messageSlotPos;
        arrayOfMessages = messageArray;
    }

    



    public override void SetMessageToSlot(GameObject message)
    {
        for (int index = 0; index < messageEntryPos.Count; index++)
        {
            if (!messageEntryPos[index].slotIsAquired)
            {
                message.transform.position = messageEntryPos[index].messageSlot.transform.position;
                messageEntryPos[index].slotIsAquired = true;
                arrayOfMessages.Add(message);
                return;
            }


            if (messageEntryPos.IndexOf(messageEntryPos[index]) == 0)
            {
                ListIsFull = true;
            }
        }
    }

    public override void GiveMessageToPlayer(GameObject holder)
    {
        if (arrayOfMessages[0])
        {
            //move message to player
            arrayOfMessages[0].transform.parent = holder.transform;
            arrayOfMessages[0].transform.position = holder.transform.position;
            messageEntryPos[0].slotIsAquired = false;
            ListIsFull = false;
            //if second is aquired
            if (arrayOfMessages[1])
            {
                //moving second message to first slot
                arrayOfMessages[1].transform.position = messageEntryPos[0].messageSlot.transform.position;
                messageEntryPos[1].slotIsAquired = false;
                messageEntryPos[0].slotIsAquired = true;

                arrayOfMessages.Remove(arrayOfMessages[0]);

            }
            //if second is empty
            else
            {
                messageEntryPos[0].slotIsAquired = false;
                arrayOfMessages.Remove(arrayOfMessages[0]);
            }
        }
    }
}


public class Response: entry
{
    public override void SetMessageToSlot(GameObject message)
    {
        Debug.Log("Setting Response Message");
    }

    public override void GiveMessageToPlayer(GameObject holder)
    {
        Debug.Log("Taking Response Message");
    }
}



public class entry
{

    public bool ListIsFull;

    public entry()
    {
        ListIsFull = false;
    }

    public virtual void SetMessageToSlot(GameObject message)
    {
       
    }

    public virtual void GiveMessageToPlayer(GameObject holder)
    {

    }
}