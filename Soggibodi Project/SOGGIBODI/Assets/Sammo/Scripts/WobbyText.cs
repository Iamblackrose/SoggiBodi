using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWobble : MonoBehaviour
{
    TMP_Text textMesh;

    Mesh mesh;

    Vector3[] vertices;

    List<int> wordIndexes;
    List<int> wordLengths;

    public Vector2 speed = new(1,1);

    public Gradient rainbow;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();

        speed.x = Random.Range(5.5f, 20.5f);
        speed.x = Random.Range(5.5f, 20.5f);

        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();

        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);

        var colours = new GradientColorKey[2];
        colours[0] = new GradientColorKey(Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f), 0.0f);
        colours[1] = new GradientColorKey(Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f), 1.0f);

        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        rainbow.SetKeys(colours, alphas);
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        Color[] colors = mesh.colors;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * 0.001f, 1f));
            colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
            colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
            colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));

            Vector3 offset = Wobble(Time.time + i);
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * speed.x), Mathf.Cos(time * speed.y));
    }
}