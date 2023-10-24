using UnityEngine;

public class Note : MonoBehaviour
{
	private double timeInstantiated;

	public float assignedTime;

	private float targetAlpha;

	private int score;

	private int FadeRate = 2;

	private SpriteRenderer sprite;

	private void Start()
	{
		timeInstantiated = SongManager.GetAudioSourceTime();
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float num = (float)((SongManager.GetAudioSourceTime() - timeInstantiated) / (double)(SongManager.Instance.noteTime * 2f));
		score = ScoreManager.comboScore;
		targetAlpha = 1f - (float)score / 20f;
		Color color = sprite.color;
		if (Mathf.Abs(color.a - targetAlpha) > 0.0001f)
		{
			color.a = Mathf.Lerp(color.a, targetAlpha, (float)FadeRate * Time.deltaTime);
			sprite.color = color;
		}
		if (num > 1f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, num);
		GetComponent<SpriteRenderer>().enabled = true;
	}
}
