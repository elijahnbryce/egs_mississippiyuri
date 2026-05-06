using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class OrbitingPlayerController : MonoBehaviour
{
    [SerializeField] private OrbittingPlayer firePlayer, waterPlayer;
    [SerializeField] private GameObject critjectilePrefab;
    [SerializeField, Range(0, 180)] private float degreeMargin = 45;
    private float dotThreshold;

    private void Reset()
    {
        // Reset is called when the component is first added or manually reset in Inspector
        var children = GetComponentsInChildren<OrbittingPlayer>();

        if (children.Length >= 2)
        {
            firePlayer = children[0];
            waterPlayer = children[1];
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(firePlayer, $"FirePlayer is missing on {name}!");
        Assert.IsNotNull(waterPlayer, $"WaterPlayer is missing on {name}!");
    }

    private void OnValidate()
    {
        dotThreshold = Mathf.Cos(degreeMargin * Mathf.Deg2Rad);
    }

    private void Update()
    {
        GetInput();
        HandleShooting();
    }

    // Shooting 

    private void HandleShooting()
    {
        if (Keyboard.current == null)
            return;


        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {

            bool crit = IsFacingSameDirection(waterPlayer.transform, firePlayer.transform);
            bool a = waterPlayer.Psh.TryShoot(!crit);
            bool b = firePlayer.Psh.TryShoot(!crit);

            if (crit && a && b)
            {
                Vector3 spawnPos = (waterPlayer.FirePoint.position + firePlayer.FirePoint.position) / 2f;
                Vector2 middleDir = (waterPlayer.transform.up + firePlayer.transform.up).normalized;
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, middleDir);
                GameObject proj = Instantiate(critjectilePrefab, spawnPos, spawnRotation);
            }
        }
    }

    // Input per dragon type

    private void GetInput()
    {
        if (Keyboard.current == null)
            return;

        // WASD controls
        if (Keyboard.current.aKey.isPressed)
            waterPlayer.HandleMovement(1f);

        if (Keyboard.current.dKey.isPressed)
            waterPlayer.HandleMovement(-1f);

        // arrow controls
        if (Keyboard.current.leftArrowKey.isPressed)
            firePlayer.HandleMovement(1f);

        if (Keyboard.current.rightArrowKey.isPressed)
            firePlayer.HandleMovement(-1f);

    }

    public bool IsFacingSameDirection(Transform objA, Transform objB)
    {
        Vector2 dirA = objA.right;
        Vector2 dirB = objB.right;

        float dot = Vector2.Dot(dirA, dirB);

        return dot >= dotThreshold;
    }
}
