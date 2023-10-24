using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;
	public AudioSource hitSFX;
	public AudioSource missSFX;
	public TMPro.TextMeshPro scoreText;
	public static int comboScore;

	public SpriteRenderer bellyDancer;
	public SpriteRenderer square;
	internal float targetAlpha;
	internal float targetAlphaInverse;
	internal float FadeRate = 2f;

	private void Start()
	{
		Instance = this;
		comboScore = 0;
	}

	public static void Hit()
	{
		comboScore++;
		Instance.hitSFX.Play();
	}

	public static void Miss()
	{
		comboScore = 0;
		Instance.missSFX.Play();
	}

	private void Update()
	{
		scoreText.text = comboScore.ToString();
		targetAlpha = (float)comboScore / 20f;
		targetAlphaInverse = 1f - targetAlpha;
		Color color = bellyDancer.color;
		Color color2 = square.color;
		Color color3 = scoreText.color;
		if (Mathf.Abs(color.a - targetAlpha) > 0.0001f)
		{
			color.a = Mathf.Lerp(color.a, targetAlpha, FadeRate * Time.deltaTime);
			color2.a = Mathf.Lerp(color2.a, targetAlphaInverse, FadeRate * Time.deltaTime);
			color3.a = Mathf.Lerp(color3.a, targetAlphaInverse, FadeRate * Time.deltaTime);
			bellyDancer.color = color;
			square.color = color2;
			scoreText.color = color3;
		}
	}
}
