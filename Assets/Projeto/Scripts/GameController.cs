using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int score;
    public Text txtScore;
    public GameObject hitPrefab;

    public AudioSource fxGame;
    public AudioClip fxCenouraColetada;
    public AudioClip fxExplosao;
    
    public void Pontuacao(int qtdPontos)
    {
        score += qtdPontos;
        txtScore.text = score.ToString();

        // Som da coleta da cenoura
        fxGame.PlayOneShot(fxCenouraColetada);
    }
}
