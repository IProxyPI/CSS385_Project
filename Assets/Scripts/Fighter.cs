using UnityEngine;

public class Fighter : MonoBehaviour
{
    private float nextMove = 0;
    private bool blocking = false;
    private bool curBlock = false;

    [SerializeField] float _transition = 0.1f;
    [SerializeField] float _attackLength = 1f;
    [SerializeField] float _stunLength = 0.6f;

    [SerializeField] Transform _origin;
    [SerializeField] GameObject _attackPrefab;
    [SerializeField] GameObject _stunPrefab;
    [SerializeField] GameObject _blockPrefab;

    private GameObject block; // Don't exactly know how to implement this like the others for now

    // Update is called once per frame
    void Update()
    {
        CheckBlocking();
    }

    public bool Action(int action)
    {
        Debug.Log("Action");
        if (Time.time > nextMove)
        {
            if (action == 0)
            {
                blocking = true;
            }
            if (action == -1)
            {
                blocking = false;
            }
            if (!blocking)
            {
                if (action == 1)
                {
                    Debug.Log("Attack");
                    Attack();
                }
                if (action == 2)
                {
                    Stun();
                }
            }
            Transition();
        }

        return true;
    }

    private void Attack()
    {
        nextMove = Time.time + _attackLength;
        GameObject attack = Instantiate(_attackPrefab, _origin.position, _origin.rotation);
        while (true)
        {
            if (Time.time > nextMove)
            {
                Debug.Log(Time.time + " " + nextMove);
                Destroy(attack);
                break;
            }
        }
    }

    private void Stun()
    {
        nextMove += Time.time + _stunLength;
        GameObject stun = Instantiate(_stunPrefab, _origin.position, _origin.rotation);
        while (true)
        {
            if (Time.time > nextMove)
            {
                Destroy(stun);
                break;
            }
        }
    }


    private void CheckBlocking()
    {
        if (blocking)
        {
            if (!curBlock)
            {
                curBlock = true;
                block = Instantiate(_blockPrefab, _origin.position, _origin.rotation);
            }
        }
        else
        {
            curBlock = false;
            Destroy(block);
        }
    }

    private void Transition()
    {
        nextMove = Time.time + _transition;
    }
}
