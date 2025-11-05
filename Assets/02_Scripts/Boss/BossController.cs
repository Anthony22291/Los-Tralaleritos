using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;
    public Transform whipOrigin;
    public GameObject whipWarningPrefab;
    public GameObject whipHitboxPrefab;
    public GameObject spikeWarningPrefab;
    public GameObject groundSpikePrefab;
    public GameObject rockShardPrefab;

    [Header("Stats")]
    public float moveSpeed = 2f;
    public float attackDecisionCooldown = 2f;

    [Header("Jump Attack")]
    public float jumpWindupTime = 0.6f;
    public float jumpApexTime = 0.3f;
    public float jumpLandRadius = 1.5f;
    public int jumpDamage = 20;
    public int rockShardsCount = 5;
    public float rockShardsRadius = 2.5f;

    [Header("Whip Attack")]
    public float whipWarningTime = 0.6f;
    public float whipRange = 3f;
    public float whipWidth = 0.5f;
    public int whipDamage = 10;

    [Header("Spike Attack")]
    public float spikeWarningTime = 0.8f;
    public int spikeCount = 4;
    public float spikeRadiusAroundPlayer = 2f;
    public int spikeDamage = 15;

    [Header("Regen")]
    public TreeRegen[] regenTrees;
    public float regenPerTick = 2f;
    public float regenTickTime = 2f;
    private float regenTimer;

    private Rigidbody2D rb;
    private bool isBusy;
    private float decisionTimer;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        if (player == null) return;
        HandleRegenFromTrees();

        if (isBusy) return;

        MoveTowardsPlayer();

        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            decisionTimer = attackDecisionCooldown;
            StartCoroutine(DoRandomAttack());
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;
    }

    void HandleRegenFromTrees()
    {
        regenTimer -= Time.deltaTime;
        if (regenTimer <= 0f)
        {
            regenTimer = regenTickTime;
            bool anyTreeAlive = false;

            foreach (var t in regenTrees)
            {
                if (t != null && t.IsAlive)
                {
                    anyTreeAlive = true;
                    break;
                }
            }

            if (anyTreeAlive)
            {
                // Curar al boss
                BossHealth health = GetComponent<BossHealth>();
                if (health != null)
                {
                    health.Heal((int)regenPerTick);
                }
            }
        }
    }

    IEnumerator DoRandomAttack()
    {
        isBusy = true;
        rb.velocity = Vector2.zero;

        int choice = Random.Range(0, 3);

        switch (choice)
        {
            case 0:
                yield return StartCoroutine(WhipAttack());
                break;
            case 1:
                yield return StartCoroutine(SpikeAttack());
                break;
            case 2:
                yield return StartCoroutine(JumpAttack());
                break;
        }

        isBusy = false;
    }

    // ==== Látigo ====
    IEnumerator WhipAttack()
    {
        Vector2 dir = (player.position - whipOrigin.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        GameObject warn = Instantiate(whipWarningPrefab, whipOrigin.position, rot);
        warn.transform.localScale = new Vector3(whipRange, whipWidth, 1f);

        yield return new WaitForSeconds(whipWarningTime);
        Destroy(warn);

        GameObject whip = Instantiate(whipHitboxPrefab, whipOrigin.position, rot);
        whip.transform.localScale = new Vector3(whipRange, whipWidth, 1f);

        var hit = whip.GetComponent<TempAttackHitbox>();
        if (hit != null) hit.damage = whipDamage;
    }

    // ==== Espinas ====
    IEnumerator SpikeAttack()
    {
        Vector2 playerPos = player.position;
        Vector2[] spawnPoints = new Vector2[spikeCount];

        for (int i = 0; i < spikeCount; i++)
        {
            float ang = (360f / spikeCount) * i;
            Vector2 offset = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad))
                             * spikeRadiusAroundPlayer;

            spawnPoints[i] = playerPos + offset;
            Instantiate(spikeWarningPrefab, spawnPoints[i], Quaternion.identity);
        }

        yield return new WaitForSeconds(spikeWarningTime);

        foreach (var pos in spawnPoints)
        {
            GameObject spike = Instantiate(groundSpikePrefab, pos, Quaternion.identity);
            var atk = spike.GetComponent<TempAttackHitbox>();
            if (atk != null) atk.damage = spikeDamage;
        }

        foreach (var w in GameObject.FindGameObjectsWithTag("SpikeWarning"))
            Destroy(w);
    }

    // ==== Salto ====
    IEnumerator JumpAttack()
    {
        rb.velocity = Vector2.zero;
        Vector2 targetPos = player.position;
        GameObject warn = Instantiate(spikeWarningPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(jumpWindupTime);
        transform.position = targetPos;
        Destroy(warn);
        yield return new WaitForSeconds(jumpApexTime);

        DoAreaDamage(transform.position, jumpLandRadius, jumpDamage);
        SpawnRockShards(transform.position);
    }

    void DoAreaDamage(Vector2 center, float radius, int dmg)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        foreach (var h in hits)
        {
            if (h.CompareTag("Player"))
            {
                PlayerHealth ph = h.GetComponent<PlayerHealth>();
                if (ph != null) ph.TakeDamage(dmg);
            }
        }
    }

    void SpawnRockShards(Vector2 center)
    {
        for (int i = 0; i < rockShardsCount; i++)
        {
            float ang = Random.Range(0f, 360f);
            Vector2 offset = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad))
                             * Random.Range(0.5f, rockShardsRadius);

            GameObject shard = Instantiate(rockShardPrefab, center + offset, Quaternion.identity);
            var atk = shard.GetComponent<TempAttackHitbox>();
            if (atk != null) atk.damage = jumpDamage;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpLandRadius);
    }
}

