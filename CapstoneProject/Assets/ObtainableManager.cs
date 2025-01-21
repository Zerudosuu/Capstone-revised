using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using UnityEditor.ShaderGraph.Internal;
using static Yarn.Compiler.BasicBlock;

public class ObtainableManager : MonoBehaviour
{
    [Header("Coin Reward")] [SerializeField]
    private GameObject coinPref;

    [SerializeField] private Transform coinSpawnPos;
    [SerializeField] private Transform coinTargetPos;
    [SerializeField] private int coinQuan;


    [Header("EXP Reward")] [SerializeField]
    private GameObject expPref;

    [SerializeField] private Transform expSpawnPos;
    [SerializeField] private Transform expTargetPos;
    [SerializeField] private int expQuan;

    [Header("Reference")] [SerializeField] private float randomPos;


    private int coinDestroy;
    private int expDestroy;


    public void StartDistributingReward()
    {
        StartCoroutine(GiveReward(300, 100));
    }

    //This function will handle the sequence of giving Reward
    public IEnumerator GiveReward(int coinQuant, int expQuant)
    {
        Coroutine coinCoroutine = StartCoroutine(SpawnReward(coinPref, coinSpawnPos, coinTargetPos, coinQuant));
        Coroutine expCoroutine = StartCoroutine(SpawnReward(expPref, expSpawnPos, expTargetPos, expQuant));

        yield return coinCoroutine;
        yield return expCoroutine;
    }


    //This function will handle the Animation of the reward collection
    private IEnumerator SpawnReward(GameObject type, Transform spawn, Transform target, int quantity)
    {
        var delay = 0f;

        //To reduce assets spawning it will be devided to 10
        quantity = quantity / 10;

        for (int i = 0; i < quantity; i++)
        {
            GameObject _spawnReward = Instantiate(type, spawn.position, Quaternion.identity, gameObject.transform);

            Vector3 randomOffset = spawn.position + new Vector3(
                Random.Range(-randomPos, randomPos),
                Random.Range(-randomPos, randomPos),
                Random.Range(-randomPos, randomPos)
            );

            Sequence coinSequence = DOTween.Sequence();

            coinSequence.Append(_spawnReward.transform.DOMove(randomOffset, 0.5f).SetEase(Ease.OutQuad));

            coinSequence.Append(_spawnReward.transform.DOMove(target.position, 1f).SetEase(Ease.InQuad))
                .OnComplete(() =>
                {
                    Destroy(_spawnReward);
                    if (type == coinPref)
                        coinDestroy++;
                    else
                        expDestroy++;
                });

            coinSequence.PrependInterval(delay);

            delay += 0.05f;
        }

        yield return null;
    }
}