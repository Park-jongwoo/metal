using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    /* -------------- 프로퍼티 -------------- */
    public RectTransform uiGroup;
    public Animator _animation;

    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;
    public string[] talkData;
    public Text talkText;

    Player enterPlayer;

    /* -------------- 이벤트 함수 -------------- */
    public void Enter(Player player) // 존에 들어 갈 때 
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit() // 존에서 나갈 때
    {
        _animation.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);

        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);

    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
