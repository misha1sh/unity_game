﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateToCamera : MonoBehaviour {
    public Vector3 qwte;
    private void LateUpdate() {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
