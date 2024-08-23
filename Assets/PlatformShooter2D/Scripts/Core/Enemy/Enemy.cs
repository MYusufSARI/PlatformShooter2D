using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header(" Settings ")]
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    [Header(" Elements ")]
    private Rigidbody2D _rigidBody;

    [Header(" Data ")]
    private Movement _movement;
    private ColorChanger _colorChanger;
    private Knockback _knockback;
    private Flash _flash;
    private Health _health;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
    }


    private void Start()
    {
        StartCoroutine(ChangeDirectionRoutine());

        StartCoroutine(RandomJumpRoutine());
    }


    public void Initialize(Color color)
    {
        _colorChanger.SetDefaultColor(color);
    }


    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            var _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1

            _movement.SetCurrentDirection(_currentDirection);

            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }


    private IEnumerator RandomJumpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int damageAmount, float knockBackThrust)
    {
        _health.TakeDamage(damageAmount);
        _knockback.GetKnockedBack(PlayerController.Instance.transform.position, knockBackThrust);


    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }

}
