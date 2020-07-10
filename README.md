# UniUIRebuildChecker

リビルドされた UI オブジェクトの情報を取得できる機能  

## 使用例

```cs
using Kogane;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class UIRebuildCheckerBehaviour : MonoBehaviour
{
    private void Update()
    {
        var list = UIRebuildChecker.Check();

        if ( list.Count <= 0 ) return;

        var sb = new StringBuilder();

        sb.Append( "[最適化]リビルドされた UI の合計数：" );
        sb.Append( list.GroupBy( x => x.Canvas ).Sum( x => x.First().RebuildTargets.Count ).ToString() );
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine( "■ 詳細" );
        sb.AppendLine();

        foreach ( var rebuildData in list )
        {
            sb.Append( "UI オブジェクト：" );
            sb.Append( rebuildData.Graphic.name );
            sb.AppendLine();

            sb.Append( "所属するキャンバス：" );
            sb.Append( rebuildData.Canvas.name );
            sb.AppendLine();

            sb.Append( "キャンバスに所属しているオブジェクトの数：" );
            sb.Append( rebuildData.RebuildTargets.Count.ToString() );
            sb.AppendLine();

            sb.AppendLine();
        }

        var text = sb.ToString();

        Debug.Log( text );
    }
}
```

## 補足

* Position、Rotation、Scale が変更された場合に情報が取得できないことがある  
