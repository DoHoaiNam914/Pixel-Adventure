using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public static Gameplay Instance;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private int _currentLevel;

    [SerializeField]
    private Text _scoreText;

    [Header("Game Over Scene")]
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private Text _deadScoreText;

    [Header("Complete Scene")]
    [SerializeField]
    private Text _finalScoreText;
    [SerializeField]
    private GameObject _completePanel;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioSource _backgroundMusic;
    [SerializeField]
    private AudioSource _jumpSoundFx;
    [SerializeField]
    private AudioSource _collectSoundFx;
    [SerializeField]
    private AudioSource _deadSoundFx;

    private float _axisHorizontal = 0f;
    private bool _onGround;
    private bool _onWall;
    private bool _canDoubleJump = false;
    private bool _isDoubleJump = false;
    private float _wallJumpCooldown = 0f;

    private Stats _awardStat = Stats.Idle;

    private int _scorePoint = 0;

    private enum Stats
    {
        Idle,
        Run,
        Fall,
        Jump,
        WallJump,
        DoubleJump
    }

    private void UpdateAnimationStat()
    {
        if (Mathf.Abs(_axisHorizontal) > 0f)
        {
            _awardStat = Stats.Run;
        }
        else
        {
            _awardStat = Stats.Idle;
        }

        if (!_onGround)
        {
            if (_rigidbody.velocity.y >= 0f)
            {
                if (_isDoubleJump)
                {
                    _awardStat = Stats.DoubleJump;
                }
                else
                {
                    _awardStat = Stats.Jump;
                }
            }
            else
            {
                if (_onWall)
                {
                    _awardStat = Stats.WallJump;
                }
                else
                {
                    _awardStat = Stats.Fall;
                }
            }
        }

        _animator.SetInteger("AwardStat", (int)_awardStat);
    }

    public void AddPoint(int point)
    {
        _scoreText.text = $"Score: {_scorePoint += point}";
    }

    public void PickupItem(GameObject itemObject)
    {
        int point = 0;

        switch (itemObject.tag)
        {
            case "Two Point":
                point = 2;
                break;
            case "Four Point":
                point = 4;
                break;
            case "One Point":
                point = 1;
                break;
        }

        _collectSoundFx.Play();
        AddPoint(point);
        Destroy(itemObject);
    }

    private void Die()
    {
        _rigidbody.bodyType = RigidbodyType2D.Static;
        _boxCollider.enabled = false;
        _backgroundMusic.Stop();
        _deadSoundFx.Play();
        _animator.SetTrigger("Dead");
        _deadScoreText.text = $"Score: {_scorePoint}";
        _gameOverPanel.SetActive(true);
    }

    public void CompleteLevel()
    {
        _animator.enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Static;
        _backgroundMusic.Stop();
        _finalScoreText.text = $"Score: {_scorePoint}";
        _completePanel.SetActive(true);

        if (_currentLevel + 1 > PlayerPrefs.GetInt("UnlockedLevel", 1))
        {
            PlayerPrefs.SetInt("UnlockedLevel", _currentLevel + 1);
        }
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnNextLevelButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody.bodyType == RigidbodyType2D.Static)
        {
            return;
        }

        _onGround = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Terrain")) || Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        _onWall = !_onGround && Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, _spriteRenderer.flipX ? Vector2.left : Vector2.right, 0.1f, LayerMask.GetMask("Terrain"));

        _axisHorizontal = Input.GetAxis("Horizontal");

        if (_wallJumpCooldown > 0.2f)
        {
            if (_axisHorizontal > 0f)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_axisHorizontal < 0f)
            {
                _spriteRenderer.flipX = true;
            }

            _rigidbody.velocity = new Vector2(_axisHorizontal * 10f, _rigidbody.velocity.y);

            if (_onWall && _rigidbody.velocity.y < 0f)
            {
                _canDoubleJump = false;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -0.2f);
            }
        }
        else
        {
            _wallJumpCooldown += Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (_canDoubleJump)
            {
                if (!_onGround && !_onWall)
                {
                    _isDoubleJump = true;
                }

                _canDoubleJump = false;
            }

            if (_onGround || _isDoubleJump)
            {
                _jumpSoundFx.Play();
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 24f);

                if (!_isDoubleJump)
                {
                    _canDoubleJump = true;
                }
            }
            else if (_onWall)
            {
                bool isFacingLeft = _spriteRenderer.flipX;

                if (!_isDoubleJump)
                {
                    _canDoubleJump = true;
                }

                _jumpSoundFx.Play();
                _rigidbody.velocity = new Vector2(-Mathf.Sign(isFacingLeft ? -1f : 1f) * ((isFacingLeft ? Mathf.Abs(_axisHorizontal) : _axisHorizontal) == 1f ? 2f : (isFacingLeft != (_axisHorizontal < 0f) ? 8f : 4f)), 20f);

                if (isFacingLeft == (_axisHorizontal < 0f))
                {
                    _spriteRenderer.flipX = !isFacingLeft;
                }

                _wallJumpCooldown = 0f;
            }
        }

        UpdateAnimationStat();
        _isDoubleJump = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps") || collision.gameObject.CompareTag("Bullets") || (collision.gameObject.CompareTag("Enemies") && Vector2.Dot(collision.relativeVelocity, Vector2.up) <= 13f))
        {
            Die();
        } else if (collision.gameObject.CompareTag("Enemies") && Vector2.Dot(collision.relativeVelocity, Vector2.up) > 13f)
        {
            _rigidbody.velocity = new Vector2(0f, 20f);
        }
    }
}
