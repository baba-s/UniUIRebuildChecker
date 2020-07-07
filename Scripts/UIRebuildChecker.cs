using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Kogane
{
	/// <summary>
	/// リビルドされた UI を確認できるクラス
	/// </summary>
	public static class UIRebuildChecker
	{
		//================================================================================
		// クラス
		//================================================================================
		/// <summary>
		/// リビルドされた UI の情報を管理するクラス
		/// </summary>
		public sealed class RebuildData
		{
			/// <summary>
			/// リビルドされた UI オブジェクト
			/// </summary>
			public Graphic Graphic { get; }

			/// <summary>
			/// リビルドされた UI オブジェクトが所属している Canvas
			/// </summary>
			public Canvas Canvas { get; }

			/// <summary>
			/// リビルドされた UI オブジェクトが所属している Canvas に所属しているすべての UI オブジェクト
			/// </summary>
			public IReadOnlyList<GameObject> RebuildTargets { get; }

			/// <summary>
			/// コンストラクタ
			/// </summary>
			internal RebuildData
			(
				Graphic                   graphic,
				Canvas                    canvas,
				IReadOnlyList<GameObject> rebuildTargets
			)
			{
				Graphic        = graphic;
				Canvas         = canvas;
				RebuildTargets = rebuildTargets;
			}
		}

		//================================================================================
		// 変数
		//================================================================================
		private static IList<ICanvasElement> m_graphicRebuildQueue;

		//================================================================================
		// プロパティ
		//================================================================================
		private static IList<ICanvasElement> GraphicRebuildQueue
		{
			get
			{
				if ( m_graphicRebuildQueue == null )
				{
					var type      = typeof( CanvasUpdateRegistry );
					var fieldInfo = type.GetField( "m_GraphicRebuildQueue", BindingFlags.Instance | BindingFlags.NonPublic );

					m_graphicRebuildQueue = ( IList<ICanvasElement> ) fieldInfo.GetValue( CanvasUpdateRegistry.instance );
				}

				return m_graphicRebuildQueue;
			}
		}

		//================================================================================
		// 関数
		//================================================================================
		/// <summary>
		/// リビルドされた UI を確認して返します
		/// </summary>
		public static List<RebuildData> Check()
		{
			var rebuildList = new List<RebuildData>();

			for ( var i = 0; i < GraphicRebuildQueue.Count; i++ )
			{
				var canvasElement = GraphicRebuildQueue[ i ];

				// 破棄済みのゲームオブジェクトにアクセスしてしまうことがあるため
				// null チェックしています
				if ( canvasElement == null ) continue;

				var graphic = canvasElement as Graphic;

				if ( graphic == null ) continue;

				var canvas = graphic.canvas;

				if ( canvas == null ) continue;

				var rebuildTargets = canvas
						.GetComponentsInChildren<Transform>( true )
						.Select( x => x.gameObject )
						.ToArray()
					;

				var rebuildData = new RebuildData
				(
					graphic: graphic,
					canvas: canvas,
					rebuildTargets: rebuildTargets
				);

				rebuildList.Add( rebuildData );
			}

			return rebuildList;
		}
	}
}