using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Linq;

public class RealBlockBehavior : BlockBehavior {
    public float Multiplier;

    private Rigidbody Rigidbody;
    private Destructable Destructable;
    public override void Start ()
    {
        base.Start();

        Rigidbody = GetComponent<Rigidbody>();
        Destructable = GetComponent<DestructableBlock>();
        if (Destructable == null)
        {
            Destructable = GetComponent<Destructable>();
        }

        InitializeMass();
        InitializeHealth();
    }

    void FixedUpdate()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
            OnDestroyed();
        }
    }

    public float Health
    {
        get
        {
            return Destructable != null ? Destructable.Health : 0.0f;
        }
    }

    private float _initialHealth;
    public float InitialHealth
    {
        get
        {
            return _initialHealth;
        }
    }

    public float Mass
    {
        get
        {
            return Rigidbody != null ? Rigidbody.mass : 0.0f;
        }
    }

    private void InitializeMass()
    {
        if (Rigidbody != null)
        {
            float mass = Rigidbody.mass * Multiplier;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(Player).State.FindUpgradesWithOption("AffectsBlockMass"))
            {
                if (upgradeOptions["AffectsBlockMass"].Split(' ').Contains(Block.Type.ToString()))
                {
                    mass *= float.Parse(upgradeOptions["MassModifier"]);
                }
            }
            Rigidbody.mass = mass;
        }
    }

    private void InitializeHealth()
    {
        if (Destructable != null)
        {
            float health = Destructable.Health * Multiplier;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(Player).State.FindUpgradesWithOption("AffectsBlockHealth"))
            {
                if (upgradeOptions["AffectsBlockHealth"].Split(' ').Contains(Block.Type.ToString()))
                {
                    health *= float.Parse(upgradeOptions["HealthModifier"]);
                }
            }
            _initialHealth = health;
            Destructable.Health = health;
        }
    }
}
