using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vektor {
    /// <summary>
    /// Returns the force needed to stop the rigidbody within the given distance
    /// </summary>
    /// <param name="mass"></param>
    /// <param name="velocity"></param>
    /// <param name="stopDistance"></param>
    /// <returns></returns>
    public static float GetDampForce (float mass, float velocity, float stopDistance) {
        return (0.5f * mass * -velocity) / stopDistance;
    }


    private static playerHandler _playerHandler;

    public static void initHandler(playerHandler _player) {
        _playerHandler = _player;
    }

    public static playerHandler getHandler() {
        return _playerHandler;
    }
}

//Interface for objects which depend on a switch, button, etc
public interface ISwitchable {
    void OnSwitch(bool switchState);
}

//Enums for Platforms
[System.Serializable]
public enum PathMode {
    EndToEnd,
    Loop
}

[System.Serializable]
public enum BehaviorMode {
    Autonomous,
    AdvanceOnEvent,
    AdvancedOnCondition
}
