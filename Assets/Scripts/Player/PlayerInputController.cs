using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

// Use a separate PlayerInput component for setting up input.
public class PlayerInputController : MonoBehaviour
{
    public float moveSpeed;
    public float burstSpeed;
    public GameObject projectile;

    private bool m_Charging;
    private Vector2 m_Move;

    public void Start()
    {
        foreach (var device in InputSystem.devices)
        {
            Debug.Log(device.name);
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        var move = context.ReadValue<Vector2>();
        m_Move = move;
        
        if (move.normalized.x != 0)
        {
            var direction = move.normalized.x / Math.Abs(move.normalized.x);
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (context.interaction is SlowTapInteraction)
                {
                    StartCoroutine(BurstFire((int)(context.duration * burstSpeed)));
                }
                else
                {
                    Fire();
                }
                m_Charging = false;
                break;

            case InputActionPhase.Started:
                if (context.interaction is SlowTapInteraction)
                    m_Charging = true;
                break;

            case InputActionPhase.Canceled:
                m_Charging = false;
                break;
        }
    }

    public void OnGUI()
    {
        if (m_Charging)
            GUI.Label(new Rect(100, 100, 200, 100), "Charging...");
    }

    public void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        Move(m_Move);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = new Vector3(direction.x, direction.y, 0);
        transform.position += move * scaledMoveSpeed;
    }

    private IEnumerator BurstFire(int burstAmount)
    {
        for (var i = 0; i < burstAmount; ++i)
        {
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Fire()
    { 
        var transform = this.transform;
        var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = transform.position + transform.forward * 0.6f;
        newProjectile.transform.rotation = transform.rotation;
        const int size = 1;
        newProjectile.transform.localScale *= size;
        
        var fireDir = Vector3.right * transform.localScale.x;
        newProjectile.GetComponent<Rigidbody2D>().mass = Mathf.Pow(size, 3);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(fireDir * 20f, ForceMode2D.Impulse);
        newProjectile.GetComponent<SpriteRenderer>().material.color =
            new Color(Random.value, Random.value, Random.value, 1.0f);
    }
}
