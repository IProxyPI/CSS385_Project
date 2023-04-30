using UnityEngine;

public class Fighter : MonoBehaviour
{
    private float nextMove = 0;
    private bool blocking = false;

    [SerializeField] float _transition = 0.1f;
    [SerializeField] float _attackLength = 1f;
    [SerializeField] float _stunLength = 0.6f;

    [SerializeField] Transform _origin;
    [SerializeField] GameObject _attackPrefab;
    [SerializeField] GameObject _stunPrefab;
    [SerializeField] GameObject _blockPrefab;

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    public bool Action(int action)
    {
        
        if (Time.time > nextMove) {
            if (action == 0)
            {
                blocking = true;
                Block();
            }
            if (action == -1)
            {
                blocking = false;
                EndBlock();
            }
            if (!blocking)
            {
                if (action == 1)
                {
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
    }

    private void Stun()
    {
        nextMove += Time.time + _stunLength;
        GameObject stun = Instantiate(_stunPrefab, _origin.position, _origin.rotation);
    }

    private void Block()
    {
        GameObject block = Instantiate(_blockPrefab, _origin.position, _origin.rotation);
    }

    private void EndBlock()
    {
        // Destroy block object
    }
    
    private void Transition()
    {
        nextMove = Time.time + _transition;
    }

    private void UpdateTimer()
    {

    }
}
