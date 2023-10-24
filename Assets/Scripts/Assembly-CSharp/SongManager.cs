using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Networking;

public class SongManager : MonoBehaviour
{
	public static SongManager Instance;

	public AudioSource audioSource;

	public Lane[] lanes;

	public float songDelayInSeconds;

	public double marginOfError;

	public int inputDelayInMilliseconds;

	public string fileLocation;

	public float noteTime;

	public float noteSpawnY;

	public float noteTapY;

	public static MidiFile midiFile;

	public float noteDespawnY => noteTapY - (noteSpawnY - noteTapY);

	private void Start()
	{
		Instance = this;
		if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
		{
			StartCoroutine(ReadFromWebsite());
		}
		else
		{
			ReadFromFile();
		}
	}

	private IEnumerator ReadFromWebsite()
	{
		UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation);
		try
		{
			yield return www.SendWebRequest();
			//if (www.get_isNetworkError() || www.get_isHttpError())
			//{
			//	Debug.LogError(www.get_error());
			//	yield break;
			//}
			//using (MemoryStream stream = new MemoryStream(www.get_downloadHandler().get_data()))
			//{
			//	midiFile = MidiFile.Read(stream);
			//	GetDataFromMidi();
			//}
		}
		finally
		{
			((IDisposable)www)?.Dispose();
		}
	}

	private void ReadFromFile()
	{
		midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
		GetDataFromMidi();
	}

	public void GetDataFromMidi()
	{
		ICollection<Melanchall.DryWetMidi.Interaction.Note> notes = midiFile.GetNotes();
		Melanchall.DryWetMidi.Interaction.Note[] array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
		notes.CopyTo(array, 0);
		Lane[] array2 = lanes;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetTimeStamps(array);
		}
		Invoke("StartSong", songDelayInSeconds);
	}

	public void StartSong()
	{
		audioSource.Play();
	}

	public static double GetAudioSourceTime()
	{
		return (double)Instance.audioSource.timeSamples / (double)Instance.audioSource.clip.frequency;
	}

	private void Update()
	{
	}
}
