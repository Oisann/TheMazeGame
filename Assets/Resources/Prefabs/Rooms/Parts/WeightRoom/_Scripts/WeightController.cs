using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Networking;
using System;

public class WeightController : NetworkBehaviour {

	public Text weightNumber;
	public Text targetWeightNumber;

	[SyncVar]
	public float currentWeight = 0f;
	[SyncVar]
	public float correctAnswer = -1f;
	public List<RockIndentifier> rocks = new List<RockIndentifier>();

	private int rockCount = -1;
    public List<Transform> sps = new List<Transform>();

    private System.Random rand;
    private RoomController rc;
    private bool isComplete = false;
	private List<RockIndentifier> currentRocks = new List<RockIndentifier>();
	private int currentRockCount = -1;
	private float currentWeightOffset = 0f;
	private float oldWeightOffset = 0f;
    
	void Start () {
        if(isServer) {
            //transform.name = transform.name.Split(" ".ToCharArray())[0];
            rc = transform.parent.GetComponentInParent<RoomController>();
            rand = new System.Random((transform.name + "" + rc.mazeSeed).GetHashCode());
            for(int i=0; i < transform.parent.GetChild(0).childCount; i++) {
                sps.Add(transform.parent.GetChild(0).GetChild(i));
            }
        }
    }
	
	void Update () {
		if(isServer) {
			if(rocks.Count != rockCount) {
                correctAnswer = 0f;
                List<RockIndentifier> r = new List<RockIndentifier>();
                r.AddRange(rocks);
                if(r.Count < 1)
                    return;
                int count = rand.Next(1, r.Count);
                for(int i=0; i < count; i++) {
                    int j = rand.Next(0, r.Count);
                    correctAnswer += r[j].weight;
                    r.RemoveAt(j);
                }
				rockCount = rocks.Count;
			}

			if(currentRockCount != currentRocks.Count || currentWeightOffset != oldWeightOffset) {
				currentWeight = currentWeightOffset;
				oldWeightOffset = currentWeightOffset;

				foreach(RockIndentifier r in currentRocks) {
					currentWeight += r.weight;
				}

				currentRockCount = currentRocks.Count;
			}
		}
        if(System.Math.Round(currentWeight, 0) == System.Math.Round(correctAnswer, 0)) {
            isComplete = true;
            weightNumber.color = Color.green;
            if(isServer)
                rc.puzzleComplete = isComplete;
        } else if(currentWeight < correctAnswer) {
            weightNumber.color = new Color(1f, (165f / 256f), 0f);
        } else if(currentWeight > correctAnswer) {
            weightNumber.color = Color.red;
        } else {
            weightNumber.color = Color.gray;
        }
		weightNumber.text = System.Math.Round(currentWeight, 0) + " KG";
		targetWeightNumber.text = System.Math.Round(correctAnswer, 0) + " KG";
	}

	void OnTriggerEnter(Collider other) {
        if(isComplete)
            return;
		if(other.name.StartsWith("Rock") && isServer) {
            //currentWeight = other.GetComponent<RockIndentifier> ().weight;
            //currentWeight += other.GetComponent<RockIndentifier>().weight;
			RockIndentifier r = other.GetComponent<RockIndentifier>();
			if(!currentRocks.Contains(r))
				currentRocks.Add(r);
		}

		if(other.CompareTag("Player") && isServer) {
			currentWeightOffset += other.GetComponent<PlayerWeight>().getWeight();
		}
	}

	void OnTriggerExit(Collider other) {
        if(isComplete)
            return;
        if (other.name.StartsWith("Rock") && isServer) {
			RockIndentifier r = other.GetComponent<RockIndentifier>();
			if(currentRocks.Contains(r))
				currentRocks.Remove(r);
		}

		if(other.CompareTag("Player") && isServer) {
			currentWeightOffset -= other.GetComponent<PlayerWeight>().getWeight();
		}
	}

    public void AddRock(RockIndentifier rock) {
        rocks.Add(rock);
        int i = rand.Next(0, sps.Count);
        rock.transform.position = sps[i].position;
        sps.Remove(sps[i]);
    }
}
