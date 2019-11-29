using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System;

using System.Linq;

public class SpeechManager : MonoSingleton<SpeechManager> {

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, Action>();

    private void Start()
    {
        keywords.Add("Show Detail" ,()=> {
          //  BoardManager.Instance.OnVoiceREcive(CommondType.ShowDetail);

        });
        keywords.Add("Display", () => {
           // AirCraftsManager.Instance.OnDisPlayAll(true);
        });
        keywords.Add("Display Off", () => {
           // AirCraftsManager.Instance.OnDisPlayAll(false);
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
       

    }
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if (keywords.TryGetValue(args.text , out keywordAction))
        {
            keywordAction.Invoke();
        }

    }

}
