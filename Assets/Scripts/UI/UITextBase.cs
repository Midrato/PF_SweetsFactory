using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextBase : MonoBehaviour
{
    protected TextMeshProUGUI myText;

    private string currentText;

    protected virtual void Start(){
        myText = GetComponent<TextMeshProUGUI>();
        UpdateContent();
        currentText = myText.text;
    }

    protected virtual void Update(){
        UpdateContent();
        // 差分が発生したら特定の処理を実行
        if(currentText != myText.text){
            ChangedContent();
            currentText = myText.text;
        }
    }

    // テキスト内容の更新の際に用いる関数
    public virtual void UpdateContent(){
    }

    // テキスト内容のが変わった際に実行される関数
    public virtual void ChangedContent(){
    }
}
