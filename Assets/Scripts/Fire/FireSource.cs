using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSource : Fire {

    [Range(0.0f, 10.0f)]
    public int range = 5;

    public override void SpreadFire()
    {
        List<Vector3> upSpawns = new List<Vector3>();
        List<Vector3> downSpawns = new List<Vector3>();
        for(int i = 1; i <= range; i++)
        {
            Vector3 x_neg_offset = transform.position + new Vector3(-offset * i, raySpawnOffset, 0);
            Vector3 x_pos_offset = transform.position + new Vector3(offset * i, raySpawnOffset, 0);
            Vector3 z_neg_offset = transform.position + new Vector3(0, raySpawnOffset, -offset * i);
            Vector3 z_pos_offset = transform.position + new Vector3(0, raySpawnOffset, offset * i);
            Vector3 y_pos_offset = transform.position + new Vector3(0, offset * i, 0);
            Vector3 y_neg_offset = transform.position + new Vector3(0, -offset * i, 0);

            downSpawns.Add(x_neg_offset);
            downSpawns.Add(x_pos_offset);
            downSpawns.Add(z_neg_offset);
            downSpawns.Add(z_pos_offset);
            downSpawns.Add(y_neg_offset);
            upSpawns.Add(y_pos_offset);
        }

        SpawnFire(downSpawns, upSpawns, true);
    }
}