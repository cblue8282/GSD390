using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PTURN, ETURN, W, L }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerGO;          // assign the actual player GameObject in scene
    public GameObject enemyPF;
    public Transform playerLoc;
    public Transform enemyLoc;

    public BattleState state;
    Fighter playerF;
    Fighter enemyF;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public TextMeshProUGUI statetext;

    private Animator playerAnim;
    private Vector2 playerStartPos;
    public Transform playerSprite;
    public Transform enemySprite;

    void Start()
    {
        state = BattleState.START;

        playerF = playerGO.GetComponent<Fighter>();
        playerAnim = playerGO.GetComponentInChildren<Animator>();
        playerStartPos = playerGO.transform.localPosition;

        // Start the battle
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerF.currHp = playerF.maxHp;
        playerHUD.SetHUD(playerF);

        GameObject enemyGO = Instantiate(enemyPF, enemyLoc);
        enemyF = enemyGO.GetComponent<Fighter>();
        enemyHUD.SetHUD(enemyF);

        statetext.text = "";

        yield return null;

        state = BattleState.PTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        if (playerAnim != null)
            playerAnim.SetTrigger("Attack");

        yield return StartCoroutine(SlideTransform(playerSprite, new Vector2(1f, 0), 0.3f));

        yield return new WaitForSeconds(0.3f);

        bool isDead = enemyF.TakeDamage(Random.Range(3, 10));
        enemyHUD.SetHP(enemyF.currHp);

        if (isDead)
        {
            state = BattleState.W;
            EndBattle();
        }
        else
        {
            yield return new WaitForSeconds(1);
            state = BattleState.ETURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        if (playerAnim != null)
            playerAnim.SetTrigger("Hurt");

        // Slide enemy sprite forward/back
        yield return StartCoroutine(SlideTransform(enemySprite, new Vector2(-1f, 0), 0.3f));

        bool isDead = playerF.TakeDamage(Random.Range(3, 10));
        playerHUD.SetHP(playerF.currHp);

        if (isDead)
        {
            state = BattleState.L;
            EndBattle();
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            state = BattleState.PTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerHeal()
    {
        if (playerAnim != null)
            playerAnim.SetTrigger("Heal");

        yield return StartCoroutine(SlideTransform(playerSprite, new Vector2(-0.5f, 0), 0.2f));

        playerF.Heal(Random.Range(3, 10));
        playerHUD.SetHP(playerF.currHp);

        yield return new WaitForSeconds(1);

        state = BattleState.ETURN;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator SlideTransform(Transform t, Vector2 offset, float duration)
    {
        Vector2 startPos = t.localPosition;
        Vector2 targetPos = startPos + offset;
        float elapsed = 0f;

        // Slide forward
        while (elapsed < duration)
        {
            t.localPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.localPosition = targetPos;

        yield return new WaitForSeconds(0.1f);

        // Slide back
        elapsed = 0f;
        while (elapsed < duration)
        {
            t.localPosition = Vector2.Lerp(targetPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.localPosition = startPos;
    }

    void EndBattle()
    {
        if (state == BattleState.W)
            statetext.text = "WIN";
        else if (state == BattleState.L)
            statetext.text = "LOSS";
    }

    void PlayerTurn() { }

    public void OnAttackButton()
    {
        if (state != BattleState.PTURN) return;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PTURN) return;
        StartCoroutine(PlayerHeal());
    }

    public void OnRestartButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
