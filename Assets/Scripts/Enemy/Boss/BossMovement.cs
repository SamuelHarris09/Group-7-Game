using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed1 = 20f;
    [SerializeField] float moveSpeed2 = 20f;
    [SerializeField] float moveSpeed3 = 20f;
    [SerializeField] float hitWindow1 = 5f;
    [SerializeField] float hitWindow2 = 5f;
    [SerializeField] float hitWindow3 = 5f;

    [Header("Telegraph")]
    [SerializeField] private GameObject telegraphPrefab;
    [SerializeField] private float telegraphDuration = 0.6f;
    [SerializeField] private Vector2 telegraphScale = new Vector2(3f, 0.5f);

    [Header("Screen Shake")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.3f;

    [Header("Boss defeated")]
    [SerializeField] private GameObject finnishMenu;
    [SerializeField] private Sprite[] bossDamage;

    [Header("Wave 1")]
    [SerializeField] private GameObject handWave1;
    [SerializeField] private LineRenderer armLine;
    [SerializeField] private GameObject Wave1Platforms;
    [SerializeField] private Transform[] wave1TargetPos;
    [SerializeField] private int wave1Repetitons = 3;
    [SerializeField] float stopDuration = 1.5f;
    [SerializeField] float sweepDistance = 30.5f;

    [Header("Wave 2")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [SerializeField] private Transform[] leftPositions;
    [SerializeField] private Transform[] rightPositions;

    [SerializeField] private GameObject Wave2Platforms;
    [SerializeField] private int wave2Repetitions = 3;
    [SerializeField] private float wave2WaitTime = 1.5f;
    [SerializeField] private float wave2RepetTime = 1.5f;

    [Header("Wave 3")]
    [SerializeField] private GameObject handWave3;
    [SerializeField] private Transform[] wave3TargetPos;
    [SerializeField] private int wave3Repetitions = 3;
    [SerializeField] private float wave3SweepDistance = 30.5f;
    [SerializeField] private float wave3Delay = 0.3f;
    [SerializeField] private float fakeOutDelay = 1f;

    private bool damageWindowActive = false;
    private bool wasHitThisCycle = false;
    [SerializeField] private BossState currentState = BossState.Wave1;
    [SerializeField] private float startDelay = 3f;
    
    SpriteRenderer bossBody;

    private void Start()
    {
        StartCoroutine(BossLoop());
        Wave1Platforms.SetActive(true);
        Wave2Platforms.SetActive(false);

        bossBody = GetComponent<SpriteRenderer>();
    }

    public enum BossState
    {
        Wave1,
        Wave2,
        Wave3
    }

    IEnumerator BossLoop()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Wave1:
                    yield return StartCoroutine(Wave1());
                    break;

                case BossState.Wave2:
                    yield return StartCoroutine(Wave2());
                    break;

                case BossState.Wave3:
                    yield return StartCoroutine(Wave3());
                    break;
            }

            yield return null;
        }
    }

    #region Wave 1
    IEnumerator Wave1()
    {
        yield return new WaitForSeconds(startDelay);
        SetWave1PlatformActive(true);
        SetWave2PlatformActive(false);

        SetHandActive(true);

        wasHitThisCycle = false;
        
        for (int i = 0; i < wave1Repetitons; i++)
        {
            Transform target = wave1TargetPos[Random.Range(0, wave1TargetPos.Length)];

            Vector2 center = target.position;

            Vector2 leftPos = center + Vector2.left * sweepDistance;
            Vector2 rightPos = center + Vector2.right * sweepDistance;

            yield return StartCoroutine(MoveToPosition(handWave1.transform, leftPos));
            yield return StartCoroutine(MoveToPosition(handWave1.transform, rightPos));

            yield return new WaitForSeconds(stopDuration);
        }

        damageWindowActive = true;
        float timer = 0f;

        while (timer < hitWindow1)
        {
            if (wasHitThisCycle)
            {
                damageWindowActive = false;

                handWave1.SetActive(false); 

                currentState = BossState.Wave2;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        damageWindowActive = false;
        currentState = BossState.Wave1;
    }

    IEnumerator MoveToPosition(Transform hand, Vector2 target)
    {
        Vector2 start = hand.position;

        float distance = Vector2.Distance(start, target);
        float duration = distance / moveSpeed1;

        float time = 0;

        while (time < duration)
        {
            float t = time / duration;
            t = 1 - Mathf.Pow(1 - t, 3);

            hand.position = Vector2.Lerp(start, target, t);

            time += Time.deltaTime;
            yield return null;
        }

        hand.position = target;
    }
    #endregion
    #region Wave 2
    IEnumerator Wave2()
    {
        bossBody.sprite = bossDamage[0];
        SetWave1PlatformActive(false);
        SetWave2PlatformActive(true);

        SetHandActive(false);

        wasHitThisCycle = false;

        while (!wasHitThisCycle)
        {
            for (int i = 0; i < wave2Repetitions; i++)
            {
                yield return StartCoroutine(MoveHands(leftPositions[1].position, rightPositions[1].position));
                yield return new WaitForSeconds(wave2WaitTime);

                yield return StartCoroutine(MoveHands(leftPositions[2].position, rightPositions[2].position));
                yield return new WaitForSeconds(wave2RepetTime);

                yield return StartCoroutine(MoveHands(leftPositions[3].position, rightPositions[3].position));
                yield return new WaitForSeconds(wave2WaitTime);
            }

            damageWindowActive = true;

            float timer = 0f;

            while (timer < hitWindow2)
            {
                if (wasHitThisCycle)
                {
                    damageWindowActive = false;
                    currentState = BossState.Wave3;
                    yield break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            damageWindowActive = false;
        }
    }

    IEnumerator MoveHands(Vector2 leftTarget, Vector2 rightTarget)
    {
        while (Vector2.Distance(leftHand.position, leftTarget) > 0.05f ||
               Vector2.Distance(rightHand.position, rightTarget) > 0.05f)
        {
            leftHand.position = Vector2.MoveTowards(
                current: leftHand.position,
                target: leftTarget,
                maxDistanceDelta: moveSpeed2 * Time.deltaTime
            );

            rightHand.position = Vector2.MoveTowards(
                current: rightHand.position,
                target: rightTarget,
                maxDistanceDelta: moveSpeed2 * Time.deltaTime
            );

            yield return null;
        }
    }
    #endregion
    #region Wave 3
    IEnumerator Wave3()
    {
        bossBody.sprite = bossDamage[1];
        SetWave1PlatformActive(false);
        SetWave2PlatformActive(true);

        SetHandActive(false);

        wasHitThisCycle = false;

        for (int i = 0; i < wave3Repetitions; i++)
        {
            Transform target = wave3TargetPos[Random.Range(0, wave3TargetPos.Length)];
            Vector2 center = target.position;

            Vector2 leftPos = center + Vector2.left * wave3SweepDistance;
            Vector2 rightPos = center + Vector2.right * wave3SweepDistance;

            yield return StartCoroutine(HandSweep(leftPos, rightPos));
            yield return StartCoroutine(OffsetSlams());

            yield return new WaitForSeconds(1f);
        }

        damageWindowActive = true;
        float timer = 0f;

        while (timer < hitWindow3)
        {
            if(wasHitThisCycle && timer < fakeOutDelay)
            {
                damageWindowActive = false;

                Vector2 punishLeft = handWave3.transform.position + Vector3.left * wave3SweepDistance;
                Vector2 punishRigth = handWave3.transform.position + Vector3.right * wave3SweepDistance;

                yield return StartCoroutine(HandSweep(punishLeft, punishRigth));

                wasHitThisCycle = false;
                damageWindowActive = true;

                timer = fakeOutDelay;
            } 

            if (wasHitThisCycle && timer > fakeOutDelay)
            {
                damageWindowActive = false;
                
                BossDefeated();
                
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        damageWindowActive = false;
        currentState = BossState.Wave3;
    }

    IEnumerator HandSweep(Vector2 from, Vector2 to)
    {
        //yield return StartCoroutine(ShowTelegraph(from));

        handWave3.transform.position = from;

        yield return StartCoroutine(MoveToPosition(handWave3.transform, to));

        StartCoroutine(ScreenShake(shakeDuration, shakeMagnitude));
    }

    IEnumerator MoveSingleHand(Transform handTransform, Vector2 target)
    {
        while (Vector2.Distance(handTransform.position, target) > 0.05f)
        {
            handTransform.position = Vector2.MoveTowards(
                handTransform.position, target, moveSpeed3 * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator OffsetSlams()
    {
        Coroutine left = StartCoroutine(MoveSingleHand(leftHand, leftPositions[1].position));

        yield return new WaitForSeconds(wave3Delay);

        Coroutine right = StartCoroutine(MoveSingleHand(rightHand, rightPositions[1].position));

        yield return left;
        yield return right;

        StartCoroutine(ScreenShake(shakeDuration, shakeMagnitude));

        yield return new WaitForSeconds(0.2f);

        left = StartCoroutine(MoveSingleHand(leftHand, leftPositions[2].position));

        yield return new WaitForSeconds(wave3Delay);

        right = StartCoroutine(MoveSingleHand(rightHand, rightPositions[2].position));

        yield return left;
        yield return right;

        StartCoroutine(ScreenShake(shakeDuration, shakeMagnitude));
    }
    #endregion
    #region Visual Affects
    IEnumerator ShowTelegraph(Vector2 position)
    {
        if (telegraphPrefab == null)
            yield break;

        GameObject tele = Instantiate(telegraphPrefab, position, Quaternion.identity);
        tele.transform.localScale = telegraphScale;

        yield return new WaitForSeconds(telegraphDuration);

        Destroy(tele);
    }

    IEnumerator ScreenShake(float duration, float magnitude)
    {
        if (cameraTransform == null)
            yield break;

        Vector3 originalPos = cameraTransform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
    #endregion
    #region Remove this before making a build!

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SetWave(BossState.Wave1);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SetWave(BossState.Wave2);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            SetWave(BossState.Wave3);
    }

    public void SetWave(BossState newState)
    {
        StopAllCoroutines();
        currentState = newState;
        StartCoroutine(BossLoop());
    }
    #endregion

    void SetHandActive(bool active)
    {
        handWave1.SetActive(active);

        if (armLine != null)
            armLine.enabled = active;
    }

    void SetWave1PlatformActive(bool active)
    {
        Wave1Platforms.SetActive(active);
    }

    void SetWave2PlatformActive(bool active)
    {
        Wave2Platforms.SetActive(active);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerIndexC = LayerMask.NameToLayer("HitBox");

        if (other.gameObject.layer == layerIndexC)
        {
            TakeHit();
        }
    }

    public void TakeHit()
    {
        if (!damageWindowActive)
            return;

        wasHitThisCycle = true;
    }

    private void BossDefeated()
    {
        Time.timeScale = 0;
        bossBody.enabled = false;
        finnishMenu.SetActive(true);
    }
}