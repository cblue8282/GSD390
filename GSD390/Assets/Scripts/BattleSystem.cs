using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PTURN, ETURN, W, L }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPF;
    public GameObject enemyPF;
    public Transform playerLoc;
    public Transform enemyLoc;
    public BattleState state;
    Fighter playerF;
    Fighter enemyF;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public TextMeshProUGUI statetext;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPF, playerLoc);
        playerF = playerGO.GetComponent<Fighter>();

        GameObject enemyGO = Instantiate(enemyPF, enemyLoc);
        enemyF = enemyGO.GetComponent<Fighter>();

        statetext.text = "";

        playerHUD.SetHUD(playerF);
        enemyHUD.SetHUD(enemyF);

        yield return new WaitForSeconds(0);

        state = BattleState.PTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyF.TakeDamage(Random.Range(3, 10));
        enemyHUD.SetHP(enemyF.currHp);

        yield return new WaitForSeconds(0);

        if (isDead)
        {
            state = BattleState.W;
            EndBattle();
        } else
        {
            state = BattleState.ETURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        bool isDead = playerF.TakeDamage(Random.Range(3, 10));
        playerHUD.SetHP(playerF.currHp);
        yield return new WaitForSeconds(0);

        if (isDead)
        {
            state = BattleState.L;
            EndBattle();
        }
        else
        {
            state = BattleState.PTURN;
            PlayerTurn();
        }
    }
    void EndBattle()
    {
        if (state == BattleState.W)
        {
            statetext.text = "WIN";
        } else if (state == BattleState.L) {
            statetext.text = "LOSS";
        }
    }

    void PlayerTurn()
    {
        
    }

    IEnumerator PlayerHeal()
    {
        playerF.Heal(Random.Range(3,10));
        playerHUD.SetHP(playerF.currHp);
        yield return new WaitForSeconds(0);
        state = BattleState.ETURN;
        StartCoroutine(EnemyTurn());
    }

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
