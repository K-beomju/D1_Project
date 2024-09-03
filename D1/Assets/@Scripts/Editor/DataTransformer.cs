using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System;
using System.ComponentModel;
using System.Linq;
using Data;



public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcelDataToJson()
    {
        ParseExcelDataToJson<HeroInfoDataLoader, HeroInfoData>("HeroInfo");
        Debug.Log("DataTransformer Completed");
    }


    #region Helpers
    /// 1. where Loader : new()
    // 이 제약 조건은 Loader 타입이 반드시 기본 생성자(매개변수가 없는 생성자)를 가지고 있어야 한다는 의미입니다.
    // new() 제약 조건은 메서드 내부에서 Loader 타입의 객체를 생성할 수 있도록 보장해 줍니다. 
    // 즉, Loader loader = new Loader();와 같이 Loader 타입의 객체를 만들 수 있어야 한다는 뜻입니다.

    // 2. where LoaderData : new()
    // LoaderData 타입도 마찬가지로 기본 생성자를 가져야 한다는 제약 조건입니다.
    // 이 제약 조건을 통해 LoaderData 타입의 객체도 메서드 내에서 new 키워드를 통해 인스턴스를 생성할 수 있습니다.
    private static void ParseExcelDataToJson<Loader, LoaderData>(string fileName) where Loader : new() where LoaderData : new()
    {
        // Loader 타입 객체 생성. 클래스를 만들 수 있는 이유는 위에서 Loader : new() 라는 제약 조건을 설정했기 때문 
        // -> 이 객체는 엑셀 데이터를 담아 JSON 파일로 변환하는 컨테이너 역할  
        Loader loader = new Loader();

        /*
        실습.01 

        MyClass myClass = new MyClass();
        Type type = typeof(MyClass);
        FieldInfo fieldInfos = type.GetField("myField");

        Debug.Log(fieldInfos.Name);
        Debug.Log(fieldInfos.GetValue(myClass));
        fieldInfos.SetValue(myClass, 100);
        Debug.Log(fieldInfos.GetValue(myClass));
        */

        // 리플렉션을 사용해 객체의 필드 정보를 동적으로 조회 
        // Loader 클래스에서 정의된 첫번째 필드를 선택하는 코드 
        // -> 첫 번째 필드를 가져오는 이유는 해당 필드가 데이터를 저장하는 필드라고 가정하기 때문입니다.

        FieldInfo field = loader.GetType().GetFields()[0];

        // ParseExcelDataToList 메서드: 이 메서드는 엑셀 데이터를 파싱하여 LoaderData 객체들의 리스트로 변환합니다. 각 엑셀 행이 LoaderData 객체로 변환되며, 그 리스트가 해당 필드에 저장됩니다.
        field.SetValue(loader, ParseExcelDataToList<LoaderData>(fileName));

        //설명: loader 객체를 JSON 형식의 문자열로 변환합니다. **JsonConvert.SerializeObject**는 C# 객체를 JSON 문자열로 직렬화하는 기능을 합니다.
        // Formatting.Indented: 직렬화할 때 JSON 파일을 읽기 쉽게 들여쓰기(indent)를 적용합니다. 이렇게 하면 사람이 쉽게 읽을 수 있는 구조로 출력됩니다.
        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        // Application.dataPath: 현재 프로젝트의 Assets 폴더 경로를 가져오는 Unity의 내장 변수입니다. 이를 사용해 적절한 파일 경로를 설정합니다.
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{fileName}Data.json", jsonStr);

        // Unity 에디터에서 JSON 파일이 새로 생성되었음을 알리기 위해 **AssetDatabase.Refresh()**를 호출합니다. 
        // 이렇게 하면 Unity 에디터가 파일 시스템 변화를 감지하고, 에셋 데이터를 업데이트하게 됩니다.
        AssetDatabase.Refresh();
    }

    // 엑셀 데이터를 읽어서 -> LoaderData 객체들의 리스트로 변환하는 작업 수행 
    // 이 과정을 통해 각 행(row) 데이터를 'LoaderData'라는 제네릭 타입의 객체로 변환하고, 리스트로 만들어 반환함. 
    // csv나 데이터베이스 객채로 변환하는 과정에서 많이 사용 
    private static List<LoaderData> ParseExcelDataToList<LoaderData>(string fileName) where LoaderData : new()
    {
        // 목적: 엑셀 파일에서 읽어온 데이터를 담을 LoaderData 객체들의 리스트를 생성합니다.
        List<LoaderData> loaderDatas = new List<LoaderData>();

        // 폴더 경로 
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/ExcelData/{fileName}Data.csv").Split("\n");

        // CSV 파일의 각 줄을 처리
        // 설명: 첫 번째 줄(보통 헤더)이 아닌 두 번째 줄부터 마지막 줄까지 순회합니다. 이 루프는 각 줄(데이터 행)을 처리하는 역할을 합니다.

        for (int l = 1; l < lines.Length; l++)
        {
            // 5. 각 행을 필드로 나누기
            string[] row = lines[l].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0]))
                continue;
            // 설명: 각 줄을 쉼표(,)를 기준으로 나누어 열(column) 단위로 배열로 변환합니다. 이렇게 하면 각 열의 데이터가 배열의 요소로 저장됩니다.
            // 검사: 해당 줄에 데이터가 없거나 첫 번째 셀이 비어 있으면 그 줄은 무시하고 다음 줄로 넘어갑니다.

            // 위 설명과 똑같음 new() 
            LoaderData loaderData = new LoaderData();

            // LoaderData 객체의 필드 가져오기
            var fields = GetFieldsInBase(typeof(LoaderData));
            // 설명: LoaderData 클래스에서 정의된 모든 필드를 리플렉션을 통해 가져옵니다. 이 필드는 각 열(column) 데이터와 매핑될 예정입니다.
            // GetFieldsInBase: 이 메서드는 LoaderData 타입의 필드 목록을 반환합니다.


            // 각 필드에 데이터를 할당
            for (int f = 0; f < fields.Count; f++)
            {
                FieldInfo field = loaderData.GetType().GetField(fields[f].Name);
                Type type = field.FieldType;

                if (type.IsGenericType)
                {
                    object value = ConvertList(row[f], type);
                    field.SetValue(loaderData, value);
                }
                else
                {
                    object value = ConvertValue(row[f], field.FieldType);
                    field.SetValue(loaderData, value);
                }

                // 루프: LoaderData 객체의 모든 필드를 순회하며 각 필드에 엑셀 데이터의 해당 열 값을 할당합니다.
                // field.SetValue(loaderData, value): 해당 필드에 값을 설정합니다.
                // 일반 필드: ConvertValue 메서드를 통해 데이터를 필드의 타입에 맞게 변환합니다. 예를 들어, 문자열을 숫자로 변환하는 작업을 수행할 수 있습니다.
                // 리스트 필드: 필드가 제네릭 타입일 경우(예: List<int>), ConvertList 메서드를 사용해 엑셀 데이터를 리스트 형태로 변환하여 설정합니다.
            }

            // 객체 리스트에 추가
            loaderDatas.Add(loaderData);

        }

        return loaderDatas;
    }

    private static object ConvertValue(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        // 타입 변환 
        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);
        // 설명: TypeDescriptor.GetConverter(type)는 지정된 타입에 대한 TypeConverter를 가져옵니다. 이 변환기를 통해 문자열을 해당 타입으로 변환할 수 있습니다.
        // 예: "42"라는 문자열이 있으면, 이를 int 타입으로 변환하여 숫자 42로 바꿔줍니다.
        // ConvertFromString: 문자열을 지정된 타입으로 변환합니다.

    }

    private static object ConvertList(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        //Reflection 

        // 리스트 타입 설정
        Type valueType = type.GetGenericArguments()[0];
        Type genericListType = typeof(List<>).MakeGenericType(valueType);
        var genericList = Activator.CreateInstance(genericListType) as IList;
        /*        
            type.GetGenericArguments()[0]: 리스트의 제네릭 타입(예: List<int>에서 int)을 추출합니다.
            MakeGenericType: List<valueType>과 같은 형태로 제네릭 리스트 타입을 동적으로 생성합니다.
            Activator.CreateInstance: 위에서 동적으로 생성한 리스트 타입의 인스턴스를 생성합니다. 즉, List<int>나 List<string>과 같은 리스트 객체가 동적으로 만들어집니다.
        */

        // 리스트의 각 아이템 변환
        var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();
        /*
            설명: 문자열 데이터를 & 문자로 구분하여 여러 개의 값으로 나눕니다. 이 나눈 값들을 각각 ConvertValue 메서드를 사용해 리스트의 각 아이템으로 변환합니다.
            예: "1&2&3"라는 값이 있으면 이를 "1", "2", "3"으로 나누고, 각 값이 숫자 1, 2, 3으로 변환됩니다.
        */

        foreach (var item in list)
            genericList.Add(item);

        return genericList;
    }


    // 이 코드는 **리플렉션(Reflection)**을 사용하여 클래스의 상속 계층에서 모든 필드를 추출하는 메서드입니다. 
    // 이 메서드는 부모 클래스에서 정의된 필드들까지 포함하여 현재 클래스와 그 상속 계층에 존재하는 필드들을 모두 가져옵니다. 
    // 필드 중복을 방지하기 위해 HashSet을 사용하며, 상속된 필드들을 관리하기 위해 스택 구조를 사용합니다.

    /*
        type: 필드를 추출할 클래스의 타입을 나타냅니다.
        bindingFlags: 필드를 가져올 때 사용할 바인딩 플래그입니다. 여기에서는 Public, NonPublic, Instance 필드를 모두 포함하도록 기본값이 설정되어 있습니다.
    */
    public static List<FieldInfo> GetFieldsInBase(Type type, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    {
        // 1. 필드를 저장할 리스트 및 필드 중복 방지용 HashSet
        List<FieldInfo> fields = new List<FieldInfo>();
        HashSet<string> fieldNames = new HashSet<string>();
        /* 
            fields 리스트: FieldInfo 객체들을 담을 리스트입니다. 이 리스트에 각 필드 정보를 추가합니다.
            fieldNames HashSet: 필드의 이름을 저장하여 중복된 필드를 필터링합니다. 동일한 이름의 필드가 이미 존재하면 리스트에 추가되지 않도록 합니다.
        */

        // 2. 타입 상속 계층을 추적하는 스택
        Stack<Type> stack = new Stack<Type>();

        while (type != typeof(object))
        {
            stack.Push(type);
            type = type.BaseType;
        }
        /*
            목적: 이 코드는 상속 계층을 추적하여 스택에 저장합니다. 상속 계층에서 가장 아래에 있는 부모 클래스부터 차례대로 스택에 쌓습니다.
            동작: type이 object가 될 때까지(즉, 최상위 부모 클래스인 System.Object까지) 상속 계층의 모든 클래스를 스택에 넣습니다. 
            각 타입은 상속 계층에서 해당 클래스의 부모 클래스를 가리키는 BaseType 속성을 통해 추적됩니다.
        */

        // 3. 상속 계층에서 필드 추출
        while (stack.Count > 0)
        {
            Type currentType = stack.Pop();

            foreach (var field in currentType.GetFields(bindingFlags))
            {
                if (fieldNames.Add(field.Name))
                {
                    fields.Add(field);
                }
            }
        }
        /* 
            설명: 스택에 쌓인 타입들을 하나씩 꺼내면서 해당 타입에 정의된 필드들을 가져옵니다.
            currentType.GetFields(bindingFlags): 각 타입의 필드를 가져옵니다. bindingFlags를 사용해 public, private, instance 필드 등을 가져올 수 있습니다.
            필드 중복 체크: 필드의 이름이 fieldNames HashSet에 이미 있는지 확인합니다. 만약 없다면 그 필드를 리스트에 추가하고, fieldNames에 해당 필드의 이름을 기록합니다. 이 과정을 통해 중복 필드를 방지합니다.
        */

        /* 
        실습.02 
        BindingFlags는 리플렉션을 사용할 때 검색할 멤버의 범위와 조건을 제어하는 플래그입니다. 
        class MyClass
        {
            private int myField = 42;
        }

        public class Program
        {
            public static void Main()
            {
                Type type = typeof(MyClass);
                FieldInfo fieldInfo = type.GetField("myField", BindingFlags.NonPublic | BindingFlags.Instance);

                MyClass obj = new MyClass();
                Console.WriteLine("Field Value: " + fieldInfo.GetValue(obj));
            }
        }

        BindingFlags.NonPublic | BindingFlags.Instance: 이 플래그는 private 필드이면서 인스턴스 필드인 myField를 검색하라는 의미입니다.
        결과: 이 코드는 private 필드인 myField의 값을 가져와 출력합니다.

        여러 BindingFlags 조합
        BindingFlags는 비트 플래그 방식으로 구현되어 있기 때문에 비트 연산자를 사용해 여러 플래그를 조합할 수 있습니다. 
        예를 들어, BindingFlags.Public | BindingFlags.Instance는 public이면서 인스턴스 멤버를 검색하는 것입니다.
        */

        return fields;
    }
    #endregion

#endif
}
