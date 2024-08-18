using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 씬(Scene) 관리를 위한 확장된 SceneManager 클래스입니다.
// 씬 로드와 현재 씬 정보 제공 등의 기능을 포함하고 있습니다.
public class SceneManagerEx
{
    // 현재 씬을 반환하는 프로퍼티입니다. BaseScene 타입의 씬을 찾습니다.
    // BaseScene은 사용자가 정의한 씬의 베이스 클래스일 가능성이 큽니다.
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }


    // 지정된 씬 타입에 따라 씬을 로드하는 메서드입니다.
    // LoadScene 메서드는 Unity의 SceneManager를 사용하여 씬을 로드합니다.
    public void LoadScene(Define.EScene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }   

    // Enum 값인 씬 타입을 해당 씬의 이름(문자열)으로 변환하는 메서드입니다.
    private string GetSceneName(Define.EScene type)
    {
        string name = System.Enum.GetName(typeof(Define.EScene), type);
        return name;
    } 

    public void Clear()
    {

    }
}
