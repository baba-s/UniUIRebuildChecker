# UniUIRebuildChecker

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

        var stringBuilder = new StringBuilder();

        stringBuilder.Append( "[最適化]リビルドされた UI の合計数：" );
        stringBuilder.Append( list.GroupBy( x => x.Canvas ).Sum( x => x.First().RebuildTargets.Count ).ToString() );
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine( "■ 詳細" );
        stringBuilder.AppendLine();

        foreach ( var rebuildData in list )
        {
            stringBuilder.Append( "UI オブジェクト：" );
            stringBuilder.Append( rebuildData.Graphic.name );
            stringBuilder.AppendLine();

            stringBuilder.Append( "所属するキャンバス：" );
            stringBuilder.Append( rebuildData.Canvas.name );
            stringBuilder.AppendLine();

            stringBuilder.Append( "キャンバスに所属しているオブジェクトの数：" );
            stringBuilder.Append( rebuildData.RebuildTargets.Count.ToString() );
            stringBuilder.AppendLine();

            stringBuilder.AppendLine();
        }

        var text = stringBuilder.ToString();

        Debug.Log( text );
    }
}
```