using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum Type
    {
        A,
        B,
        C,
        D
    };

    /* -------------- 에너미 변수 -------------- */
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public int score;
    public GameManager manager;
    internal Transform target;
    public bool isChase;
    public BoxCollider meleeArea;
    public bool isAttack;
    public GameObject bullet;
    public GameObject[] coins;
    public bool isFirstC;
    public bool isDead;

    /* -------------- 선언 변수??? -------------- */
    internal Rigidbody _rigidbody;
    internal BoxCollider _boxCollider;
    internal MeshRenderer[] _meshRenderers;
    internal NavMeshAgent _nav;
    internal Animator _animator;
    public float aggroRange = 10f;
    /* -------------- 이동관련변수 -------------- */
    private bool _isWandering = false;
    public float wanderRadius = 10f;

    private bool _isAggro = false;
    private AggroPulling _aggroPulling;

    /* ------------------ 이벤트------------------ */
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _aggroPulling = GetComponentInChildren<AggroPulling>();

    }

    void Update()
    {
        if (_nav.enabled && enemyType != Type.D)
        {
            if (isChase)
            {
                _nav.SetDestination(target.position);
                _nav.isStopped = !isChase;

                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;


                // 플레이어와의 거리를 체크하여 일정 거리 이상 멀어지면 배회 시작
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if (distanceToPlayer > aggroRange)
                {
                    isChase = false;
                    isAttack = false;
                    _animator.SetBool("isAttack", false);
                    StartCoroutine(Wander());
                }
            }
            else
            {
                if (!_isWandering && !_isAggro)
                    StartCoroutine(Wander());
            }
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
        Targeting();

        
    }

    /* --------------- 기능 함수 --------------- */
    void FreezeVelocity()
    {
        
    }

    IEnumerator ChaseStart()
    {
        Debug.Log("ChaseStart called");
        yield return new WaitForSeconds(0.8f);
        isChase = true;
        _animator.SetBool("isWalk", true);
    }
    IEnumerator DelayedChaseStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine("ChaseStart");
    }
    /* -------------- 배회하는 변수 -------------- */
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander Start");
        _isWandering = true;

        while (!_isAggro)
        {
            // 플레이어를 감지하는 로직 추가
            Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange, LayerMask.GetMask("Player"));
            if (colliders.Length > 0)
            {
                Transform playerTransform = colliders[0].transform;

                // 배회 중 어그로가 없을 때 플레이어를 탐지하면 어그로 설정
                _aggroPulling.gameObject.SetActive(true);
                _aggroPulling.isAggro = true;
                _aggroPulling.SetTarget(playerTransform);

                // 배회 중 어그로가 없을 때 플레이어를 탐지하면 바로 공격
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null)
                {
                    yield return new WaitForSeconds(2f);
                    enemy.SetTarget(playerTransform);
                    StartCoroutine("ChaseStart");
                    
                }

                break; // 어그로가 설정되면 루프 종료
            }

            Vector3 newPosition = RandomNavSphere(transform.position, wanderRadius, -1);
            _nav.SetDestination(newPosition);
            _animator.SetBool("isWalk", true);

            yield return new WaitForSeconds(2f);
        }

        _isWandering = false;
    }

    /* -------------- 타겟 지정변수 -------------- */
    public void SetTarget(Transform newTarget)
    {
        Debug.Log("SetTarget called");
        target = newTarget;
        isChase = true;
        _animator.SetBool("isWalk", true);
        StartCoroutine("ChaseStart");
    }


    void ClearTarget()
    {
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttack", false);

        SetIsNavEnabled(false);

        if (_isAggro && _aggroPulling != null) // 어그로 풀링이 존재하는 경우에만 처리
            _aggroPulling.gameObject.SetActive(true);

        _isAggro = false;

        // 어그로가 없는 경우에만 배회
        if (!_isAggro)
            StartCoroutine(Wander());
    }

    /* -------------- 공격관련 변수 -------------- */
    void Targeting()
    {
        if (enemyType != Type.D && !isDead && isChase)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
                StartCoroutine("Attack");
                
        }
    }

    IEnumerator Attack()
    {
        
        isChase = false;
        isAttack = true;
        _animator.SetBool("isAttack", true);
        _isWandering = false;

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(1f);
                _rigidbody.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(2f);
                _rigidbody.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAttack = false;
        _animator.SetBool("isAttack", false);
        

    }

    /* -------------- 피격관련 변수 -------------- */
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌했을 때의 처리
            SetTarget(other.transform);
        }

        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in _meshRenderers)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in _meshRenderers)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (MeshRenderer mesh in _meshRenderers)
                mesh.material.color = Color.gray;

            gameObject.layer = 14;
            isDead = true;
            isChase = false;
            _nav.enabled = false;
            _animator.SetTrigger("doDie");

            Player player = target.GetComponent<Player>();
            player.score += score;

            int ranCoin = UnityEngine.Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

            switch (enemyType)
            {
                case Type.A:
                    manager.enemyCntA--;
                    break;
                case Type.B:
                    manager.enemyCntB--;
                    break;
                case Type.C:
                    manager.enemyCntC--;
                    break;
                case Type.D:
                    manager.enemyCntD--;
                    break;
            }

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                _rigidbody.freezeRotation = false;
                _rigidbody.AddForce(reactVec * 5, ForceMode.Impulse);
                _rigidbody.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                _rigidbody.AddForce(reactVec * 5, ForceMode.Impulse);
            }

            Destroy(gameObject, 4);
        }
    }

    /* -------------- 외부참조 함수 -------------- */
    public void SetIsNavEnabled(bool bol)
    {
        isChase = bol;
        _nav.enabled = bol;
    }

    
}
